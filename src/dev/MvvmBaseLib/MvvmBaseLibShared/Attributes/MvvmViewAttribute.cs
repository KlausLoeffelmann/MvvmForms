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

    public MvvmViewAttribute(string viewname, string assemblyName)
    {
        this.ViewTypeName = viewname;
        this.Assemblyname = assemblyName;
        this.ContextGuid = Guid.NewGuid();
    }

    public MvvmViewAttribute(string viewName, string assemblyname, string contextGuid)
    {
        this.ViewTypeName = viewName;
        this.Assemblyname = assemblyname;
        this.ContextGuid = new Guid(contextGuid);
    }

    public Guid? ContextGuid { get; set; }
    public string Assemblyname { get; set; }
    public string ViewTypeName { get; set; }
}
