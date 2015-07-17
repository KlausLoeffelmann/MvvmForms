Imports System.Runtime.CompilerServices

Friend Module ExtendedTypeReflector

    Dim NumTypes As New List(Of TypeCode) From {TypeCode.Byte, TypeCode.Decimal, TypeCode.Double,
                                                TypeCode.Int16, TypeCode.Int32, TypeCode.Int64,
                                                TypeCode.SByte, TypeCode.Single, TypeCode.UInt16,
                                                TypeCode.UInt32, TypeCode.UInt64}

    ''' <summary>
    ''' Typprüfung auf Numerisch
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function IsNumeric(ByVal t As Type) As Boolean
        If NumTypes.IndexOf(Type.GetTypeCode(t)) > -1 Then
            Return True
        End If

        If t Is GetType(System.Numerics.BigInteger) Then
            Return True
        End If

        If t Is GetType(System.Numerics.Complex) Then
            Return True
        End If
        Return False
    End Function

    <Extension()>
    Public Function IsGenericNumeric(ByVal t As Type) As Boolean
        If t.IsGenericType Then
            Return t.GetGenericArguments(0).IsNumeric
        End If
        Return False
    End Function
End Module
