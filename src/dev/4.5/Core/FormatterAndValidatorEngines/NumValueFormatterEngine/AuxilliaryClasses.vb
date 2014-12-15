Imports System.Collections.ObjectModel

''' <summary>
''' Auflistung, in der alle Operatoren gleicher Priorität gesammelt werden, damit 
''' es die Möglichkeit gibt, sie von links nach rechts (in einem Rutsch) zu verarbeiten.
''' </summary>
''' <remarks></remarks>
Public Class ADOperatorsOfSamePriority
    Inherits Collection(Of FormulaEvaluatorFunction)

    Private myPriority As Byte

    Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As FormulaEvaluatorFunction)
        If Not item.IsOperator Then
            Dim locEx As New ArgumentException("Nur Operatoren (keine Funktionen) können dieser Auflistung hinzugefügt werden!")
            Throw locEx
        End If
        If Me.Count = 0 Then
            myPriority = item.Priority
        Else
            'Überprüfen, ob es dieselbe Priorität ist, sonst Ausnahme!
            If item.Priority <> myPriority Then
                Dim locEx As New ArgumentException("Nur Operatoren der Priorität " & myPriority & " können dieser Auflistung hinzugefügt werden!")
                Throw locEx
            End If
        End If
        MyBase.InsertItem(index, item)
    End Sub

    Protected Overrides Sub SetItem(ByVal index As Integer, ByVal item As FormulaEvaluatorFunction)
        Dim locEx As New ArgumentException("Elemente können in dieser Auflistung nicht ausgetauscht werden!")
        Throw locEx
    End Sub

    ''' <summary>
    ''' Liefert die Priorität dieser Operatorenauflistung zurück.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Priority() As Byte
        Get
            Return myPriority
        End Get
    End Property
End Class

''' <summary>
''' Fasst alle Operatorenlisten nach Priorität kategorisiert in einer übergeordneten Auflistung zusammen.
''' </summary>
''' <remarks></remarks>
Public Class PrioritizedOperators
    Inherits KeyedCollection(Of Byte, ADOperatorsOfSamePriority)

    Private myHighestPriority As Byte
    Private myLowestPriority As Byte

    ''' <summary>
    ''' Fügt einer der untergeordneten Auflistungen einen neuen Operator hinzu, 
    ''' in Abhängigkeit von seiner Priorität.
    ''' </summary>
    ''' <param name="Function"></param>
    ''' <remarks></remarks>
    Public Sub AddFunction(ByVal [Function] As FormulaEvaluatorFunction)
        'Feststellen, ob es schon eine Auflistung für diese Operator-Priorität gibt
        If Me.Contains([Function].Priority) Then
            'Ja - dieser hinzufügen,
            Me([Function].Priority).Add([Function])
        Else
            'Nein - anlegen und hinzufügen.
            Dim locOperatorsOfSamePriority As New ADOperatorsOfSamePriority()
            locOperatorsOfSamePriority.Add([Function])
            Me.Add(locOperatorsOfSamePriority)
        End If
    End Sub

    Protected Overrides Function GetKeyForItem(ByVal item As ADOperatorsOfSamePriority) As Byte
        Return item.Priority
    End Function

    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As ADOperatorsOfSamePriority)

        If Me.Count = 0 Then
            myHighestPriority = item.Priority
            myLowestPriority = item.Priority
            MyBase.InsertItem(index, item)
            Return
        End If

        MyBase.InsertItem(index, item)

        If myHighestPriority < item.Priority Then
            myHighestPriority = item.Priority
        End If

        If myLowestPriority > item.Priority Then
            myLowestPriority = item.Priority
        End If
    End Sub

    ''' <summary>
    ''' Liefert alle Operatorzeichen einer bestimmten Priorität als Char-Array zurück.
    ''' </summary>
    ''' <param name="Priority">Die Priorität, deren Operatoren zusammengestellt werden sollen.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OperatorChars(ByVal Priority As Byte) As Char()
        If Me.Contains(Priority) Then
            Dim locChars As New List(Of Char)
            For Each locFunction As FormulaEvaluatorFunction In Me(Priority)
                locChars.Add(Convert.ToChar(locFunction.FunctionName))
            Next
            Return locChars.ToArray
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Liefert die Funktion zurück, die sich durch ein Operator-Zeichen einer bestimmten Priorität ergibt.
    ''' </summary>
    ''' <param name="Priority">Die Priorität, die den Operatoren entspricht, die nach den Operatorzeichen durchsucht werden sollen.</param>
    ''' <param name="OperatorChar">Das Operatorzeichen mit der angegebenen Priorität, dessen Funktionsklasse ermittelt werden soll.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OperatorByChar(ByVal Priority As Byte, ByVal OperatorChar As Char) As FormulaEvaluatorFunction
        If Me.Contains(Priority) Then
            For Each locFunction As FormulaEvaluatorFunction In Me(Priority)
                If OperatorChar = Convert.ToChar(locFunction.FunctionName) Then
                    Return locFunction
                End If
            Next
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Liefert die höchste Prioritätennummer zurück.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property HighestPriority() As Byte
        Get
            Return myHighestPriority
        End Get
    End Property

    ''' <summary>
    ''' Liefert die kleinste Prioritätennummer zurück.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LowestPriority() As Byte
        Get
            Return myLowestPriority
        End Get
    End Property
End Class
