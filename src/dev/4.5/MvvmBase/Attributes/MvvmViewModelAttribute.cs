using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// Bestimmt für ein ViewModel die standardmäßig für Windows Forms zu verwendende View.
/// </summary>
/// <remarks></remarks>
public sealed class MvvmViewModelAttribute : Attribute
{
    /// <summary>
    /// Erstellt eine neue Instanz dieser Attribut-Klasse, die eine Klasse als ViewModel kennzeichnet.
    /// </summary>
    /// <remarks></remarks>
    public MvvmViewModelAttribute() : base()
    {
    }

}
