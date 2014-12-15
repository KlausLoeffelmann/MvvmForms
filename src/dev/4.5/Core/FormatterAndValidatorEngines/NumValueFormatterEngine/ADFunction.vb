''' <summary>
''' Speichert die Parameter für eine Funktion, die von der Klasse zur Berechnung beliebige mathematischer 
''' Ausdrücke <see cref="FormulaEvaluator">FormulaEvaluator</see>berücksichtigt werden kann.
''' </summary>
''' <remarks></remarks>
Public Class FormulaEvaluatorFunction
    Implements IComparable

    ''' <summary>
    ''' Stellt einen Delegaten zur Verfügung, mit der benutzerdefinierte Funktionen für die Formelberechnung 
    ''' durch <see cref="FormulaEvaluator">FormulaEvaluator</see> implementiert werden können.
    ''' </summary>
    ''' <param name="parArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function FormularEvaluatorFunctionDelegate(ByVal parArray As Double()) As Double

    Protected myFunctionname As String
    Protected myParameters As Integer
    Protected myFunctionProc As FormularEvaluatorFunctionDelegate
    Protected myConsts As ArrayList
    Protected myIsOperator As Boolean
    Protected myPriority As Byte

    ''' <summary>
    ''' Erstellt eine neue Intanz dieser Klasse. 
    ''' Verwenden Sie diese Überladungsversion, um Operatoren zu erstellen, die aus einem Zeichen bestehen,
    ''' </summary>
    ''' <param name="OperatorChar">Das Zeichen, das den Operator darstellt.</param>
    ''' <param name="FunctionProc">Der ADFunctionDelegat für die Berechnung durch diesen Operator.</param>
    ''' <param name="Priority">Die Operatorpriorität (3= Potenz, 2=Punkt, 1=Strich).</param>
    ''' <remarks></remarks>
    Sub New(ByVal OperatorChar As Char, ByVal FunctionProc As FormularEvaluatorFunctionDelegate, ByVal Priority As Byte)

        If Priority < 1 Then
            Dim Up As New ArgumentException("Priority kann für Operatoren nicht kleiner 1 sein.")
            Throw Up
        End If

        myFunctionname = OperatorChar.ToString
        myParameters = 2
        myFunctionProc = FunctionProc
        myIsOperator = True
        myPriority = Priority
    End Sub

    ''' <summary>
    ''' Erstellt eine neue Intanz dieser Klasse. 
    ''' Verwenden Sie diese Überladungsversion, um Funktionen zu erstellen, die aus mehreren Zeichen bestehen.
    ''' </summary>
    ''' <param name="FunctionName">Die Zeichenfolge, die den Funktionsnamen darstellt.</param>
    ''' <param name="FunctionProc">Der ADFunctionDelegat für die Berechnung durch diese Funktion.</param>
    ''' <param name="Parameters">Die Anzahl der Parameter, die diese Funktion entgegen nimmt.</param>
    ''' <remarks></remarks>
    Sub New(ByVal FunctionName As String, ByVal FunctionProc As FormularEvaluatorFunctionDelegate, ByVal Parameters As Integer)
        myFunctionname = FunctionName
        myFunctionProc = FunctionProc
        myParameters = Parameters
        myIsOperator = False
        myPriority = 0
    End Sub

    ''' <summary>
    ''' Liefert den Funktionsnamen bzw. das Operatorenzeichen zurück.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FunctionName() As String
        Get
            Return myFunctionname
        End Get
    End Property

    ''' <summary>
    ''' Liefert die Anzahl der zur Anwendung kommenden Parameter für diese Funktion zurück.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Parameters() As Integer
        Get
            Return myParameters
        End Get
    End Property

    ''' <summary>
    ''' Zeigt an, ob es sich bei dieser Instanz um einen Operator handelt.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsOperator() As Boolean
        Get
            Return myIsOperator
        End Get
    End Property

    ''' <summary>
    ''' Ermittelt die Priorität, die dieser Operator hat. (3= Potenz, 2=Punkt, 1=Strich)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Priority() As Byte
        Get
            Return myPriority
        End Get
    End Property

    ''' <summary>
    ''' Ermittelt den Delegaten, der diese Funktion oder diesen Operator berechnet.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FunctionProc() As FormularEvaluatorFunctionDelegate
        Get
            Return myFunctionProc
        End Get
    End Property

    ''' <summary>
    ''' Ruft den Delegaten auf, der diese Funktion (diesen Operator) berechnet.
    ''' </summary>
    ''' <param name="parArray">Das Array, dass die Argumente der Funktion enthält.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Operate(ByVal parArray As Double()) As Double
        If Parameters > -1 Then
            If parArray.Length <> Parameters Then
                Dim Up As New ArgumentException _
                    ("Anzahl Parameter entspricht nicht der Vorschrift der Funktion " & FunctionName)
                Throw Up
            End If
        End If
        Return myFunctionProc(parArray)
    End Function

    ''' <summary>
    ''' Vergleicht zwei Instanzen dieser Klasse anhand ihres Prioritätswertes.
    ''' </summary>
    ''' <param name="obj">Eine ADFunction-Instanz, die mit dieser Instanz verglichen werden soll.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If obj.GetType Is GetType(FormulaEvaluatorFunction) Then
            Return myPriority.CompareTo(DirectCast(obj, FormulaEvaluatorFunction).Priority) * -1
        Else
            Dim up As New ArgumentException("Nur ActiveDev.Function-Objekte können verglichen/sortiert werden")
            Throw up
        End If
    End Function

End Class
