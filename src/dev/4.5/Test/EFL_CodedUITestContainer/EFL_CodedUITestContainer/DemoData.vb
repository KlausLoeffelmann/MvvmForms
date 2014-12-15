Imports System.Collections.ObjectModel

Public Class Product

    Property IDPurchasedBy() As Integer
    Property ProductName() As String
    Property ProductNo() As String
    Property Category() As String
    Property Amount() As Integer
    Property UnitPrice() As Decimal

    Public Overrides Function ToString() As String
        Return Me.ProductNo & ": " & Me.ProductName
    End Function

    Public Shared Function RandomProducts(ByVal Kontakte As List(Of Contact)) As List(Of Product)

        Dim tmpRandom As New Random(42)
        Dim tmpListOfProducts As New List(Of Product)

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

        Dim tmpProduct As Product

        'Everybody purchased something! :-)
        For Each adrItem In Kontakte
            'Every customer purchased between one and 20 products.
            For anzahlGekaufterArtikel = 1 To tmpRandom.Next(1, 10)
                tmpProduct = New Product()
                Dim tmpStr() = tmpProductMasterData(tmpRandom.Next(0, tmpProductMasterData.Count - 1)).Split("|"c)
                tmpProduct.IDPurchasedBy = adrItem.IDContact
                tmpProduct.ProductName = tmpStr(1)
                tmpProduct.ProductNo = tmpStr(2)
                tmpProduct.Amount = tmpRandom.Next(1, 4)
                tmpProduct.UnitPrice = (tmpRandom.Next(1, 20) * 5) - 0.05D
                tmpProduct.Category = tmpStr(0)
                tmpListOfProducts.Add(tmpProduct)
            Next
        Next
        Return tmpListOfProducts
    End Function


End Class

Public Class Address

    Sub New(ByVal Straße As String, _
        ByVal Plz As String, ByVal Ort As String)
        Me.Street = Straße
        Me.ZIP = Plz
        Me.City = Ort
    End Sub

    Public Property Street() As String
    Public Property ZIP() As String
    Public Overridable Property City() As String

    Public Overrides Function ToString() As String
        Return ZIP & ": " & City & " - " & Street
    End Function

    Public Shared Function RandomAddresses(ByVal Count As Integer) As List(Of Address)

        Dim tmpListOfAddresses As New List(Of Address)
        Dim tmpRandom As New Random(42)

        Dim tmpStreetNames As String() = {"Wiedenbrückerstr.", "Stauffenberg Ave.", "Broadway", "Parkstr.", _
                         "Kurgartenweg", "Alter Postweg", "Long Turnpike", "Zzyzx Rd.", "Main Street", _
                         "Streetway", "Postplatz", "Beamer Place", "Mercedes Way", "Porsche Drive", _
                         "Weidering", "One Way", "Endof Rd.", "Gotlost Way", "Satnav Rd."}

        Dim tmpCities As String() = {"Bellevue", "Dortmund", "Lippstadt", "Redmond", _
                    "Los Angeles", "Las Vegas", "Seattle", "New York", "Berlin", "Bielefeld", _
                    "Braunschweig", "Munich", "Cologne", "Hamburg", _
                    "Bad Waldliesborn", "Bremen", "Encinitas", "Anaheim"}

        For i As Integer = 1 To Count
            tmpListOfAddresses.Add(New Address(tmpStreetNames(tmpRandom.Next(tmpStreetNames.Length - 1)), _
                            tmpRandom.Next(99999).ToString("00000"), _
                            tmpCities(tmpRandom.Next(tmpCities.Length - 1))))
        Next
        Return tmpListOfAddresses
    End Function
End Class

Public Class Contact

    Sub New(ByVal ID As Integer, ByVal Name As String, _
            ByVal Vorname As String, ByVal Straße As String, _
            ByVal Plz As String, ByVal Ort As String)
        Me.IDContact = ID
        Me.LastName = Name
        Me.FirstName = Vorname
        Me.Street = Straße
        Me.ZIP = Plz
        Me.City = Ort
    End Sub

    Public Property IDContact() As Integer
    Public Property LastName() As String
    Public Property FirstName() As String
    Public Property Street() As String
    Public Property ZIP() As String
    Public Overridable Property City() As String

    Public Overrides Function ToString() As String
        Return """" + LastName + ", " + FirstName + """"
    End Function

    Public Shared Function RandomContacts(ByVal Count As Integer) As List(Of Contact)

        Dim tmpListOfAddresses As New List(Of Contact)
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
            tmpListOfAddresses.Add(New Contact( _
                            i, _
                            tmpLastName, _
                            tmpFirstName, _
                            tmpStreetNames(tmpRandom.Next(tmpStreetNames.Length - 1)), _
                            tmpRandom.Next(99999).ToString("00000"), _
                            tmpCities(tmpRandom.Next(tmpCities.Length - 1))))
        Next
        Return tmpListOfAddresses
    End Function

    Shared Sub PrintContacts(ByVal Contacts As List(Of Contact))
        'Option Infer ist 'On', deswegen wird
        'Item automatisch zum Typ 'Adresse'
        For Each Item In Contacts
            Console.WriteLine(Item)
        Next
    End Sub

End Class
