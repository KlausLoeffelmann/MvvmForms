Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports System.ComponentModel.Design

'<TypeConverter(GetType(DataSourceTypeConverter))>
Public Class DataSourceType

    Sub New()
        MyBase.New()
    End Sub

    Property DataSourceType As Type

    Public Overrides Function ToString() As String
        Return DataSourceType.ToString
    End Function
End Class

Public Class DatafieldNameConverter
    Inherits StringConverter

    Private myProperties As List(Of String)

    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
        Return False
    End Function

    Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
        Dim ftbcManagerAssignableControl = TryCast(context.Instance, IAssignableFormToBusinessClassManager)
        If ftbcManagerAssignableControl IsNot Nothing Then
            Dim ftbcm = ftbcManagerAssignableControl.AssignedManagerControl
            If ftbcm IsNot Nothing Then
                If ftbcm.DataSourceType IsNot Nothing Then
                    Dim propList As New List(Of PropertyCheckBoxItemController)
                    CreateSubPropsAsList(ftbcm.DataSourceType, "", propList, 0, False)
                    myProperties = New List(Of String)
                    propList.ForEach(Sub(item)
                                         myProperties.Add(item.ToString)
                                     End Sub)
                End If
            End If
        End If

        Return New StandardValuesCollection(myProperties)
    End Function

End Class

Public Class DataSourceTypeUIEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overrides Function EditValue(context As System.ComponentModel.ITypeDescriptorContext,
                                        provider As System.IServiceProvider, value As Object) As Object
        'Return MyBase.EditValue(context, provider, value)
        If System.Windows.Forms.Control.ModifierKeys = System.Windows.Forms.Keys.Shift Then
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If
        Dim wfEditService As IWindowsFormsEditorService = DirectCast(provider.GetService(
                                        GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)

        Dim myReturnValue As Type = Nothing

        If wfEditService IsNot Nothing Then
            Using frmTemp = New DataSourceTypeUIForm
                frmTemp.WFEditorService = wfEditService
                frmTemp.ComponentInstance = TryCast(context.Instance, Component)
                Dim site = frmTemp.ComponentInstance.Site
                If site IsNot Nothing Then
                    Dim host = TryCast(site.GetService(GetType(IDesignerHost)), IDesignerHost)
                    frmTemp.DialogResultValue = DirectCast(frmTemp.ComponentInstance, FormToBusinessClassManager).DataSourceType
                    myReturnValue = frmTemp.DialogResultValue           ' falls wir den Dialog später abbrechen, soll das der Wert sein, den wir zurückgeben wollen
                End If
                wfEditService.ShowDialog(frmTemp)
                If frmTemp.DialogResult = System.Windows.Forms.DialogResult.OK Then
                    myReturnValue = frmTemp.DialogResultValue
                End If
            End Using
        End If

        Return myReturnValue

    End Function

End Class
