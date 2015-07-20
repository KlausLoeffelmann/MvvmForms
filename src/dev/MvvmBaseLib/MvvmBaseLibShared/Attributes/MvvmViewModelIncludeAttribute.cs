using System;

/// <summary>
/// Defines for a complex property (not a value type, not a primitve type), 
/// which does NOT include INotifyPropertyChanged (or BindableBase, MvvmBase), to be 
/// included in the ViewModel's potential bindable property list in the Binding Designer.
/// </summary>
/// <remarks>By default, only properties with types which implement INotifyPropertyChanged are displayed in a 
/// ViewModel's list for potential binding, when editing the PropertyBinding collection of the View's control 
/// at design time. This is a precaution, especially for types which create bindable property paths. If one of 
/// properties in a property path does NOT implement INotifyPropertyChanged, the binding engine would not know 
/// when the property chain becomes broken by assigning Nothing (null in Csharp) to such a property. 
/// The outer part would still be bound, but it would probably not be accessible anymore via the ViewModel. 
/// That's why we have to opt in to such a complex property for having it a candidate for binding.</remarks>
[AttributeUsage(AttributeTargets.Property)]
public class MvvmViewModelIncludeAttribute : Attribute
{

    public MvvmViewModelIncludeAttribute() : base()
    {
    }

}