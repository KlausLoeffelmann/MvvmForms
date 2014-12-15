Imports System.Collections.ObjectModel
Imports ActiveDevelop.EntitiesFormsLib
Imports System.Windows.Input

<BusinessClass>
Public Class ProductTest
    Inherits AttributeControlledComparableBase

    <EqualityIndicator, DisplayIndicatorAttribute(1, "00000:")>
    Property ProductNo As String

    <DisplayIndicatorAttribute(2)>
    Property ProductName As String

    Property Category As String
    Property Amount As Decimal
    Property UnitPrice As Decimal
    Property SpecialPrice As Decimal?
    Property DatePurchased As Date
    Property ProductIntroduced As Date?

    Public Shared Function RandomProducts(ByVal Customers As ObservableCollection(Of CustomerTest)) As ObservableCollection(Of ProductTest)

        Dim tmpRandom As New Random(42)
        Dim tmpListOfProducts As New ObservableCollection(Of ProductTest)

        Dim tmpProductMasterData As String() = {"DVD|Catch me if you can|1-234", _
                                      "DVD/Blue Ray|Being John Malkovich|2-134", _
                                      "DVD/Blue Ray|Bodyguard|3-123", _
                                      "DVD/Blue Ray|Castaway|9-646", _
                                      "DVD/Blue Ray|The Maiden Heist|3-534", _
                                      "DVD/Blue Ray|Transporter 3|4-324", _
                                      "DVD/Blue Ray|The Social Network|9-423", _
                                      "DVD/Blue Ray|Runaway Jury|5-554", _
                                      "DVD/Blue Ray|24 - Season 7|2-424", _
                                      "Books, IT|Parallel Programming with Microsoft Visual Studio 2010 Step by Step|5-506", _
                                      "Books, IT|Visual Basic 2010 - Developer's Handbook|5-506", _
                                      "Books, IT|Microsoft Visual C# 2010 - Developer's Handbook|3-543", _
                                      "Books, IT|How We Test Software at Microsoft|5-401", _
                                      "Books, IT|Microsoft SQL Server 2008 R2 - Developer's Handbook|5-513", _
                                      "Audibooks|Harry Potter and the Deathly Hallows| 4-444", _
                                      "Audibooks|The Jungle Book|2-321", _
                                      "Audibooks|A tale of two cities|9-009", _
                                      "Audibooks|Pride and prejudice|7-321", _
                                      "Books, Novels|Eclipse (The Twilight Saga, Book 3)|9-445", _
                                      "Books, Novels|The Cathedral of the Sea|5-436", _
                                      "Books, Novels|The Da Vinci Code|4-444", _
                                      "Books, Novels|Der Schwarm (German Edition)|3-333", _
                                      "Books, Novels|The Rose Killer|6-666"}

        Dim tmpProduct As ProductTest

        'Everybody purchased something! :-)
        For Each adrItem In Customers
            'Every customer purchased between one and 20 products.
            Dim contactProducts As New ObservableCollection(Of ProductTest)
            For anzahlGekaufterArtikel = 1 To tmpRandom.Next(1, 10)
                tmpProduct = New ProductTest()
                Dim tmpStr() = tmpProductMasterData(tmpRandom.Next(0, tmpProductMasterData.Count - 1)).Split("|"c)
                tmpProduct.ProductName = tmpStr(1)
                tmpProduct.ProductNo = tmpStr(2)
                tmpProduct.Amount = tmpRandom.Next(1, 4)
                tmpProduct.UnitPrice = (tmpRandom.Next(1, 20) * 5) - 0.05D
                tmpProduct.SpecialPrice = If(tmpRandom.Next(3) = 2, Nothing, tmpProduct.UnitPrice * 0.9D)
                tmpProduct.Category = tmpStr(0)
                tmpProduct.DatePurchased = New Date(2010, 1, 1).AddDays(tmpRandom.Next(720))
                tmpListOfProducts.Add(tmpProduct)
                contactProducts.Add(tmpProduct)
            Next
            adrItem.PurchasedProducts = contactProducts
        Next
        Return tmpListOfProducts
    End Function
End Class

<BusinessClass>
Public Class ContactTest
    Inherits AttributeControlledComparableBase

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal ID As Integer, ByVal Name As String, _
            ByVal Vorname As String, ByVal Straße As String, _
            ByVal Plz As String, ByVal Ort As String, Optional DateOfBirth As Date? = Nothing)
        Me.IDContact = ID
        Me.LastName = Name
        Me.FirstName = Vorname
        Me.Street = Straße
        Me.ZIP = Plz
        Me.City = Ort
        Me.DateOfBirth = DateOfBirth
    End Sub

    Property IDPurchasedBy As Integer

    <EqualityIndicator, DisplayIndicatorAttribute(1, "00000", ": ")>
    Public Property IDContact() As Integer

    <DisplayIndicatorAttribute(2, , ", ")>
    Public Property LastName() As String

    <DisplayIndicatorAttribute(3)>
    Public Property FirstName() As String
    Public Property Street() As String
    Public Property ZIP() As String
    Public Property City() As String
    Public Property DateOfBirth As Date?

    Public Shared Function GetRandomContactsAsync(contacts As ObservableBindingList(Of ContactTest),
                                                    count As Integer,
                                                    delay As Integer) As Threading.Tasks.Task

        Dim workerTask As New Threading.Tasks.Task(
            Sub()
                Dim tmpRandom As New Random(42)

                Dim tmpLastNames As String() = {"Heckhuis", "Löffelmann", "Jones", "Lowel", _
                            "Ardelean", "Beckham", "Baur", "Picard", "Trouv", "Feigenbaum", _
                            "Miller", "Wallace", "Merkel", "Spooner", "Spoonman", "Huffman", _
                            "Rode", "Trouw", "Schindler", "Brown", "Walker", "Cruise", "Meier", "Maier", "Mayer", _
                            "Tinoco", "O'Reilly", "O'Donnell", "Ó Briain", "Russel", "English", _
                            "Clarke", "Schumacher"}

                Dim tmpStreetNames As String() = {"Wiedenbrückerstr.", "Stauffenberg Ave.", "Broadway", "Parkstr.", _
                                 "Kurgartenweg", "Alter Postweg", "Long Turnpike", "Zzyzx Rd.", "Main Street", _
                                 "Streetway", "Postplatz", "Beamer Place", "Mercedes Way", "Porsche Drive", _
                                 "Weidering", "One Way", "Endof Rd.", "Gotlost Way", "Satnav Rd."}

                Dim tmpFirstNames As String() = {"Jürgen", "Gabriele", "Dianne", "Katrin", "Jack", _
                            "Arnold", "Christian", "Frank", "Curt", "Peter", "Anne", "Anja", _
                            "Theo", "Bob", "Katrin", "Guido", "Barbara", "Bernhard", "Margarete", _
                            "Alfred", "Melanie", "Britta", "José", "Thomas", "Dara", "Klaus", "Axel", _
                            "Gabby", "Gareth", "Bob", "Denise", "Kristen"}

                Dim tmpCities As String() = {"Bellevue", "Dortmund", "Lippstadt", "Redmond", _
                            "Los Angeles", "Las Vegas", "Seattle", "New York", "Berlin", "Bielefeld", _
                            "Braunschweig", "Munich", "Cologne", "Hamburg", _
                            "Bad Waldliesborn", "Bremen", "Encinitas", "Anaheim"}

                For i As Integer = 1 To count
                    Dim tmpLastName, tmpFirstName As String
                    tmpLastName = tmpLastNames(tmpRandom.Next(tmpLastNames.Length - 1))
                    tmpFirstName = tmpFirstNames(tmpRandom.Next(tmpLastNames.Length - 1))
                    Dim contactToAdd = New ContactTest( _
                                    i, _
                                    tmpLastName, _
                                    tmpFirstName, _
                                    tmpStreetNames(tmpRandom.Next(tmpStreetNames.Length - 1)), _
                                    tmpRandom.Next(99999).ToString("00000"), _
                                    tmpCities(tmpRandom.Next(tmpCities.Length - 1)),
                                    New Date(1950, 1, 1).AddDays(tmpRandom.Next(19000)))

                    contacts.Add(contactToAdd)
                    If delay > 0 Then
                        Threading.Thread.Sleep(delay)
                    End If
                Next
            End Sub)
        workerTask.Start()
        Return workerTask

    End Function

    Public Shared Function RandomContacts(ByVal Count As Integer) As ObservableCollection(Of ContactTest)

        Dim tmpListOfAddresses As New ObservableCollection(Of ContactTest)
        Dim tmpRandom As New Random(42)

        Dim tmpLastNames As String() = {"Heckhuis", "Löffelmann", "Jones", "Lowel", _
                    "Ardelean", "Beckham", "Baur", "Picard", "Trouv", "Feigenbaum", _
                    "Miller", "Wallace", "Merkel", "Spooner", "Spoonman", "Huffman", _
                    "Rode", "Trouw", "Schindler", "Brown", "Walker", "Cruise", "Meier", "Maier", "Mayer", _
                    "Tinoco", "O'Reilly", "O'Donnell", "Ó Briain", "Russel", "English", _
                    "Clarke", "Schumacher"}

        Dim tmpStreetNames As String() = {"Wiedenbrückerstr.", "Stauffenberg Ave.", "Broadway", "Parkstr.", _
                         "Kurgartenweg", "Alter Postweg", "Long Turnpike", "Zzyzx Rd.", "Main Street", _
                         "Streetway", "Postplatz", "Beamer Place", "Mercedes Way", "Porsche Drive", _
                         "Weidering", "One Way", "Endof Rd.", "Gotlost Way", "Satnav Rd."}

        Dim tmpFirstNames As String() = {"Jürgen", "Gabriele", "Dianne", "Katrin", "Jack", _
                    "Arnold", "Christian", "Frank", "Curt", "Peter", "Anne", "Anja", _
                    "Theo", "Bob", "Katrin", "Guido", "Barbara", "Bernhard", "Margarete", _
                    "Alfred", "Melanie", "Britta", "José", "Thomas", "Dara", "Klaus", "Axel", _
                    "Gabby", "Gareth", "Bob", "Denise", "Kristen"}

        Dim tmpCities As String() = {"Bellevue", "Dortmund", "Lippstadt", "Redmond", _
                    "Los Angeles", "Las Vegas", "Seattle", "New York", "Berlin", "Bielefeld", _
                    "Braunschweig", "Munich", "Cologne", "Hamburg", _
                    "Bad Waldliesborn", "Bremen", "Encinitas", "Anaheim"}

        For i As Integer = 1 To Count
            Dim tmpLastName, tmpFirstName As String
            tmpLastName = tmpLastNames(tmpRandom.Next(tmpLastNames.Length - 1))
            tmpFirstName = tmpFirstNames(tmpRandom.Next(tmpLastNames.Length - 1))
            tmpListOfAddresses.Add(New ContactTest( _
                                    i, _
                                    tmpLastName, _
                                    tmpFirstName, _
                                    tmpStreetNames(tmpRandom.Next(tmpStreetNames.Length - 1)), _
                                    tmpRandom.Next(99999).ToString("00000"), _
                                    tmpCities(tmpRandom.Next(tmpCities.Length - 1)),
                                    New Date(1950, 1, 1).AddDays(tmpRandom.Next(19000))))
        Next
        Return tmpListOfAddresses
    End Function
End Class

Public Class CustomerTest
    Inherits ContactTest

    Sub New()
        MyBase.New()
    End Sub

    <EqualityIndicator>
    Public Property CustomerID As Integer
    Public Property PurchasedProducts As ObservableCollection(Of ProductTest)

    Public Shared Function RandomCustomers(ByVal Count As Integer, maxItemsPerCustomer As Integer) As ObservableCollection(Of CustomerTest)

        Dim tmpListOfAddresses As New ObservableCollection(Of CustomerTest)
        Dim tmpRandom As New Random(42)

        Dim tmpLastNames As String() = {"Heckhuis", "Löffelmann", "Jones", "Lowel", _
                    "Ardelean", "Beckham", "Baur", "Picard", "Trouv", "Feigenbaum", _
                    "Miller", "Wallace", "Merkel", "Spooner", "Spoonman", "Huffman", _
                    "Rode", "Trouw", "Schindler", "Brown", "Walker", "Cruise", "Meier", "Maier", "Mayer", _
                    "Tinoco", "O'Reilly", "O'Donnell", "Ó Briain", "Russel", "English", _
                    "Clarke", "Schumacher"}

        Dim tmpStreetNames As String() = {"Wiedenbrückerstr.", "Stauffenberg Ave.", "Broadway", "Parkstr.", _
                         "Kurgartenweg", "Alter Postweg", "Long Turnpike", "Zzyzx Rd.", "Main Street", _
                         "Streetway", "Postplatz", "Beamer Place", "Mercedes Way", "Porsche Drive", _
                         "Weidering", "One Way", "Endof Rd.", "Gotlost Way", "Satnav Rd."}

        Dim tmpFirstNames As String() = {"Jürgen", "Gabriele", "Dianne", "Katrin", "Jack", _
                    "Arnold", "Christian", "Frank", "Curt", "Peter", "Anne", "Anja", _
                    "Theo", "Bob", "Katrin", "Guido", "Barbara", "Bernhard", "Margarete", _
                    "Alfred", "Melanie", "Britta", "José", "Thomas", "Dara", "Klaus", "Axel", _
                    "Gabby", "Gareth", "Bob", "Denise", "Kristen"}

        Dim tmpCities As String() = {"Bellevue", "Dortmund", "Lippstadt", "Redmond", _
                    "Los Angeles", "Las Vegas", "Seattle", "New York", "Berlin", "Bielefeld", _
                    "Braunschweig", "Munich", "Cologne", "Hamburg", _
                    "Bad Waldliesborn", "Bremen", "Encinitas", "Anaheim"}

        For i As Integer = 1 To Count
            Dim tmpLastName, tmpFirstName As String
            tmpLastName = tmpLastNames(tmpRandom.Next(tmpLastNames.Length - 1))
            tmpFirstName = tmpFirstNames(tmpRandom.Next(tmpLastNames.Length - 1))
            tmpListOfAddresses.Add(New CustomerTest With
                                    {
                                    .CustomerID = i,
                                    .IDContact = i * 10,
                                    .FirstName = tmpFirstName,
                                    .LastName = tmpLastName,
                                    .Street = tmpStreetNames(tmpRandom.Next(tmpStreetNames.Length - 1)),
                                    .ZIP = tmpRandom.Next(99999).ToString("00000"),
                                    .City = tmpCities(tmpRandom.Next(tmpCities.Length - 1)),
                                    .DateOfBirth = If(tmpRandom.Next(3) = 2,
                                                      Nothing,
                                                      New Date(1950, 1, 1).AddDays(tmpRandom.Next(19000)))
                                    })
        Next
        Return tmpListOfAddresses
    End Function

End Class
