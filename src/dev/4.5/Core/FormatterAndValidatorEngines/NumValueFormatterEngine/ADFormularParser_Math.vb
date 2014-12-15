Partial Public Class FormulaEvaluator

    Public Shared Function Addition(ByVal Args() As Double) As Double
        Return Args(0) + Args(1)
    End Function

    Public Shared Function Substraction(ByVal Args() As Double) As Double
        Return Args(0) - Args(1)
    End Function

    Public Shared Function Multiplication(ByVal Args() As Double) As Double
        Return Args(0) * Args(1)
    End Function

    Public Shared Function Division(ByVal Args() As Double) As Double
        Return Args(0) / Args(1)
    End Function

    Public Shared Function Remainder(ByVal Args() As Double) As Double
        Return Decimal.Remainder(New Decimal(Args(0)), New Decimal(Args(1)))
    End Function

    Public Shared Function Power(ByVal Args() As Double) As Double
        Return Args(0) ^ Args(1)
    End Function

    Public Shared Function Sin(ByVal Args() As Double) As Double
        Return Math.Sin(Args(0))
    End Function

    Public Shared Function Cos(ByVal Args() As Double) As Double
        Return Math.Cos(Args(0))
    End Function

    Public Shared Function Tan(ByVal Args() As Double) As Double
        Return Math.Tan(Args(0))
    End Function

    Public Shared Function Sqrt(ByVal Args() As Double) As Double
        Return Math.Sqrt(Args(0))
    End Function

    Public Shared Function PI(ByVal Args() As Double) As Double
        Return Math.PI
    End Function

    Public Shared Function Tanh(ByVal Args() As Double) As Double
        Return Math.Tanh(Args(0))
    End Function

    Public Shared Function LogDec(ByVal Args() As Double) As Double
        Return Math.Log10(Args(0))
    End Function

    Public Shared Function XVar(ByVal Args() As Double) As Double
        Return XVariable
    End Function

    Public Shared Function YVar(ByVal Args() As Double) As Double
        Return YVariable
    End Function

    Public Shared Function ZVar(ByVal Args() As Double) As Double
        Return ZVariable
    End Function

    Public Shared Function Max(ByVal Args() As Double) As Double

        Dim retDouble As Double

        If Args.Length = 0 Then
            Return 0
        Else
            retDouble = Args(0)
            For Each locDouble As Double In Args
                If retDouble < locDouble Then
                    retDouble = locDouble
                End If
            Next
        End If
        Return retDouble

    End Function

    Public Shared Function Min(ByVal Args() As Double) As Double

        Dim retDouble As Double

        If Args.Length = 0 Then
            Return 0
        Else
            retDouble = Args(0)
            For Each locDouble As Double In Args
                If retDouble > locDouble Then
                    retDouble = locDouble
                End If
            Next
        End If
        Return retDouble

    End Function

    Public Shared Property XVariable() As Double
        Get
            Return myXVariable
        End Get
        Set(ByVal Value As Double)
            myXVariable = Value
        End Set
    End Property

    Public Shared Property YVariable() As Double
        Get
            Return myYVariable
        End Get
        Set(ByVal Value As Double)
            myYVariable = Value
        End Set
    End Property

    Public Shared Property ZVariable() As Double
        Get
            Return myZVariable
        End Get
        Set(ByVal Value As Double)
            myZVariable = Value
        End Set
    End Property

End Class
