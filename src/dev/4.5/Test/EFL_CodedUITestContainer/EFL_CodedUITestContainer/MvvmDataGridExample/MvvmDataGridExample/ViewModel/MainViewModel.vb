Imports System.Collections.ObjectModel
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class MainViewModel
    Inherits MvvmBase

    Sub New()
        Dim kostenartenLookup = New ObservableCollection(Of Kostenart)

        Dim bewirtungskosten = New Kostenart() With {.Name = "Bewirtungskosten"}
        Dim sonstigekosten = New Kostenart() With {.Name = "Sonstiges"}
        Dim honorarkosten = New Kostenart() With {.Name = "Honorar"}
        Dim fahrtkosten = New Kostenart() With {.Name = "Fahrtkosten"}
        Dim mobilkosten = New Kostenart() With {.Name = "Mobiltelefon"}

        kostenartenLookup.Add(bewirtungskosten)
        kostenartenLookup.Add(sonstigekosten)
        kostenartenLookup.Add(honorarkosten)
        kostenartenLookup.Add(mobilkosten)
        kostenartenLookup.Add(fahrtkosten)
        kostenartenLookup.Add(New Kostenart() With {.Name = "Telefon"})

        _buchungen = New ObservableCollection(Of Buchung)()

        For index = 1 To 10000
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/12", .Nummer = 12, .Datum = New Date(2010, 7, 1), .Buchungstext = "Sushi", .Kostenarten = kostenartenLookup, .Ausgaben = 54, .Satz = 19, .Kostenart = bewirtungskosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/13", .Nummer = 13, .Datum = New Date(2010, 7, 2), .Buchungstext = "Sparkasse", .Kostenarten = kostenartenLookup, .Ausgaben = 9.15, .Satz = 0, .Kostenart = sonstigekosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/14", .Nummer = 14, .Datum = New Date(2010, 7, 3), .Buchungstext = "Sparkasse", .Kostenarten = kostenartenLookup, .Ausgaben = -39.26, .Satz = 19, .Kostenart = sonstigekosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/15", .Nummer = 15, .Datum = New Date(2010, 7, 4), .Wichtig = True, .Buchungstext = "Kunde ABC", .Kostenarten = kostenartenLookup, .Ausgaben = 0, .Satz = 0, .Kostenart = honorarkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/16", .Nummer = 16, .Datum = New Date(2010, 7, 5), .Buchungstext = "T-Mobile", .Kostenarten = kostenartenLookup, .Ausgaben = 180, .Satz = 0, .Kostenart = mobilkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/17", .Nummer = 17, .Datum = New Date(2010, 7, 6), .Buchungstext = "Benzin", .Kostenarten = kostenartenLookup, .Ausgaben = 28, .Satz = 19, .Kostenart = fahrtkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/18", .Nummer = 18, .Datum = New Date(2010, 7, 7), .Buchungstext = "Kunde DEF", .Kostenarten = kostenartenLookup, .Ausgaben = 92.17, .Satz = 7, .Kostenart = honorarkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/19", .Nummer = 19, .Datum = New Date(2010, 7, 8), .Buchungstext = "Kunde a", .Kostenarten = kostenartenLookup, .Ausgaben = 3.99, .Satz = 0, .Kostenart = honorarkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/20", .Nummer = 20, .Datum = New Date(2010, 7, 7), .Wichtig = True, .Buchungstext = "Kunde DEF", .Kostenarten = kostenartenLookup, .Ausgaben = -22.87, .Satz = 7, .Kostenart = honorarkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/21", .Nummer = 21, .Datum = New Date(2010, 7, 8), .Buchungstext = "Kunde a", .Kostenarten = kostenartenLookup, .Ausgaben = 3.99, .Satz = 0, .Kostenart = honorarkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/22", .Nummer = 22, .Datum = New Date(2010, 7, 7), .Buchungstext = "Kunde DEF", .Kostenarten = kostenartenLookup, .Ausgaben = -102.99, .Satz = 7, .Kostenart = honorarkosten})
            _buchungen.Add(New Buchung() With {.Url = "http://umstatzinfo.com/23", .Nummer = 23, .Datum = New Date(2010, 7, 8), .Buchungstext = "Kunde a", .Kostenarten = kostenartenLookup, .Ausgaben = 8.91, .Satz = 0, .Kostenart = honorarkosten})
        Next



    End Sub

    Private _buchungen As ObservableCollection(Of Buchung)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Buchungen As ObservableCollection(Of Buchung)
        Get
            Return _buchungen
        End Get
        Set(ByVal value As ObservableCollection(Of Buchung))
            MyBase.SetProperty(_buchungen, value)
        End Set
    End Property
    Public Const BuchungenProperty As String = "Buchungen"
End Class
