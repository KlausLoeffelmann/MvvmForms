Imports ActiveDevelop.EntitiesFormsLib

Public Class AddEditTimeCollectionItemView
    Implements IWinFormsMvvmView

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Public Function GetMvvmController() As MvvmManager Implements IWinFormsMvvmView.GetMvvmController
        Return MvvmManagerMain
    End Function

    Private Sub DirtyStateManager1_IsDirtyChanged(sender As Object, e As IsDirtyChangedEventArgs) Handles DirtyStateManager1.IsDirtyChanged
        If DirtyStateManager1.IsDirty = True Then
            SaveChangesStatusLabel.Enabled = True
        Else
            SaveChangesStatusLabel.Enabled = False
        End If
    End Sub
End Class
