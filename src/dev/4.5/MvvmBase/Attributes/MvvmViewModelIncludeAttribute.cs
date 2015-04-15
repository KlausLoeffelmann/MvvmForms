using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// Bestimmt beim Setzen über einer komplexe Eigenschaft, dass die entsprechende Eigenschaft zur Auswahl im ViewModel berücksichtigt werden soll.
/// </summary>
/// <remarks>Standardmäßig werden innerhalb eines ViewModels nur Eigenschaften als bindbare Datenquellen berücksichtigt, denen 
/// entweder primitive Datentypen zugrunde liegen, oder die selbst einem Datentypen entsprechend, der seinerseits mit dem 
/// MvvmViewModelAttribute gekennzeichnet ist. In einigen Fällen ist es einfacher, eine komplexe Eigenschaft direkt mit in ein 
/// ViewModel aufzunehmen. In diesem Fall wird die entsprechende Eigenschaft mit diesem Attribut gekennzeichnet.</remarks>
[AttributeUsage(AttributeTargets.Property), Obsolete("Dieses Attribut ist obsolet, da nunmehr alle komplexen Eigenschaften eines ViewModels berücksichtigt werden.")]
public class MvvmViewModelIncludeAttribute : Attribute
{

    public MvvmViewModelIncludeAttribute() : base()
    {
    }

}