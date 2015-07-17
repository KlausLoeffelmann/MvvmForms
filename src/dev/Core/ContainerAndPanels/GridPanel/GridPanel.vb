Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing

''' <summary>
''' !!!EXPERIMENTAL!!!: Stellt ein Grid zur Verfügung, in dem sich Steuerelemente automatisch 
''' anhand ihrer Größer selbst anordnen und Layouten.
''' </summary>
<ProvideProperty("GridInfo", GetType(Control)),
 ToolboxBitmap(GetType(TableLayoutPanel)),
 ToolboxItem(True)>
Public Class GridPanel
    Inherits Panel
    Implements IExtenderProvider

    Private myExtendedControls As New Dictionary(Of Control, GridPanelGridInfo)
    Private myLayoutEngine As GridPanelLayout

    Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub OnControlAdded(e As System.Windows.Forms.ControlEventArgs)
        MyBase.OnControlAdded(e)
        If Not myExtendedControls.ContainsKey(e.Control) Then
            Me.SetGridInfo(e.Control, EnsureEmptySlot(e.Control))
        End If
    End Sub

    Protected Overrides Sub OnControlRemoved(e As System.Windows.Forms.ControlEventArgs)
        Try
            myExtendedControls.Remove(e.Control)
        Catch ex As Exception
        End Try
        MyBase.OnControlRemoved(e)
    End Sub

    Private Function EnsureEmptySlot(ctrl As Control) As GridPanelGridInfo

        'Durchschauen, ob diese Zelle schon vergeben ist:
        Dim notOK As Boolean

        Dim tempGrid = GetGridInfo(ctrl)

        Do
            notOK = False
            For Each items In myExtendedControls
                If items.Key Is ctrl Then
                    Continue For
                End If

                If items.Value.Row = tempGrid.Row AndAlso items.Value.Column = tempGrid.Column Then
                    'Kollision:
                    tempGrid.Column += 1
                    If tempGrid.Column > MaxColumns Then
                        tempGrid.Row += 1
                        tempGrid.Column = 0
                    End If

                    notOK = True
                    Exit For

                End If
            Next
        Loop Until Not notOK

        Return tempGrid
    End Function

    Public Function CanExtend(extendee As Object) As Boolean Implements System.ComponentModel.IExtenderProvider.CanExtend

        If GetType(Control).IsAssignableFrom(extendee.GetType) Then
            If Me.Controls.Contains(DirectCast(extendee, Control)) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Sub SetGridInfo(ctrl As Control, value As GridPanelGridInfo)

        If ctrl Is Nothing Then Return

        'Schauen, ob das Steuerelement bereits an einer anderen Stelle vorhanden war.
        If myExtendedControls.ContainsKey(ctrl) Then
            myExtendedControls.Remove(ctrl)
        End If

        myExtendedControls(ctrl) = value
        value = EnsureEmptySlot(ctrl)
        myExtendedControls(ctrl) = value

        Me.PerformLayout()
    End Sub

    Public Function GetGridInfo(ctrl As Control) As GridPanelGridInfo
        If myExtendedControls.ContainsKey(ctrl) Then
            Return myExtendedControls(ctrl)
        Else
            Return New GridPanelGridInfo
        End If
    End Function

    Public Overrides ReadOnly Property LayoutEngine As System.Windows.Forms.Layout.LayoutEngine
        Get
            If myLayoutEngine Is Nothing Then
                myLayoutEngine = New GridPanelLayout
            End If
            Return myLayoutEngine
        End Get
    End Property

    Public Property MaxColumns As Integer = 10
    Public Property AutoTabIndex As Boolean = True


End Class

