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

    'TODO: Macht das Sinn? - Besprechen!
    <Extension()>
    Public Function Edit(Of BusinessClassType As New)(ByVal ContainerControl As IFormToBusinessClassManagerHost,
                                                      ByVal Entity As BusinessClassType,
                                                      ByVal ValidationErrorHandler As ValidationErrorHandlerDelegate(Of BusinessClassType),
                                                      ByVal SaveChangesHandler As SaveChangesHandlerDelegate(Of BusinessClassType)) As BusinessClassType

    End Function

    'TODO: Macht das Sinn? - Besprechen!
    <Extension()>
    Public Function EditVisually(Of BusinessClassType As _
         {New, EntityObject}, FormType As _
         {System.Windows.Forms.Form, 
             IFormToBusinessClassManagerHost})(ByVal obj As BusinessClassType,
                                           ByVal form As FormType) _
                                                  As BusinessClassType
        Return form.Edit(obj)
    End Function

End Module
