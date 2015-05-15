Imports System.Runtime.CompilerServices
Imports Microsoft.CodeAnalysis.Editing

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

    ''' <summary>
    ''' Returns a RaiseEvent-Statement in the format of 'RaiseEvent EventName(me,e_parametername)' where EventName and e_parametername can be passed as arguments.
    ''' </summary>
    ''' <param name="eventName">Name of the Event.</param>
    ''' <param name="e_parameterName">Name of the EventParameter.</param>
    ''' <returns></returns>
    <Extension>
    Public Function RaiseEventStatement(generator As SyntaxGenerator,
                                        eventName As String, e_parameterName As String) As RaiseEventStatementSyntax

        Dim eventNameIdentifier = SyntaxFactory.IdentifierName(eventName)
        Dim syntaxList = SyntaxFactory.SeparatedList(Of ArgumentSyntax)(DirectCast(
                    ({SyntaxFactory.SimpleArgument(SyntaxFactory.MeExpression()),
                      SyntaxFactory.SimpleArgument(SyntaxFactory.IdentifierName(e_parameterName))}),
                    IEnumerable(Of ArgumentSyntax)))
        Dim arguments = SyntaxFactory.ArgumentList(syntaxList)
        Return SyntaxFactory.RaiseEventStatement(eventNameIdentifier, arguments)

    End Function

End Module
