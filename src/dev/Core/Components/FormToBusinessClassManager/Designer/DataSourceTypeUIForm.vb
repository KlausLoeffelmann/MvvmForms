Imports System.Windows.Forms.Design
Imports System.Reflection
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Windows.Forms
Imports System.Data.Objects.DataClasses
Imports ActiveDevelop.EntitiesFormsLib.ViewModelBase

Public Class DataSourceTypeUIForm

    Private myDialogResultValue As Type

    Private Class DisplayableAssemblyItem

        Property ShortName As String
        Property Target As String
        Property Version As String
        Property Location As String
        Property EntryPoint As MethodInfo
        Property FullName As String
        Property OrderNr As Integer

        Public Shared Function GetDisplayableAssemblies(includeGapAssemblies As Boolean,
                                                        assemblyList As IEnumerable(Of Assembly)) As List(Of DisplayableAssemblyItem)

            Dim eflAssemblyName = GetType(DataSourceTypeUIForm).Assembly.GetName().Name
            Dim efl_pclAssemblyName = GetType(ActiveDevelop.EntitiesFormsLib.ViewModelBase.MvvmViewAttribute).Assembly.GetName().Name
            Dim eflAssemblies = {eflAssemblyName, efl_pclAssemblyName}

            Dim displayedAssemblies = (From item In assemblyList
                         Where item.GlobalAssemblyCache = includeGapAssemblies Or Not item.GlobalAssemblyCache
                         Select New DisplayableAssemblyItem With
                                {.ShortName = item.GetName.Name,
                                 .Target = item.ImageRuntimeVersion,
                                 .Version = item.GetName.Version.ToString,
                                 .Location = item.Location,
                                 .EntryPoint = item.EntryPoint,
                                 .FullName = item.FullName,
                                 .OrderNr = If(item.GlobalAssemblyCache, 3, If(eflAssemblies.Contains(item.GetName.Name), 2, 1))
                                }).ToList
            '' OrderNr: GAC als letztes, EFL davor und der rest zuerst

            'Die EntityFormsLib rausschmeißen.
            displayedAssemblies.RemoveAll(Function(item) eflAssemblies.Contains(item.ShortName))

            'Alle anderen zurückgeben
            Return displayedAssemblies.OrderBy(Function(item) item.OrderNr).ThenBy(Function(item2) item2.ShortName).ToList

        End Function
    End Class

    Public Class DisplayableTypeItem

        Property ShortName As String
        Property BaseTypeShortName As String
        Property Fullname As String
        Property OriginalItemReference As Type

    End Class

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Me.TopLevel = False
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ControlBox = False
        Me.ShowInTaskbar = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow

        nvrAssembly.DisplayMember = """{0} ({1})"",{ShortName},{Version}"
        nvrAssembly.SearchPattern = """{0} {1} {2} {3} "",{ShortName},{Version},{Location},{Fullname}"

        nvrBusinessClass.DisplayMember = """{0} ({1})"",{ShortName},{FullName}"
        nvrBusinessClass.SearchPattern = """{0} {1}"",{ShortName},{FullName}"

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If ComponentInstance IsNot Nothing Then
            Dim t_s = TypeDiscoveryService.GetTypes(GetType(Object), False)     ' das mit dem gettype(object) ist doof, laut msdn darf der parameter bei dem funktionsaufruf auch null sein (siehe interface beschreiung). die Implementierung sieht das aber wohl etwas anders, wenn wir kriegen hier eine argumentnullexception
            Dim typliste As New List(Of System.Type)
            For Each item In t_s
                typliste.Add(DirectCast(item, System.Type))
            Next

            'Dim x = (From item In typliste Where item.FullName = "System.Int16").FirstOrDefault

            ' die liste mit den Types packen wir in das Tag
            ' das machen wir aus faulheit, da wir dann das disposen der liste nicht selbst machen müssen
            Me.nvrAssembly.Tag = typliste

            Dim assemblies = (From tItem In typliste
                              Select tItem.Assembly Distinct).ToList


            Dim ds = DisplayableAssemblyItem.GetDisplayableAssemblies(chkGACAssemblies.Checked, assemblies)
            Me.nvrAssembly.DataSource = ds

            If Me.DialogResultValue IsNot Nothing Then
                Dim lastType = (From titem In typliste Where titem.FullName = Me.DialogResultValue.FullName).FirstOrDefault
                Dim lastAsmName = lastType.Assembly.GetName.Name

                ' Assembly vorbelegen
                Me.nvrAssembly.Value = (From item In ds Where item.ShortName = lastAsmName).FirstOrDefault


                If Me.nvrAssembly.Value IsNot Nothing Then
                    'Den Typ versuchen vorzudefinieren
                    nvrBusinessClass.Value = (From item In DirectCast(nvrBusinessClass.DataSource, List(Of DisplayableTypeItem)) Where item.Fullname = lastType.FullName).FirstOrDefault
                End If
            End If
        End If
    End Sub

    Public Property DialogResultValue As Type
        Friend Set(value As Type)
            myDialogResultValue = value
        End Set
        Get
            Return myDialogResultValue
        End Get
    End Property

    Private Sub nvrAssembly_GetColumnSchema(sender As Object, e As GetColumnSchemaEventArgs) Handles nvrAssembly.GetColumnSchema
        Dim fn As New DataGridViewColumnFieldnames
        fn.Add("ShortName", "Kurzname")
        fn.Add("Version", "Version")
        fn.Add("Target", ".NET-Framework Target")
        fn.Add("Location", "Speicherort")
        fn.Add("EntryPoint", "Einstiegspunkt")
        fn.Add("Fullname", "Vollqualifizierter Name")
        e.SchemaFieldnames = fn
    End Sub

    Private Sub nvrBusinessClass_GetColumnSchema(sender As Object, e As GetColumnSchemaEventArgs) Handles nvrBusinessClass.GetColumnSchema
        Dim fn As New DataGridViewColumnFieldnames
        fn.Add("ShortName", "Kurzname")
        fn.Add("BaseTypeShortName", "Basisklasse")
        fn.Add("Fullname", "Vollqualifizierter Name")
        e.SchemaFieldnames = fn
    End Sub

    Protected Overrides Sub OnClosed(e As System.EventArgs)
        MyBase.OnClosed(e)
        If WFEditorService IsNot Nothing Then WFEditorService.CloseDropDown()
    End Sub

    Private Sub chkGACAssemblies_CheckedChanged(sender As Object, e As EventArgs) Handles chkGACAssemblies.CheckedChanged
        Dim typliste = DirectCast(Me.nvrAssembly.Tag, List(Of System.Type))
        Dim assemblies = (From tItem In typliste
                  Select tItem.Assembly Distinct).ToList


        Dim ds = DisplayableAssemblyItem.GetDisplayableAssemblies(chkGACAssemblies.Checked, assemblies)
        Me.nvrAssembly.DataSource = ds
    End Sub

    Private Sub nvrAssembly_ValueChanged(sender As Object, e As ValueChangedEventArgs) Handles nvrAssembly.ValueChanged
        TraceEx.TraceInformation("nvrAssembly_ValueChanged. ValueChangedCause is " & e.ValueChangedCause.ToString)
        If e.ValueChangedCause = ValueChangedCauses.User Then
            If Debugger.IsAttached AndAlso System.Windows.Forms.Control.ModifierKeys = Windows.Forms.Keys.Shift Then
                Debugger.Break()
            End If
        End If

        If nvrAssembly.Value Is Nothing Then
            'Redundant?
            nvrBusinessClass.Value = Nothing
            nvrBusinessClass.DataSource = Nothing
            nvrBusinessClass.Enabled = False
            Return
        End If

        nvrBusinessClass.Enabled = True

        Dim typsFromSelAssembly = (From ty As Type In DirectCast(Me.nvrAssembly.Tag, List(Of System.Type)) _
                                  Where ty.Assembly.FullName = DirectCast(nvrAssembly.Value, DisplayableAssemblyItem).FullName).ToList

        Dim entityObjectsTypes As List(Of Type)

        Try
            'If Debugger.IsAttached Then
            '    Debugger.Break()
            'End If

            entityObjectsTypes = (From ty As Type In typsFromSelAssembly _
                                  Where GetType(EntityObject).IsAssignableFrom(ty) Or
                                  GetType(INotifyPropertyChanged).IsAssignableFrom(ty)).ToList

        Catch ex As ReflectionTypeLoadException
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
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

        Dim BusinessClassTypes As New List(Of System.Type)
        For Each item In typsFromSelAssembly
            For Each atts In item.GetCustomAttributes(True)
                'Attribute finden, die auf BusinessClassAttribute oder MvvmViewAttribute basieren.
                Dim bcAtt As Attribute = GetBaseAttribute(atts)

                If bcAtt IsNot Nothing Then
                    BusinessClassTypes.Add(item)
                    Exit For
                End If
            Next
        Next

        entityObjectsTypes.AddRange(BusinessClassTypes)

        Dim orderedResults = (From item In entityObjectsTypes
                           Select dItem = New DisplayableTypeItem With {
                               .BaseTypeShortName = item.BaseType.Name,
                               .Fullname = item.FullName,
                               .ShortName = item.Name,
                               .OriginalItemReference = item
                           }
                            Order By dItem.ShortName).ToList

        nvrBusinessClass.DataSource = orderedResults
    End Sub

    ''' <summary>
    ''' Ueberprueft das uebergebene Attribut auf Pflichtattribute, welche der Typ mindestens annotiert haben muss
    ''' </summary>
    ''' <param name="atts">Attribut welches ueberprueft werden soll</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function GetBaseAttribute(atts As Object) As Attribute
        Dim bcAtt As Attribute = TryCast(atts, BusinessClassAttribute)

        If bcAtt Is Nothing Then
            bcAtt = TryCast(atts, MvvmViewModelAttribute)
        End If
        Return bcAtt
    End Function

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If nvrBusinessClass.Value Is Nothing Then
            myDialogResultValue = Nothing
        Else
            myDialogResultValue = DirectCast(nvrBusinessClass.Value, DisplayableTypeItem).OriginalItemReference
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK

        MyBase.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        myDialogResultValue = Nothing
        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        MyBase.Close()
    End Sub

    Friend Property WFEditorService As IWindowsFormsEditorService
    Friend Property ComponentInstance As Component
    Friend ReadOnly Property ComponentDesigner As FormToBusinessClassManagerDesigner
        Get
            Return TryCast(DesignerHost.GetDesigner(Me.ComponentInstance), FormToBusinessClassManagerDesigner)
        End Get
    End Property


    Friend ReadOnly Property DesignerHost As IDesignerHost
        Get
            Return TryCast(ComponentInstance.Site.GetService(GetType(IDesignerHost)), IDesignerHost)
        End Get
    End Property

    Friend ReadOnly Property DesignTimeAssemblyLoader As IDesignTimeAssemblyLoader
        Get
            Return ComponentInstance.Site.GetService(Of IDesignTimeAssemblyLoader)()
        End Get
    End Property

    Friend ReadOnly Property ReferenceService As IReferenceService
        Get
            Return ComponentInstance.Site.GetService(Of IReferenceService)()
        End Get
    End Property

    Friend ReadOnly Property TypeDiscoveryService As ITypeDiscoveryService
        Get
            Return ComponentInstance.Site.GetService(Of ITypeDiscoveryService)()
        End Get
    End Property

End Class

