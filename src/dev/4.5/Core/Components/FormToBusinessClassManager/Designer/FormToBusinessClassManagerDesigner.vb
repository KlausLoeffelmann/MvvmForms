Imports System.ComponentModel.Design
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Reflection
Imports System.Drawing
Imports System.IO
Imports System.Data.Objects.DataClasses

Public Class FormToBusinessClassManagerDesigner
    Inherits ComponentDesigner

    Private myVerbs As DesignerVerbCollection
    Private myBuildVerb As DesignerVerb = Nothing
    Private myModifyVerb As DesignerVerb = Nothing
    Private myRebuildVerb As DesignerVerb = Nothing
    Private myCleanupVerb As DesignerVerb = Nothing

    ' DesignerVerbCollection wird von ComponentDesigner überschrieben.
    ' Ein DesignerVerb ist quasi ein Command im Designer an das entsprechende Steuerelement.
    Public Overrides ReadOnly Property Verbs() As DesignerVerbCollection
        Get
            'If myVerbs Is Nothing Then
            '    ' Verbs-Collection erstellen und definieren.
            '    myVerbs = New DesignerVerbCollection()
            '    myBuildVerb = New DesignerVerb("Formular aus ADO.NET Entities aufbauen...", Sub(sender As Object, e As EventArgs)
            '                                                                                    CreateFormFromEntityObjects(Nothing, True)
            '                                                                                End Sub)
            '    myVerbs.Add(myBuildVerb)

            '    myModifyVerb = New DesignerVerb("Feldauswahl ändern...", Sub(sender As Object, e As EventArgs)
            '                                                                 Dim businessManager = DirectCast(Me.Component, FormToBusinessClassManager)
            '                                                                 CreateFormFromEntityObjects(businessManager.LastSettings, True)
            '                                                             End Sub)
            '    myVerbs.Add(myModifyVerb)

            '    myRebuildVerb = New DesignerVerb("Formular erneut aufbauen", Sub(sender As Object, e As EventArgs)
            '                                                                     Dim businessManager = DirectCast(Me.Component, FormToBusinessClassManager)
            '                                                                     CreateFormFromEntityObjects(businessManager.LastSettings, False)
            '                                                                 End Sub)
            '    myVerbs.Add(myRebuildVerb)

            '    myCleanupVerb = New DesignerVerb("Generierte Steuerelemente entfernen", New EventHandler(AddressOf DeleteFormFormEntityObjects))
            '    myVerbs.Add(myCleanupVerb)
            '    SetVerbDeps()
            'End If
            Return myVerbs
        End Get
    End Property

    Sub New()
    End Sub

    ''' <summary>
    ''' Aktiviert oder Deaktiviert die DesignerVerbs
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetVerbDeps()
        Dim ftbcm = DirectCast(Me.Component, FormToBusinessClassManager)
        Dim lastSettingsExists = ftbcm.LastSettings IsNot Nothing
        'myBuildVerb.Enabled = Not lastSettingsExists
        'myModifyVerb.Enabled = lastSettingsExists
        'myRebuildVerb.Enabled = lastSettingsExists
        'myCleanupVerb.Enabled = lastSettingsExists
        myBuildVerb.Enabled = False
        myModifyVerb.Enabled = False
        myRebuildVerb.Enabled = False
        myCleanupVerb.Enabled = False

    End Sub

    Private Sub DeleteFormFormEntityObjects(ByVal sender As Object, ByVal e As EventArgs)
        Dim HostingForm As Form = Nothing
        Dim host As IDesignerHost = Nothing

        host = TryCast(Me.GetService(GetType(IDesignerHost)), IDesignerHost)

        If host IsNot Nothing Then
            Dim componentHost As IComponent = host.RootComponent
            If TypeOf componentHost Is Form Then
                HostingForm = TryCast(componentHost, Form)
            End If
        End If

        Dim businessManager = DirectCast(Me.Component, FormToBusinessClassManager)

        ' Designer Transaction anlegen, damit nur ein Undo-Item dabei rauskommt
        Using transaction = host.CreateTransaction("Delete Controls")

            ' die Feld-Liste zwischenspeichern
            Dim listOfFields = businessManager.LastSettings.ListOfFields


            DeleteOldControls(host, HostingForm, listOfFields)

            ' die letzte Auswahl der Componenten löschen, damit die Maske neu generiert werden kann
            DirectCast(Me.Component, FormToBusinessClassManager).LastSettings = Nothing
            SetVerbDeps()
            transaction.Commit()
        End Using
    End Sub

    Private Sub DeleteOldControls(ByVal host As IDesignerHost, ByVal form As ContainerControl, ByVal listofFields As List(Of PropertyCheckBoxItemController))
        For Each field In listofFields
            Dim label = GetControl(form, field.PropertyFullname & "Label")
            If label IsNot Nothing Then
                host.DestroyComponent(label)
            End If
            Dim input = GetControl(form, field.PropertyFullname & "Input")
            If input IsNot Nothing Then
                host.DestroyComponent(input)
            End If
        Next
    End Sub

    Private Function GetControl(ByVal root As Control, ByVal controlName As String) As Control
        If root.Name = controlName Then
            Return root
        End If
        If root.Controls IsNot Nothing Then
            For Each child As Control In root.Controls
                Dim ret = GetControl(child, controlName)
                If ret IsNot Nothing Then Return ret
            Next
        End If
        Return Nothing

    End Function

    Private Function GetControlByName(ByVal name As String, ByVal container As ContainerControl, ByRef foundControl As Control, ByVal showErrorIfFails As Boolean) As Boolean
        Dim found = GetControl(container, name)
        If found Is Nothing Then
            If showErrorIfFails Then MessageBox.Show("Das Control mit dem Namen " & name & " konnte nicht gefunden werden.", "Fehler")
            Return False
        End If
        foundControl = found
        Return True
    End Function
End Class

''' <summary>
''' Dient nur als Mapping-Index um bei bestimmten Konstellationen (ID im Namen)
''' anstelle der zugewiesenen Typen das RelationalPopup zu verwenden.
''' </summary>
''' <remarks></remarks>
Public Class ForeignKey
End Class

