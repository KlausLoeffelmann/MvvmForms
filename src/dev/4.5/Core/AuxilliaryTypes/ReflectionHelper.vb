Imports System.Data.Objects.DataClasses
Imports System.Reflection
Imports System.Text
Imports System.Runtime.CompilerServices
Imports System.ComponentModel.Design
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Input
Imports ActiveDevelop.EntitiesFormsLib
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

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

    ''' <summary>
    ''' Erstellt eine Liste mit allen Eigenschaften und Untereigenschaften des Business-Class-Objektes (rekusiv),
    ''' die dann in den entsprechenden Klapplisten zur Verfügung gestellt werden.
    ''' </summary>
    ''' <param name="t"></param>
    ''' <param name="propRoot"></param>
    ''' <param name="proplist"></param>
    ''' <param name="depthCount"></param>
    ''' <remarks></remarks>
    Friend Sub CreateSubPropsAsList(t As Type, propRoot As String, proplist As List(Of PropertyCheckBoxItemController),
                                    depthCount As Integer, excludePropertiesByDefault As Boolean,
                                    Optional host As IDesignerHost = Nothing)


        Dim isEntityObject = False

        If GetType(EntityObject).IsAssignableFrom(t.GetType) Or GetType(INotifyPropertyChanged).IsAssignableFrom(t.GetType) Then
            isEntityObject = True
        End If

        If depthCount > 10 Then
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
                        depthCount += 1
                        CreateSubPropsAsList(propItem.PropertyType, If(Not String.IsNullOrWhiteSpace(propRoot),
                                            propRoot & "." & propItem.Name, propItem.Name),
                                            proplist, depthCount, excludePropertiesByDefault)
                        depthCount -= 1
                    End If
                End If
            End If

        Next

    End Sub

    ' ''' <summary>
    ' ''' Ermittelt alle referenzierten DLLs eines gehosteten Controls auf Basis seines Designers und mögliche Container-Ziele im Formular in einem Rutsch.
    ' ''' </summary>
    ' ''' <param name="host">Der Designer-Host.</param>
    ' ''' <param name="hostingForm">Form oder ContainerControl, auf dem das Steuerelement liegt.</param>
    ' ''' <param name="referencedAssemblies">Eine instanzierte Assembly-Liste, die die referenzierten Assemblies aufnimmt.</param>
    ' ''' <param name="possibleTargets">Die Möglichen Container-Ziele Auf dem Formular. (Wird nur ermittelt, wenn eine instanziierte, leere Liste mit übergeben wird.)</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function RetrieveRefAssembliesAndEligibleContainerControls(ByRef host As IDesignerHost, refService As IReferenceService, loaderHelper As IDesignTimeAssemblyLoader, ByRef hostingForm As ContainerControl,
    '                             ByRef referencedAssemblies As List(Of Assembly),
    '                             ByRef possibleTargets As List(Of Control)) As Boolean

    '    If host IsNot Nothing Then
    '        Dim componentHost As IComponent = host.RootComponent
    '        If TypeOf componentHost Is Form Then
    '            hostingForm = TryCast(componentHost, Form)
    '        ElseIf TypeOf componentHost Is UserControl Then
    '            hostingForm = TryCast(componentHost, UserControl)
    '        End If
    '    Else
    '        If Debugger.IsAttached Then
    '            Debugger.Break()
    '        End If
    '    End If

    '    Dim t As Type = Nothing
    '    Try
    '        t = host.GetType(host.RootComponentClassName)
    '    Catch ex As Exception
    '    End Try

    '    Dim useAssembly As Assembly

    '    If t Is Nothing Then
    '        'If Debugger.IsAttached Then
    '        '    Debugger.Break()
    '        'End If
    '    Else
    '        If Debugger.IsAttached Then
    '            Debugger.Break()
    '        End If

    '        '
    '        For Each item In refService.GetReferences
    '            Dim zzz = item
    '            Console.WriteLine(zzz)
    '        Next

    '        'loaderHelper.LoadRuntimeAssembly()
    '        useAssembly = t.Assembly
    '        referencedAssemblies.Add(useAssembly)
    '        ' erst einmal die eigentliche Assembly mit dem gerade bearbeiteten Control hinzufügen
    '        ' und nun alle referenzierten Assemblies

    '        For Each assName As AssemblyName In useAssembly.GetReferencedAssemblies()
    '            Dim loadedAss As Assembly
    '            Try
    '                loadedAss = Assembly.Load(assName)
    '                'Dim tmp = Assembly.ReflectionOnlyLoad(assName.FullName)
    '                referencedAssemblies.Add(loadedAss)
    '            Catch ex As Exception
    '                Try
    '                    Dim tmp = Assembly.ReflectionOnlyLoad(assName.FullName)
    '                    referencedAssemblies.Add(tmp)
    '                Catch ex2 As Exception
    '                End Try
    '            End Try
    '        Next
    '    End If

    '    ' und nun alle möglichen Ziele für die Generierung ermitteln
    '    If possibleTargets IsNot Nothing Then
    '        For Each Item As Component In host.Container.Components
    '            'TODO: die Filterung ggf als Lambda implementieren
    '            If (TryCast(Item, Form) IsNot Nothing OrElse TryCast(Item, Panel) IsNot Nothing OrElse
    '                    TryCast(Item, TabPage) IsNot Nothing OrElse TryCast(Item, GroupBox) IsNot Nothing) Then
    '                possibleTargets.Add(CType(Item, Control))
    '            End If
    '        Next
    '    End If

    '    Return True
    'End Function

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