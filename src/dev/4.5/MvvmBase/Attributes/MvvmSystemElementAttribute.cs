using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Defines a property as infrastructure or as a system property, so it is not showing up 
/// for the developer to pick in certain Designer Dialogs.
/// </summary>
public class MvvmSystemElementAttribute : Attribute
{

    public MvvmSystemElementAttribute() : base()
    {
    }

}