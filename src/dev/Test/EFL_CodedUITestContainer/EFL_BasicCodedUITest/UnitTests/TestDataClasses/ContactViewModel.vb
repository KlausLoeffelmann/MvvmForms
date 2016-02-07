Imports System.Collections.ObjectModel
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class ContactViewModel
    Inherits MvvmViewModelBase

    Private myLastname As String
    Private myFirstname As String
    Private myAddressLine1 As String
    Private myAddressLine2 As String
    Private myCity As String
    Private myZip As String
    Private myID As Guid

    <ModelPropertyIgnore>
    Public Property ID As Guid
        Get
            Return myID
        End Get
        Set(value As Guid)
            SetProperty(myID, value)
        End Set
    End Property

    '--- Lastname---
    Public Property Lastname As String
        Get
            Return myLastname
        End Get
        Set(value As String)
            SetProperty(myLastname, value)
        End Set
    End Property

    '--- Firstname---
    Public Property Firstname As String
        Get
            Return myFirstname
        End Get
        Set(value As String)
            SetProperty(myFirstname, value)
        End Set
    End Property

    '--- AddressLine1---
    Public Property AddressLine1 As String
        Get
            Return myAddressLine1
        End Get
        Set(value As String)
            SetProperty(myAddressLine1, value)
        End Set
    End Property

    '--- AddressLine2---
    Public Property AddressLine2 As String
        Get
            Return myAddressLine2
        End Get
        Set(value As String)
            SetProperty(myAddressLine2, value)
        End Set
    End Property

    '--- City---
    Public Property City As String
        Get
            Return myCity
        End Get
        Set(value As String)
            SetProperty(myCity, value)
        End Set
    End Property

    '--- Zip---
    Public Property Zip As String
        Get
            Return myZip
        End Get
        Set(value As String)
            SetProperty(myZip, value)
        End Set
    End Property

    Public Shared Function GetTestData(count As Integer) As ObservableCollection(Of ContactViewModel)
        Dim contactList As New ObservableCollection(Of ContactViewModel)
        Dim rnd As New Random(Now.Millisecond)

        Dim lastNames As String() = {"Miller", "Goldblum", "Hanks", "Müller",
                    "Ardelean", "Lehnert", "Sonntag", "Fox", "Westermann", "Vüllers",
                    "Hollmann", "Vielstedde", "Weigel", "Weichel", "Weichelt", "Hoffmann",
                    "Rode", "Trouw", "Schindler", "Neumann", "Jungemann", "Ardelean",
                    "Löffelmann", "Albrecht", "Langenbach", "Braun", "Steward", "Englisch",
                    "Clarke", "Lehnert", "Rosenau", "Thiemann", "Eckert", "Urgien"}

        Dim streets As String() = {"Bremer Str.", "Stauffenbergstr.", "Lincoln Street", "Parkstr.",
                         "Rüdenkuhle", "Alter Postweg", "Lange Wende", "Marktplatz", "Locust Lane",
                         "Straßengasse", "Church Street North", "Platzstr.", "Route 32", "Himmelsbachweg",
                         "Virginia Avenue", "Potterberg", "Sycamore Lane", "Leingartenweg", "Lehnertweg"}

        Dim firstNames As String() = {"Bryce", "Curtis", "Damon", "Adriana", "Hans",
                    "Calliope", "Christian", "Uta", "Michaela", "Franz", "Anne", "Anja",
                    "Theo", "Percy", "Lenore", "Guido", "Barbara", "Bernhard", "Margarete",
                    "Daniel", "Melanie", "Britta", "José", "Thomas", "Daja", "Klaus", "Axel",
                    "Stephan", "Gareth", "Edric", "Tagan", "Kerstin", "Andreas", "Guido", "Frank"}

        Dim cities As String() = {"Seattle", "Dortmund", "Lippstadt", "Soest",
                    "Houston", "Chandler", "München", "Berlin", "Rheda", "Bielefeld",
                    "Redmond", "Unterschleißheim", "Anchorage", "Modesto",
                    "Bad Waldliesborn", "Lippetal", "Stirpe", "Albuquerque"}

        For i As Integer = 1 To count

            Dim lastName, firstName As String

            lastName = lastNames(rnd.Next(lastNames.Length - 1))
            firstName = firstNames(rnd.Next(lastNames.Length - 1))
            contactList.Add(New ContactViewModel With {.ID = Guid.NewGuid(),
                                                       .Firstname = firstName,
                                                       .Lastname = lastName,
                                                       .AddressLine1 = streets(rnd.Next(streets.Length - 1)),
                                                       .City = cities(rnd.Next(cities.Length - 1)),
                                                       .Zip = rnd.Next(99999).ToString("00000")})
        Next
        Return contactList

    End Function

End Class