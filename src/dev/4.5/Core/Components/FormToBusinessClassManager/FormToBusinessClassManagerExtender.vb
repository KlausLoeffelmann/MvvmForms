'*****************************************************************************************
'                              FormsToBusinessClassManagerExtender.vb
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

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Data.Objects
Imports System.Data.Objects.DataClasses
Imports System.ComponentModel

Public Delegate Function ValidationErrorHandlerDelegate(Of BusinessClassType As New)(ByVal BusinessObject As BusinessClassType, ByVal issueList As List(Of String)) As Boolean
Public Delegate Function SaveChangesHandlerDelegate(Of BusinessClassType As New)(ByVal BusinessObject As BusinessClassType, ByVal issueList As List(Of String)) As Boolean

''' <summary>
''' Stellt Extender-Methoden für die Zusammenarbeit mit der 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> und WindowsForms-Formularen bereit.
''' </summary>
''' <remarks></remarks>
Public Module FormToBusinessClassManagerExtender

    ''' <summary>
    ''' Ruft ein Formular auf, das eine FormToBusinessClassManager-Komponente und entsprechende Eingabefelder enthält, 
    ''' um aus den automatisch validierten Benutzereingaben eine neue Business-Objekt-Instanz zu ermitteln.
    ''' </summary>
    ''' <typeparam name="BusinessClassType">Der Typ der Business-Klasse (muss instanziierbar sein), der aus dem Formular erstellt werden soll.</typeparam>
    ''' <param name="BusinessClassForm">Das Formular, dessen Eingabefelderinhalte die Eigenschaften der neuen Business-Klassenobjekt-Instanz definieren.</param>
    ''' <returns>Eine Instanz vom Typ BusinessClassType, </returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function AddNew(Of BusinessClassType As New)(ByVal BusinessClassForm As IFormToBusinessClassManagerHost) As BusinessClassType
        BusinessClassForm.FormToBusinessClassManager.DataSource = New BusinessClassType
        If Not GetType(System.Windows.Forms.Form).IsAssignableFrom(BusinessClassForm.FormToBusinessClassManager.HostingForm.GetType) Then
            Dim up As New TypeMismatchException("The FormToBusinessClassManager component can only be hosted on forms.")
            Throw up
        Else

            Dim frm = DirectCast(BusinessClassForm, System.Windows.Forms.Form)

            AddHandler frm.FormClosing, Sub(sender As Object, e As FormClosingEventArgs)
                                            If e.CloseReason = CloseReason.None Then
                                                Dim ce As New CancelEventArgs
                                                BusinessClassForm.FormToBusinessClassManager.OnBeforeFormValidating(ce)
                                                If ce.Cancel Then
                                                    Exit Sub
                                                End If

                                                Dim res = BusinessClassForm.FormToBusinessClassManager.ValidateForm
                                                If res IsNot Nothing AndAlso res.Count > 0 Then
                                                    Dim ve As New AfterFormValidatedEventArgs(res)
                                                    BusinessClassForm.FormToBusinessClassManager.OnAfterFormValidated(ve)
                                                    e.Cancel = True
                                                End If
                                            End If
                                        End Sub

            Dim dr = frm.ShowDialog()
            If dr = DialogResult.Cancel Then
                Return Nothing
            Else
                BusinessClassForm.FormToBusinessClassManager.AssignFieldsFromNullableControls()
                Return DirectCast(BusinessClassForm.FormToBusinessClassManager.DataSource, BusinessClassType)
            End If
        End If
    End Function

    ''' <summary>
    ''' Ruft ein Formular auf, dass eine FormToBusinessClassManager-Komponente und entsprechende Eingabefelder enthält, die mit den Eigenschaftenwerten der übergebenen Business-Objekt-Instanz befüllt werden, 
    ''' um aus den automatisch validierten Benutzereingaben eine neue Business-Objekt-Instanz zu ermitteln.
    ''' </summary>
    ''' <typeparam name="BusinessClassType"></typeparam>
    ''' <param name="BusinessClassForm"></param>
    ''' <param name="Entity"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function Edit(Of BusinessClassType As New)(ByVal BusinessClassForm As IFormToBusinessClassManagerHost, ByVal Entity As BusinessClassType) As BusinessClassType
        BusinessClassForm.FormToBusinessClassManager.DataSource = Entity
        If Not GetType(System.Windows.Forms.Form).IsAssignableFrom(BusinessClassForm.FormToBusinessClassManager.HostingForm.GetType) Then
            Dim up As New TypeMismatchException("The FormToBusinessClassManager component can only be hosted on forms.")
            Throw up
        Else
            Dim frm = DirectCast(BusinessClassForm, System.Windows.Forms.Form)
            AddHandler frm.Load, Sub(sender As Object, e As EventArgs)
                                     BusinessClassForm.FormToBusinessClassManager.AssignFieldsToNullableControls()
                                 End Sub

            AddHandler frm.FormClosing, Sub(sender As Object, e As FormClosingEventArgs)
                                            If e.CloseReason = CloseReason.None Then
                                                If frm.DialogResult = DialogResult.Cancel Then
                                                    e.Cancel = False
                                                    Return
                                                End If
                                                Dim ce As New CancelEventArgs
                                                BusinessClassForm.FormToBusinessClassManager.OnBeforeFormValidating(ce)
                                                If ce.Cancel Then
                                                    Exit Sub
                                                End If

                                                Dim res = BusinessClassForm.FormToBusinessClassManager.ValidateForm
                                                If res IsNot Nothing AndAlso res.Count > 0 Then
                                                    Dim ve As New AfterFormValidatedEventArgs(res)
                                                    BusinessClassForm.FormToBusinessClassManager.OnAfterFormValidated(ve)
                                                    e.Cancel = True
                                                End If
                                            End If
                                        End Sub

            Dim dr = frm.ShowDialog()
            If dr = DialogResult.Cancel Then
                Return Nothing
            Else
                BusinessClassForm.FormToBusinessClassManager.AssignFieldsFromNullableControls()
                Return DirectCast(BusinessClassForm.FormToBusinessClassManager.DataSource, BusinessClassType)
            End If
        End If
    End Function
End Module
