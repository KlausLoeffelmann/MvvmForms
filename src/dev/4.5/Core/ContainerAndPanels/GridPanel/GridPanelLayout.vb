Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.Layout

Public Class GridPanelLayout
    Inherits LayoutEngine

    Private myGrid(255, 255) As GridCell

    Private Structure GridCell
        Property Location As Point
        Property Size As Size
        Property PositionOccupied As Boolean
        Property ControlToPosition As Control
        Property FinalControlLocation As Point
        Property FinalControlSize As Size
        Property Ticket As Guid
    End Structure

    Public Overrides Function Layout(container As Object, layoutEventArgs As System.Windows.Forms.LayoutEventArgs) As Boolean
        Dim parent As Control = CType(container, Control)
        Dim maxRow, maxColumn As Integer

        Dim parentDisplayRectangle As Rectangle = parent.DisplayRectangle
        Dim nextControlLocation As Point = parentDisplayRectangle.Location

        Dim usedGridPanel = DirectCast(parent, GridPanel)

        parent.SuspendLayout()

        'Das Ticket für diesen Durchlauf generieren.
        'Steuerelementverweise mit "altem" Ticket sind nicht gültig.
        Dim currentTicket = Guid.NewGuid

        Dim offsetX = parent.Padding.Left
        Dim offsety = parent.Padding.Top

        'Erstmal alle Größen übertragen.
        For Each c As Control In parent.Controls

            If Not c.Visible Then
                Continue For
            End If

            Dim gi = usedGridPanel.GetGridInfo(c)
            If myGrid(gi.Row, gi.Column).PositionOccupied Then
                'TODO: Sollen wir hier irgendwas machen?
            End If

            myGrid(gi.Row, gi.Column).PositionOccupied = True
            myGrid(gi.Row, gi.Column).ControlToPosition = c
            myGrid(gi.Row, gi.Column).Ticket = currentTicket

            'Autosize berücksichtigen

            If c.AutoSize Then
                myGrid(gi.Row, gi.Column).FinalControlSize = c.GetPreferredSize(parentDisplayRectangle.Size)
            Else
                myGrid(gi.Row, gi.Column).FinalControlSize = c.Size
            End If

            myGrid(gi.Row, gi.Column).Location = New Point(offsetX, offsety)
            myGrid(gi.Row, gi.Column).Location = Point.Add(myGrid(gi.Row, gi.Column).Location, New Size(c.Margin.Left, c.Margin.Top))
            myGrid(gi.Row, gi.Column).Size = myGrid(gi.Row, gi.Column).FinalControlSize
            myGrid(gi.Row, gi.Column).Size = Size.Add(myGrid(gi.Row, gi.Column).Size, New Size(c.Margin.Right, c.Margin.Bottom))

            If gi.Row > maxRow Then maxRow = gi.Row
            If gi.Column > maxColumn Then maxColumn = gi.Column
        Next

        'In den verwendete Zeilen jetzt die Maximalwerte der Größen ausrechnen
        Dim maxheight As Integer
        For rowCount = 0 To maxRow
            maxheight = 0
            For colCount = 0 To maxColumn
                If maxheight < myGrid(rowCount, colCount).Size.Height Then
                    maxheight = myGrid(rowCount, colCount).Size.Height
                End If
            Next
            myGrid(rowCount, 255).Size = New Size(0, maxheight)
        Next

        'In den verwendeten Spalten jetzt die Maximalwerte der Größen ausrechnen
        Dim maxWidth As Integer
        For colCount = 0 To maxColumn
            maxWidth = 0
            For rowCount = 0 To maxRow
                If maxWidth < myGrid(rowCount, colCount).Size.Width Then
                    maxWidth = myGrid(rowCount, colCount).Size.Width
                End If
            Next
            myGrid(255, colCount).Size = New Size(maxWidth, 0)
        Next

        'Die Offsets der Spalten und Zeilen anpassen
        Dim lastColPos = offsetX
        For colCount = 0 To maxColumn
            myGrid(255, colCount).Location = New Point(lastColPos, 0)
            lastColPos += myGrid(255, colCount).Size.Width
        Next

        Dim lastRowPos = offsety
        For RowCount As Integer = 0 To maxRow
            myGrid(RowCount, 255).Location = New Point(0, lastRowPos)
            lastRowPos += myGrid(RowCount, 255).Size.Height
        Next

        'Steuerelemente gemäß der maximalen Ausmaße positionieren und im Bedarfsfall TabIndex setzen.
        Dim currentTabIndex As Integer = 0

        For colCount = 0 To maxColumn
            For rowCount = 0 To maxRow
                Dim ctp = myGrid(rowCount, colCount).ControlToPosition

                If myGrid(rowCount, colCount).Ticket <> currentTicket Then
                    'Das ist ein überbleibselverweise von letztem Mal.
                    myGrid(rowCount, colCount).ControlToPosition = Nothing
                    ctp = Nothing
                End If

                If ctp IsNot Nothing Then

                    If DirectCast(parent, GridPanel).AutoTabIndex Then
                        ctp.TabIndex = currentTabIndex
                        currentTabIndex += 1
                    End If

                    myGrid(rowCount, colCount).FinalControlLocation = New Point(myGrid(255, colCount).Location.X + myGrid(rowCount, colCount).ControlToPosition.Margin.Left,
                                                                                myGrid(rowCount, 255).Location.Y + myGrid(rowCount, colCount).ControlToPosition.Margin.Top)

                    'Anchoring berücksichtigen
                    Dim cellSize = New Size(myGrid(255, colCount).Size.Width, myGrid(rowCount, 255).Size.Height)

                    'vertikale Ausrichtung
                    If ctp.Anchor.HasFlag(AnchorStyles.Left) And (Not ctp.Anchor.HasFlag(AnchorStyles.Right)) Then
                        'Nichts machen - das geht schon.
                    ElseIf (Not ctp.Anchor.HasFlag(AnchorStyles.Left)) And (Not ctp.Anchor.HasFlag(AnchorStyles.Right)) Then
                        'Keiner: Mitte
                        myGrid(rowCount, colCount).FinalControlLocation = New Point(myGrid(255, colCount).Location.X + (cellSize.Width \ 2 - ctp.Size.Width \ 2) + ctp.Margin.Left,
                                                                                    myGrid(rowCount, colCount).FinalControlLocation.Y)
                    ElseIf (Not ctp.Anchor.HasFlag(AnchorStyles.Left)) And ctp.Anchor.HasFlag(AnchorStyles.Right) Then
                        'Recht verankert, also rechtsbündig
                        myGrid(rowCount, colCount).FinalControlLocation = New Point(myGrid(255, colCount).Location.X + (cellSize.Width - (ctp.Size.Width + ctp.Margin.Right)), myGrid(rowCount, colCount).FinalControlLocation.Y)
                    ElseIf (ctp.Anchor.HasFlag(AnchorStyles.Left)) And (ctp.Anchor.HasFlag(AnchorStyles.Right)) Then
                        myGrid(rowCount, colCount).FinalControlSize = New Size(myGrid(255, colCount).Size.Width - (ctp.Margin.Right), myGrid(rowCount, colCount).FinalControlSize.Height)
                    End If

                    'Horizontale Ausrichtung
                    If ctp.Anchor.HasFlag(AnchorStyles.Top) And (Not ctp.Anchor.HasFlag(AnchorStyles.Bottom)) Then
                        'Nichts machen - das geht schon.
                    ElseIf (Not ctp.Anchor.HasFlag(AnchorStyles.Top)) And (Not ctp.Anchor.HasFlag(AnchorStyles.Bottom)) Then
                        'Keiner: Mitte, aber Margin berücksichtigen!
                        myGrid(rowCount, colCount).FinalControlLocation = New Point(myGrid(rowCount, colCount).FinalControlLocation.X,
                                                                                    myGrid(rowCount, 255).Location.Y + (cellSize.Height \ 2 - ctp.Size.Height \ 2) + ctp.Margin.Top)

                    ElseIf (Not ctp.Anchor.HasFlag(AnchorStyles.Top)) And ctp.Anchor.HasFlag(AnchorStyles.Bottom) Then
                        'Unten verankert, also untenliegend
                        myGrid(rowCount, colCount).FinalControlLocation = New Point(myGrid(rowCount, colCount).FinalControlLocation.X,
                                                                                    myGrid(rowCount, 255).Location.Y + (cellSize.Height - (ctp.Size.Height + ctp.Margin.Bottom)))

                    ElseIf (ctp.Anchor.HasFlag(AnchorStyles.Top)) And (ctp.Anchor.HasFlag(AnchorStyles.Bottom)) Then
                        myGrid(rowCount, colCount).FinalControlSize = New Size(myGrid(rowCount, colCount).FinalControlSize.Width,
                                                                               myGrid(rowCount, 255).Size.Height - (ctp.Margin.Top))
                    End If

                    myGrid(rowCount, colCount).ControlToPosition.Location = New Point(myGrid(rowCount, colCount).FinalControlLocation.X,
                                                                                      myGrid(rowCount, colCount).FinalControlLocation.Y)
                    myGrid(rowCount, colCount).ControlToPosition.SetBounds(
                                        0,
                                        0,
                                        myGrid(rowCount, colCount).FinalControlSize.Width,
                                        myGrid(rowCount, colCount).FinalControlSize.Height,
                                        BoundsSpecified.Size)

                End If
            Next
        Next

        parent.ResumeLayout()
        Return True
    End Function

End Class
