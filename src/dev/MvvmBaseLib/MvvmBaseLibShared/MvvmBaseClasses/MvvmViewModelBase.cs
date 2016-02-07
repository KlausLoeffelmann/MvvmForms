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
    public abstract class MvvmViewModelBase : BindableBase, IEditableObject, INotifyDataErrorInfo
    {
        private Dictionary<string, string> myErrorDictionary = new Dictionary<string, string>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public MvvmViewModelBase() : base()
        { }

        static public bool IsDebugMode { get; set; } = false;

        /// <summary>
        /// Wird aufgerufen, wenn das Editieren eines Datensatzes beginnt. Typischerweise beim Editieren in einer Zeile innerhalb eines Grids, wenn die Zeile betreten wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void NotifyBeginEditEventHandler(object sender, EventArgs e);
        public event NotifyBeginEditEventHandler NotifyBeginEdit;

        /// <summary>
        /// Wird aufgerufen, wenn das Editieren eines Datensatzes beendet ist. Typischerweise beim Editieren in einer Zeile innerhalb eines Grids, wenn die Zeile verlassen wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void NotifyEndEditEventHandler(object sender, EventArgs e);
        public event NotifyEndEditEventHandler NotifyEndEdit;

        /// <summary>
        /// Wird aufgerufen, wenn das Editieren eines Datensatzes beendet ist. Typischerweise beim Editieren in einer Zeile innerhalb eines Grids, wenn die Zeile verlassen wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        public delegate void NotifyCancelEditEventHandler(object sender, EventArgs e);
        public event NotifyCancelEditEventHandler NotifyCancelEdit;

        /// <summary>
        /// Erstellt eine flache Kopie der Instanz dieser Klasse.
        /// </summary>
        /// <typeparam name="bbType"></typeparam>
        /// <returns></returns>
        public bbType ShallowClone<bbType>() where bbType : BindableBase
        {
            return (bbType)base.MemberwiseClone();
        }

        // ''' <summary>
        // ''' Erstellt eine vollständige Kopie (deep clone) dieser Instanz.
        // ''' </summary>
        // ''' <typeparam name="bbType"></typeparam>
        // ''' <returns></returns>
        // ''' <remarks></remarks>
        public bbType DeepClone<bbType>()
        {

            MemoryStream memStream = new MemoryStream();
            var js = new System.Runtime.Serialization.Json.DataContractJsonSerializer(this.GetType(), TypeDetector.GetTypes(this));
            bbType clonedObject = default(bbType);

            //Als JSon in den Speicher serialisieren
            try
            {
                js.WriteObject(memStream, this);
            }
            catch
            {
                throw;
            }

            //Die Kopie wieder zurückserialisieren.
            try
            {
                memStream.Seek(0, SeekOrigin.Begin);
                clonedObject = (bbType)js.ReadObject(memStream);
            }
            catch
            {
                throw;
            }

            return clonedObject;
        }

        /// <summary>
        /// Erstellt eine Instanz dieses Typs auf Basis einer fremden Datenklasse. Angenommen wird hierbei, 
        /// dass dieser Typ ein ViewModel des MVVM-Patterns bildet, während die fremde Datenklasse das Model bildet, 
        /// und es die Anforderung gibt, Model und ViewModel zu kopieren.
        /// </summary>
        /// <typeparam name="ViewModelType">Der Typ (vorzugsweise der Typ der Ableitung dieser Klasse), der als neue ViewModel-Instanz 
        /// fungieren soll, und die Daten aus der Model-Instanz übernimmt.</typeparam>
        /// <typeparam name="modelType">Der Typ der Instanz der Model-Daten-Klasse, aus der die Eigenschaftenwerte übernommen werden.</typeparam>
        /// <param name="model">Die Instanz der Model-Klasse, aus der die Daten übernommen werden.</param>
        /// <returns>Eine neue Instanz der ViewModel-Klasse.</returns>
        /// <remarks>Wenn Eigenschaften in der ViewModel-Klasse vorhanden sind, die im Model nicht existieren, werden diese 
        /// Eigenschaften nicht versucht zu kopieren, und es gibt keine Fehlermeldung. Mithilfe des 
        /// <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes können Sie einen anderen 
        /// Namen für die dem ViewModel entsprechende Eigenschaft im Model bestimmen. Mit des 
        /// <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes können Sie bestimmen, dass
        /// eine Eigenschaft, obwohl sowohl im Model als auch im ViewModel vorhanden, nicht kopiert wird.
        /// </remarks>
        public static ViewModelType FromModel<ViewModelType, modelType>(modelType model) where ViewModelType : MvvmViewModelBase, new()
        {
            ViewModelType viewModelToReturn = new ViewModelType();
            viewModelToReturn.CopyPropertiesFrom(model);
            return viewModelToReturn;
        }

        /// <summary>
        /// Erstellt eine Kopie aus Objekten einer Liste eines Models (im MVVM-Pattern), und überträgt die entsprechenden Eigenschaften 
        /// einer jeden Instanz in Objekte einer Liste eines ViewModels.
        /// </summary>
        /// <typeparam name="ViewModelType">Der Typ des ViewModels, aus dem die neue Liste bestehen soll.</typeparam>
        /// <typeparam name="Modeltype">Der Typ des Models, aus dem die vorhandene Liste besteht.</typeparam>
        /// <param name="modelCollection">Die Auflistung mit den entsprechenden Model-Objekten.</param>
        /// <returns>Liste mit Objekten eines ViewModels aus der Liste der Models.</returns>
        /// <remarks> Diese Methode erstellt eine Liste aus ViewModel-Objekten im MVVM-Pattern auf Basis einer Liste 
        /// mit Model-Objekten. Dabei verwendet diese Funktion die statische Methode <see cref="FromModel(Of ViewModelType, modelType)(modelType)">FromModel</see>.
        /// Wenn Eigenschaften in der ViewModel-Klasse vorhanden sind, die im Model nicht existieren, werden diese 
        /// Eigenschaften nicht versucht zu kopieren, und es gibt keine Fehlermeldung. Mithilfe des 
        /// <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes können Sie einen anderen 
        /// Namen für die dem ViewModel entsprechende Eigenschaft im Model bestimmen. Mit des 
        /// <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes können Sie bestimmen, dass
        /// eine Eigenschaft, obwohl sowohl im Model als auch im ViewModel vorhanden, nicht kopiert wird.
        /// </remarks>
        public static IEnumerable<ViewModelType> FromModelList<ViewModelType, Modeltype>(IEnumerable<Modeltype> modelCollection) where ViewModelType : MvvmViewModelBase, new()
        {
            return
                from modelItem in modelCollection
                select FromModel<ViewModelType, Modeltype>(modelItem);
        }

        /// <summary>
        /// Kopiert die Inhalte der Eigenschaften von einer anderen Instanz dieser Klasse in die aktuelle. 
        /// (Soll insbesondere zur Vereinfachung beim Mvvm-Pattern zur Übernahme von Daten eines Models 
        /// in ein ViewModel verwendet werden).
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Wenn Eigenschaften in der ViewModel-Klasse vorhanden sind, die im Model nicht existieren, werden diese 
        /// Eigenschaften nicht versucht zu kopieren, und es gibt keine Fehlermeldung. Mithilfe des 
        /// <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes können Sie einen anderen 
        /// Namen für die dem ViewModel entsprechende Eigenschaft im Model bestimmen. Mit des 
        /// <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes können Sie bestimmen, dass
        /// eine Eigenschaft, obwohl sowohl im Model als auch im ViewModel vorhanden, nicht kopiert wird.
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
        /// Kopiert die Inhalte der Eigenschaften von einer anderen Instanz dieser Klasse in die aktuelle.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks></remarks>
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

            //INSTANT C# WARNING: Every field in a C# anonymous type initializer is immutable:
            //ORIGINAL LINE: For Each propToPropItem In From propItem In GetType.GetRuntimeProperties Where propItem.GetCustomAttribute(Of ModelPropertyIgnoreAttribute)() Is Nothing Select New With { .ViewModelProperty = propItem, .ModelProperty = model.GetType.GetRuntimeProperty(If(propItem.GetCustomAttribute(Of ModelPropertyNameAttribute)() Is Nothing, propItem.Name, propItem.GetCustomAttribute(Of ModelPropertyNameAttribute).PropertyName))}
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