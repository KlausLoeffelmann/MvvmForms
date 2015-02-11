Imports System.Reflection
Imports System.ComponentModel
Imports System.IO

''' <summary>
''' Kapselt Funktionalität, um aus einer beliebigen Datenklasse (dataSource) über String-Funktionalitäten (DisplayMember) 
''' Werte von Eigenschaften in bestimmten Formaten innerhalb von Listen zur Verfügung zu stellen.
''' </summary>
''' <remarks>Verwenden Sie die statische <see cref="ObjectAnalyser.ObjectToString">ObjectToString-Methode</see>, um eine Eigenschaft eines bestimmten Objektes 
''' in seine Zeichenkettenrepräsentation mit einem bestimmten Format umzuwandeln.</remarks>
Public Class ObjectAnalyser
    Implements IListSource

    ''' <summary>
    ''' Interne Hilfsklasse für die ObjectAnalyser-Klasse - nicht für die öffentlichen Verwendung gedacht.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ObjectWrapper
        Private myBoundItem As Object
        Private myGetValMethods() As LambdaMethodWrapper
        Private myFormat As String

        Public Sub New(ByVal obj As Object, ByVal format As String, ByVal getValMethods() As LambdaMethodWrapper)
            myBoundItem = obj
            myGetValMethods = getValMethods
            myFormat = format
        End Sub

        Public Property DataBoundItem As Object
            Get
                Return myBoundItem
            End Get
            Set(value As Object)
                If Not SchemaControlDisabled Then
                    Throw New ArgumentException("As long as the SchemaControlEnabled-Property is not Set, switching the Object to be analysed without genereating new schemabased Func-Delegate-Lists is not possible.")
                End If
                myBoundItem = value
            End Set
        End Property

        Public ReadOnly Property LambdaMethodWrappers As LambdaMethodWrapper()
            Get
                Return myGetValMethods
            End Get
        End Property

        ''' <summary>
        ''' Bestimmt oder ermittelt, ob das DataBoundItem ausgetauscht werden darf, wobei hier aus Performance-Gründen keine Schemaüberprüfung (kein Test auf homogenität) durchgeführt wird.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SchemaControlDisabled As Boolean

        Public Overrides Function ToString() As String
            Dim vals(myGetValMethods.Length - 1) As Object
            Dim i = 0
            For Each valMethod In myGetValMethods
                vals(i) = valMethod.ValueEvaluator.Invoke(myBoundItem)
                i += 1
            Next
            Return String.Format(myFormat, vals)
        End Function
    End Class

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse, ohne DataSource und DisplayMember-Eigenschaften zu bestimmen.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        MyBase.new()
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt DataSource und DisplayMember-Eigenschaften.
    ''' </summary>
    ''' <param name="datasource"></param>
    ''' <param name="displayMember"></param>
    ''' <remarks></remarks>
    Sub New(ByVal datasource As IEnumerable, ByVal displayMember As String)
        Me.DataSource = datasource
        Me.DisplayMember = displayMember
#If DEBUG Then
        DebugChannel = Console.Out
#End If
    End Sub

    ''' <summary>
    ''' Ermittelt die formatierte String-Repräsentation eines Objektes, der sich aus verschiedenen Eigenschaftenwerten zusammensetzen kann.
    ''' </summary>
    ''' <param name="theObject"></param>
    ''' <param name="FormatString"></param>
    ''' <returns></returns>
    ''' <remarks><para>Der DisplayMember wird aus mindestens zwei Teilen als Zeichenfolge angegeben werden, 
    ''' die durch Komma getrennt werden. Der erste Teil - in Anführungszeichen - definiert die Formatierung, 
    ''' der zweite Teil, welche Eigenschaften der Datenquelle angezeigt werden sollen. 
    ''' Eigenschaftennamen werden in geschwifte Klammern gesetzt. Das ist wichtig, um zu einem späteren Zeitpunkt 
    ''' hier noch um Formelauswertungsfunktioanlitäten zu erweitern.
    ''' </para>
    ''' <para>Beispiel: Kundennummer 6-stellig und Betrag mit zwei Nachkomma, Tausendertrennzeichen und Euro-Symbol darstellen:</para>
    ''' <code>
    ''' Me.DisplayMember = """{0:000000}: {1:#,##0.00} €"", {Kundennummer},{Betrag}"
    ''' </code>
    ''' <para>Beispiel: Kundennummer 6-stellig, mit anschließendem Doppelpüunkt, Leerzeichen,
    ''' dann Nachname, Komma und Vorname:</para>
    ''' <code>
    ''' Me.DisplayMember = """{0:000000}: {1}, {2}"", {Kundennummer},{Nachname},{Vorname}"
    ''' </code>
    ''' </remarks>
    Public Shared Function ObjectToString(ByVal theObject As Object, ByVal FormatString As String) As String
        Dim funccalls = GetValMethods(FormatString, theObject, FormatStrEnum.FormatStringRequired, True, Nothing)
        Dim wrapper = New ObjectWrapper(theObject, funccalls.Item1, funccalls.Item2)
        Return wrapper.ToString
    End Function

    ''' <summary>
    ''' Liefert den ObjectWrapper, der die Liste der Func-Delegates für die einzelnen DisplayMember-Auswerter hält, auf Basis des Objektes und des Formatstrings zurück.
    ''' </summary>
    ''' <param name="theObject"></param>
    ''' <param name="formatString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObjectToFunctionWrapper(theObject As Object, formatString As String) As ObjectWrapper
        Dim funccalls = GetValMethods(formatString, theObject, FormatStrEnum.FormatStringRequired, True, Nothing)
        Dim wrapper = New ObjectWrapper(theObject, funccalls.Item1, funccalls.Item2)
        Return wrapper
    End Function

    ''' <summary>
    ''' Liefert Wert und PropertyInfo-Objekt eines Objektes auf Basis einer geschachteteln Eigenschaften-Zeichenketten angabe zurück (e.g.: control.parent.Anchor)
    ''' </summary>
    ''' <param name="theObject">Objekt, dessen Wert und Eigenschafteninfo ermittelt werden soll.</param>
    ''' <param name="NestedPropertyName">String, der den Eigenschaftenpfad bestimmt: (e.g.: "Parent.Anchor" für den Wert der Anchor-Eigenschaft des Parents von theObject, wenn theObjekt ein Steuerelement darstellt)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function PropertyInfoFromNestedPropertyName(ByVal theObject As Object, ByVal NestedPropertyName As String) As Tuple(Of Object, PropertyInfo)
        Dim nestedProperties = NestedPropertyName.Split("."c)
        Dim currentPropertyInfo As PropertyInfo = Nothing   ' Die aktuelle Eigenschaft in der Eigenschaftenkette ...
        Dim currentObject = theObject                       ' ...und das dazu gehörende Objekt.
        Dim previousObject As Object = theObject
        For Each propName In nestedProperties
            'Rausfinden, ob es eine entsprechende Eigenschaft gibt:
            If currentObject IsNot Nothing Then
                currentPropertyInfo = currentObject.GetType.GetProperty(propName)
            Else
                currentPropertyInfo = Nothing
                Exit For
            End If

            If currentPropertyInfo Is Nothing Then
                Return Nothing
            End If
            previousObject = currentObject
            currentObject = currentPropertyInfo.GetValue(currentObject, Nothing)
        Next
        Return New Tuple(Of Object, PropertyInfo)(previousObject, currentPropertyInfo)
    End Function

    ''' <summary>
    ''' Liefert eine Liste mit Objekten auf Basis eines Suchstringszurück.
    ''' </summary>
    ''' <param name="searchString">Der Suchstring mit dem Text, nachdem in der formatierten Werteliste gesucht werden soll.</param>
    ''' <param name="ignoreCaseSensitivity">Bestimmt, ob die Suche Groß-Kleinschreibung berücksichtigen soll, oder nicht.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchExpression(ByVal searchString As String, ByVal ignoreCaseSensitivity As Boolean) As List(Of Object)
        Dim useSearchString = searchString      ' this var can be casesensitiv or all Uppercase
        If ignoreCaseSensitivity Then
            useSearchString = searchString.ToUpper
        End If

        If myDataSource Is Nothing OrElse myDataSource.GetEnumerator Is Nothing Then
            'keine Daten vorhanden, also auch kein Suchergebnis
            Return New List(Of Object)()
        End If
        Dim enu = myDataSource.GetEnumerator
        If Not enu.MoveNext() Then
            Return New List(Of Object)()
        End If
        If String.IsNullOrEmpty(mySearchIn) Then
            ' Kein suchstring festgelegt
            handleError("Es ist kein Suchmuster per SearchIn-Property festgelegt worden.")
            Return Nothing
        End If
        Dim searchRet = GetValMethods(mySearchIn, enu.Current, FormatStrEnum.FormatStringDisallowed, True, Me.DebugChannel)
        Dim functs = searchRet.Item2
        Dim ret As New List(Of Object)
        For Each wrapperItem In myObjectList
            If wrapperItem IsNot Nothing Then
                For Each fkt As Func(Of Object, Object) In (From items In functs Select items.ValueEvaluator).ToArray
                    Dim item = wrapperItem.DataBoundItem
                    Dim val = fkt.Invoke(item)
                    If val IsNot Nothing Then
                        If ignoreCaseSensitivity Then
                            ' compare with uppercase (useSearchString is uppercase)
                            If val.ToString.ToUpper.Contains(useSearchString) Then
                                ret.Add(item)
                            End If
                        Else
                            ' compare casesensitiv (useSearchString is also casesensitiv)
                            If val.ToString.Contains(useSearchString) Then
                                ret.Add(item)
                            End If
                        End If
                    End If
                Next
            End If
        Next
        Return ret
    End Function

    Property DisplayList As List(Of String)

#Region "Auswertung von DisplayMember"
    Private Enum FormatStrEnum
        FormatStringRequired = 0
        FormatStringDisallowed = 1
    End Enum

    Private Shared Function GetValMethods(ByVal paramString As String,
                                          ByVal refObj As Object,
                                          ByVal mode As FormatStrEnum,
                                          ByVal throwExOnError As Boolean,
                                          Optional ByVal debugChannel As TextWriter = Nothing) As Tuple(Of String, LambdaMethodWrapper())
        If paramString Is Nothing Then
            ghandleError("Es wurde kein  paramString angegeben.", debugChannel, throwExOnError)
            Return Nothing
        End If

        'als erstes müsste der Format-String angegeben worden sein
        ' d.h. der Text muss mit " beginnen und mit " enden
        Dim endIdx As Integer = 0
        Dim startIdx = paramString.IndexOf(""""c)

        If startIdx >= 0 Then
            If mode = FormatStrEnum.FormatStringDisallowed Then
                ghandleError("Es wurde ein FormatString angegeben. Das ist nicht erlaubt", debugChannel, throwExOnError)
                ' falls handle Error keine Exception geworfen hat, muss die Methode hier verlassen werden
                Return Nothing
            End If

            If startIdx <> 0 AndAlso paramString.Trim()(0) <> """"c Then
                ghandleError("Der Formatstring muss mit "" beginnen", debugChannel, throwExOnError)
                ' falls handle Error keine Exception geworfen hat, muss die Methode hier verlassen werden
                Return Nothing
            End If

            endIdx = paramString.IndexOf(""""c, startIdx + 1)
            If endIdx < 0 Then
                ghandleError("Der Formatstring wird mit "" geöffnet, aber nicht geschlossen.", debugChannel, throwExOnError)
                ' falls handle Error keine Exception geworfen hat, muss die Methode hier verlassen werden
                Return Nothing
            End If
        Else
            ' " nicht gefunden
            If mode = FormatStrEnum.FormatStringRequired Then
                ghandleError("Es wurde kein FormatString angegeben", debugChannel, throwExOnError)
                ' falls handle Error keine Exception geworfen hat, muss die Methode hier verlassen werden
                Return Nothing
            End If
        End If
        Dim fmtStr = paramString.Substring(0, endIdx).Substring(startIdx + 1)
        Dim valStr As String
        If endIdx > 0 Then
            valStr = paramString.Substring(endIdx + 1)
        Else
            valStr = paramString
        End If
        Dim idxOfComma = valStr.IndexOf(","c)
        If idxOfComma >= 0 Then
            ' erstes Komma entfernen, damit das Array displayMembers nicht mit einem leeren Eintrag beginnt
            valStr = valStr.Substring(idxOfComma + 1)
        End If
        Dim displayMembers As String() = valStr.Split(","c)
        If displayMembers.Length = 0 Then
            ghandleError("Die Displaymember müssen durch Komma seperiert werden. DisplayMember='" & paramString & "'", debugChannel, throwExOnError)
            ' falls handle Error keine Exception geworfen hat, muss die Methode hier verlassen werden
            Return Nothing
        End If

        ' nun können die PropertyMethods der Felder ermittelt werden
        Dim ret(displayMembers.Length - 1) As LambdaMethodWrapper
        Dim i As Integer = 0
        For Each mapEntry In displayMembers
            Dim fieldname = mapEntry.Replace("{", "").Replace("}", "").Trim(" "c)
            If fieldname.IndexOf("("c) > 0 Or fieldname.IndexOf(")"c) > 0 Then
                ' es ist ein funktionsaufruf
                Dim methodName = fieldname.Replace("(", "").Replace(")", "").Trim
                Dim funcCall = (From mi In refObj.GetType.GetMethods()
                             Where mi.Name.ToUpper = methodName.ToUpper).FirstOrDefault
                If funcCall IsNot Nothing Then
                    ret(i) = New LambdaMethodWrapper With {.ValueEvaluator = New Func(Of Object, Object)(
                                                                                Function(inp) As Object
                                                                                    Return funcCall.Invoke(inp, Nothing)
                                                                                End Function),
                                                         .MethodOrPropertyname = fieldname}
                Else
                    ' Methode nicht gefunden
                    ghandleError("Die Methode '" & methodName & "' wurde nicht gefunden(typ='" & refObj.GetType.FullName & "'", debugChannel, throwExOnError)
                    ret(i) = New LambdaMethodWrapper With {.ValueEvaluator = New Func(Of Object, Object)(
                                                                                        Function(inp) As Object
                                                                                            Return fieldname & " not found."
                                                                                        End Function),
                                                         .MethodOrPropertyname = Nothing}
                End If
            Else
                ' es ist eine Property, die abgerufen werden soll
                Dim propName = fieldname
                If propName.Contains("."c) Then
                    Dim tmp As Tuple(Of Object, PropertyInfo)
                    ret(i) = New LambdaMethodWrapper With {.ValueEvaluator = New Func(Of Object, Object)(
                                                                                Function(inp) As Object
                                                                                    tmp = PropertyInfoFromNestedPropertyName(inp, propName)
                                                                                    Return tmp.Item2.GetValue(tmp.Item1, Nothing)
                                                                                End Function),
                                                         .MethodOrPropertyname = fieldname}

                Else
                    Dim propCall = (From pi In refObj.GetType.GetProperties
                                    Where pi.Name.ToUpper = propName.ToUpper).FirstOrDefault
                    If propCall IsNot Nothing Then
                        ret(i) = New LambdaMethodWrapper With {.ValueEvaluator = New Func(Of Object, Object)(
                                                                                Function(inp) As Object
                                                                                    Return propCall.GetValue(inp, Nothing)
                                                                                End Function),
                                                         .MethodOrPropertyname = fieldname}

                    Else
                        'property nicht gefunden
                        ghandleError("Die Property '" & propName & "' wurde nicht gefunden(typ='" & refObj.GetType.FullName & "'", debugChannel, throwExOnError)
                        ret(i) = New LambdaMethodWrapper With {.ValueEvaluator = New Func(Of Object, Object)(
                                                                                            Function(inp) As Object
                                                                                                Return fieldname & " not found."
                                                                                            End Function),
                                                             .MethodOrPropertyname = Nothing}
                    End If
                End If
            End If

            i += 1
        Next
        Return New Tuple(Of String, LambdaMethodWrapper())(fmtStr, ret)
    End Function
#End Region

#Region "Erstellen der Objektliste"

    Private myObjectList As New List(Of ObjectWrapper)

    Private Sub CreateBindingObjects()
        myObjectList.Clear()
        If myDataSource Is Nothing OrElse myDataSource.GetEnumerator Is Nothing Then
            ' keine zu bindenden Daten gefunden
            ' somit auch kein Referenzobject für GetvalMethods
            Return
        End If
        Dim enu = myDataSource.GetEnumerator
        If Not enu.MoveNext() Then
            ' keine Daten vorhanden
            ' somit auch kein Referenzobject für GetvalMethods
            Return
        End If

        Dim bindingRet = GetValMethods(Me.DisplayMember, enu.Current, FormatStrEnum.FormatStringRequired, Me.ThrowExceptions, Me.DebugChannel)
        If bindingRet Is Nothing Then
            ' da war etwas nicht ok
            ' Fehlermeldung wurde bereits über den handleError bekannt gegeben
            Return
        End If

        Dim formatStr = bindingRet.Item1
        Dim getValFuncs = bindingRet.Item2

        For Each Item In DataSource
            Dim ow = New ObjectWrapper(Item, formatStr, getValFuncs)
            myObjectList.Add(ow)
        Next
        ' gepuffertes Suchpattern löschen
        mySearchInMethods = Nothing
    End Sub

#End Region


#Region "Suchfunktionen"
    Private mySearchIn As String
    Private mySearchInMethods As Func(Of Object, Object) = Nothing
    Public Property SearchIn As String
        Get
            Return mySearchIn
        End Get
        Set(ByVal value As String)
            mySearchIn = value
            ' die GetValMethods dürfen erst beim Suchen erstellt werden
            ' hintergrund. Wird der SearchIn-String festgelegt, bevor Daten vorhanden sind,
            ' ist kein Referenztyp vorhanden
            'GetValMethods(mySearchIn,...
            mySearchInMethods = Nothing
        End Set
    End Property

#End Region

#Region "Binding"
    Private myDisplayMember As String
    Public Property DisplayMember As String
        Get
            Return myDisplayMember
        End Get
        Set(ByVal value As String)
            myDisplayMember = value
            CreateBindingObjects()
            OnBindingChanged(New BindingChangedEventArgs())
        End Set
    End Property

    Private myDataSource As IEnumerable
    Public Property DataSource As IEnumerable
        Get
            Return myDataSource
        End Get
        Set(ByVal value As IEnumerable)
            myDataSource = value
            CreateBindingObjects()
            OnBindingChanged(New BindingChangedEventArgs())
        End Set
    End Property

    Public Event BindingChanged As EventHandler(Of BindingChangedEventArgs)

    Protected Sub OnBindingChanged(ByVal e As BindingChangedEventArgs)
        RaiseEvent BindingChanged(Me, e)
    End Sub
#End Region

#Region "Fehlerhandling"

    Public Property ThrowExceptions As Boolean
    Public Property DebugChannel As TextWriter = Nothing

    Private Shared Sub ghandleError(ByVal ex As Exception, ByVal debugChannel As TextWriter, ByVal throwExceptionOnError As Boolean)
        If debugChannel IsNot Nothing Then debugChannel.WriteLine("Fehler im ObjectAnalyser: " & ex.Message)
        If throwExceptionOnError Then
            Throw ex
        End If
    End Sub

    Private Shared Sub ghandleError(ByVal msg As String, ByVal debugChannel As TextWriter, ByVal throwExceptionOnError As Boolean)
        ghandleError(New Exception(msg), debugChannel, throwExceptionOnError)
    End Sub

    Private Sub handleError(ByVal ex As Exception)
        ObjectAnalyser.ghandleError(ex, Me.DebugChannel, Me.ThrowExceptions)
    End Sub

    Private Sub handleError(ByVal msg As String)
        ObjectAnalyser.ghandleError(msg, Me.DebugChannel, Me.ThrowExceptions)
    End Sub


#End Region

#Region "IListSource Interface"
    Public ReadOnly Property ContainsListCollection As Boolean Implements System.ComponentModel.IListSource.ContainsListCollection
        Get
            Return True
        End Get
    End Property

    Public Function GetList() As System.Collections.IList Implements System.ComponentModel.IListSource.GetList
        Return myObjectList
    End Function
#End Region
End Class

Public Class LambdaMethodWrapper

    Property ValueEvaluator As Func(Of Object, Object)
    Property MethodOrPropertyname As String

End Class

''' <summary>
''' Stellt Ereignisparameter für das BindingChanged-Ereignis zur Verfügung.
''' </summary>
''' <remarks></remarks>
Public Class BindingChangedEventArgs
    Inherits EventArgs

    Public Sub New()
        MyBase.New()
    End Sub
End Class
