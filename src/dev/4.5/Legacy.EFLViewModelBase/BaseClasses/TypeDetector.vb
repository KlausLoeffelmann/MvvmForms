Imports System.Threading.Tasks
Imports System.Runtime.CompilerServices
Imports System.Reflection

Public Class TypeDetector

    Private myObjectToExamine As Object
    Private myTypes As List(Of Type)

    Private Sub New(objToTypeDetect As Object)
        myObjectToExamine = objToTypeDetect
    End Sub

    Private Sub FindTypes(t As Type)
        For Each propItem In myObjectToExamine.GetType.GetRuntimeProperties
            If propItem.PropertyType.GetTypeInfo.IsPrimitive Then
                Continue For
            End If

            If myTypes.Contains(propItem.PropertyType) Then
                Continue For
            Else
                myTypes.Add(propItem.PropertyType)
                FindTypes(propItem.PropertyType)
            End If
        Next
    End Sub

    Public Shared Function GetTypes(obj As Object) As IEnumerable(Of Type)

        If obj.GetType.GetTypeInfo.IsPrimitive Then
            Return New List(Of Type) From {obj.GetType}
        End If

        Dim td As New TypeDetector(obj)
        td.myTypes = New List(Of Type) From {obj.GetType}
        td.FindTypes(obj.GetType)
        Return td.myTypes

    End Function
End Class
