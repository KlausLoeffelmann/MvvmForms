'*****************************************************************************************
'                                         ReflectionHelper.vb
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

Imports System.Data.Objects.DataClasses
Imports System.Reflection
Imports System.Text
Imports System.Runtime.CompilerServices
Imports System.ComponentModel.Design
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Input
Imports ActiveDevelop.EntitiesFormsLib

Public Module ReflectionHelper

    <Extension()>
    Public Function TypeStringWithGenericParameters(type As Type) As String
        Dim result As New StringBuilder
        result.Append(type.Name)
        Dim genericParameters = type.GetGenericArguments
        If genericParameters IsNot Nothing Then
            For index = 0 To genericParameters.Length - 1
                If index = 0 Then
                    result.Append("<")
                End If
                result.Append(genericParameters(index).TypeStringWithGenericParameters)
                If index < genericParameters.Length - 1 Then
                    result.Append(",")
                Else
                    result.Append(">")
                End If
            Next
        End If
        Return result.ToString

    End Function

    'TODO: This has to be refactored. We need a tree view where we could drill into recursive property pathes.
    ''' <summary>
    ''' Creates a list with all properties and sub properties of the ViewModel (former Business Class Objects) recursively. This function is 
    ''' for creating the property pathes for binding in the MvvmForms PropertyBindings Designer UI.
    ''' </summary>
    ''' <param name="t">The type whose properties are discovered including its sub properties for building property pathes.</param>
    ''' <param name="propRoot">Since this method is called recusively, here is the current property path root where to begin.</param>
    ''' <param name="proplist">The current list of allready discovered property pathes. Pass an empty but instanciated list at the beginning.</param>
    ''' <param name="depthCountLimit">The maximum property path depth, if it should be limitted.</param>
    ''' <param name="excludePropertiesByDefault">For business class objects, if this is set, only those 
    ''' properties are taken into account when they are marked with BusinessPropertyAttribute.IncludeProperty. 
    ''' This is legacy a legacy feature. Don't use.</param>
    ''' <remarks></remarks>
    Public Sub CreateSubPropsAsList(t As Type, propRoot As String,
                                    proplist As List(Of PropertyCheckBoxItemController),
                                    depthCountLimit As Integer, excludePropertiesByDefault As Boolean,
                                    Optional host As IDesignerHost = Nothing)

        Dim isEntityObject = False

        If GetType(EntityObject).IsAssignableFrom(t.GetType) Or GetType(INotifyPropertyChanged).IsAssignableFrom(t.GetType) Then
            isEntityObject = True
        End If

        If depthCountLimit > 10 Then
            If Debugger.IsAttached Then
                Debugger.Break()
            End If

            Throw New OverflowException("Die Verschachtelungstiefe in der Rekursion bei der Ermittlung verschachtelter Eigenschaften wurde zu groß (>10)" & vbNewLine &
                                        "(Info: propRoot war: " & propRoot & ".")
        End If

        For Each propItem As PropertyInfo In t.GetProperties

            'Primitive Eigenschaften sind grundsätzlich OK, es sei denn, das BusinessClassProperty-Attribut steht drüber mit Exclude.
            Dim businessClassAttributeAsList = propItem.GetCustomAttributes(GetType(BusinessClassPropertyAttribute), True)
            If businessClassAttributeAsList IsNot Nothing And businessClassAttributeAsList.Count > 0 Then
                'Attribut war da --> auswerten.
                If Not excludePropertiesByDefault Then
                    'Exclude Property?
                    If (DirectCast(businessClassAttributeAsList(0), BusinessClassPropertyAttribute).Options And
                        BusinessPropertyAttributeOptions.ExcludeProperty) = BusinessPropertyAttributeOptions.ExcludeProperty Then
                        Continue For
                    End If
                Else
                    'Include Property?
                    If Not (DirectCast(businessClassAttributeAsList(0), BusinessClassPropertyAttribute).Options And
                            BusinessPropertyAttributeOptions.IncludeProperty) = BusinessPropertyAttributeOptions.IncludeProperty Then
                        'Entwickler hat gezielt gebeten, die Eigenschaft mit aufzunehmen.
                        Continue For
                    End If
                End If
            End If

            'System-Eigenschaft soll nicht gebunden werden können.
            If propItem.GetCustomAttributes(GetType(MvvmSystemElementAttribute), True).Count > 0 Then
                Continue For
            End If

            Dim tmpPropertyType As Type

            Try
                'Wir versuchen zu ermitteln, verhindern aber, dass das Fehlschlagen
                'zum Abbruch des Gesamten Property-Einsamels führt.
                tmpPropertyType = propItem.PropertyType
            Catch ex As FileNotFoundException
                If Debugger.IsAttached Then
                    Debugger.Break()
                End If

                Continue For
            End Try

#Disable Warning

            If propItem.PropertyType.IsPrimitive Or
                propItem.PropertyType Is GetType(String) Or
                propItem.PropertyType Is GetType(Decimal) Or
                propItem.PropertyType Is GetType(Date) Or
                propItem.PropertyType.IsEnum Then
                proplist.Add(New PropertyCheckBoxItemController(propItem.Name, propItem.PropertyType, propRoot))
            ElseIf propItem.PropertyType.IsGenericType AndAlso propItem.PropertyType.GetGenericTypeDefinition.Equals(GetType(Nullable(Of ))) Then
                'Nullables sind grundsätzlich OK.
                proplist.Add(New PropertyCheckBoxItemController(propItem.Name, propItem.PropertyType, propRoot))
            ElseIf isEntityObject And propItem.GetCustomAttributes(GetType(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), True).Length > 0 Then
                'Skalare Eigenschaften sind ebenfalls grundsätzlich OK.
                proplist.Add(New PropertyCheckBoxItemController(propItem.Name, propItem.PropertyType, propRoot))
            Else
                'Nur Businessklassen oder Entityobjekte, aber keine geschachtelten Entityobjekte (das wären Navigationseigenschaften).
                If (propItem.PropertyType.GetCustomAttributes(GetType(BusinessClassAttribute), True).Length > 0) Or
                    (GetType(INotifyPropertyChanged).IsAssignableFrom(propItem.PropertyType)) Or
                    (propItem.GetCustomAttributes(GetType(MvvmViewModelIncludeAttribute), True).Length > 0) Or
                    (GetType(ICommand).IsAssignableFrom(propItem.PropertyType)) Or
                    ((Not isEntityObject) And (GetType(EntityObject).IsAssignableFrom(propItem.PropertyType))) Then

                    'Hier müssen wir rekursiv ran, aber das Element selbst muss auch in die Liste.
                    proplist.Add(New PropertyCheckBoxItemController(propItem.Name, propItem.PropertyType, propRoot))

                    If Not GetType(IEnumerable).IsAssignableFrom(propItem.PropertyType) Then
                        depthCountLimit += 1
                        CreateSubPropsAsList(propItem.PropertyType, If(Not String.IsNullOrWhiteSpace(propRoot),
                                            propRoot & "." & propItem.Name, propItem.Name),
                                            proplist, depthCountLimit, excludePropertiesByDefault)
                        depthCountLimit -= 1
                    End If
                End If
            End If

#Enable Warning

        Next

    End Sub


    ''' <summary>
    ''' Füllt die Eigenschaften einer Klasseninstanz vom Typ String, die Null (Nothing in VB) sind, mit Leerstring.
    ''' </summary>
    ''' <param name="classInstance"></param>
    ''' <remarks>Diese Funktion ist notwendig zur Abwärtskompatibilität einiger Datenbanken, 
    ''' in denen Strings nur Emtpy aber nicht als Nothing gespeichert werden dürfen.</remarks>
    Public Sub PopulateEmptyStrings(classInstance As Object)
        For Each propItem In classInstance.GetType.GetProperties
            If GetType(String).IsAssignableFrom(propItem.PropertyType) Then
                'Wir haben String, jetzt schauen, ob was drin ist:
                Dim obj = propItem.GetValue(classInstance, Nothing)
                If obj Is Nothing Then
                    propItem.SetValue(classInstance, String.Empty, Nothing)
                End If
            End If
        Next
    End Sub

End Module

''' <summary>
''' Hilfsklasse, für den Aufbau der entsprechenden Anzeige im Selektor des Eigenschaften/Feld-Mappings.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class PropertyCheckBoxItemController

    Sub New(ByVal propertyName As String, ByVal propertyType As Type, ByVal path As String)
        Me.PropertyName = propertyName
        Me.PropertyType = propertyType
        Me.Path = path
    End Sub

    Property PropertyName As String
    Property PropertyType As Type
    Property GroupName As String
    Property Options As BusinessPropertyAttributeOptions
    Property Path As String

    Public Function PropertyFullname() As String
        Return If(Not String.IsNullOrWhiteSpace(Path), Path & ".", "") & PropertyName
    End Function

    Public Overrides Function ToString() As String

        Dim genericInfo As String = ""

        If PropertyType.IsGenericType Then
            For Each item In PropertyType.GetGenericArguments
                genericInfo &= " <" & item.Name & ">"
            Next
        End If
        Return PropertyFullname() & " (" & PropertyType.Name & genericInfo & ")"
    End Function
End Class