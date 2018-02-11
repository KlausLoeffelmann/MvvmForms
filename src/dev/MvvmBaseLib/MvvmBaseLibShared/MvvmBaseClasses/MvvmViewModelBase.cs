using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    /// <summary>
    /// Abstract class, which is derived in an actual ViewModel. 
    /// Supports INotifyPropertyChanged, IEditableObject, INotifyDataErrorInfo.
    /// </summary>
    public abstract class MvvmViewModelBase : BindableBase, IEditableObject, INotifyDataErrorInfo
    {
        private Dictionary<string, string> myErrorDictionary = new Dictionary<string, string>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public MvvmViewModelBase() : base()
        { }

        /// <summary>
        /// Flag, which let's you set the MvvmViewModel based types into debug mode.
        /// If set, verbose infos are printed into the debug listener when using object cloning functionality.
        /// </summary>
        static public bool IsDebugMode { get; set; } = false;

        /// <summary>
        /// Called when editing a data set begins. Typically when editing in a line within a grid, if the line was entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void NotifyBeginEditEventHandler(object sender, EventArgs e);
        public event NotifyBeginEditEventHandler NotifyBeginEdit;

        /// <summary>
        /// Called when editing of a data set is finished. Typically when editing in a line within a grid, when the line has been left.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void NotifyEndEditEventHandler(object sender, EventArgs e);
        public event NotifyEndEditEventHandler NotifyEndEdit;

        /// <summary>
        /// Called when editing of a data set is finished. Typically when editing in a line within a grid, when the line has been left.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void NotifyCancelEditEventHandler(object sender, EventArgs e);
        public event NotifyCancelEditEventHandler NotifyCancelEdit;

        /// <summary>
        /// Creates a shallow copy of the instance of this class.
        /// </summary>
        /// <typeparam name="bbType"></typeparam>
        /// <returns></returns>
        public bbType ShallowClone<bbType>() where bbType : BindableBase
        {
            return (bbType)base.MemberwiseClone();
        }

        // ''' <summary>
        // ''' Creates a full copy (deep clone) of this instance.
        // ''' </summary>
        // ''' <typeparam name="bbType"></typeparam>
        // ''' <returns></returns>
        // ''' <remarks></remarks>
        public bbType DeepClone<bbType>()
        {

            MemoryStream memStream = new MemoryStream();
            bbType clonedObject = default(bbType);

            //Als JSon in den Speicher serialisieren
            try
            {
                SerializeToStream(memStream);
            }
            catch
            {
                throw;
            }

            //Die Kopie wieder zurückserialisieren.
            try
            {
                memStream.Seek(0, SeekOrigin.Begin);
                clonedObject = (bbType) DeserializeFromStream(memStream);
            }
            catch
            {
                throw;
            }

            return clonedObject;
        }

        internal static object DeserializeFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize(jsonTextReader);
            }
        }

        internal void SerializeToStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(stream))
            using (var jsonTextWriter = new JsonTextWriter(sw))
            {
                serializer.Serialize(jsonTextWriter,this);
            }
        }

        /// <summary>
        /// Creates an instance of this type based on an external data class. 
        /// It is assumed that this type forms a ViewModel of the MVVM pattern, while the foreign data class 
        /// forms the model, and there is a requirement to copy Model and ViewModel.
        /// </summary>
        /// <typeparam name="ViewModelType">The type (preferably the type of derivation of this class), 
        /// which is to act as the new ViewModel instance, and takes the data from the model instance.</typeparam>
        /// <typeparam name="modelType">The type of instance of the model data class from which the property values are taken over.</typeparam>
        /// <param name="model">The instance of the model class from which the data is transferred.</param>
        /// <returns>A new instance of the ViewModel class.</returns>
        /// <remarks>If there are properties in the ViewModel class that do not exist in the model, these properties are 
        /// not copied and there is no error message. 
        /// Using the <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes you can 
        /// specify a different name for the property corresponding to the ViewModel in the model. 
        /// With the <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes you can 
        /// determine that a property is not copied, even though it exists in both the model and the ViewModel.
        /// </remarks>
        public static ViewModelType FromModel<ViewModelType, modelType>(modelType model) where ViewModelType : MvvmViewModelBase, new()
        {
            ViewModelType viewModelToReturn = new ViewModelType();
            viewModelToReturn.CopyPropertiesFrom(model);
            return viewModelToReturn;
        }

        /// <summary>
        /// Creates a copy of objects from a list of a model (in the MVVM pattern) and transfers the corresponding properties of each instance to objects in a list of a ViewModel.
        /// </summary>
        /// <typeparam name="ViewModelType">The type of the ViewModel the new list should consist of.</typeparam>
        /// <typeparam name="Modeltype">The type of model that the existing list consists of.</typeparam>
        /// <param name="modelCollection">The collection with the corresponding model objects.</param>
        /// <returns>List with objects of a ViewModel from the list of models.</returns>
        /// <remarks>This method creates a list of ViewModel objects in the MVVM pattern based on a list of model objects. 
        /// This function uses the static generic method FromModel. If there are properties in the ViewModel class that do 
        /// not exist in the model, these properties are not copied and there is no error message. 
        /// Using the <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes 
        /// you can specify a different name for the property corresponding to the ViewModel in the model. 
        /// With the <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes you 
        /// can determine that a property is not copied, even though it exists in both the model and the ViewModel.
        /// </remarks>
        public static IEnumerable<ViewModelType> FromModelList<ViewModelType, Modeltype>(IEnumerable<Modeltype> modelCollection) where ViewModelType : MvvmViewModelBase, new()
        {
            return
                from modelItem in modelCollection
                select FromModel<ViewModelType, Modeltype>(modelItem);
        }

        /// <summary>
        /// Copies the contents of the properties from another instance of this class to the current one.
        /// (Should be used in particular to simplify the Mvvm pattern for transferring data from a model to a ViewModel).
        /// </summary>
        /// <param name="model">The instance of the model class from which the data is transferred.</param>
        /// <remarks>If there are properties in the ViewModel class that do not exist in the model, these properties are 
        /// not copied and there is no error message. 
        /// Using the <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes you can 
        /// specify a different name for the property corresponding to the ViewModel in the model. 
        /// With the <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes you can 
        /// determine that a property is not copied, even though it exists in both the model and the ViewModel.
        /// </remarks>
        [DebuggerHidden]
        public virtual void CopyPropertiesFrom(object model)
        {
            //Properties holen

            if (!(this.GetType().GetTypeInfo().IsAssignableFrom(model.GetType().GetTypeInfo())))
            {
                CopyPropertiesFromDifferentType(model);
                return;
            }

            var props = model.GetType().GetRuntimeProperties();

            //Wir verwenden hier GetRuntimeProperties anstelle von GetProperties. Das wird für 
            //.NET 4.5 beispielsweise per Projection auf GetProperties gemapped (öffentliche Member, aber die reichen), 
            //funktioniert aber auch in WinRt und Windows Phone.
            foreach (var propItem in props)
            {
                try
                {
                    object value = propItem.GetValue(model, null);
                    propItem.SetValue(this, value, null);
                }
                catch (Exception ex)
                {
                    CopyPropertiesException copPropException = new CopyPropertiesException("Model property '" + propItem.Name + "' could not be copied to ViewModel property '" + propItem.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                    if (IsDebugMode)
                        Debug.WriteLine("Model property '" + propItem.Name + "' could not be copied to ViewModel property '" + propItem.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                }
            }
        }

        private void CopyPropertiesFromDifferentType(object model)
        {
            //Liste erstellen, mit ViewModel- und Model-PropertyInfo-Objekten
            //Create a list with ViewModel and Model PropertyInfo objects.

            //INSTANT C# WARNING: Every field in a C# anonymous type initializer is immutable:
            //ORIGINAL LINE: For Each propToPropItem In From propItem In GetType.GetRuntimeProperties Where propItem.GetCustomAttribute(Of ModelPropertyIgnoreAttribute)() Is Nothing 
            //                  Select New With { .ViewModelProperty = propItem, .ModelProperty = model.GetType.GetRuntimeProperty(If(propItem.GetCustomAttribute(Of ModelPropertyNameAttribute)() Is Nothing, 
            //                  propItem.Name, propItem.GetCustomAttribute(Of ModelPropertyNameAttribute).PropertyName))}
            foreach (var propToPropItem in
            from propItem in GetType().GetRuntimeProperties()
            where (propItem.GetCustomAttribute<ModelPropertyIgnoreAttribute>() == null &&
                  propItem.CanWrite)
            select new
            {
                ViewModelProperty = propItem,
                ModelProperty = model.GetType().GetRuntimeProperty(((propItem.GetCustomAttribute<ModelPropertyNameAttribute>() == null) ?
                        propItem.Name : propItem.GetCustomAttribute<ModelPropertyNameAttribute>().PropertyName))
            })
            {
                //Falls es die entsprechende Eigenschaft im Model nicht gibt, dann überspringen.
                //Skip the ViewModel property, if a corresponding property does not exist in the model.
                if (propToPropItem.ModelProperty == null)
                {
                    continue;
                }

                //Liste Element für Element abarbeiten, und Werte aus der einen Eigenschaft in die andere Eigenschaft übernehmen.
                //Interate through list item by item and copy the property values from one property to the other.
                try
                {
                    object value = null;
                    if (typeof(IDiscoverableValue).GetTypeInfo().IsAssignableFrom(propToPropItem.ModelProperty.PropertyType.GetTypeInfo()))
                    {
                        if (propToPropItem.ModelProperty.CanRead)
                        {
                            var source = (IDiscoverableValue)propToPropItem.ModelProperty.GetValue(model, null);
                            value = source.GetValue();
                        }
                    }
                    else
                    {
                        if (propToPropItem.ModelProperty.CanRead)
                        {
                            value = propToPropItem.ModelProperty.GetValue(model, null);
                        }
                    }

                    if (typeof(IDiscoverableValue).GetTypeInfo().IsAssignableFrom(propToPropItem.ViewModelProperty.PropertyType.GetTypeInfo()))
                    {
                        if (propToPropItem.ViewModelProperty.CanWrite)
                        {
                            var target = (IDiscoverableValue)propToPropItem.ViewModelProperty.GetValue(this, null);
                            target.SetValue(value);
                        }
                    }
                    else
                    {
                        if (propToPropItem.ViewModelProperty.CanWrite)
                        {
                            propToPropItem.ViewModelProperty.SetValue(this, value, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CopyPropertiesException copPropException =
                        new CopyPropertiesException("Model property '" + propToPropItem.ModelProperty.Name + "' could not be copied to ViewModel property '" + propToPropItem.ModelProperty.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                    if (IsDebugMode)
                        Debug.WriteLine("Model property '" + propToPropItem.ModelProperty.Name + "' could not be copied to ViewModel property '" + propToPropItem.ModelProperty.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Copies the contents of the properties of this class to another instance of the same or a different type.
        /// (Should be used in particular to simplify the Mvvm pattern for transferring data from a ViewModel to a Model).
        /// </summary>
        /// <param name="model">The instance of the model class to which the data is transferred.</param>
        /// <remarks>If there are properties in the Model class that do not exist in the ViewModel, these properties are 
        /// not copied and there is no error message. 
        /// Using the <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes you can 
        /// specify a different name for the property corresponding to the ViewModel in the model. 
        /// With the <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes you can 
        /// determine that a property is not copied, even though it exists in both the model and the ViewModel.
        /// </remarks>
        public virtual void CopyPropertiesTo(object model)
        {
            //Properties holen

            if (!(this.GetType().GetTypeInfo().IsAssignableFrom(model.GetType().GetTypeInfo())))
            {
                CopyPropertiesToDifferentType(model);
                return;
            }

            var props = model.GetType().GetRuntimeProperties();

            foreach (var propItem in props)
            {
                try
                {
                    object value = propItem.GetValue(this, null);
                    propItem.SetValue(model, value, null);
                }
                catch (Exception ex)
                {
                    CopyPropertiesException copPropException = new CopyPropertiesException("Model property '" + propItem.Name + "' could not be copied to ViewModel property '" + propItem.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                    if (IsDebugMode)
                        Debug.WriteLine("Model property '" + propItem.Name + "' could not be copied to ViewModel property '" + propItem.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                }
            }
        }

        private void CopyPropertiesToDifferentType(object model)
        {

            //Liste erstellen, mit ViewModel- und Model-PropertyInfo-Objekten
            //Create a list with ViewModel and Model PropertyInfo objects.
            foreach (var propToPropItem in
                from propItem in GetType().GetRuntimeProperties()
                where (propItem.GetCustomAttribute<ModelPropertyIgnoreAttribute>() == null)
                select new
                {
                    ViewModelProperty = propItem,
                    ModelProperty = model.GetType().GetRuntimeProperty(((propItem.GetCustomAttribute<ModelPropertyNameAttribute>() == null) ?
                    propItem.Name : propItem.GetCustomAttribute<ModelPropertyNameAttribute>().PropertyName))
                })
            {

                //Falls es die entsprechende Eigenschaft im Model nicht gibt, dann überspringen.
                //Skip the ViewModel property, if a corresponding property does not exist in the model.
                if (propToPropItem.ModelProperty == null)
                {
                    continue;
                }

                //Liste Element für Element abarbeiten, und Werte aus der einen Eigenschaft in die andere Eigenschaft übernehmen.
                //Interate through list item by item and copy the property values from one property to the other.
                try
                {
                    object value = null;
                    if (typeof(IDiscoverableValue).GetTypeInfo().IsAssignableFrom(propToPropItem.ViewModelProperty.PropertyType.GetTypeInfo()))
                    {
                        if (propToPropItem.ViewModelProperty.CanRead)
                        {
                            var source = (IDiscoverableValue)propToPropItem.ViewModelProperty.GetValue(this, null);
                            value = source.GetValue();
                        }
                    }
                    else
                    {
                        if (propToPropItem.ViewModelProperty.CanRead)
                        {
                            value = propToPropItem.ViewModelProperty.GetValue(this, null);
                        }
                    }

                    if (typeof(IDiscoverableValue).GetTypeInfo().IsAssignableFrom(propToPropItem.ModelProperty.PropertyType.GetTypeInfo()))
                    {
                        if (propToPropItem.ModelProperty.CanWrite)
                        {
                            var target = (IDiscoverableValue)propToPropItem.ModelProperty.GetValue(model, null);
                            target.SetValue(value);
                        }
                    }
                    else
                    {
                        if (propToPropItem.ModelProperty.CanWrite)
                        {
                            propToPropItem.ModelProperty.SetValue(model, value, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CopyPropertiesException copPropException = new CopyPropertiesException("ViewModel property '" + propToPropItem.ViewModelProperty.Name + "' could not be copied to Model property '" + propToPropItem.ModelProperty.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                    if (IsDebugMode)
                        Debug.WriteLine("ViewModel property '" + propToPropItem.ViewModelProperty.Name + "' could not be copied to Model property '" + propToPropItem.ModelProperty.Name + "'." + Environment.NewLine + "Reason: " + ex.Message);
                }
            }
        }

        void IEditableObject.BeginEdit()
        {
            this.BeginEdit();
        }
        private void BeginEdit()
        {
            OnNotifyBeginEdit(EventArgs.Empty);
        }

        private void OnNotifyBeginEdit(EventArgs e)
        {
            if (NotifyBeginEdit != null)
                NotifyBeginEdit(this, e);
        }

        void IEditableObject.CancelEdit()
        {
            this.CancelEdit();
        }
        private void CancelEdit()
        {
            OnNotifyCancelEdit(EventArgs.Empty);
        }

        private void OnNotifyCancelEdit(EventArgs e)
        {
            if (NotifyCancelEdit != null)
                NotifyCancelEdit(this, e);
        }

        void IEditableObject.EndEdit()
        {
            this.EndEdit();
        }
        private void EndEdit()
        {
            OnNotifyEndEdit(EventArgs.Empty);
        }

        private void OnNotifyEndEdit(EventArgs e)
        {
            if (NotifyEndEdit != null)
                NotifyEndEdit(this, e);
        }


        public virtual IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException("Not yet implemented.");
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs eArgs)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, eArgs);
        }

        [MvvmSystemElement, XmlIgnore, JsonIgnore]
        public bool HasErrors
        {
            get
            {
                //Throw New NotImplementedException("Not yet implemented.")
                return false;
            }
        }
    }
}