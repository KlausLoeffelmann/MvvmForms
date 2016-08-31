Imports System.Collections.ObjectModel
Imports ActiveDevelop.EntitiesFormsLib
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class MainNodeTestViewModel
    Inherits MvvmViewModelBase

    Sub New()
        Personen = New ObservableCollection(Of PersonenViewModelNodeTest)()

        Personen.Add(New PersonenViewModelNodeTest() With {.Nachname = "AAAAA"})
        Dim person = New PersonenViewModelNodeTest() With {.Nachname = "BBBBB"}

        person.Anschriften.Add(New AnschriftenViewModelNodeTest())
        person.Anschriften.Add(New AnschriftenViewModelNodeTest() With {.Orte = Nothing})
        Dim anschrift = New AnschriftenViewModelNodeTest()
        anschrift.Orte.Add(New OrtViewModelNodeTest())
        anschrift.Orte.Add(New OrtViewModelNodeTest())

        person.Anschriften.Add(anschrift)
        person.Anschriften.Add(New AnschriftenViewModelNodeTest())

        Personen.Add(person)
        Dim person2 = New PersonenViewModelNodeTest() With {.Nachname = "CCCCC"}

        person2.Anschriften.Add(New AnschriftenViewModelNodeTest())
        Personen.Add(person2)

        Personen2 = New ObservableCollection(Of PersonenViewModelNodeTest)()
        Dim p = New PersonenViewModelNodeTest() With {.Nachname = "mit Unter Personen"}

        p.Personen.Add(p)

        Personen2.Add(p)

        ViewModelTypes = New ObservableCollection(Of PropertyBindingNodeDefinition)()
        Dim prop = New PropertyBindingNodeDefinition()
        Dim propertiesList = New List(Of PropertyCheckBoxItemController)

        ReflectionHelper.CreateFlatSubPropAsList(GetType(CircTestPerson), "", propertiesList, False)

        For Each x In propertiesList
            Dim p1 = x.PropertyType
            Dim x1 = 2
        Next

        Dim props = New List(Of PropertyBindingNodeDefinition)(From pItem In propertiesList
                                                               Order By pItem.PropertyFullname
                                                               Select New PropertyBindingNodeDefinition With
                                                                                                   {.Binding = New BindingProperty() With
                                                                                                       {.PropertyName = pItem.PropertyFullname,
                                                                                                        .PropertyType = pItem.PropertyType}})
        For Each prop In props
            ViewModelTypes.Add(prop)
        Next


        'p.Personen.Add(New PersonenViewModelNodeTest())
        'p = New PersonenViewModelNodeTest() With {.Nachname = "mit Unter Personen"}
        'p.Personen.Add(p)
        'p.Personen.Add(p)
        'p.Personen.Add(New PersonenViewModelNodeTest())

        'Personen2.Add(p)
        'Personen2.Add(New PersonenViewModelNodeTest())
    End Sub

    Property Personen As ObservableCollection(Of PersonenViewModelNodeTest)


    Property Personen2 As ObservableCollection(Of PersonenViewModelNodeTest)


    Property ViewModelTypes As ObservableCollection(Of PropertyBindingNodeDefinition)

    Property Rekursion As MainNodeTestViewModel

    Private myNumVal As Decimal?
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property NumVal As Decimal?
        Get
            Return myNumVal
        End Get
        Set(ByVal value As Decimal?)
            If MyBase.SetProperty(myNumVal, value) Then
                'Debug.WriteLine($"Neues Num-Val: {value}")
            End If
        End Set
    End Property
    Public Const NumValProperty As String = "NumVal"

    Private _selected As PersonenViewModelNodeTest
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property SelectedNode As PersonenViewModelNodeTest
        Get
            Return _selected
        End Get
        Set(ByVal value As PersonenViewModelNodeTest)
            MyBase.SetProperty(_selected, value)
        End Set
    End Property
End Class

Public Class PersonenViewModelNodeTest
    Inherits MvvmViewModelBase
    Property Nachname As String = "Mustermann"

    Property Anschriften As New ObservableCollection(Of AnschriftenViewModelNodeTest)

    Property Personen As New ObservableCollection(Of PersonenViewModelNodeTest)
End Class

Public Class AnschriftenViewModelNodeTest
    Property Anschrift As String = "Musterhausen"

    Property Orte As New ObservableCollection(Of OrtViewModelNodeTest)
End Class

Public Class OrtViewModelNodeTest
    Private _bez As String = "Musterort"
    Public Property Bezeichnung As String
        Get
            Return _bez
        End Get
        Set(ByVal value As String)
            _bez = value
        End Set
    End Property
End Class