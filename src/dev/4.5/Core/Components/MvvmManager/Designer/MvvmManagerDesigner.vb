Imports System.ComponentModel.Design
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class MvvmManagerDesigner
    Inherits ComponentDesigner

    Private myVerbs As DesignerVerbCollection
    Private myBindingManagerFormVerb As DesignerVerb = Nothing
    Private myCurrentChangeService As IComponentChangeService

    Sub New()
    End Sub

    Public Overrides ReadOnly Property Verbs() As DesignerVerbCollection
        Get

            myCurrentChangeService = TryCast(Me.GetService(GetType(IComponentChangeService)), IComponentChangeService)

            If myVerbs Is Nothing Then
                ' Verbs-Collection erstellen und definieren.
                myVerbs = New DesignerVerbCollection()
                myBindingManagerFormVerb =
                    New DesignerVerb("View-Bindings verwalten...",
                            Sub(sender As Object, e As EventArgs)
                                Dim host As IDesignerHost = TryCast(Me.GetService(GetType(IDesignerHost)), IDesignerHost)

                                If host Is Nothing Then
                                    'Beep()
                                    MessageBox.Show("Konnte DesignerHost nicht ermitteln.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Return
                                End If

                                Dim container As IContainer = TryCast(Me.GetService(GetType(IContainer)), IContainer)

                                If Debugger.IsAttached Then
                                    Debugger.Break()
                                End If
                                Dim frm2show As New frmManageBindings
                                frm2show.ShowDialog(host, container, DirectCast(Me.Component, MvvmManager), myCurrentChangeService)

                            End Sub)

                myVerbs.Add(myBindingManagerFormVerb)
                myBindingManagerFormVerb.Enabled = True
            End If
            Return myVerbs
        End Get
    End Property

End Class
