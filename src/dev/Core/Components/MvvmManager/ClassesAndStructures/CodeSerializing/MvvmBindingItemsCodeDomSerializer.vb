'*****************************************************************************************
'                               MvvmBindingItemsCodeDomSerializer.vb
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
Imports System.ComponentModel.Design.Serialization
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class MvvmBindingItemsCodeDomSerializer
    Inherits CollectionCodeDomSerializer

    Protected Overrides Function SerializeCollection(manager As IDesignerSerializationManager,
                                                     targetExpression As CodeExpression,
                                                     targetType As Type,
                                                     originalCollection As ICollection,
                                                     valuesToSerialize As ICollection) As Object

        If originalCollection Is Nothing OrElse
            originalCollection.Count = 0 Then
            Return Nothing
        End If

        Dim uniqueName As String = MyBase.GetUniqueName(manager, valuesToSerialize)
        Dim className As String = targetType.ToString

        If Debugger.IsAttached Then
            Debugger.Break()
        End If

        Dim ownerAsMvvmManager As IComponent
        Dim isInherited As Boolean

        'Diese Infos können wir zu einem späteren Zeitpunkt eventuell noch gebrauchen.
        'Im Moment nur als "Lernobjekt" und zu Analysezwecken implementiert.
        Dim context As ExpressionContext = TryCast(manager.Context.Item(GetType(ExpressionContext)), ExpressionContext)
        If ((Not context Is Nothing) AndAlso (context.Expression Is targetExpression)) Then
            ownerAsMvvmManager = TryCast(context.Owner, IComponent)
            If (Not ownerAsMvvmManager Is Nothing) Then
                Dim attribute As InheritanceAttribute = DirectCast(TypeDescriptor.GetAttributes(ownerAsMvvmManager).Item(GetType(InheritanceAttribute)), InheritanceAttribute)
                isInherited = ((Not attribute Is Nothing) AndAlso (attribute.InheritanceLevel = InheritanceLevel.Inherited))
            End If
        End If

        Dim codeStatementCollection As CodeStatementCollection = New CodeStatementCollection()

        'Neue Version - Die Property-Items werden direkt zusammengebaut
        Dim currentBindings = DirectCast(valuesToSerialize, MvvmBindingItems)
        Dim count As Integer = 1
        For Each controlItem In currentBindings
            If controlItem.Data IsNot Nothing AndAlso controlItem.Data.PropertyBindings IsNot Nothing Then
                For Each item In controlItem.Data.PropertyBindings

                    Dim addParams As New List(Of CodeExpression)

                    'Control-Objekt als Field erstellen,
                    'wir überlassen es dabei dem Control, sich selbst als Ausdruck zu serialisieren
                    Dim controlAsCodeFieldReferenceExpression As CodeExpression = Nothing
                    If TypeOf controlItem.Control Is IComponent Then
                        controlAsCodeFieldReferenceExpression = MyBase.SerializeToExpression(manager, controlItem.Control)
                    Else
                        Continue For
                    End If

                    'Wenn es kein Control ist, müssen wir es um eine Casting-Anweisung erweitern.
                    If ((Not controlAsCodeFieldReferenceExpression Is Nothing) AndAlso Not GetType(Control).IsAssignableFrom(controlItem.Control.GetType)) Then
                        controlAsCodeFieldReferenceExpression = New CodeCastExpression(GetType(Control), controlAsCodeFieldReferenceExpression)
                    End If

                    'Falls das Steuerelement nicht mehr existiert, da es gelöscht wurde, ignorieren wir diesen Schritt.
                    If controlAsCodeFieldReferenceExpression Is Nothing Then
                        Continue For
                    End If

                    addParams.Add(controlAsCodeFieldReferenceExpression)

                    Dim enumSerializer As New EnumCodeDomSerializer()

                    'BindingSettings-Objekt erstellen, als Integer-Werte, damit Flags-Enums funktionieren.
                    Dim bindSetting = New CodeObjectCreateExpression(GetType(BindingSetting),
                                                                    New CodeExpression() {DirectCast(enumSerializer.Serialize(manager, item.BindingSetting.BindingMode), CodeExpression),
                                                                     DirectCast(enumSerializer.Serialize(manager, item.BindingSetting.UpdateSourceTrigger), CodeExpression)})

                    addParams.Add(bindSetting)

                    Dim t = New BindingProperty()

                    'ControlProperty-Parameter:
                    addParams.Add(New CodeObjectCreateExpression(GetType(BindingProperty), {New CodePrimitiveExpression(item.ControlProperty.PropertyName),
                                                                                    New CodeTypeOfExpression(item.ControlProperty.PropertyType)}))

                    'Converter-Parameter
                    If item.Converter IsNot Nothing Then
                        addParams.Add(New CodeTypeOfExpression(item.Converter))
                    Else
                        addParams.Add(New CodePrimitiveExpression(Nothing))
                    End If

                    'ConverterParameter-Parameter
                    If String.IsNullOrEmpty(item.ConverterParameter) Then
                        addParams.Add(New CodePrimitiveExpression(Nothing))
                    Else
                        addParams.Add(New CodePrimitiveExpression(item.ConverterParameter))
                    End If

                    'ControlProperty-Parameter:
                    addParams.Add(New CodeObjectCreateExpression(GetType(BindingProperty), {New CodePrimitiveExpression(item.ViewModelProperty.PropertyName),
                                                                                    New CodeTypeOfExpression(item.ViewModelProperty.PropertyType)}))

                    Dim AddMethodStatement As New CodeMethodInvokeExpression(targetExpression,
                                                                     "AddPropertyBinding",
                                                                     addParams.ToArray)
                    codeStatementCollection.Add(AddMethodStatement)
                Next
            End If
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


Friend Class EnumCodeDomSerializer
    Inherits CodeDomSerializer

    Public Overrides Function Serialize(manager As IDesignerSerializationManager, value As Object) As Object

        Dim enums As [Enum]() = Nothing
        Dim converter As TypeConverter = TypeDescriptor.GetConverter(value)

        If converter.CanConvertTo(GetType([Enum]())) Then
            enums = DirectCast(converter.ConvertTo(value, GetType([Enum]())), [Enum]())
        Else
            enums = New [Enum]() {DirectCast(value, [Enum])}
        End If

        Dim left As CodeExpression = Nothing
        Dim right As CodeExpression = Nothing

        For Each e As [Enum] In enums
            right = GetEnumExpression(e)
            If left Is Nothing Then
                left = right
            Else
                left = New CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BitwiseOr, right)
            End If
        Next
        Return left
    End Function

    Private Function GetEnumExpression(e As [Enum]) As CodeExpression
        Dim converter As TypeConverter = TypeDescriptor.GetConverter(e)
        If converter IsNot Nothing AndAlso converter.CanConvertTo(GetType(String)) Then
            Return New CodeFieldReferenceExpression(New CodeTypeReferenceExpression(e.[GetType]().FullName), DirectCast(converter.ConvertTo(e, GetType(String)), String))
        Else
            Return Nothing
        End If
    End Function
End Class
