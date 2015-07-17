'*****************************************************************************************
'                              PropertyBindingsCodeDomeSerializer.vb
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

Imports System.CodeDom
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization

<Obsolete("We are not using this approach anymore, where trying to generate code for the whole binding structure.")>
Public Class PropertyBindingsCodeDomSerializer
    Inherits CollectionCodeDomSerializer

    Protected Overrides Function SerializeCollection(manager As IDesignerSerializationManager,
                                                     targetExpression As CodeExpression,
                                                     targetType As Type,
                                                     originalCollection As ICollection,
                                                     valuesToSerialize As ICollection) As Object

        Dim addExpression As New CodeMethodReferenceExpression(targetExpression, "Add")

        If originalCollection Is Nothing OrElse
            originalCollection.Count = 0 Then
            Return Nothing
        End If

        If Debugger.IsAttached Then
            Debugger.Break()
        End If
        Dim uniqueName As String = MyBase.GetUniqueName(manager, valuesToSerialize)
        Dim className As String = targetType.ToString

        Dim codeStatementCollection As CodeStatementCollection = New CodeStatementCollection()

        'Neue Version - Die Property-Items werden direkt zusammengebaut
        Dim currentBindings = DirectCast(valuesToSerialize, PropertyBindings)
        Dim count As Integer = 1

        Dim context As ExpressionContext = TryCast(manager.Context.Item(GetType(ExpressionContext)), ExpressionContext)

        For Each itemToSerialize In currentBindings
            Dim addParams As New List(Of CodeExpression)

            'BindingSettings-Objekt erstellen
            Dim bindSetting = New CodeObjectCreateExpression(GetType(BindingSetting),
                                                                    New CodeExpression() {New CodePropertyReferenceExpression(New CodeVariableReferenceExpression("ActiveDevelop.EntitiesFormsLib.MvvmBindingModes"), itemToSerialize.BindingSetting.BindingMode.ToString()),
                                                                                          New CodePropertyReferenceExpression(New CodeVariableReferenceExpression("ActiveDevelop.EntitiesFormsLib.UpdateSourceTriggerSettings"), itemToSerialize.BindingSetting.UpdateSourceTrigger.ToString())})
            addParams.Add(bindSetting)

            'ControlProperty-Parameter:
            addParams.Add(New CodeObjectCreateExpression(GetType(BindingProperty), {New CodePrimitiveExpression(itemToSerialize.ControlProperty.PropertyName),
                                                                                    New CodeTypeOfExpression(itemToSerialize.ControlProperty.PropertyType)}))

            'Converter-Parameter
            If itemToSerialize.Converter IsNot Nothing Then
                addParams.Add(New CodeTypeOfExpression(itemToSerialize.Converter))
            Else
                addParams.Add(New CodePrimitiveExpression(Nothing))
            End If

            'ConverterParameter-Parameter
            If String.IsNullOrEmpty(itemToSerialize.ConverterParameter) Then
                addParams.Add(New CodePrimitiveExpression(Nothing))
            Else
                addParams.Add(New CodePrimitiveExpression(itemToSerialize.ConverterParameter))
            End If

            'ControlProperty-Parameter:
            addParams.Add(New CodeObjectCreateExpression(GetType(BindingProperty), {New CodePrimitiveExpression(itemToSerialize.ViewModelProperty.PropertyName),
                                                                                    New CodeTypeOfExpression(itemToSerialize.ViewModelProperty.PropertyType)}))

            Dim AddMethodStatement As New CodeMethodInvokeExpression(targetExpression,
                                                                     "Add",
                                                                     addParams.ToArray)
            codeStatementCollection.Add(AddMethodStatement)
        Next
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
