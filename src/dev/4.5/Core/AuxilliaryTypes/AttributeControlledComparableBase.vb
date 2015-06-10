'*****************************************************************************************
'                                AttributeControlledComparableBase
'                                =================================
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

Imports System.Linq.Expressions
Imports System.Text

Public Class IndicatorAttribute
    Inherits Attribute

End Class

Public Class EqualityIndicatorAttribute
    Inherits IndicatorAttribute

End Class

Public Class DisplayIndicatorAttribute
    Inherits IndicatorAttribute

    ''' <summary>
    ''' Definiert die Ausgabesteuerung für ToString oder bei der Darstellung in Tabellen oder Listen.
    ''' </summary>
    ''' <param name="order"></param>
    ''' <param name="formatString"></param>
    ''' <param name="seperatorString"></param>
    ''' <param name="SuppresInToString"></param>
    ''' <param name="schemaName"></param>
    ''' <remarks></remarks>
    Sub New(order As Integer, Optional formatString As String = "", Optional seperatorString As String = "",
            Optional SuppresInToString As Boolean = False, Optional schemaName As String = "")
        Me.Order = order
        Me.FormatString = formatString
        Me.SeperatorString = seperatorString
        Me.SchemaName = schemaName
    End Sub

    Public Property Order As Integer
    Public Property FormatString As String
    Public Property SeperatorString As String
    Public Property SchemaName As String
End Class

''' <summary>
''' Basisklasse, für durch Attribute gesteuerte Equal-Implementierung und ToString-Überschreibungen.
''' </summary>
''' <remarks></remarks>
Public Class AttributeControlledComparableBase

    'Diese Klasse verwenden wir, mit unterschiedlichen Rückgabetypen, die über den Typparameter definiert werden, 
    'zweimal. Einmal für die Equlity-Property-Funktionsliste (Boolean) und ein weiteres Mal
    'für die Display-Property-Funktionsliste (String).
    Private Class IndicatorFunctionDelegate(Of ReturnType)
        Property IndicatorAttribute As IndicatorAttribute
        Property FunctionDelegate As Func(Of Object, Object, ReturnType)
    End Class

    Private Shared myTypedGroupedEqulityProperties As Dictionary(Of Type, List(Of IndicatorFunctionDelegate(Of Boolean)))
    Private Shared myTypedGroupedDisplayProperties As Dictionary(Of Type, List(Of IndicatorFunctionDelegate(Of String)))

    Private ReadOnly myEqulityProperties As List(Of IndicatorFunctionDelegate(Of Boolean))
    Private ReadOnly myDisplayProperties As List(Of IndicatorFunctionDelegate(Of String))

    Shared Sub New()
        'Wir müssen das SyncLocken, da es hier Racing Conditions geben kann,
        'wenn der Typ mehr oder weniger gleichzeitig in mehreren Tasks/Threads 
        'das erste Mal verwendet wird.
        SyncLock GetType(AttributeControlledComparableBase)
            myTypedGroupedEqulityProperties = New Dictionary(Of Type, List(Of IndicatorFunctionDelegate(Of Boolean)))
            myTypedGroupedDisplayProperties = New Dictionary(Of Type, List(Of IndicatorFunctionDelegate(Of String)))
        End SyncLock
    End Sub

    Sub New()

        SyncLock GetType(AttributeControlledComparableBase)
            'Build List of Properties to Compare.
            'Es reicht an dieser Stelle, das Vorhandensein nur einer Liste abzufragen, denn entweder es werden
            'bede (Equality UND ToString) oder keine der statischen Liste hinzugefügt.
            If Not myTypedGroupedEqulityProperties.ContainsKey(Me.GetType) Then
                'Liste mit Eigenschaften erstellen, über die Ableitungen von IndicatorAttribute stehen.
                Dim propsWithAtts = (From propItem In Me.GetType.GetProperties().AsParallel
                                    Let att = (From attItem In propItem.GetCustomAttributes(True)
                                               Where TryCast(attItem, IndicatorAttribute) IsNot Nothing
                                               Select DirectCast(attItem, IndicatorAttribute)).ToList
                                    Where att IsNot Nothing
                                    Select propItem, att).ToList

                'Aus den PropertyInfos mit Lambdas erst Expressions machen und die dann zu Func-Delegates kompilieren.
                Dim equlityExpressionList As New List(Of IndicatorFunctionDelegate(Of Boolean))
                Dim toStringExpressionList As New List(Of IndicatorFunctionDelegate(Of String))

                'Erst die Liste der Equlity-Funktions-Delegaten
                For Each propInfoItem In From propWithAttsitem In propsWithAtts
                                         Let indAtt = (From attItem In propWithAttsitem.att
                                                      Where GetType(EqualityIndicatorAttribute).IsAssignableFrom(attItem.GetType)).SingleOrDefault
                                         Where indAtt IsNot Nothing
                                         Select propWithAttsitem.propItem, eqAtt = DirectCast(indAtt, EqualityIndicatorAttribute)

                    Dim tmpExpression As Expression(Of Func(Of Object, Object, Boolean)) =
                                Function(obj1 As Object, Obj2 As Object) Object.Equals(propInfoItem.propItem.GetValue(obj1, Nothing),
                                                                        propInfoItem.propItem.GetValue(obj2, Nothing))
                    equlityExpressionList.Add(New IndicatorFunctionDelegate(Of Boolean) With {
                                                .FunctionDelegate = tmpExpression.Compile(),
                                                .IndicatorAttribute = propInfoItem.eqAtt})
                Next

                'Erst die Liste der ToString-Funktions-Delegaten
                For Each propInfoItem In From propWithAttsitem In propsWithAtts
                                         Let indAtt = (From attItem In propWithAttsitem.att
                                                      Where GetType(DisplayIndicatorAttribute).IsAssignableFrom(attItem.GetType)).SingleOrDefault
                                         Where indAtt IsNot Nothing
                                         Select propWithAttsitem.propItem, toStringAtt = DirectCast(indAtt, DisplayIndicatorAttribute)
                                         Order By toStringAtt.Order

                    'Das hier ist unsere eigentliche ToString-Funktion für eine Property.
                    Dim tmpExpression As Expression(Of Func(Of Object, Object, String)) = Function(obj1 As Object, obj2 As Object) _
                                                                                      If(propInfoItem.propItem.GetValue(obj1, Nothing) Is Nothing, "* - N/A - *",
                                                                                        String.Format("{0:" & propInfoItem.toStringAtt.FormatString & "}" &
                                                                                                                        propInfoItem.toStringAtt.SeperatorString,
                                                                                                      propInfoItem.propItem.GetValue(obj1, Nothing)))

                    'Die fügen wir der Expressliste hinzu.
                    toStringExpressionList.Add(New IndicatorFunctionDelegate(Of String) With {
                                                .FunctionDelegate = tmpExpression.Compile(),
                                                .IndicatorAttribute = propInfoItem.toStringAtt})
                Next


                myTypedGroupedEqulityProperties.Add(Me.GetType, equlityExpressionList)
                myTypedGroupedDisplayProperties.Add(Me.GetType, toStringExpressionList)

            End If

            myEqulityProperties = myTypedGroupedEqulityProperties(Me.GetType)
            myDisplayProperties = myTypedGroupedDisplayProperties(Me.GetType)
        End SyncLock
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean

        If obj Is Nothing Then
            Return False
        End If

        'Gleicher oder kompatible Typ?
        If Me.GetType.IsAssignableFrom(obj.GetType) Then
            Dim flag = True
            For Each functionDelegateItem In myEqulityProperties
                flag = flag And functionDelegateItem.FunctionDelegate(obj, Me)
                If Not flag Then
                    Return False
                End If
            Next
            Return True
        Else
            Return MyBase.Equals(obj)
        End If
    End Function

    Public Overrides Function ToString() As String

        If myDisplayProperties IsNot Nothing Then
            Dim sb As New StringBuilder
            For Each functionDelegateItem In myDisplayProperties
                sb.Append(functionDelegateItem.FunctionDelegate(Me, Nothing))
            Next
            Return sb.ToString
        Else
            Return MyBase.ToString
        End If
    End Function

End Class

'Sample:
Public Class BusinessTestObject
    Inherits AttributeControlledComparableBase

    <EqualityIndicator, DisplayIndicatorAttribute(2, seperatorstring:=" (")>
    Public Property Vorname As String

    <EqualityIndicator, DisplayIndicatorAttribute(1, seperatorstring:=", ")>
    Public Property Nachname As String

    <EqualityIndicator, DisplayIndicatorAttribute(3, "dd/MM/yyyy", ")")>
    Public Property Geburtsdatum As Date?

    Public Property PLZ As String
    Public Property Ort As String

End Class
