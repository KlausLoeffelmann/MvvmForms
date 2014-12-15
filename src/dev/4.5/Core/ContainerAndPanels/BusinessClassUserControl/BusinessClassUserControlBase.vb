Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections.ObjectModel

Public Class BusinessClassUserControlBase(Of BusinessClassType)
    Inherits UserControl

    Private mySurroundingPanel As GridPanel
    Private myBusinessClassValue As BusinessClassType
    Private myFormController As FormToBusinessClassManager

    Sub New()
        MyBase.new()
        myFormController = New FormToBusinessClassManager
        myFormController.HostingUserControl = Me
        InitalizeComponentsInternally()
    End Sub

    Private Sub InitalizeComponentsInternally()
        Me.SuspendLayout()
        mySurroundingPanel = New GridPanel
        mySurroundingPanel.Dock = DockStyle.Fill
        mySurroundingPanel.Name = GetType(BusinessClassType).Name & "GridPanel"
        Me.Controls.Add(mySurroundingPanel)
        AddControlsToPanel(mySurroundingPanel)
        Me.ResumeLayout()
    End Sub

    Private Sub AddControlsToPanel(panel As GridPanel)

        Dim currentRow As Integer = 0
        Dim currentColumn As Integer = 0
        Dim currentTabIndex As Integer = 0

        For Each tItem In GetType(BusinessClassType).GetProperties
            Dim tmpObj = tItem.GetCustomAttributes(GetType(UIPropertyDefAttribute), True)
            If tmpObj IsNot Nothing AndAlso tmpObj.Count > 0 Then
                Dim tiTempAtt = DirectCast(tmpObj(0), UIPropertyDefAttribute)
                If tiTempAtt.Row > -1 Then
                    currentRow = tiTempAtt.Row
                    currentColumn = 0
                End If

                If tiTempAtt.Column > -1 Then
                    currentColumn = tiTempAtt.Column
                End If

                'Caption ist vorhanden, wir benötigen ein Label-Control zusätzlich
                If Not String.IsNullOrWhiteSpace(tiTempAtt.Caption) Then
                    Dim tmpLabelControl = GetCaptionControl()
                    tmpLabelControl.Text = tiTempAtt.Caption
                    tmpLabelControl.Name = tItem.Name & "Caption"
                    tmpLabelControl.Size = New Size(tiTempAtt.Width, tiTempAtt.Height)
                    tmpLabelControl.AutoSize = True
                    tmpLabelControl.Anchor = AnchorStyles.Left

                    panel.SetGridInfo(tmpLabelControl, New GridPanelGridInfo(currentRow, currentColumn, 0, 0))
                    panel.Controls.Add(tmpLabelControl)
                    currentColumn += 1
                End If

                Dim tmpControl = GetControlForType(tItem.PropertyType, tiTempAtt)
                tmpControl.Name = tItem.Name
                tmpControl.Size = New Size(tiTempAtt.Width, tiTempAtt.Height)
                DirectCast(tmpControl, INullableValueDataBinding).AssignedManagerControl = myFormController
                DirectCast(tmpControl, INullableValueDataBinding).DatafieldName = tItem.Name
                panel.SetGridInfo(tmpControl, New GridPanelGridInfo(currentRow, currentColumn, 0, 0))
                panel.Controls.Add(tmpControl)
                currentColumn += 1
            End If
        Next
    End Sub

    Protected Overridable Function GetCaptionControl() As Control
        Return New Label
    End Function

    Protected Overridable Function GetControlForType(t As Type, uidef As UIPropertyDefAttribute) As Control
        Select Case t
            Case GetType(String)
                If uidef.Multiline > 2 Then
                    Return New ActiveDevelop.EntitiesFormsLib.NullableMultilineTextValue
                Else
                    Return New ActiveDevelop.EntitiesFormsLib.NullableMaskTextValue
                End If

            Case GetType(Integer), GetType(Long), GetType(Short), GetType(Byte)
                Return New ActiveDevelop.EntitiesFormsLib.NullableIntValue

            Case GetType(Double), GetType(Single), GetType(Decimal)
                Return New ActiveDevelop.EntitiesFormsLib.NullableNumValue

            Case GetType(Date)
                Return New ActiveDevelop.EntitiesFormsLib.NullableDateValue

            Case GetType(Boolean)
                Return New ActiveDevelop.EntitiesFormsLib.NullableCheckBox

            Case Else
                Return New ActiveDevelop.EntitiesFormsLib.NullableMaskTextValue
        End Select
    End Function

    Property BusinessClassValue As BusinessClassType
        Get
            If Not DesignMode Then
                Me.myFormController.AssignFieldsFromNullableControls()
            End If
            Return myBusinessClassValue
        End Get
        Set(value As BusinessClassType)
            If Not DesignMode Then
                myBusinessClassValue = value
            End If
            Me.myFormController.AssignFieldsToNullableControls()
        End Set
    End Property
End Class

Public Class ControlWithAttributeCollection
    Inherits KeyedCollection(Of String, ControlWithAttribute)

    Protected Overrides Function GetKeyForItem(item As ControlWithAttribute) As String
        Return item.Control.Name
    End Function
End Class

Public Class ControlWithAttribute
    Property Control As Control
    Property Attribute As UIPropertyDefAttribute
End Class
