'*****************************************************************************************
'                                  IsFormDirtyChangedEventArgs.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'                    Copyright -2015 by Klaus Loeffelmann
'
'    This program is free software; you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation; either version 2 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty Of
'    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License For more details.
'
'    You should have received a copy of the GNU General Public License along
'    with this program; if not, write to the Free Software Foundation, Inc.,
'    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
'
'    MvvmForms is dual licenced. A permissive licence can be obtained - CONTACT INFO:
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Imports System.Data.Objects

''' <summary>
''' Beinhaltet das Steuerelement, das dazu geführt hat, dass eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> 
''' das <see cref="EntitiesFormsLib.FormToBusinessClassManager.IsFormDirtyChanged">IsFormDirtyChanged-Ereignis</see> ausgelöst hat.
''' </summary>
''' <remarks>Das <see cref="EntitiesFormsLib.FormToBusinessClassManager.IsFormDirtyChanged">IsFormDirtyChanged-Ereignis</see> wird dann ausgelöst, wenn 
''' der Inhalt eines der Eingabefelder eines Formulars, das durch die 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet wird, vom Anwender verändert wurde, sodass 
''' der Entwickler entsprechende Maßnahmen ergreifen kann, um dem Anwender anzuzeigen, dass Änderungen am Formular noch gespeichert werden müssen.</remarks>
Public Class IsFormDirtyChangedEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse und bestimmt das Steuerelement, das zum Auslösen des Ereignisses geführt hat.
    ''' </summary>
    ''' <param name="causingControl"></param>
    ''' <remarks></remarks>
    Sub New(causingControl As Object)
        Me.CausingControl = causingControl
    End Sub

    ''' <summary>
    ''' Steuerelement, das zum Auslösen des Ereignisses geführt hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property CausingControl As Object

End Class

''' <summary>
''' Wird für das Ermitteln des Ziel-Objektkontextes verwendet, wenn eine 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> 
''' das <see cref="EntitiesFormsLib.FormToBusinessClassManager.GetTargetObjectContext">IsFormDirtyChanged-Ereignis</see> ausgelöst hat.
''' </summary>
Public Class GetTargetObjectContextEventArgs
    Inherits EventArgs

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        TargetObjectContext = Nothing
    End Sub

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse und definiert den Objekt-Context, der für das Schreiben in die Zielentität erfordelich ist.
    ''' </summary>
    ''' <param name="targetObjectContext"></param>
    ''' <remarks></remarks>
    Sub New(targetObjectContext As ObjectContext)
        Me.TargetObjectContext = targetObjectContext
    End Sub

    ''' <summary>
    ''' Steuerelement, das zum Auslösen des Ereignisses geführt hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TargetObjectContext As ObjectContext

End Class
