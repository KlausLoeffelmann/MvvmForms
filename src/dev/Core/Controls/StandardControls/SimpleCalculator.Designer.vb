<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SimpleCalculator
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.calcResult = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnPlus = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnDecimalSeparator = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn0 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnToggleSign = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnMinus = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn3 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn2 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn1 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnMulti = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn6 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn5 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn4 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnDiv = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn9 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn8 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btn7 = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnCE = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnC = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnDel = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        Me.btnCalc = New ActiveDevelop.EntitiesFormsLib.CalculatorButton()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.calcResult)
        Me.SplitContainer1.Panel1MinSize = 22
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(225, 185)
        Me.SplitContainer1.SplitterDistance = 25
        Me.SplitContainer1.TabIndex = 2
        '
        'calcResult
        '
        Me.calcResult.Dock = System.Windows.Forms.DockStyle.Fill
        Me.calcResult.Location = New System.Drawing.Point(0, 0)
        Me.calcResult.Margin = New System.Windows.Forms.Padding(0)
        Me.calcResult.Name = "calcResult"
        Me.calcResult.Size = New System.Drawing.Size(225, 22)
        Me.calcResult.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.btnPlus, 3, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDecimalSeparator, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.btn0, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.btnToggleSign, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMinus, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btn3, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.btn2, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.btn1, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMulti, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btn6, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btn5, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btn4, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDiv, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btn9, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btn8, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btn7, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCE, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnC, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnDel, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnCalc, 3, 4)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(225, 156)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'btnPlus
        '
        Me.btnPlus.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnPlus.AutoCheck = False
        Me.btnPlus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnPlus.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlus.KeyVal = ""
        Me.btnPlus.Location = New System.Drawing.Point(168, 93)
        Me.btnPlus.Margin = New System.Windows.Forms.Padding(0)
        Me.btnPlus.Name = "btnPlus"
        Me.btnPlus.Size = New System.Drawing.Size(57, 31)
        Me.btnPlus.TabIndex = 15
        Me.btnPlus.Tag = ""
        Me.btnPlus.Text = "+"
        Me.btnPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnPlus.UseVisualStyleBackColor = True
        '
        'btnDecimalSeparator
        '
        Me.btnDecimalSeparator.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnDecimalSeparator.AutoCheck = False
        Me.btnDecimalSeparator.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDecimalSeparator.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDecimalSeparator.KeyVal = "."
        Me.btnDecimalSeparator.Location = New System.Drawing.Point(112, 124)
        Me.btnDecimalSeparator.Margin = New System.Windows.Forms.Padding(0)
        Me.btnDecimalSeparator.Name = "btnDecimalSeparator"
        Me.btnDecimalSeparator.Size = New System.Drawing.Size(56, 32)
        Me.btnDecimalSeparator.TabIndex = 14
        Me.btnDecimalSeparator.Text = ".,"
        Me.btnDecimalSeparator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnDecimalSeparator.UseVisualStyleBackColor = True
        '
        'btn0
        '
        Me.btn0.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn0.AutoCheck = False
        Me.btn0.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn0.KeyVal = "0"
        Me.btn0.Location = New System.Drawing.Point(56, 124)
        Me.btn0.Margin = New System.Windows.Forms.Padding(0)
        Me.btn0.Name = "btn0"
        Me.btn0.Size = New System.Drawing.Size(56, 32)
        Me.btn0.TabIndex = 13
        Me.btn0.Text = "0"
        Me.btn0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn0.UseVisualStyleBackColor = True
        '
        'btnToggleSign
        '
        Me.btnToggleSign.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnToggleSign.AutoCheck = False
        Me.btnToggleSign.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnToggleSign.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnToggleSign.KeyVal = Nothing
        Me.btnToggleSign.Location = New System.Drawing.Point(0, 124)
        Me.btnToggleSign.Margin = New System.Windows.Forms.Padding(0)
        Me.btnToggleSign.Name = "btnToggleSign"
        Me.btnToggleSign.Size = New System.Drawing.Size(56, 32)
        Me.btnToggleSign.TabIndex = 12
        Me.btnToggleSign.Text = "±"
        Me.btnToggleSign.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnToggleSign.UseVisualStyleBackColor = True
        '
        'btnMinus
        '
        Me.btnMinus.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnMinus.AutoCheck = False
        Me.btnMinus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnMinus.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMinus.KeyVal = ""
        Me.btnMinus.Location = New System.Drawing.Point(168, 62)
        Me.btnMinus.Margin = New System.Windows.Forms.Padding(0)
        Me.btnMinus.Name = "btnMinus"
        Me.btnMinus.Size = New System.Drawing.Size(57, 31)
        Me.btnMinus.TabIndex = 11
        Me.btnMinus.Tag = ""
        Me.btnMinus.Text = "-"
        Me.btnMinus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnMinus.UseVisualStyleBackColor = True
        '
        'btn3
        '
        Me.btn3.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn3.AutoCheck = False
        Me.btn3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn3.KeyVal = "3"
        Me.btn3.Location = New System.Drawing.Point(112, 93)
        Me.btn3.Margin = New System.Windows.Forms.Padding(0)
        Me.btn3.Name = "btn3"
        Me.btn3.Size = New System.Drawing.Size(56, 31)
        Me.btn3.TabIndex = 10
        Me.btn3.Text = "3"
        Me.btn3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn3.UseVisualStyleBackColor = True
        '
        'btn2
        '
        Me.btn2.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn2.AutoCheck = False
        Me.btn2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn2.KeyVal = "2"
        Me.btn2.Location = New System.Drawing.Point(56, 93)
        Me.btn2.Margin = New System.Windows.Forms.Padding(0)
        Me.btn2.Name = "btn2"
        Me.btn2.Size = New System.Drawing.Size(56, 31)
        Me.btn2.TabIndex = 9
        Me.btn2.Text = "2"
        Me.btn2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn2.UseVisualStyleBackColor = True
        '
        'btn1
        '
        Me.btn1.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn1.AutoCheck = False
        Me.btn1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn1.KeyVal = "1"
        Me.btn1.Location = New System.Drawing.Point(0, 93)
        Me.btn1.Margin = New System.Windows.Forms.Padding(0)
        Me.btn1.Name = "btn1"
        Me.btn1.Size = New System.Drawing.Size(56, 31)
        Me.btn1.TabIndex = 8
        Me.btn1.Text = "1"
        Me.btn1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn1.UseVisualStyleBackColor = True
        '
        'btnMulti
        '
        Me.btnMulti.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnMulti.AutoCheck = False
        Me.btnMulti.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnMulti.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMulti.KeyVal = ""
        Me.btnMulti.Location = New System.Drawing.Point(168, 31)
        Me.btnMulti.Margin = New System.Windows.Forms.Padding(0)
        Me.btnMulti.Name = "btnMulti"
        Me.btnMulti.Size = New System.Drawing.Size(57, 31)
        Me.btnMulti.TabIndex = 7
        Me.btnMulti.Tag = ""
        Me.btnMulti.Text = "×"
        Me.btnMulti.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnMulti.UseVisualStyleBackColor = True
        '
        'btn6
        '
        Me.btn6.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn6.AutoCheck = False
        Me.btn6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn6.KeyVal = "6"
        Me.btn6.Location = New System.Drawing.Point(112, 62)
        Me.btn6.Margin = New System.Windows.Forms.Padding(0)
        Me.btn6.Name = "btn6"
        Me.btn6.Size = New System.Drawing.Size(56, 31)
        Me.btn6.TabIndex = 6
        Me.btn6.Text = "6"
        Me.btn6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn6.UseVisualStyleBackColor = True
        '
        'btn5
        '
        Me.btn5.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn5.AutoCheck = False
        Me.btn5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn5.KeyVal = "5"
        Me.btn5.Location = New System.Drawing.Point(56, 62)
        Me.btn5.Margin = New System.Windows.Forms.Padding(0)
        Me.btn5.Name = "btn5"
        Me.btn5.Size = New System.Drawing.Size(56, 31)
        Me.btn5.TabIndex = 5
        Me.btn5.Text = "5"
        Me.btn5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn5.UseVisualStyleBackColor = True
        '
        'btn4
        '
        Me.btn4.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn4.AutoCheck = False
        Me.btn4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn4.KeyVal = "4"
        Me.btn4.Location = New System.Drawing.Point(0, 62)
        Me.btn4.Margin = New System.Windows.Forms.Padding(0)
        Me.btn4.Name = "btn4"
        Me.btn4.Size = New System.Drawing.Size(56, 31)
        Me.btn4.TabIndex = 4
        Me.btn4.Text = "4"
        Me.btn4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn4.UseVisualStyleBackColor = True
        '
        'btnDiv
        '
        Me.btnDiv.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnDiv.AutoCheck = False
        Me.btnDiv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDiv.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDiv.KeyVal = ""
        Me.btnDiv.Location = New System.Drawing.Point(168, 0)
        Me.btnDiv.Margin = New System.Windows.Forms.Padding(0)
        Me.btnDiv.Name = "btnDiv"
        Me.btnDiv.Size = New System.Drawing.Size(57, 31)
        Me.btnDiv.TabIndex = 3
        Me.btnDiv.Tag = ""
        Me.btnDiv.Text = "÷"
        Me.btnDiv.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnDiv.UseVisualStyleBackColor = True
        '
        'btn9
        '
        Me.btn9.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn9.AutoCheck = False
        Me.btn9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn9.KeyVal = "9"
        Me.btn9.Location = New System.Drawing.Point(112, 31)
        Me.btn9.Margin = New System.Windows.Forms.Padding(0)
        Me.btn9.Name = "btn9"
        Me.btn9.Size = New System.Drawing.Size(56, 31)
        Me.btn9.TabIndex = 2
        Me.btn9.Text = "9"
        Me.btn9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn9.UseVisualStyleBackColor = True
        '
        'btn8
        '
        Me.btn8.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn8.AutoCheck = False
        Me.btn8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn8.KeyVal = "8"
        Me.btn8.Location = New System.Drawing.Point(56, 31)
        Me.btn8.Margin = New System.Windows.Forms.Padding(0)
        Me.btn8.Name = "btn8"
        Me.btn8.Size = New System.Drawing.Size(56, 31)
        Me.btn8.TabIndex = 1
        Me.btn8.Text = "8"
        Me.btn8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn8.UseVisualStyleBackColor = True
        '
        'btn7
        '
        Me.btn7.Appearance = System.Windows.Forms.Appearance.Button
        Me.btn7.AutoCheck = False
        Me.btn7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btn7.KeyVal = "7"
        Me.btn7.Location = New System.Drawing.Point(0, 31)
        Me.btn7.Margin = New System.Windows.Forms.Padding(0)
        Me.btn7.Name = "btn7"
        Me.btn7.Size = New System.Drawing.Size(56, 31)
        Me.btn7.TabIndex = 0
        Me.btn7.Text = "7"
        Me.btn7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btn7.UseVisualStyleBackColor = True
        '
        'btnCE
        '
        Me.btnCE.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnCE.AutoCheck = False
        Me.btnCE.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCE.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCE.KeyVal = Nothing
        Me.btnCE.Location = New System.Drawing.Point(0, 0)
        Me.btnCE.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCE.Name = "btnCE"
        Me.btnCE.Size = New System.Drawing.Size(56, 31)
        Me.btnCE.TabIndex = 16
        Me.btnCE.Text = "C&E"
        Me.btnCE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnCE.UseVisualStyleBackColor = True
        '
        'btnC
        '
        Me.btnC.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnC.AutoCheck = False
        Me.btnC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnC.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnC.KeyVal = Nothing
        Me.btnC.Location = New System.Drawing.Point(56, 0)
        Me.btnC.Margin = New System.Windows.Forms.Padding(0)
        Me.btnC.Name = "btnC"
        Me.btnC.Size = New System.Drawing.Size(56, 31)
        Me.btnC.TabIndex = 17
        Me.btnC.Text = "&C"
        Me.btnC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnC.UseVisualStyleBackColor = True
        '
        'btnDel
        '
        Me.btnDel.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnDel.AutoCheck = False
        Me.btnDel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnDel.KeyVal = Nothing
        Me.btnDel.Location = New System.Drawing.Point(112, 0)
        Me.btnDel.Margin = New System.Windows.Forms.Padding(0)
        Me.btnDel.Name = "btnDel"
        Me.btnDel.Size = New System.Drawing.Size(56, 31)
        Me.btnDel.TabIndex = 18
        Me.btnDel.Text = "←"
        Me.btnDel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnDel.UseVisualStyleBackColor = True
        '
        'btnCalc
        '
        Me.btnCalc.Appearance = System.Windows.Forms.Appearance.Button
        Me.btnCalc.AutoCheck = False
        Me.btnCalc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCalc.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCalc.KeyVal = ""
        Me.btnCalc.Location = New System.Drawing.Point(168, 124)
        Me.btnCalc.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCalc.Name = "btnCalc"
        Me.btnCalc.Size = New System.Drawing.Size(57, 32)
        Me.btnCalc.TabIndex = 19
        Me.btnCalc.Tag = ""
        Me.btnCalc.Text = "="
        Me.btnCalc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.btnCalc.UseVisualStyleBackColor = True
        '
        'SimpleCalculator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "SimpleCalculator"
        Me.Size = New System.Drawing.Size(225, 185)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents calcResult As System.Windows.Forms.TextBox
    Protected WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnPlus As CalculatorButton
    Friend WithEvents btnDecimalSeparator As CalculatorButton
    Friend WithEvents btn0 As CalculatorButton
    Friend WithEvents btnToggleSign As CalculatorButton
    Friend WithEvents btnMinus As CalculatorButton
    Friend WithEvents btn3 As CalculatorButton
    Friend WithEvents btn2 As CalculatorButton
    Friend WithEvents btn1 As CalculatorButton
    Friend WithEvents btnMulti As CalculatorButton
    Friend WithEvents btn6 As CalculatorButton
    Friend WithEvents btn5 As CalculatorButton
    Friend WithEvents btn4 As CalculatorButton
    Friend WithEvents btnDiv As CalculatorButton
    Friend WithEvents btn9 As CalculatorButton
    Friend WithEvents btn8 As CalculatorButton
    Friend WithEvents btn7 As CalculatorButton
    Friend WithEvents btnCE As CalculatorButton
    Friend WithEvents btnC As CalculatorButton
    Friend WithEvents btnDel As CalculatorButton
    Friend WithEvents btnCalc As CalculatorButton
End Class
