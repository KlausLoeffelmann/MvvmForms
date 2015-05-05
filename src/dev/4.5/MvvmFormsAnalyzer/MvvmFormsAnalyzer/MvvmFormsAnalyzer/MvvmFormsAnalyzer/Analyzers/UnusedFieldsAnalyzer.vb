<DiagnosticAnalyzer(LanguageNames.VisualBasic)>
Public Class UnusedFieldsAnalyzerAnalyzer
    Inherits DiagnosticAnalyzer

    Public Const DiagnosticId = "ADMF0001"

    ' You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    Friend Shared ReadOnly Title As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerTitle),
                                                                                      My.Resources.ResourceManager,
                                                                                      GetType(My.Resources.Resources))

    Friend Shared ReadOnly MessageFormat As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerMessageFormat),
                                                                                              My.Resources.ResourceManager,
                                                                                              GetType(My.Resources.Resources))
    Friend Shared ReadOnly Description As LocalizableString = New LocalizableResourceString(NameOf(My.Resources.AnalyzerDescription),
                                                                                            My.Resources.ResourceManager,
                                                                                            GetType(My.Resources.Resources))
    Friend Const Category = "Naming"

    Friend Shared Rule As New DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault:=True, description:=Description)

    Public Overrides ReadOnly Property SupportedDiagnostics As ImmutableArray(Of DiagnosticDescriptor)
        Get
            Return ImmutableArray.Create(Rule)
        End Get
    End Property

    Public Overrides Sub Initialize(context As AnalysisContext)

        context.RegisterCompilationStartAction(
            Sub(innerContext As CompilationStartAnalysisContext)

                'We're tracking all private fields and we list all references to them.
                'If we have fields with no references, those fields are not in use.
                Dim fieldlist As New FieldReferenceTracker

                context.RegisterSyntaxNodeAction(
                    Sub(innercontext2 As SyntaxNodeAnalysisContext)
                        Dim declaredSymbol = innercontext2.SemanticModel.GetDeclaredSymbol(innercontext2.Node)
                        Debug.WriteLine("Found declared Symbol:" & declaredSymbol.Name)
                    End Sub, SyntaxKind.ClassBlock)

                context.RegisterSymbolAction(
                    Sub(innerContext2 As SymbolAnalysisContext)
                        'Only, if parent is class and has Model or Viewmodel Attribute on top.
                        Dim contType = innerContext2.Symbol.ContainingType
                        If contType IsNot Nothing Then
                            Debug.WriteLine("Found containing type:" & contType.Name)
                        End If
                        'For Each item In contType.GetAttributes
                        '    If item.AttributeClass.Name = "ModelAttribute" Then

                        '    End If
                        'Next
                        If DirectCast(innerContext2.Symbol, IFieldSymbol).DeclaredAccessibility = Accessibility.Private Then
                            fieldlist.DefinedFields.Add(DirectCast(innerContext2.Symbol, IFieldSymbol))
                            Debug.WriteLine("Found private field:" & innerContext2.Symbol.Name)
                        End If
                    End Sub, SymbolKind.Field)

                context.RegisterSyntaxNodeAction(
                    Sub(innercontext2 As SyntaxNodeAnalysisContext)

                        Dim symInfo = innercontext2.SemanticModel.GetSymbolInfo(innercontext2.Node)

                        Dim actualSymbol = symInfo.Symbol

                        If actualSymbol IsNot Nothing Then
                            'Test, if symbol is a field.
                            If actualSymbol.Kind = SymbolKind.Field AndAlso
                               DirectCast(actualSymbol, IFieldSymbol).DeclaredAccessibility = Accessibility.Private Then
                                fieldlist.ReferenceFields.Add(DirectCast(actualSymbol, IFieldSymbol))
                            End If
                        End If

                    End Sub, SyntaxKind.IdentifierName, SyntaxKind.SimpleMemberAccessExpression)

                innerContext.RegisterCompilationEndAction(
                    Sub(innerInnercontext As CompilationAnalysisContext)
                        For Each fieldSymbolItem In fieldlist.DefinedFields
                            'This is done at the very end: If there are DefinedFields for which
                            'there are no fields in the reference fields, those fields are not in use.
                            If Not fieldlist.ReferenceFields.Contains(fieldSymbolItem) Then
                                innerInnercontext.ReportDiagnostic(
                                    Diagnostic.Create(Rule,
                                                      fieldSymbolItem.Locations.First,
                                                      fieldSymbolItem.Name))
                            End If
                        Next
                    End Sub)
                                                     End Sub)
            End Sub
End Class

Public Class FieldReferenceTracker

    Public Property ClassType As ClassTypes
    Public Property DefinedFields As HashSet(Of IFieldSymbol) = New HashSet(Of IFieldSymbol)
    Public Property ReferenceFields As HashSet(Of IFieldSymbol) = New HashSet(Of IFieldSymbol)

End Class

Public Enum ClassTypes
    None
    Model
    ViewModel
    DataTransport
End Enum
