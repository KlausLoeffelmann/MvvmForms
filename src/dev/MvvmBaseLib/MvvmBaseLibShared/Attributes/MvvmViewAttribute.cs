using System;
using System.Reflection;

/// <summary>
/// Kennzeichnet eine Klasse als MVVM-View und damit, 
/// welches Form/UserControl für ein bestimmtes ViewModel implizit verwendet werden soll.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MvvmViewAttribute : Attribute
{

    /// <summary>
    /// Erstellt eine neue Instanz dieser Klasse.
    /// </summary>
    /// <param name="viewTypeName">Typ der View. In Abhängigkeit von der verwendeten UI-Technologie kann das beispielsweise 
    /// der vollqualifizierte Name des Typs einer Fenster-Klasse sein.</param>
    /// <remarks></remarks>
    public MvvmViewAttribute(string viewTypeName)
    {
        this.ViewTypeName = viewTypeName;
        this.Assemblyname = this.GetType().GetTypeInfo().AssemblyQualifiedName;
        this.ContextGuid = Guid.NewGuid();
    }

    /// <summary>
    /// Creates a new instance of this class and passed ViewName and AssemblyName.
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="assemblyName"></param>
    public MvvmViewAttribute(string viewName, string assemblyName)
    {
        this.ViewTypeName = viewName;
        this.Assemblyname = assemblyName;
        this.ContextGuid = Guid.NewGuid();
    }

    /// <summary>
    /// Creates a new instance of this class, and passed ViewName, AssemblyName and ContextGuid.
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="assemblyname"></param>
    /// <param name="contextGuid"></param>
    public MvvmViewAttribute(string viewName, string assemblyname, string contextGuid)
    {
        this.ViewTypeName = viewName;
        this.Assemblyname = assemblyname;
        this.ContextGuid = new Guid(contextGuid);
    }

    /// <summary>
    /// The ContextGuid.
    /// </summary>
    public Guid? ContextGuid { get; set; }

    /// <summary>
    /// The name of the Assembly.
    /// </summary>
    public string Assemblyname { get; set; }

    /// <summary>
    /// The name of the View's Type to which these assignment applies.
    /// </summary>
    public string ViewTypeName { get; set; }
}
