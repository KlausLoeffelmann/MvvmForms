Imports System.Windows.Forms
Imports System.Reflection
Imports System.Data.Objects.DataClasses
Imports System.ComponentModel
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

Public Class frmEntityObjectSelector

    Private Delegate Function ToStrDelegeate(ByVal obj As Object) As String
    Private Class ComboboxObjectWrapper(Of T)

        Private myObj As T
        Private mytoStrMethod As Func(Of T, String)
        Public Sub New(ByVal obj As T, ByVal toStrMethod As Func(Of T, String))
            myObj = obj
            mytoStrMethod = toStrMethod
        End Sub

        Public ReadOnly Property TheObject As T
            Get
                Return myObj
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return mytoStrMethod(myObj)
        End Function
    End Class

    Private myPossibleTargetControls As List(Of Control)
    Private myReferencedAssemblies As List(Of Assembly)
    Private mySelectedProperties As List(Of PropertyCheckBoxItemController)
    Private myLastSettings As FormToBusinessClassManager.EntityObjectSettings

    'Public Overloads Function ShowDialog(ByVal controls As List(Of ControlCollection)) As Tuple(Of DialogResult, Assembly, EntityObject, List(Of PropertyInfo))
    Public Overloads Function ShowDialog(ByVal possibleControls As List(Of Control),
                                         ByVal referencedAssemblies As List(Of Assembly),
                                         ByVal lastSettings As FormToBusinessClassManager.EntityObjectSettings) As Tuple(Of DialogResult, Control, Assembly, Type, List(Of PropertyCheckBoxItemController))

        myReferencedAssemblies = referencedAssemblies
        myPossibleTargetControls = possibleControls
        myLastSettings = lastSettings

        cmbDestContainer.Items.Clear()
        Dim lastDestContainerWrapper As ComboboxObjectWrapper(Of Control) = Nothing
        For Each destItem In myPossibleTargetControls
            Dim w = New ComboboxObjectWrapper(Of Control)(destItem, Function(c As Control)
                                                                        Return c.Name
                                                                    End Function
            )
            cmbDestContainer.Items.Add(w)
            'Letztes Zielelement aus den Controls aussuchen, falls identisch
            If lastSettings IsNot Nothing AndAlso destItem.Name = lastSettings.ParentControlName Then
                lastDestContainerWrapper = w
            End If
        Next
        If lastSettings IsNot Nothing AndAlso lastDestContainerWrapper Is Nothing Then
            MessageBox.Show("Das Zielelement '" & lastSettings.ParentControlName & "' ist nicht mehr vorhanden/ungültig.", "Fehler")
            Return Tuple.Create(DialogResult.Abort, DirectCast(Nothing, Control), DirectCast(Nothing, Assembly), DirectCast(Nothing, Type), New List(Of PropertyCheckBoxItemController)())
        End If

        cmbAssembly.Items.Clear()
        Dim lastAssemblyWrapper As ComboboxObjectWrapper(Of Assembly) = Nothing
        For Each refass In myReferencedAssemblies
            Dim w = New ComboboxObjectWrapper(Of Assembly)(refass, Function(a As Assembly)
                                                                       Return a.GetName().ToString
                                                                   End Function
            )
            cmbAssembly.Items.Add(w)
            'Letztes Zielelement aus den Controls aussuchen, falls identisch
            If lastSettings IsNot Nothing AndAlso refass.GetName.ToString = lastSettings.AssemblyWithEntityObjectName Then
                lastAssemblyWrapper = w
            End If
        Next
        If lastSettings IsNot Nothing AndAlso lastAssemblyWrapper Is Nothing Then
            MessageBox.Show("Die Assembly '" & lastSettings.AssemblyWithEntityObjectName & "' ist nicht mehr vorhanden/ungültig.", "Fehler")
            Return Tuple.Create(DialogResult.Abort, DirectCast(Nothing, Control), DirectCast(Nothing, Assembly), DirectCast(Nothing, Type), New List(Of PropertyCheckBoxItemController)())
        End If

        'If Debugger.IsAttached Then
        '    Debugger.Break()
        'End If

        cmbDestContainer.Enabled = True
        cmbAssembly.Enabled = True
        cmbEntityObject.Enabled = True
        If lastSettings IsNot Nothing Then
            ' es sind gespeicherte einstellungen vorhanden
            ' keine Änderung an Assembly, ZielContainer, und EntityObject erlauben
            cmbDestContainer.Enabled = False
            cmbAssembly.Enabled = False
            cmbEntityObject.Enabled = False

            'nun versuchen, die letzte Auswahl wiederherzustellen (nur DestControl,Assembly; FieldList wird in cmbEntityObject_SelectedIndexChanged gefüllt)
            cmbDestContainer.SelectedIndex = -1
            cmbAssembly.SelectedIndex = -1

            cmbDestContainer.SelectedItem = lastDestContainerWrapper
            cmbAssembly.SelectedItem = lastAssemblyWrapper

            If Debugger.IsAttached Then
                'cmbDestContainer.Items.GetType
                Debugger.Break()
            End If
        Else
            If cmbDestContainer.Items.Count > 0 Then
                cmbDestContainer.SelectedIndex = 0
            Else
                cmbDestContainer.SelectedIndex = -1
            End If

            If cmbAssembly.Items.Count > 0 Then
                cmbAssembly.SelectedIndex = 0
            Else
                cmbAssembly.SelectedIndex = -1
            End If
        End If


        ' Dialog anzeigen
        Dim ret = Me.ShowDialog()


        ' Dialogrückgabe erstellen
        If ret = DialogResult.OK Then
            ' Fieldlist anhand der ausgewählten Felder erstellen
            mySelectedProperties = New List(Of PropertyCheckBoxItemController)
            For i = 0 To CheckedListBox1.Items.Count - 1
                If CheckedListBox1.GetItemChecked(i) Then
                    mySelectedProperties.Add(DirectCast(CheckedListBox1.Items(i), PropertyCheckBoxItemController))
                End If
            Next

            Dim destContWrapper = DirectCast(cmbDestContainer.SelectedItem, ComboboxObjectWrapper(Of Control))
            Dim entityWrapper = DirectCast(cmbEntityObject.SelectedItem, ComboboxObjectWrapper(Of Type))

            Return Tuple.Create(ret, destContWrapper.TheObject, entityWrapper.TheObject.Assembly, entityWrapper.TheObject, mySelectedProperties)
        Else
            Return Tuple.Create(ret, DirectCast(Nothing, Control), DirectCast(Nothing, Assembly), DirectCast(Nothing, Type), New List(Of PropertyCheckBoxItemController)())
        End If
    End Function

    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        FillEntityOrBusinessObjects()
    End Sub

    Private Sub FillEntityOrBusinessObjects()
        cmbEntityObject.Items.Clear()
        cmbEntityObject.SelectedIndex = -1

        If cmbAssembly.SelectedIndex < 0 Then Return

        Dim selAss = DirectCast(cmbAssembly.SelectedItem, ComboboxObjectWrapper(Of Assembly)).TheObject
        If selAss Is Nothing Then Return

        Dim EntityObjectsTypes As List(Of Type)
        Try
            If Debugger.IsAttached Then
                'cmbDestContainer.Items.GetType
                Debugger.Break()
            End If

            EntityObjectsTypes = (From ty As Type In selAss.GetTypes _
                                          Where (ty.BaseType Is GetType(EntityObject) Or
                                                 ty.BaseType Is GetType(INotifyPropertyChanged))).ToList
        Catch ex As ReflectionTypeLoadException

            Dim leText As String = ""

            If ex.LoaderExceptions IsNot Nothing Then
                Array.ForEach(ex.LoaderExceptions, Sub(item As Exception)
                                                       leText &= item.Message & vbNewLine
                                                   End Sub)
            End If
            leText = ex.Message & vbNewLine & leText
            MessageBox.Show(leText)
            Throw ex
        End Try

        'Dim BusinessClassTypes = (From item As Type In selAss.GetTypes _
        '                        Let busiAtt = (From attItem In item.GetCustomAttributes(True)
        '                               Where attItem.GetType.BaseType Is GetType(BusinessClassAttribute)).SingleOrDefault
        '                        Where busiAtt IsNot Nothing
        '                        Select item).ToList

        Dim BusinessClassTypes As New List(Of System.Type)
        For Each item In selAss.GetTypes

            For Each atts In item.GetCustomAttributes(True)
                If atts.GetType.Equals(GetType(BusinessClassAttribute)) Then
                    BusinessClassTypes.Add(item)
                End If
            Next
        Next

        EntityObjectsTypes.AddRange(BusinessClassTypes)

        Dim lastEntityObjectWrapper As ComboboxObjectWrapper(Of Type) = Nothing
        For Each item In EntityObjectsTypes
            Dim w = New ComboboxObjectWrapper(Of Type)(item, Function(t As Type)
                                                                 Return t.FullName
                                                             End Function
            )
            cmbEntityObject.Items.Add(w)
            If myLastSettings IsNot Nothing AndAlso item.FullName = myLastSettings.EntityObjectTypeName Then
                lastEntityObjectWrapper = w
            End If
        Next
        If myLastSettings IsNot Nothing AndAlso lastEntityObjectWrapper Is Nothing Then
            MessageBox.Show("Die Entität '" & myLastSettings.EntityObjectTypeName & "' ist nicht mehr vorhanden/ungültig.", "Fehler")
            Me.DialogResult = DialogResult.Abort
            Return
        End If
        If myLastSettings IsNot Nothing Then
            cmbEntityObject.SelectedItem = lastEntityObjectWrapper
        Else
            If cmbEntityObject.Items.Count > 0 Then
                cmbEntityObject.SelectedIndex = 0
            Else
                cmbEntityObject.SelectedIndex = -1
            End If
        End If

    End Sub

    Private Sub cmbEntityObject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEntityObject.SelectedIndexChanged
        CheckedListBox1.Items.Clear()

        Dim isEntityObject As Boolean


        If cmbEntityObject.SelectedIndex < 0 Then Return
        Dim selectedObject = DirectCast(cmbEntityObject.SelectedItem, ComboboxObjectWrapper(Of Type)).TheObject
        If (selectedObject.BaseType Is GetType(EntityObject) Or
            selectedObject.BaseType Is GetType(INotifyPropertyChanged)) Then
            isEntityObject = True
        End If

        If selectedObject Is Nothing Then Return

        If isEntityObject Then
            For Each item As PropertyInfo In selectedObject.GetProperties()
                Dim ScalarInEDM = item.GetCustomAttributes(GetType(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), True)

                If ScalarInEDM.Length >= 1 Then
                    CheckedListBox1.Items.Add(New PropertyCheckBoxItemController(item.Name, item.PropertyType, ""), True)
                End If
            Next
        Else
            Dim PropList As New List(Of PropertyCheckBoxItemController)
            CreateSubPropsAsList(selectedObject, "", PropList, 0, False)
            For Each pItem In PropList
                CheckedListBox1.Items.Add(pItem, True)
            Next
        End If

        'Else
        'If Debugger.IsAttached Then
        '    Debugger.Break()
        'End If
        '' Es sind gespeicherte Änderungen vorhanden
        '' also erst die Auswahl (in der Reihenfolge, wie sie vorkommen) und dann die restlichen Einträge

        '' 1. die letzte Auswahl in der richtigen Reihenfolge wiederherstellen
        'For Each field As String In myLastSettings.ListOfFields
        '    Dim propInfo = entityObject.GetProperty(field)
        '    If propInfo IsNot Nothing Then
        '        Dim ScalarInEDM = propInfo.GetCustomAttributes(GetType(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), True)
        '        If ScalarInEDM.Length >= 1 Then
        '            CheckedListBox1.Items.Add(propInfo.Name, True)
        '        End If
        '    End If
        'Next

        '' 2. nun die Felder, die nicht ausgewählt waren
        'For Each item As PropertyInfo In entityObject.GetProperties()
        '    If Not myLastSettings.ListOfFields.Contains(item.Name) Then
        '        ' feld war letztes mal nicht ausgewählt
        '        Dim ScalarInEDM = item.GetCustomAttributes(GetType(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), True)
        '        If ScalarInEDM.Length >= 1 Then
        '            CheckedListBox1.Items.Add(item.Name, False)
        '        End If
        '    End If
        'Next

        'End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        If CheckedListBox1.SelectedIndex < 1 Then Return
        Dim ItemCount = CheckedListBox1.Items.Count


        ' Kopie der ausgewählten Elemente erstellen, da in der for each Schleife keine Elemente entfernt werden können
        Dim itemscopy(CheckedListBox1.SelectedItems.Count - 1) As Object
        CheckedListBox1.SelectedItems.CopyTo(itemscopy, 0)

        'Reorder
        For Each obj As Object In itemscopy
            Dim idx = CheckedListBox1.Items.IndexOf(obj)
            Dim checked = CheckedListBox1.GetItemCheckState(idx)
            CheckedListBox1.Items.RemoveAt(idx)
            Dim newIdx = Math.Max(idx - 1, 0)
            CheckedListBox1.Items.Insert(newIdx, obj)
            CheckedListBox1.SetItemCheckState(newIdx, checked)
        Next

        'Selektion wiederherstellen
        Reselect(itemscopy)
    End Sub

    Private Sub btnMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveDown.Click
        If CheckedListBox1.SelectedIndex < 0 Then Return
        If CheckedListBox1.SelectedIndex >= CheckedListBox1.Items.Count Then Return

        Dim ItemCount = CheckedListBox1.Items.Count

        ' Kopie der ausgewählten Elemente erstellen, da in der for each Schleife keine Elemente entfernt werden können
        Dim itemscopy(CheckedListBox1.SelectedItems.Count - 1) As Object
        CheckedListBox1.SelectedItems.CopyTo(itemscopy, 0)

        'reorder
        For Each obj As Object In itemscopy
            Dim idx = CheckedListBox1.Items.IndexOf(obj)
            Dim checked = CheckedListBox1.GetItemCheckState(idx)
            CheckedListBox1.Items.RemoveAt(idx)
            Dim newIdx = Math.Min(idx + 1, ItemCount - 1)
            CheckedListBox1.Items.Insert(newIdx, obj)
            CheckedListBox1.SetItemCheckState(newIdx, checked)
        Next

        'Selektion wiederherstellen
        Reselect(itemscopy)
    End Sub

    Private Sub Reselect(ByVal selectedItems() As Object)
        For Each item In selectedItems
            Dim idx = CheckedListBox1.Items.IndexOf(item)
            If idx >= 0 AndAlso idx < CheckedListBox1.Items.Count Then
                CheckedListBox1.SetSelected(idx, True)
            End If
        Next
    End Sub
End Class

