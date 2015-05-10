Imports System.Runtime.CompilerServices

Module AnalyzerExtensionMethods

    <Extension>
    Public Function HasTypeInInheritanceHierarchy(typeSymbol As INamedTypeSymbol,
                                                  baseTypeName As String) As Boolean

        Do While String.IsNullOrEmpty(typeSymbol?.Name) OrElse typeSymbol.Name <> "Object"
            If typeSymbol.Name = baseTypeName Then
                Return True
            End If
            typeSymbol = typeSymbol.BaseType
        Loop
        Return False
    End Function

    <Extension>
    Public Function GetMethodOrInheritedMethod(typeSymbol As INamedTypeSymbol,
                                               methodname As String) As ISymbol

        Do While String.IsNullOrEmpty(typeSymbol?.Name) OrElse typeSymbol.Name <> "Object"
            Dim memberList = typeSymbol.GetMembers(methodname)
            If memberList.Count > 0 Then
                Return memberList(0)
            End If
            typeSymbol = typeSymbol.BaseType
        Loop
        Return Nothing
    End Function

End Module
