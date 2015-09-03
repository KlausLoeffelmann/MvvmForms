Imports System.Collections.ObjectModel
Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class Buchung
    Inherits MvvmViewModelBase

    Private _url As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Url As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            MyBase.SetProperty(_url, value)
        End Set
    End Property
    Public Const UrlProperty As String = "Url"


    Private _nummer As Integer
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Nummer As Integer
        Get
            Return _nummer
        End Get
        Set(ByVal value As Integer)
            MyBase.SetProperty(_nummer, value)
        End Set
    End Property
    Public Const NummerProperty As String = "Nummer"


    Private _datum As Date
    ''' <summary>
    ''' 
    ''' </summary>s
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Datum As Date
        Get
            Return _datum
        End Get
        Set(ByVal value As Date)
            MyBase.SetProperty(_datum, value)
        End Set
    End Property
    Public Const DatumProperty As String = "Datum"

    Private _buchungstext As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Buchungstext As String
        Get
            Return _buchungstext
        End Get
        Set(ByVal value As String)
            MyBase.SetProperty(_buchungstext, value)
        End Set
    End Property
    Public Const BuchungstextProperty As String = "Buchungstext"

    Private _kostenart As Kostenart
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Kostenart As Kostenart
        Get
            Return _kostenart
        End Get
        Set(ByVal value As Kostenart)
            MyBase.SetProperty(_kostenart, value)
        End Set
    End Property
    Public Const KostenartProperty As String = "Kostenart"

    Private _kostenarten As ObservableCollection(Of Kostenart)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Kostenarten As ObservableCollection(Of Kostenart)
        Get
            Return _kostenarten
        End Get
        Set(ByVal value As ObservableCollection(Of Kostenart))
            MyBase.SetProperty(_kostenarten, value)
        End Set
    End Property
    Public Const KostenartenProperty As String = "Kostenart"

    Private _ausgaben As Double
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Ausgaben As Double
        Get
            Return _ausgaben
        End Get
        Set(ByVal value As Double)
            If MyBase.SetProperty(_ausgaben, value) Then
                MyBase.OnPropertyChanged(EnthaleteneSteuerProperty)
                MyBase.OnPropertyChanged(BetragProperty)
            End If
        End Set
    End Property
    Public Const AusgabenProperty As String = "Ausgaben"

    Private _satz As Integer
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Satz As Integer
        Get
            Return _satz
        End Get
        Set(ByVal value As Integer)
            If MyBase.SetProperty(_satz, value) Then
                MyBase.OnPropertyChanged(EnthaleteneSteuerProperty)
                MyBase.OnPropertyChanged(BetragProperty)
            End If
        End Set
    End Property

    Public Const SatzProperty As String = "Satz"
    Public ReadOnly Property EnthaleteneSteuer As Double
        Get
            If Me.Satz = 0 Then
                Return 0
            Else
                Return Math.Round(Me.Ausgaben * (Me.Satz / 100), 2)
            End If
        End Get
    End Property

    Public Const EnthaleteneSteuerProperty As String = "EnthaleteneSteuer"

    Public ReadOnly Property Betrag As Double
        Get
            Return Me.Ausgaben - EnthaleteneSteuer
        End Get
    End Property


    Public Const BetragProperty As String = "Betrag"

    Private _wichtig As Boolean = False
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Bindbare Property</remarks>
    Public Property Wichtig As Boolean
        Get
            Return _wichtig
        End Get
        Set(ByVal value As Boolean)
            MyBase.SetProperty(_wichtig, value)
        End Set
    End Property
    Public Const WichtigProperty As String = "Wichtig"
End Class
