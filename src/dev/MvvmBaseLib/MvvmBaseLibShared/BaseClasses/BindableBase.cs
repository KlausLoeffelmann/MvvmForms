using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ActiveDevelop.MvvmBaseLib.Mvvm;
using Newtonsoft.Json;

namespace ActiveDevelop.MvvmBaseLib
{
        /// <summary>
        /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
        /// </summary>
        [DataContract]
        public abstract class BindableBase : INotifyPropertyChanged
        {
            private bool myIsPropertyChangeNotificationSuspended;
            private bool myIsPropertyChangingSuspended;

            public BindableBase() : base()
            {
            }

            /// <summary>
            /// Multicast event for property change notifications.
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Checks if a property already matches a desired value.  Sets the property and notifies
            /// listeners only when necessary.
            /// </summary>
            /// <typeparam name="T">Type of the property.</typeparam>
            /// <param name="storage">Reference to a property with both getter and setter.</param>
            /// <param name="value">Desired value for the property.</param>
            /// <param name="propertyName">Name of the property used to notify listeners.  This value
            /// is optional and can be provided automatically when invoked from compilers that support
            /// CallerMemberName.</param>
            /// <returns>True if the value was changed, false if the existing value matched the
            /// desired value.</returns>
            /// <remarks>
            /// <para>The setting of the properties will only be performed, if the new value does 
            /// not equal the old one. Property changes can also globally for the class instance be suspended, 
            /// by calling the method <see cref="SuspendPropertyChanges">SuspendPropertyChanges</see> which causes the 
            /// <see cref="IsPropertyChangingSuspended">IsPropertyChangingSuspended</see> property to be set.</para>
            /// <para>For the Windows Forms MvvmManager component, this is implicitely done 
            /// when assigning the ViewModel to the DataContext property of the source 
            /// so dependent properties won't overwrite their initial values on assignment. (e.g. ViewModel sets DataSource 
            /// of a List Control in the View, Control of View sets SelectedItem to nothing, corresponding property in 
            /// ViewModel becomes nothing, desired Item in Control will not be selected.)</para>
            /// <para>Call the method <see cref="ResumePropertyChanges">ResumePropertyChanges</see>
            /// to resume property changes. For the Windows Forms MvvmManager component, this is automatically done for the ViewModel, 
            /// after it has been assigned to the View's DataContext. 
            /// If you want to change this default behavior, overwrite the methods <see cref="SuspendPropertyChanges">SuspendPropertyChanges</see> and 
            /// <see cref="ResumePropertyChanges">ResumePropertyChanges</see>.</para>
            /// <para>PropertyChange notifications can be suspended by calling the 
            /// method <see cref="SuspendPropertyChangeNotification">"SuspendPropertyChangeNotification"</see> and can be resumed by calling 
            /// <see cref="ResumePropertyChangeNotification">ResumePropertyChangeNotification</see></para>.</remarks>
            protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null, Func<bool> actionOnValidate = null)
            {

                if (IsPropertyChangingSuspended)
                {
                    return false;
                }
                if (object.Equals(storage, value))
                {
                    return false;
                }

                //Aufrufen, wenn nicht null.
                bool dontChangeValue = false;

                if (actionOnValidate != null)
                {
                    dontChangeValue = actionOnValidate();
                }

                if (!dontChangeValue)
                {
                    storage = value;
                    if (!IsPropertyChangeNotificationSuspended)
                    {
                        this.OnPropertyChanged(propertyName);
                    }
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Notifies listeners that a property value has changed.
            /// </summary>
            /// <param name="propertyName">Name of the property used to notify listeners.  This value
            /// is optional and can be provided automatically when invoked from compilers that support
            /// <see cref="CallerMemberNameAttribute"/>.</param>
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            /// <summary>
            /// Bestimmt oder ermittelt, ob das Ändern von Properties derzeit gestattet ist.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            [MvvmSystemElement, XmlIgnore, JsonIgnore]
            public bool IsPropertyChangingSuspended
            {
                get
                {
                    return myIsPropertyChangingSuspended;
                }
                private set
                {
                    //At this point we MUSN'T use base.SetProperty, since it wouldn't work if suspended! 
                    //Nasty little trap!!
                    if (!(object.Equals(value, myIsPropertyChangingSuspended)))
                    {
                        myIsPropertyChangingSuspended = value;
                        OnPropertyChanged("IsPropertyChangingSuspended");
                    }
                }
            }

            /// <summary>
            /// Unterdrückt die PropertyChange-Benachrichtigung durch NotifyPropertyChanged.
            /// </summary>
            /// <remarks></remarks>
            public void SuspendPropertyChangeNotification()
            {
                if (IsPropertyChangeNotificationSuspended)
                {
                    throw new ArgumentException("Suspension of PropertyChangeNotifications is already in affect.");
                }
                IsPropertyChangeNotificationSuspended = true;
            }

            /// <summary>
            /// Stellt die PropertyChange-Benachrichtigung wieder her.
            /// </summary>
            /// <remarks></remarks>
            public void ResumePropertyChangeNotification()
            {
                if (!IsPropertyChangeNotificationSuspended)
                {
                    throw new ArgumentException("Suspension of PropertyChangeNotifications is not in affect.");
                }
                IsPropertyChangeNotificationSuspended = false;
            }

            /// <summary>
            /// Bestimmt oder ermittelt, ob die PropertyChange-Benachrichtung aktiviert ist.
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            [MvvmSystemElement, XmlIgnore, JsonIgnore]
            public bool IsPropertyChangeNotificationSuspended
            {
                get
                {
                    return myIsPropertyChangeNotificationSuspended;
                }
                private set
                {
                    this.SetProperty(ref myIsPropertyChangeNotificationSuspended, value);
                }
            }

            /// <summary>
            /// Unterdrückt das Ändern von Properties.
            /// </summary>
            /// <remarks></remarks>
            public void SuspendPropertyChanges()
            {
                if (IsPropertyChangingSuspended)
                {
                    throw new ArgumentException("Suspension of Property Changes is already in affect.");
                }
                IsPropertyChangingSuspended = true;
            }

        /// <summary>
        /// Lässt das Ändern von Properties wieder zu.
        /// </summary>
        /// <remarks></remarks>
        public void ResumePropertyChanges()
        {
            if (!IsPropertyChangingSuspended)
            {
                throw new ArgumentException("Suspension of Property Changes is not in affect.");
            }
            IsPropertyChangingSuspended = false;
        }
    }
}