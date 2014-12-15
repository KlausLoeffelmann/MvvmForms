Imports System.ComponentModel.Design.Serialization
Imports System.CodeDom
Imports System.ComponentModel.Design

''' <summary>
''' CollectionSerializer fuer die Serialisierung der GridColumnCollection
''' </summary>
''' <remarks></remarks>
Public Class MvvmDataGridColumnsCodeDomSerializer
    Inherits CollectionCodeDomSerializer

    ''' <summary>
    ''' Serialisert die Collection
    ''' </summary>
    ''' <param name="manager"></param>
    ''' <param name="targetExpression"></param>
    ''' <param name="targetType"></param>
    ''' <param name="originalCollection"></param>
    ''' <param name="valuesToSerialize"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function SerializeCollection(manager As IDesignerSerializationManager, targetExpression As CodeDom.CodeExpression, targetType As Type, originalCollection As ICollection, valuesToSerialize As ICollection) As Object
        If (manager Is Nothing) Then
            Throw New ArgumentNullException("manager")
        End If
        If (targetType Is Nothing) Then
            Throw New ArgumentNullException("targetType")
        End If
        If (originalCollection Is Nothing) Then
            Throw New ArgumentNullException("originalCollection")
        End If
        If (valuesToSerialize Is Nothing) Then
            Throw New ArgumentNullException("valuesToSerialize")
        End If

        If originalCollection Is Nothing OrElse
            originalCollection.Count = 0 Then
            Return Nothing
        End If

        Dim codeStatementCollection As CodeStatementCollection = New CodeStatementCollection()
        Try
            Dim columns = DirectCast(valuesToSerialize, GridColumnCollection)
            Dim index As Integer = 0
            Dim serializedColumns As New List(Of String)

            For Each column In columns


                Dim targetproprefex = DirectCast(targetExpression, CodePropertyReferenceExpression)
                Dim dgvName As String = String.Empty

                If TypeOf targetproprefex.TargetObject Is CodeFieldReferenceExpression Then
                    dgvName = DirectCast(targetproprefex.TargetObject, CodeFieldReferenceExpression).FieldName
                ElseIf TypeOf targetproprefex.TargetObject Is CodePropertyReferenceExpression Then
                    dgvName = DirectCast(targetproprefex.TargetObject, CodePropertyReferenceExpression).PropertyName
                ElseIf TypeOf targetproprefex.TargetObject Is CodeVariableReferenceExpression Then
                    dgvName = DirectCast(targetproprefex.TargetObject, CodeVariableReferenceExpression).VariableName
                End If

                'Eindeutigen Namen innerhalb der Collection hollen 
                Dim columnName As String = MyBase.GetUniqueName(manager, column)

                'Evtl. Customnamen setzen
                If Not String.IsNullOrEmpty(column.Name) AndAlso column.Name <> MvvmDataGridColumn.NewColumnName Then
                    If serializedColumns.Contains(dgvName & "_" & column.Name) Then
                        System.Windows.MessageBox.Show("Der Spaltenname einer Column muss eindeutig sein!")
                        column.Name = columnName
                    Else
                        columnName = column.Name
                    End If
                Else
                    'Neue Spalte
                    column.Name = columnName
                End If

                columnName = dgvName & "_" & columnName

                'Column serialiseren
                column.AddSerializedColumn(columnName, codeStatementCollection)

                codeStatementCollection.Add(New CodeMethodInvokeExpression(targetExpression, "Add", New CodeVariableReferenceExpression(columnName)))

                index += 1
                serializedColumns.Add(columnName)
            Next

        Catch ex As InvalidCastException
            'Der Assemblies der EFL wurde neu erstellt. Deswegen sind die Typen nicht mehr itendisch (bei der Serialisierung). Es wird empfohlen das Formular zu schliessen und neu zu laden
            'MessageBox.Show("Der Assemblies der EFL wurde neu erstellt. Deswegen sind die Typen nicht mehr itendisch (bei der Serialisierung). Es wird empfohlen das Formular zu schliessen und neu zu laden.")
        End Try


        Return codeStatementCollection
    End Function

    Public Overrides Function Deserialize(manager As IDesignerSerializationManager, codeObject As Object) As Object
        Try
            Return MyBase.Deserialize(manager, codeObject)
        Catch ex As Exception
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
            Throw
        End Try
    End Function

    Public Overrides Function Serialize(manager As IDesignerSerializationManager, value As Object) As Object

        If value Is Nothing Then
            Return Nothing
        End If

        Dim temp = MyBase.Serialize(manager, value)
        Return temp

    End Function
End Class