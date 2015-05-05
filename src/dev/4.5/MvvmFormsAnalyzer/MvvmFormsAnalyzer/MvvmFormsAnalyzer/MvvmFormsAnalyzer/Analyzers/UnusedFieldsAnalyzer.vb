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

    Private myFieldList As FieldReferenceTracker

    Public Overrides Sub Initialize(context As AnalysisContext)

        context.RegisterCompilationStartAction(
            Sub(innerContext As CompilationStartAnalysisContext)

                'We're tracking all private fields and we list all references to them.
                'If we have fields with no references, those fields are not in use.
                myFieldList = New FieldReferenceTracker

                context.RegisterSymbolAction(
                        AddressOf SymbolActionProc, SymbolKind.Field)

                context.RegisterSyntaxNodeAction(
                    Sub(innercontext2 As SyntaxNodeAnalysisContext)

                        Dim symInfo = innercontext2.SemanticModel.GetSymbolInfo(innercontext2.Node)

                        Dim actualSymbol = symInfo.Symbol

                        If actualSymbol IsNot Nothing Then
                            'Test, if symbol is a field.
                            If actualSymbol.Kind = SymbolKind.Field AndAlso
                               DirectCast(actualSymbol, IFieldSymbol).DeclaredAccessibility = Accessibility.Private Then
                                myFieldList.ReferenceFields.Add(DirectCast(actualSymbol, IFieldSymbol))
                            End If
                        End If

                    End Sub, SyntaxKind.IdentifierName, SyntaxKind.SimpleMemberAccessExpression)

                innerContext.RegisterCompilationEndAction(
                    Sub(innerInnercontext As CompilationAnalysisContext)
                        For Each fieldSymbolItem In myFieldList.DefinedFields
                            'This is done at the very end: If there are DefinedFields for which
                            'there are no fields in the reference fields, those fields are not in use.
                            If Not myFieldList.ReferenceFields.Contains(fieldSymbolItem.FieldSymbol) Then
                                innerInnercontext.ReportDiagnostic(
                                    Diagnostic.Create(Rule,
                                                      fieldSymbolItem.FieldSymbol.Locations.First,
                                                      fieldSymbolItem.FieldSymbol.Name))
                            End If
                        Next
                    End Sub)
            End Sub)
    End Sub

    Private Sub SymbolActionProc(innerContext As SymbolAnalysisContext)

        'We don't do it on static fields (or fields in modules).
        If innerContext.Symbol.IsStatic Then
            Return
        End If

        Dim contType = innerContext.Symbol.ContainingType

        If contType IsNot Nothing Then
            If contType.IsAnonymousType Then
                Return
            End If

        Else
            'we shouldn't be here.
            Return
        End If

        If DirectCast(innerContext.Symbol, IFieldSymbol).DeclaredAccessibility = Accessibility.Private Then
            myFieldList.DefinedFields.Add(New FieldActionTuple With {.FieldSymbol = DirectCast(innerContext.Symbol, IFieldSymbol),
                                                                     .Action = CodeFixAction.SimpleProperty})

            Debug.WriteLine("Found private field:" & innerContext.Symbol.Name)
        End If
    End Sub
End Class

Public Class FieldReferenceTracker

    Public Property ClassType As ClassTypes
    Public Property DefinedFields As HashSet(Of FieldActionTuple) = New HashSet(Of FieldActionTuple)
    Public Property ReferenceFields As HashSet(Of IFieldSymbol) = New HashSet(Of IFieldSymbol)

End Class

Public Class FieldActionTuple
    Property FieldSymbol As IFieldSymbol
    Property Action As CodeFixAction
End Class

Public Enum ClassTypes
    None
    Model
    ViewModel
    DataTransport
End Enum

Public Enum CodeFixAction
    SimpleProperty
    ManualRaiseNotifyPropertyChanged
    UseSetPropertyOfBaseClass
End Enum
