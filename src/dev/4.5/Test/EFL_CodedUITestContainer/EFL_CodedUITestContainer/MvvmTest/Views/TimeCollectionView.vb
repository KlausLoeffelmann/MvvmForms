Imports ActiveDevelop.EntitiesFormsLib
Imports System.ComponentModel
Imports System.Collections.ObjectModel

Public Class TimeCollectionView

    Private myTimeCollectionViewModel As New TimeCollectionViewModel

    Private Sub TimeCollectionView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Provide Demo-Data.
        If myTimeCollectionViewModel Is Nothing Then
            Return
        End If

        Dim item = New TimeCollectItem With {.StartTime = Now.AddDays(-1).Date.Add(New TimeSpan(8, 10, 0)),
                                                                                    .EndTime = Now.AddDays(-1).Date.Add(New TimeSpan(10, 15, 0)),
                                                                                    .ActivityDescription = "I did something at this time."}

        myTimeCollectionViewModel.TimeCollectionItems.Add(item)

        myTimeCollectionViewModel.TimeCollectionItems.Add(New TimeCollectItem With {.StartTime = Now.AddDays(-1).Date.Add(New TimeSpan(11, 55, 0)),
                                                                                    .EndTime = Now.AddDays(-1).Date.Add(New TimeSpan(14, 32, 0)),
                                                                                    .ActivityDescription = "Something else, I did at this time."})

        myTimeCollectionViewModel.TimeCollectionItems.Add(New TimeCollectItem With {.StartTime = Now.AddDays(-1).Date.Add(New TimeSpan(15, 12, 0)),
                                                                            .EndTime = Now.AddDays(-1).Date.Add(New TimeSpan(17, 32, 0)),
                                                                            .ActivityDescription = "And now for something completely different, I said at this time."})

        myTimeCollectionViewModel.TimeCollectionItems.Add(New TimeCollectItem With {.StartTime = Now.AddDays(0).Date.Add(New TimeSpan(8, 5, 0)),
                                                                            .EndTime = Now.AddDays(-1).Date.Add(New TimeSpan(9, 15, 0)),
                                                                            .ActivityDescription = "I did something at this time."})

        myTimeCollectionViewModel.TimeCollectionItems.Add(New TimeCollectItem With {.StartTime = Now.AddDays(0).Date.Add(New TimeSpan(10, 55, 0)),
                                                                                    .EndTime = Now.AddDays(-1).Date.Add(New TimeSpan(15, 32, 0)),
                                                                                    .ActivityDescription = "Something else, I did at this time."})

        myTimeCollectionViewModel.TimeCollectionItems.Add(New TimeCollectItem With {.StartTime = Now.AddDays(0).Date.Add(New TimeSpan(16, 12, 0)),
                                                                            .EndTime = Now.AddDays(-1).Date.Add(New TimeSpan(18, 55, 0)),
                                                                            .ActivityDescription = "And now for something completely different, I said at this time."})

        Me.MainMvvmManager.DataContext = myTimeCollectionViewModel

    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        myTimeCollectionViewModel.DeleteTimeCollectionItemCommand.Execute(Nothing)
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        myTimeCollectionViewModel.AddTimeCollectionItemCommand.Execute(Nothing)
    End Sub

    Private Sub ChangeButton_Click(sender As Object, e As EventArgs) Handles ChangeButton.Click
        myTimeCollectionViewModel.EditTimeCollectionItemCommand.Execute(Nothing)
    End Sub
End Class
