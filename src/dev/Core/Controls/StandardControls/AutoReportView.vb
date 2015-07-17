Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection
Imports System.Windows.Forms

'Das eigentliche Steuerelement
<ToolboxItem(False)>
Public Class AutoReportView
    Inherits System.Windows.Forms.ListView

    Public Event SelectedSourceItemChanged(ByVal sender As Object, ByVal e As SelectedSourceItemEventArgs)
    Public Event DefineReportColumns(ByVal sender As Object, ByVal e As AutoReportViewColumnsEventArgs)

    'Membervariablen
    Private myObservableList As System.Collections.ObjectModel.ObservableCollection(Of INotifyPropertyChanged)
    Private myColumnNames As AutoReportColumns

    Private myDataSource As BindingManagerBase

    Sub New()
        MyBase.New()
        'Auf Detailansicht umschalten
        Me.View = System.Windows.Forms.View.Details
        'Trennlinien zwischen den Zeilen einschalten
        Me.GridLines = True
        'Bei Fokusverlust Markierung dennoch anzeigen
        Me.HideSelection = False
        'Ganze Reihe soll selektiert werden
        Me.FullRowSelect = True

        'NullValueString vordefinieren
        Me.NullValueString = "* - - - *"
        Me.ExcludeCollections = True
    End Sub

    Public Property NullValueString As String
    Public Property ExcludeCollections As Boolean

    Property DataSource As Object
        Get
            Return myDataSource
        End Get
        Set(ByVal value As Object)
            myDataSource = TryCast(value, BindingManagerBase)
            If myDataSource Is Nothing Then
                myDataSource = New PropertyManager
            End If

        End Set
    End Property

    Private WriteOnly Property ListItems As IEnumerable(Of INotifyPropertyChanged)
        Set(ByVal value As IEnumerable(Of INotifyPropertyChanged))
            If value Is Nothing Then
                ObservableListItems = Nothing
                Return
            End If
            Dim tmpObservableList = New System.Collections.ObjectModel.ObservableCollection(Of INotifyPropertyChanged)
            For Each item In value
                tmpObservableList.Add(item)
            Next
            ObservableListItems = tmpObservableList
        End Set
    End Property

    <Description("Definiert die in der ListView angezeigten Elemente" + _
                 "oder ermittelt diese")> _
    Public Property ObservableListItems() As System.Collections.ObjectModel.ObservableCollection(Of INotifyPropertyChanged)
        Get
            Return myObservableList
        End Get

        'Setzen der Eigenschaft:
        Set(ByVal Value As System.Collections.ObjectModel.ObservableCollection(Of INotifyPropertyChanged))
            'Alle Inhalte löschen
            Me.Items.Clear()
            'Allte Spaltentitel löschen
            Me.Columns.Clear()
            If Value Is Nothing Then
                'Abbrechen, falls Nothing zugewiesen wurde
                Return
            Else
                'Liste zuweisen
                myObservableList = Value
                'Die Spaltennamen und Objekteigenschaften entweder durch das Objekt
                'selbst oder zugewiesene Attribute ermitteln und in myColumnNamens 
                'speichern.
                myColumnNames = GetColumnNames(Value)
                Dim e As New AutoReportViewColumnsEventArgs(myColumnNames)
                OnDefineColumns(e)
                'Anschließend die Spaltentitel setzen...
                SetupColumns()
                '...und die Liste mit Einträgen füllen, die sich aus myIList ergeben
                SetupEntries()
            End If
        End Set
    End Property

    Protected Overridable Sub OnDefineColumns(ByVal e As AutoReportViewColumnsEventArgs)
        RaiseEvent DefineReportColumns(Me, e)
    End Sub

    'Spaltentitel einsetzen
    Private Sub SetupColumns()
        myColumnNames.ForEach(Sub(currItem)
                                  With currItem
                                      Me.Columns.Add(.DisplayName, _
                                                     .Width,
                                                     .Alignment)
                                  End With
                              End Sub)
    End Sub

    'Einträge in die Liste schreiben
    Private Sub SetupEntries()
        For Each obj As Object In myObservableList
            With Me.Items
                Dim locLvi As New ListViewItem
                'Erste darzustellende Eigenschaft erfährt Sonderbehandlung,
                'da sie nicht durch SubItems dargestellt wird
                'Mit GetPropValue wird die Stringumwandlung der Eigenschaft
                'eines Objektes ermittelt.
                locLvi.Text = GetPropValue(obj, myColumnNames(0).PropertyName)
                For c As Integer = 1 To myColumnNames.Count - 1
                    With locLvi.SubItems
                        .Add(GetPropValue(obj, myColumnNames(c).PropertyName))
                    End With
                Next
                locLvi.Tag = obj
                'Eintrag der Liste hinzufügen
                .Add(locLvi)
            End With
        Next

        'Spaltenbreiten anpassen
        Dim ccount As Integer = 0
        For Each cn In myColumnNames
            Me.Columns(ccount).Width = cn.Width
            ccount += 1
        Next

    End Sub

    'Ermittelt den Inhalt der Eigenschaft eines Objektes als String
    Private Function GetPropValue(ByVal [object] As Object, ByVal PropertyName As String) As String

        Dim locPI As PropertyInfo = [object].GetType.GetProperty(PropertyName)
        Dim obj = locPI.GetValue([object], Nothing)
        If obj Is Nothing Then
            Return ""
        Else
            Return obj.ToString
        End If
    End Function

    'Ermittelt die durch die Objekteigenschaften vorgegebenen dazustellenden
    'Spalten, wenn keine Attribute verwendet werden. Werden Attribute verwendet,
    'ermittelt die Funktion nur die Eigenschaften eines Objektes, die mit einem
    'entsprechenden Attribut versehen sind.
    Private Function GetColumnNames(ByVal List As IList) As AutoReportColumns

        Dim locTypeToExamine As Type
        Dim locARCs As New AutoReportColumns
        Dim locExplicitlyDefined As Boolean = False

        If List Is Nothing Then
            'Soweit dürfte es eigentlich gar nicht kommen, aber wir gehen auf No. sicher.
            Dim Up As New NullReferenceException("Die Übergebende Liste ist leer!")
            Throw Up
        End If

        'Das erste Objekt ist maßgeblich für die Typen aller anderen Objekte.
        'Die Liste muss also homogen (Objektableitungen ausgenommen) sein, damit 
        'die automatische Element-Zuweisung reibungslos funktioniert.
        locTypeToExamine = List(0).GetType

        'Alle Eigenschaften des Objektes durchforsten
        For Each pi As PropertyInfo In locTypeToExamine.GetProperties
            If ExcludeCollections Then
                If pi.PropertyType IsNot GetType(String) Then
                    If GetType(IEnumerable).IsAssignableFrom(pi.PropertyType) Then
                        Continue For
                    End If
                End If
            End If
            'Vielleicht gibt es keine Attribute, die näheres bestimmen;
            'in diesem Fall wird jede Objekteigenschaft verwendet.
            'Anzeigename ist dann Eigenschaftenname.
            Dim locARC As New AutoReportColumnAttribute(DisplayName:=pi.Name, PropertyName:=pi.Name)
            'Vorgabebreite: Spalten automatisch angleichen
            locARC.Width = -2
            'Nach Attributen Ausschau halten
            For Each a As Attribute In pi.GetCustomAttributes(True)
                'Nur reagieren, wenn es sich um unseren speziellen Typ handelt
                If TypeOf a Is AutoReportColumnAttribute Then
                    'Parameter aus dem Attribute-Objekt übernehmen
                    locARC.DisplayName = a.GetType.GetProperty("DisplayName").GetValue(a, Nothing).ToString
                    locARC.Width = CInt(a.GetType.GetProperty("Width").GetValue(a, Nothing))
                    locARC.OrderNo = CInt(a.GetType.GetProperty("OrderNo").GetValue(a, Nothing))
                    locARC.ExplicitlyDefined = True
                    locExplicitlyDefined = True
                End If
            Next
            'Zur Spaltenkopf-Parameterliste hinzufügen
            locARCs.Add(locARC)
        Next

        'Wenn Attribute gefunden worden sind, dann die Eigenschaften
        'wieder rausschmeißen, denen kein Attribut zugewiesen wurde
        If locExplicitlyDefined Then

            Dim locCount As Integer = 0

            Do While locCount < locARCs.Count
                If Not locARCs(locCount).ExplicitlyDefined Then
                    locARCs.RemoveAt(locCount)
                Else
                    locCount += 1
                End If

            Loop
        End If
        'Reihenfolge berücksichtigen
        locARCs.SortByOrderNo()
        Return locARCs

    End Function

    Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
        MyBase.OnSelectedIndexChanged(e)
        If Me.SelectedItems.Count > 0 Then
            OnSelectedSourceItemChanged(New SelectedSourceItemEventArgs(Me.SelectedItems(0).Tag))
        End If
    End Sub

    Protected Overridable Sub OnSelectedSourceItemChanged(ByVal e As SelectedSourceItemEventArgs)
        RaiseEvent SelectedSourceItemChanged(Me, e)
    End Sub
End Class

Public Class AutoReportColumns
    Inherits List(Of AutoReportColumnAttribute)

    'Sortiert das ganze Array nach der Sortierreihenfolgenr. (Order-No),
    'damit die Spalten in der richtigen Reihenfolge angezeigt werden können.
    Public Sub SortByOrderNo()
        Me.Sort(Function(item1, item2)
                    Return item1.OrderNo.CompareTo(item2.OrderNo)
                End Function)
    End Sub
End Class

'Dieses Attribut kann nur auf Eigenschaften angewendet werden
<AttributeUsage(AttributeTargets.Property)> _
Public Class AutoReportColumnAttribute
    Inherits Attribute

    Private myWidth As Integer
    Private myOrderNo As Integer
    Private myAlignment As System.Windows.Forms.HorizontalAlignment
    'Vorgabe-Reihenfolgenr. für den Fall, dass diese nicht mit angegeben wurde
    Private Shared myDefaultOrderNo As Integer

    Shared Sub New()
        myDefaultOrderNo = 1
    End Sub

    'Alles über Optionale Parameter passt am besten in diesem Fall.
    Sub New(Optional ByVal DisplayName As String = "",
            Optional ByVal PropertyName As String = "",
            Optional ByVal Width As Integer = -2,
            Optional ByVal OrderNo As Integer = -1,
            Optional ByVal Alignment As System.Windows.Forms.HorizontalAlignment = HorizontalAlignment.Left)

        Me.DisplayName = DisplayName
        Me.PropertyName = PropertyName
        Me.Width = Width
        Me.Alignment = Alignment

        If OrderNo = -1 Then
            Me.OrderNo = myDefaultOrderNo
        Else
            If OrderNo > myDefaultOrderNo Then
                myDefaultOrderNo = OrderNo + 1
            Else
                myDefaultOrderNo += 1
            End If
        End If

    End Sub

    'Name des Spaltenkopfs
    Public Property DisplayName() As String

    'Name der Eigenschaft
    Public Property PropertyName As String

    'Spaltenbreite
    Public Property Width() As Integer

    'Sortierschlüssel
    Public Property OrderNo() As Integer

    'Alignment
    Public Property Alignment As System.Windows.Forms.HorizontalAlignment

    'Hält fest, ob das Attribut im Code verwendet oder zur Laufzeit aus dem Schemainfos der Klasse erstellt wurde.
    Public Property ExplicitlyDefined As Boolean
End Class

Public Class SelectedSourceItemEventArgs
    Inherits EventArgs

    Sub New(ByVal selectedSourceItem As Object)
        Me.SelectedSourceItem = selectedSourceItem
    End Sub

    Public Property SelectedSourceItem As Object
End Class

Public Class AutoReportViewColumnsEventArgs
    Inherits EventArgs

    Sub New(ByVal autoReportColumns As AutoReportColumns)
        Me.AutoReportColumns = autoReportColumns
    End Sub

    Public Property AutoReportColumns As AutoReportColumns
End Class