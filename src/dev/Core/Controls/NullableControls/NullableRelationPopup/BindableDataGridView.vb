Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Objects.DataClasses
Imports System.ComponentModel.Design
Imports System.Runtime.InteropServices

''' <summary>
''' Stellt Funktionalitäten bereit für das Anzeigen von Daten in einer DataGridView bereit, die gleichzeitig an 
''' mit der <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Komponente verwaltbar sind.
''' </summary>
''' <remarks>Diese Komponente kann direkt verwendet werden, ist aber eigentlich eine Subkomponente des 
''' <see cref="NullableValueRelationPopup">NullableValueRelationPopup-Steuerelementes.</see></remarks>
<ToolboxItem(False)>
Public Class BindableDataGridView
    Inherits DataGridView
    Implements INullableValueRelationBinding, IPermissionManageableUIContentElement

    Private myIsDirty As Boolean
    Private myValueMember As String
    Private myValueIsList As Boolean
    Private myRestoreValue As Object
    Private myIsLoading As HistoricalBoolean        ' Flag, das bestimmt, ob die DataGridView gerade neue Daten bekommt, damit SelectedValueChanged ignoriert werden kann.
    Private myGroupName As String
    Private myBindingSource As BindingSource

    Private Const WM_SETREDRAW = 11
    Private myBeginUpdateCounter As Integer             ' Updatezähler für BeginUpdate/EndUpdate (Unterdrückung von WM_PAINT).
    Private myCurrentlySelectedValue As Object          ' Zwischenspeicher für den selektierten Wert (eingeführt, da wir aus Performance-Gründen mit wechselnden, gefilterten DataSource-Objekten arbeiten)
    Private myCurrentlySelectedValueObject As Object    ' Selbe wie oben, nur als Objekt und nicht als ValueMember ausgelöst.
    Private myIsResettingDatasource As Boolean          ' Flag, das bestimmt, ob durch das notwendiggewordene Austauschen der DataSources beim Filtern dieser Vorgang gerade passiert.
    Private myValueBaseBeforeSelectedChanged As Object
    Private myIgnoreSelectionChange As Boolean
    Private myLastColumnSchema As DataGridViewColumnFieldnames
    Private myDontUpdateValueMember As Boolean          ' Flag, das verhindert, dass es beim Setzen zwischen ValueMember und ValueList zu einer Rekursion kommt.

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As IntPtr
    End Function

    ''' <summary>
    ''' Wird ausgelöst, wenn das Steuerelement dem Entwickler die Möglichkeit gibt, die Schema-Informationen des angezeigten 
    ''' Grids anzupassen.
    ''' </summary>
    ''' <param name="sender">Steuerelement, das das Ereignis ausgelöst hat.</param>
    ''' <param name="e">EventArgs-Parameter, mit denen festgelegt werden kann, welche Spalten angezeigt werden sollen.</param>
    ''' <remarks></remarks>
    Public Event GetColumnSchema(ByVal sender As Object, ByVal e As GetColumnSchemaEventArgs)

    ''' <summary>
    ''' Raised, if ThrowLayoutException is set to false and there was an exception on auto-layouting the BindableDataGrid.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Event BindableGridLayoutException(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird ausgelöst, wenn sich der Wert im Steuerelement geändert hat, um einen einbindenden Formular oder 
    ''' User Control die Möglichkeit zu geben, den Benutzer zu informieren, dass er Änderungen speichern muss.
    ''' </summary>
    ''' <param name="sender">Steuerelement, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event IsDirtyChanged(ByVal sender As Object, ByVal e As IsDirtyChangedEventArgs) Implements INullableValueDataBinding.IsDirtyChanged

    ''' <summary>
    ''' Wird ausgelöst, kurz bevor sich der Wert der Value-Eigenschaft ändert, sodass die Möglichkeit besteht, in die Werteänderung einzugreifen.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Ereignisparameter vom Typ ValueChangingEventArgs, mit denen in die Wertezuweisung eingegriffen werden kann.</param>
    ''' <remarks></remarks>
    Public Event ValueChanging(ByVal sender As Object, ByVal e As ValueChangingEventArgs(Of Object))
    ''' <summary>
    ''' Wird ausgelöst, nachdem sich der Wert der Value-Eigenschaft geändert hat.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e">Ereignisparameter vom Typ ValueChangedEventArgs, mit denen der Grund für die Werteänderung ermittelt werden kann.</param>
    ''' <remarks></remarks>
    Public Event ValueChanged(ByVal sender As Object, ByVal e As ValueChangedEventArgs) Implements INullableValueRelationBinding.ValueChanged

    ''' <summary>
    ''' Wird ausgelöst, wenn sich der selektierte Wert in der Liste geändert hat.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements INullableValueRelationBinding.SelectedValueChanged

    ''' <summary>
    ''' Wird ausgelöst, wenn bei der Zuweisung der Value-Eigenschaft ein Wert erkannt wurde, der es nicht ermöglich, den entsprechenden Eintrag in der Liste zu selektieren.
    ''' </summary>
    ''' <param name="sender">Objekt, dass das Ereignis ausgelöst hat.</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event UnassignableValueDetected(ByVal sender As Object, ByVal e As UnassignableValueDetectedEventArgs)

    'Der ist nur zur Interface-Richtlinien-Erfüllung da, und wird hier nie benutzt, da es keinen Zustand gibt,
    'bei dem dieses Steuerelement den Focus behalten könnte.
    Public Event RequestValidationFailedReaction(ByVal sender As Object, ByVal e As RequestValidationFailedReactionEventArgs) Implements INullableValueDataBinding.RequestValidationFailedReaction

    Sub New()
        Me.GroupName = NullableControlManager.GetInstance.GetDefaultGroupName(Me, "Default")
        Me.ExceptionBalloonDuration = NullableControlManager.GetInstance.GetDefaultExceptionBalloonDuration(Me, 5000)
        Me.OnUnassignableValueAction = NullableControlManager.GetInstance.GetDefaultOnUnassignableValueAction(Me, UnassignableValueAction.TryHandleUnassignableValueDetectedEvent)
        Me.UIGuid = Guid.NewGuid
        Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.DoubleBuffered = True

        myBindingSource = New BindingSource
        myBindingSource.AllowNew = False

        AddHandler Me.DataBindingComplete, Sub(sender As Object, e As DataGridViewBindingCompleteEventArgs)
                                               Dim e2 As New GetColumnSchemaEventArgs(New DataGridViewColumnFieldnames)
                                               OnGetColumnSchema(e2)
                                               If e2.AutoSizeColumnsMode <> 0 Then
                                                   Me.AutoSizeColumnsMode = e2.AutoSizeColumnsMode
                                               End If

                                               If e2 IsNot Nothing AndAlso e2.SchemaFieldnames.Count > 0 Then
                                                   myLastColumnSchema = e2.SchemaFieldnames

                                                   For Each column As DataGridViewColumn In Me.Columns
                                                       If e2.SchemaFieldnames.Contains(column.Name) Then
                                                           Try
                                                               column.HeaderText = e2.SchemaFieldnames(column.Name).DisplayName
                                                               column.DisplayIndex = e2.SchemaFieldnames(column.Name).OrdinalNo
                                                               column.AutoSizeMode = e2.SchemaFieldnames(column.Name).AutoSizeColumnMode
                                                               If e2.SchemaFieldnames(column.Name).FillWeight > 0 Then
                                                                   column.FillWeight = e2.SchemaFieldnames(column.Name).FillWeight
                                                                   Me.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                                                               End If
                                                               If e2.SchemaFieldnames(column.Name).FixedWidth.HasValue Then
                                                                   column.Width = e2.SchemaFieldnames(column.Name).FixedWidth.Value
                                                                   column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                               End If
                                                           Catch ex As Exception
                                                               Dim bdgEx As New BindableDataGridLayoutException("Layouting the DataGrid caused an internal exception." & vbNewLine &
                                                                                                                "Please check, if you've handeled the BindableDataGrid's GetColumnSchema event correctly!",
                                                                                                                e2.SchemaFieldnames(column.Name).FixedWidth,
                                                                                                                ex)
                                                               OnBindableDataGridLayoutException(bdgEx)
                                                           End Try
                                                       Else
                                                           column.Visible = False
                                                           'column.DisplayIndex = Me.Columns.Count - column.DisplayIndex
                                                       End If
                                                   Next
                                               End If
                                           End Sub
    End Sub

    Protected Overridable Sub OnBindableDataGridLayoutException(ex As BindableDataGridLayoutException)
        If ThrowLayoutException Then
            If ex IsNot Nothing Then
                Throw ex
            End If
        Else
            Dim layoutExceptionArgs As New BindableDataGridLayoutExceptionEventArgs(ex)
            RaiseEvent BindableGridLayoutException(Me, layoutExceptionArgs)
        End If
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        myRestoreValue = Me.Value
        myIgnoreSelectionChange = True
        MyBase.OnHandleCreated(e)
        myIgnoreSelectionChange = False
    End Sub

    ''' <summary>
    ''' Findet die nächste sichtbare Zeile ausgegangen von der aktuell selektierten Zeile oder Zeile 0.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextVisibleRow() As DataGridViewRow

        Dim startIndex As Integer

        If Me.SelectedRows.Count = 0 Then
            startIndex = 0
        Else
            startIndex = Me.SelectedRows(0).Index + 1
        End If

        If startIndex >= Me.Rows.Count Then
            Return Nothing
        End If

        For indexCount = startIndex To Me.Rows.Count - 1
            If Me.Rows(indexCount).Visible Then
                Return Me.Rows(indexCount)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Findet die erste sichtbare Spalte in der angegebenen Zeile oder liefert nothing (null) zurück.
    ''' </summary>
    ''' <param name="row">Die Zeile, in der die erste sichtbare Spalte gefunden werden soll.</param>
    ''' <param name="startIndex">Die Spaltennummer, ab der gesucht werden soll.</param>
    ''' <returns>Die erste sichtbare Spalte.</returns>
    ''' <remarks></remarks>
    Public Function FirstVisibleColumnInRow(row As DataGridViewRow, Optional startIndex As Integer = 0) As DataGridViewColumn
        If row IsNot Nothing Then
            For indexCount = startIndex To row.Cells.Count - 1
                If row.Cells(indexCount).OwningColumn.Visible Then
                    Return row.Cells(indexCount).OwningColumn
                End If
            Next
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Findet die vorherige sichtbare Zeile ausgegangen von der aktuell selektierten Zeile oder Zeile 0.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindPreviousVisibleRow() As DataGridViewRow

        Dim startIndex As Integer

        If Me.SelectedRows.Count = 0 Then
            startIndex = Me.Rows.Count - 1
        Else
            startIndex = Me.SelectedRows(0).Index - 1
        End If

        If startIndex < 0 Then
            Return Nothing
        End If

        For indexCount = startIndex To 0 Step -1
            If Me.Rows(indexCount).Visible Then
                Return Me.Rows(indexCount)
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Ermittelt die Anzahl der sichtbaren Spalten.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property VisibleColumnCount As Integer
        Get
            If Me.Columns IsNot Nothing Then
                Return (From item In Me.Columns Where DirectCast(item, DataGridViewColumn).Visible = True).Count
            Else
                Return 0
            End If
        End Get
    End Property

    Public Sub BeginUpdate()
        If Not Me.IsHandleCreated Then
            Return
        End If

        myBeginUpdateCounter += 1
        If myBeginUpdateCounter = 1 Then
            SendMessage(Me.Handle, WM_SETREDRAW, 0, 0)
        End If
        'myBindingSource.RaiseListChangedEvents = False
    End Sub

    Public Sub EndUpdate()
        If Not Me.IsHandleCreated Then
            Return
        End If

        If myBeginUpdateCounter = 0 Then Return
        myBeginUpdateCounter -= 1
        If myBeginUpdateCounter = 0 Then
            SendMessage(Me.Handle, WM_SETREDRAW, 1, 0)
            'myBindingSource.RaiseListChangedEvents = True
        End If
    End Sub

    Public Sub UpdateRowFromDataSource(rowNo As Integer)
        If myBindingSource IsNot Nothing Then
            Me.myBindingSource.ResetItem(rowNo)
        End If
    End Sub

    Protected Sub OnGetColumnSchema(ByVal e As GetColumnSchemaEventArgs)
        RaiseEvent GetColumnSchema(Me, e)
    End Sub

    <AttributeProvider(GetType(IListSource))>
    Shadows Property DataSource As Object Implements INullableValueRelationBinding.Datasource
        Get
            Return myBindingSource.DataSource
        End Get
        Set(ByVal value As Object)
            'Alle nothing, gibt nix zu ändern.
            If value Is Nothing And myBindingSource.DataSource Is Nothing Then
                Return
            End If

            'Beides nicht nothing, aber Werte identisch, gibt auch nix zu ändern.
            If value IsNot Nothing And myBindingSource.DataSource IsNot Nothing Then
                If value.Equals(myBindingSource.DataSource) Then
                    Return
                End If
            End If

            'Value, falls selektiert, zurücksetzen.
            If Me.Value IsNot Nothing Then
                Me.Value = Nothing
            End If

            Me.IsLoading.Value = True
            myBindingSource.DataSource = value
            MyBase.DataSource = myBindingSource
            If Me.PreventFirstRowSelection Then
                If Me.CurrentCell IsNot Nothing Then
                    Me.CurrentCell.Selected = False
                End If
            End If
            Me.IsLoading.Value = False
        End Set
    End Property

    Public Property PreventFirstRowSelection As Boolean

    ''' <summary>
    ''' Setzt die Datasource neu ohne dabei den Value zurückzusetzen
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Friend Sub ResetDatasource(value As Object)
        myIsResettingDatasource = True
        Me.DataSource = value
        myIsResettingDatasource = False
    End Sub

    Public Sub AddRow(rowData As Object)
        'Kritischer Bereich
        If myBindingSource Is Nothing Then
            Throw New ArgumentNullException("Cannot add row data until the DataSource property has been set initially!")
        End If
        myBindingSource.Add(rowData)
    End Sub

    Public Sub ClearRows()
        If myBindingSource IsNot Nothing Then
            myBindingSource.Clear()
        End If
    End Sub

    Protected Overrides Sub OnCreateControl()
        IsLoading.Value = True
        MyBase.OnCreateControl()
        IsLoading.Value = False
    End Sub

    Protected Overrides Sub OnBindingContextChanged(e As System.EventArgs)
        myIgnoreSelectionChange = True
        MyBase.OnBindingContextChanged(e)
        Me.ClearSelection()
        myIgnoreSelectionChange = False
    End Sub

    Protected Overrides Sub OnSelectionChanged(ByVal e As System.EventArgs)
        If myIgnoreSelectionChange Then
            Return
        End If
        myValueBaseBeforeSelectedChanged = Me.ValueBase
        MyBase.OnSelectionChanged(e)
        CommitValuesInternally()
        OnSelectedValueChanged(e)
        If Not Me.IsLoading.Value Then
            If myValueBaseBeforeSelectedChanged IsNot Nothing AndAlso Not myValueBaseBeforeSelectedChanged.Equals(Me.ValueBase) Then
                OnValueChanged(New ValueChangedEventArgs)
            End If
            If Not IsDirty Then
                IsDirty = True
            End If
        End If
    End Sub

    Friend Sub CommitValuesInternally()

        If myIsResettingDatasource Then Return

        If Me.SelectedRows.Count > 0 Then
            myCurrentlySelectedValueObject = Me.SelectedRows(0).DataBoundItem
            If Not String.IsNullOrWhiteSpace(ValueMember) Then
                myCurrentlySelectedValue = GetValueFromValueMember()
                Return
            Else
                myCurrentlySelectedValue = myCurrentlySelectedValueObject
                Return
            End If
        End If
        myCurrentlySelectedValue = Nothing
        myCurrentlySelectedValueObject = Nothing
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.IsLoading.Value = True
        MyBase.Dispose(disposing)
        Me.IsLoading.Value = False
    End Sub

    Protected Overridable Sub OnSelectedValueChanged(ByVal e As EventArgs)
        RaiseEvent SelectedValueChanged(Me, e)
    End Sub

    Protected Overridable Sub OnValueChanged(ByVal e As ValueChangedEventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

    Protected Overridable Sub OnValueChanging(ByVal e As ValueChangingEventArgs(Of Object))
        RaiseEvent ValueChanging(Me, e)
    End Sub

    Protected Overridable Sub OnIsDirtyChanged(ByVal e As IsDirtyChangedEventArgs)
        RaiseEvent IsDirtyChanged(Me, e)
    End Sub

    Public Property DatafieldDescription As String Implements INullableValueDataBinding.DatafieldDescription
    Public Property DatafieldName As String Implements INullableValueDataBinding.DatafieldName
    Public Property NullValueMessage As String Implements INullableValueDataBinding.NullValueMessage

    ''' <summary>
    ''' Bestimmt oder ermittelt den Namen der Eigenschaft, deren Inhalt die Value-Eigenschaft 
    ''' beim Auswählen eines Elements der Liste zurückliefert.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks><para>Ist diese Eigenschaft nicht gesetzt, wird das komplette Listenelement zurückgeliefert. 
    ''' Falls keines selektiert ist, liefert diese Eigenschaft Null (Nothing in VB) zurück.  
    ''' Falls diese Eigenschaft gesetzt ist, versucht das Steuerelement den Inhalt der entsprechenden Eigenschaft 
    ''' zu ermitteln, und liefert diesen über die Value-Eigenschaft zurück.
    ''' </para>
    ''' <para>Wichtig: Die Eigenschaft, die verwendet werden soll, sollte auf jedenfall eindeutige (also keine doppelten) 
    ''' Werte in der Liste zurückliefern, da beim Zuweisen sonst u.U. falsche Elemente in der Liste selektiert werden.</para>
    ''' </remarks>
    Public Property ValueMember As String Implements INullableValueRelationBinding.ValueMember
        Get
            Return myValueMember
        End Get
        Set(ByVal value As String)
            If value <> myValueMember Then
                myDontUpdateValueMember = True
                If Not String.IsNullOrWhiteSpace(value) Then
                    Me.ValueIsList = False
                Else
                    Me.ValueIsList = True
                End If
                myDontUpdateValueMember = False

                myValueMember = value

                If Me.DataSource IsNot Nothing Then
                    CommitValuesInternally()
                    myRestoreValue = Me.Value
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Value-Eigenschaft als Navigations-Eigenschaft angesehen werden soll, 
    ''' also die ganze DatenQuelle (DateSource) zurückliefert (true) oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Die Standardeinstellung ist false. Wenn Dieser Wert auf True gesetzt ist, 
    ''' wird die ValueMember-Eigenschaft ignoriert.</remarks>
    <DefaultValue(False)>
    Public Property ValueIsList As Boolean
        Get
            Return myValueIsList
        End Get
        Set(ByVal value As Boolean)
            If value <> myValueIsList Then
                If value Then
                    If Not myDontUpdateValueMember Then
                        ValueMember = ""
                    End If
                End If
                myValueIsList = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt den Wert dieses Steuerelements, der sich aus dem seletkierten Elemente des Grids ergibt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Ist die ValueMember-Eigenschaft dieser Instanz nicht gesetzt, wird das komplette Listenelement, das selektiert ist, zurückgeliefert. 
    ''' Falls keines selektiert ist, liefert diese Eigenschaft Null (Nothing in VB) zurück.  
    ''' Falls die ValueMember-Eigenschaft gesetzt ist, versucht das Steuerelement den Inhalt der entsprechenden Eigenschaft 
    ''' zu ermitteln, und liefert diesen über die Value-Eigenschaft zurück.<para></para>
    ''' Beim Setzen der Value-Eigenschaft versucht das Steuerelement, das Element in der Liste zu selektieren, das 
    ''' dem Eigenschaftenwert entspricht. Ist die ValueMember-Eigenschaft gesetzt, wird die gesamte Liste nach dem 
    ''' angegebenen Wert in der durch ValueMember definierten Eigenschaft der einzelnen Elemente durchsucht.
    ''' </remarks>
    <Browsable(False)>
    Public Overridable Property Value As Object Implements INullableValueDataBinding.Value
        Get
            If ValueIsList Then
                Return Me.DataSource
            Else
                Return myCurrentlySelectedValue
            End If
        End Get

        Set(ByVal value As Object)

            If value Is Nothing And Me.Value Is Nothing Then
                Return
            End If

            If value IsNot Nothing AndAlso Me.Value IsNot Nothing Then
                If value.Equals(Me.Value) Then
                    Return
                End If
            End If

            Dim e As New ValueChangingEventArgs(Of Object)(value)
            Me.OnValueChanging(e)

            value = e.NewValue

            If ValueIsList Then
                Me.DataSource = value
            Else
                If Me.Rows IsNot Nothing And Me.Rows.Count > 0 Then
                    If value Is Nothing Then
                        IsLoading.Value = True
                        Me.ClearSelection()
                        IsLoading.Value = False
                        OnValueChanged(New ValueChangedEventArgs(ValueChangedCauses.PropertySetter))
                        Return
                    End If

                    'Feststellen, ob es ein Entitätsobjekt ist:
                    If GetType(EntityObject).IsAssignableFrom(value.GetType) Then
                        IsLoading.Value = True
                        SetValueForEntityObject(DirectCast(value, EntityObject))
                        OnValueChanged(New ValueChangedEventArgs(ValueChangedCauses.PropertySetter))
                        IsLoading.Value = False
                        Return
                    End If

                    'Muss Value-Member berückschtigt werden?
                    If Not String.IsNullOrWhiteSpace(ValueMember) Then
                        'ValueMember ist gesetzt:
                        IsLoading.Value = True
                        If SetValueFromValueMember(value) = -1 Then
                            If Me.OnUnassignableValueAction = UnassignableValueAction.TryHandleUnassignableValueDetectedEvent Then
                                Dim eV As New UnassignableValueDetectedEventArgs(False, value)
                                OnUnassignableValueDetected(eV)
                                If eV.ExceptionHandled And eV.NewValue Is Nothing Then
                                    Me.ClearSelection()
                                ElseIf Not (eV.ExceptionHandled And SetValueFromValueMember(eV.NewValue) <> -1) Then
                                    Dim paraName = "Value wurde in der Ergebnisliste nicht gefunden. (Value=" & value.ToString & ")"
                                    Dim up As New UnassignableValueException(paraName, eV.NewValue, "Der zugewiesene Value-Wert hatte keine Entsprechung in der Popup-Objektliste." & vbNewLine &
                                                                              "Behandeln Sie das ValueChanging- oder das UnassignableValueDetected-Ereignis, um bestimmte Werte auszutauschen, " & vbNewLine &
                                                                              "oder setzen Sie die OnUnassigableValueAction-Eigenschaft auf eine andere Aktion." & vbNewLine & vbNewLine &
                                                                              "Falls Sie das UnassignableValueDetected-Ereignis behandelt haben, prüfen Sie, ob Sie e.ExceptionHandled=true im Ereignishandler gesetzt haben!", Me)
                                    IsLoading.Value = False
                                    Throw up
                                    Return
                                End If
                            ElseIf Me.OnUnassignableValueAction.HasFlag(UnassignableValueAction.SelectNothing) Then
                                Me.ClearSelection()
                            ElseIf Me.OnUnassignableValueAction.HasFlag(UnassignableValueAction.SelectFirstInList) Then
                                If Me.RowCount > 0 Then
                                    SelectRow(Me.Rows(0))
                                End If
                            End If

                            If Me.OnUnassignableValueAction.HasFlag(UnassignableValueAction.ThrowException) Then
                                Dim paraName = "Value wurde in der Ergebnisliste nicht gefunden. (Value=" & value.ToString & ")"
                                Dim up As New UnassignableValueException(paraName, value, "Der zugewiesene Value-Wert hatte keine Entsprechung in der Popup-Objektliste." & vbNewLine &
                                                                          "Behandeln Sie das ValueChanging- oder das UnassinableValueDetected-Ereignis, um bestimmte Werte auszutauschen, " & vbNewLine &
                                                                          "oder setzen Sie die OnUnassigableValueAction-Eigenschaft auf eine andere Aktion.", Me)
                                IsLoading.Value = False
                                Throw up
                            End If
                        End If
                        IsLoading.Value = False
                    Else
                        'If Debugger.IsAttached Then
                        '    Debugger.Break()
                        'End If

                        'ValueMember nicht nicht gesetzt:
                        Dim notFound = True
                        For Each row As DataGridViewRow In Me.Rows
                            If Object.Equals(row.DataBoundItem, value) Then
                                SelectRow(row)
                                notFound = False
                                Exit For
                            End If
                        Next

                        If notFound Then
                            Dim valueAsString = If(value Is Nothing, "* - - - *", value.ToString)
                            Dim paraName = "Value wurde in der Ergebnisliste nicht gefunden. (Value=" & valueAsString & ")"
                            Dim up As New UnassignableValueException(paraName, valueAsString, "Der zugewiesene Value-Wert hatte keine Entsprechung in der Popup-Objektliste." & vbNewLine &
                                                                      "Behandeln Sie das ValueChanging-Ereignis, um bestimmte Werte auszutauschen.", Me)
                            Throw up
                        End If
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Definiert die angegebene Zeile als aktuelle Zeile (damit sie entsprechend in den sichtbaren Bereich gescrollt wird).
    ''' </summary>
    ''' <param name="row">Zeile, die zur aktuellen Zeile erklärt werden soll.</param>
    ''' <remarks></remarks>
    Friend Sub SelectRow(row As DataGridViewRow)
        If row IsNot Nothing Then
            row.Selected = True
            CommitValuesInternally()
            If row.Cells IsNot Nothing AndAlso row.Cells.Count > 0 Then
                Dim visibleColumn = FirstVisibleColumnInRow(row)
                Me.CurrentCell = row.Cells(visibleColumn.Index)
            End If
        End If
    End Sub

    Friend Function SelectValue(value As Object) As Boolean
        If value Is Nothing Then
            ClearSelection()
            Return True
        Else
            For Each row As DataGridViewRow In Me.Rows
                If row.DataBoundItem Is value Then
                    SelectRow(row)
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    Protected Overridable Sub OnUnassignableValueDetected(ByVal e As UnassignableValueDetectedEventArgs)
        RaiseEvent UnassignableValueDetected(Me, e)
    End Sub

    <Browsable(False)>
    Public Property ValueBase As Object
        Get
            Return myCurrentlySelectedValueObject
        End Get

        Set(ByVal value As Object)
            For Each row As DataGridViewRow In Me.Rows
                If row.DataBoundItem Is value Then
                    SelectRow(row)
                    Exit For
                End If
            Next
        End Set
    End Property

    Public Property IsDirty As Boolean Implements INullableValueDataBinding.IsDirty
        Get
            Return myIsDirty
        End Get
        Private Set(value As Boolean)
            If value <> myIsDirty Then
                OnIsDirtyChanged(New IsDirtyChangedEventArgs(Me))
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die Maske, die das Steuerelement enthält, gerade die Steuerelemente mit Daten befüllt, oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Das Setzen dieser Eigenschaft kann nur über die Schnittstelle (in der Regel von der Maskensteuerung) vorgenommen werden.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(False)>
    Public Property IsLoading As HistoricalBoolean Implements INullableValueDataBinding.IsLoading
        Get
            If myIsLoading Is Nothing Then
                myIsLoading = New HistoricalBoolean With {.Value = False}
            End If
            Return myIsLoading
        End Get
        Set(ByVal value As HistoricalBoolean)
            If value Is Nothing Then
                myIsLoading = New HistoricalBoolean With {.Value = False}
            Else
                myIsLoading = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Setzt den Status zurück, dass dieses Feld vom Anwender geändert wurde, und sein Value in der Datenquelle aktualisiert werden sollte.
    ''' </summary>
    ''' <remarks>Diese Methode wird von der Infrastruktur verwendet, und sollte nicht direkt angewendet werden.</remarks>
    Public Sub ResetIsDirty() Implements INullableValueDataBinding.ResetIsDirty
        myIsDirty = False
    End Sub

    Private Function GetValueFromValueMember() As Object
        Dim propInfo = myCurrentlySelectedValueObject.GetType.GetProperty(ValueMember.Trim)
        If propInfo Is Nothing Then
            Dim up As New MissingMemberException("Die '" & ValueMember &
                "'-Eigenschaft konnte nicht gefunden werden. Überprüfen Sie die korrekte Schreibweise in der ValueMember-Eigenschaft.")
            Throw up
        End If

        Return propInfo.GetValue(myCurrentlySelectedValueObject, Nothing)
    End Function

    'Selektiert die Zeile, dessen durch ValueMember definierte Eigenschaft den Wert hat, der hier 
    'als value übergeben wird, und liefert die Nummer der Zeile zurück, die selektiert wurde.
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId:="weisen")>
    Private Function SetValueFromValueMember(ByVal value As Object) As Integer

        'Keine Elemente, nix selektiert.
        If Me.Rows.Count = 0 Then
            Return -1
        End If

        'Erstmal feststellen, ob es die Eigenschaft an sich überhaupt gibt.
        'Dazu schauen wir uns die erste Zeile an.
        Dim selObj = Me.Rows(0).DataBoundItem
        Dim propInfo = selObj.GetType.GetProperty(ValueMember)

        If propInfo Is Nothing Then
            Dim up As New MissingMemberException("Die '" & ValueMember & "'-Eigenschaft konnte nicht gefunden werden. Überprüfen Sie die korrekte Schreibweise in der ValueMember-Eigenschaft.")
            Throw up
        End If

        'Rausfinden, ob die beiden Typen dieselben sind!
        If value.GetType IsNot propInfo.PropertyType Then
            If Me.EnforceValueMemberTypeSafety Then
                Dim up As New TypeMismatchException("Der neue Wert der Value-Eigenschaft und die Eigenschaft " &
                                                    "'" & ValueMember & "' die durch die ValueMember Eigenschaft definiert wurde " &
                                                    "weisen unterschiedliche Typen auf und lassen sich nicht vergleichen.")
                Throw up
            Else
                'Versuchen, eine Typkonvertierung vorzunehmen
                Try
                    value = CTypeDynamic(value, propInfo.PropertyType)
                    If value Is Nothing Then
                        Throw New NullReferenceException("Implizierte Typkonvertierung aufgrund von EnforceValueMemberTypeSafety=false ist fehlgeschlagen; Typkonvertierung nicht möglich.")
                    End If
                Catch ex As Exception
                    Dim up As New TypeMismatchException("Der neue Wert der Value-Eigenschaft und die Eigenschaft " &
                                                        "'" & ValueMember & "' die durch die ValueMember Eigenschaft definiert wurde " &
                                                        "weisen unterschiedliche Typen auf und lassen sich nicht vergleichen.", ex)
                End Try
            End If
        End If

        Dim equilityComparer As Func(Of Object, Boolean) = If(GetType(IComparable).IsAssignableFrom(propInfo.PropertyType),
                                                   Function(funcValue As Object) As Boolean
                                                       Return DirectCast(funcValue, IComparable).CompareTo(value) = 0
                                                   End Function,
                                                   Function(funcValue As Object) As Boolean
                                                       Return (funcValue Is value)
                                                   End Function)

        'Nach Recherche bezüglich performance-problemen wurde das Suchen im Grid gegen die Suche in der BindingSource geändert.
        'hier ist zu bedenken, dass evtl. die BindingSource durch eine Eingabe eines Filters nicht mehr zu der im Grid passt, was evtl. zu
        'Problemen beim Zuweisen eines Values führen kann.
        For i = 0 To myBindingSource.Count - 1
            Dim tmpitem = myBindingSource.Item(i)


            Dim propValue = propInfo.GetValue(tmpitem, Nothing)
            If equilityComparer(propValue) Then
                SelectRow(Me.Rows(i))
                Return i
            End If
        Next
        Return -1
    End Function

    Private Function SetValueForEntityObject(ByVal Value As EntityObject) As Integer
        'Erstmal feststellen, ob es die Eigenschaft an sich überhaupt gibt.
        'Dazu schauen wir uns die erste Zeile an.
        Dim listType = Me.Rows(0).DataBoundItem.GetType

        'Rausfinden, ob die beiden Typen dieselben sind!
        If Value.GetType IsNot listType Then
            Dim up As New TypeMismatchException("Der neue Wert der Value-Eigenschaft und die Eigenschaft " &
                                                    "'" & ValueMember & "' die durch die ValueMember Eigenschaft definiert wurde " &
                                                    "weisen unterschiedliche Typen auf und lassen sich nicht vergleichen.")
            Throw up
        End If

        For tmpRowCount = 0 To Me.Rows.Count - 1
            Dim tmpEntityItem = DirectCast(Me.Rows(tmpRowCount).DataBoundItem, EntityObject)

            Dim equal = True
            For keyCount = 0 To Value.EntityKey.EntityKeyValues.Count - 1
                Dim icomp = TryCast(Value.EntityKey.EntityKeyValues(keyCount).Value, IComparable)
                If icomp Is Nothing Then
                    Return -1
                End If

                If icomp.CompareTo(tmpEntityItem.EntityKey.EntityKeyValues(keyCount).Value) <> 0 Then
                    equal = False
                    Exit For
                End If
            Next

            If equal Then
                SelectRow(Me.Rows(tmpRowCount))
                Return tmpRowCount
            End If
        Next

        Return -1
    End Function

    ''' <summary>
    ''' Wert der Value-Eigenschaft, der galt, bevor das Window-Handle dieses Steuerelementes erstellt war.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Über dieser Eigenschaft wird der Inhalt der Value-Eigenschaft zwischengespeichert, der eingestellt war, 
    ''' bevor das Handle dieses Steuerelementes erzeugt wurde. Auch wenn das Handle dieses Steuerelementes noch nicht erstellt 
    ''' wurde (es also noch nicht sichtbar war), lässt sich schon auf die entsprechenden Eigenschaften zugreifen. Das Erstellen 
    ''' des Steuerelementes sorgt aber leider dafür, dass sich die Auswahl des Steuerelementes ändert, und auf das erste Element 
    ''' zurückgesetzt wird. Über diese Eigenschaft hat die übergeordnete Klasse EINMALIG die Möglichkeit, den Zustand seiner 
    ''' Value-Eigenschaft vor Erstellen des Handles auszulesen.</remarks>
    Public ReadOnly Property RestoreValue As Object
        Get
            Try
                Return myRestoreValue
            Finally
                myRestoreValue = Nothing
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets if exceptions which are occuring while layouting the DataGrid are thrown.
    ''' </summary>
    ''' <returns></returns>
    Public Property ThrowLayoutException As Boolean = True

    <DefaultValue(GetType(UnassignableValueAction), "TryHandleUnassignableValueDetectedEvent")>
    Public Property OnUnassignableValueAction As UnassignableValueAction

    ''' <summary>
    ''' Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die FormToBusinessClassManager-Komponente, die die Verwaltung dieses NullableValue-Controls übernimmt."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property AssignedManagerComponent As FormToBusinessClassManager Implements INullableValueControl.AssignedManagerControl

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Prioritätsindex, der bestimmt, in welcher Reihenfolge 
    ''' das Steuerelement vom FormsToBusinessClass-Manager verarbeitet wird (Höhere Nummer, frühere Verarbeitung.)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Prioritätsindex, der bestimmt, in welcher Reihenfolge das Steuerelement vom FormsToBusinessClass-Manager verarbeitet wird (Höhere Nummer, frühere Verarbeitung.)"),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Advanced),
     Browsable(True), DefaultValue(0)>
    Public Property ProcessingPriority As Integer Implements IAssignableFormToBusinessClassManager.ProcessingPriority

    ''' <summary>
    ''' Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Dass diese Eigenschaft UIGuid und nicht IdentificationGuid lautet, hat historische Gründe. Diese Eigenschaft implementiert 
    ''' IPermissionManageableUIContentElement.IdentificationGuid.</remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt eine eindeutige GUID für das Steuerelement, um beispielsweise Rechte-Mappings in Datenbanken aufzubauen."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property UIGuid As System.Guid Implements IPermissionManageableUIContentElement.IdentificationGuid

    ''' <summary>
    ''' Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von Rechten oder Lizenzausbaustufen verwendet werden darf.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, in welcher Form eine Komponente auf Grund von Rechten oder Lizenzausbaustufen verwendet werden darf."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(GetType(ContentPresentPermissions), "ContentPresentPermissions.Normal")>
    Public Property ContentPresentPermission As ContentPresentPermissions Implements IPermissionManageableUIContentElement.ContentPresentPermission

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Grund, weswegen ein Benutzer nur eingeschränkten Zugriff auf das Steuerelement hat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Grund, weswegen ein Benutzer nur eingeschränkten Zugriff auf das Steuerelement hat."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True)>
    Public Property PermissionReason As String Implements IPermissionManageableUIContentElement.PermissionReason

    ''' <summary>
    ''' Definiert, um welche Art Element es sich bei dieser Komponente handelt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ElementType As PermissionManageableUIElementType Implements IPermissionManageableUIContentElement.ElementType
        Get
            Return PermissionManageableUIElementType.Content
        End Get
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob bei der Zuweisung der Value-Eigenschaft Typgleichheit zwischen 
    ''' ValueMember (entsprechende Eigenschaft des Item in der Liste) und gesetztem Wert herschen muss, 
    ''' oder das Steuerelement versucht, eine impliziete Typkonvertierung durchzuführen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Durch eine EnforceValueMemberTypeSafety-Eigenschaft beim BindableDataGrid sowie beim NullableValueRelationPopup-Steuerelement 
    ''' soll durch Setzen dieser Eigenschaft auf false (Standard ist true) die Typsicherheit dabei aufgerufen werden. 
    ''' Stellt das Control fest, dass bei der Zuweisung der Value-Eigenschaft ein anderer Typ als beim entsprechenden ValueMember vorliegt, 
    ''' versucht es bei EnforceValueMemberTypeSafety=false selbständig eine Konvertierung in den Typ vorzunehmen. 
    ''' Falls das nicht gelingt, wird eine ensprechende TypeMismatchException ausgelöst. 
    ''' Bei EnforceValueMemberTypeSafety=true wird in diesem Fall per se eine TypeMismatchException ausgelöst.
    ''' </remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob bei der Zuweisung der Value-Eigenschaft Typengleichheit zwischen ValueMember (entsprechende Eigenschaft des Item in der Liste) und gesetztem Wert herschen muss."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(True)>
    Public Property EnforceValueMemberTypeSafety As Boolean = True

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob dieses Steuerelement rechtemäßig von einem externen Controler verwaltet werden kann oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob dieses Steuerelement rechtemäßig von einem externen Controler verwaltet werden kann oder nicht."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property IsManageable As Boolean Implements IPermissionManageableComponent.IsManageable

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob eine <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager</see>-Instanz 
    ''' diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob eine FormToBusinessClassManager-Instanz diese Komponente verarbeiten soll, wenn seine AutoUpdateFields-Eigenschaft auf ProcessSelected gesetzt wurde."),
     Category("Sicherheit"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property SelectedForProcessing As Boolean Implements IAssignableFormToBusinessClassManager.SelectedForProcessing

    ''' <summary>
    ''' Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt einen Gruppierungsnamen, um eine Möglichkeit zur Verfügung zu stellen, zentral eine Reihe von Steuerelementen zu steuern."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue("Default")>
    Public Property GroupName As String Implements IAssignableFormToBusinessClassManager.GroupName
        Get
            Return myGroupName
        End Get
        Set(value As String)
            myGroupName = value
        End Set
    End Property

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt die Dauer in Millisekunden, die ein Baloontip im Falle einer Fehlermeldung angezeigt wird."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(5000)>
    Public Property ExceptionBalloonDuration As Integer Implements INullableValueControl.ExceptionBalloonDuration

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob es sich bei einem Eingabefeld um ein Key-Feld handelt oder nicht."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(False)>
    Public Property IsKeyField As Boolean Implements IKeyFieldProvider.IsKeyField
End Class

<Flags()>
Public Enum UnassignableValueAction
    ThrowException = 1
    TryHandleUnassignableValueDetectedEvent = 2
    SelectNothing = 4
    SelectNothingThenThrowException = 5
    SelectFirstInList = 8
    SelectFirstInListThenThrowException = 9
End Enum

