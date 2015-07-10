<DiagnosticAnalyzer(LanguageNames.VisualBasic)>
Public Class UnusedFieldsAnalyzer
    Inherits DiagnosticAnalyzer

    Public Const DiagnosticId = "ADMF0001"

    ' You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    Friend Shared ReadOnly Title As LocalizableString =
        New LocalizableResourceString(NameOf(My.Resources.AnalyzerTitle),
                                            My.Resources.ResourceManager,
                                            GetType(My.Resources.Resources))

    Friend Shared ReadOnly MessageFormat As LocalizableString =
        New LocalizableResourceString(NameOf(My.Resources.AnalyzerMessageFormat),
                                            My.Resources.ResourceManager,
                                            GetType(My.Resources.Resources))

    Friend Shared ReadOnly Description As LocalizableString =
        New LocalizableResourceString(NameOf(My.Resources.AnalyzerDescription),
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

        Dim myFieldList As FieldReferenceTracker

        context.RegisterCompilationStartAction(
            Sub(innerContext As CompilationStartAnalysisContext)
                'We're tracking all private fields and we list all references to them.
                'If we have fields with no references, those fields are not in use.
                myFieldList = New FieldReferenceTracker

                context.RegisterSymbolAction(
                Sub(innerContext2 As SymbolAnalysisContext)
                    'Works:
                    Dim codeFixAction As CodeFixAction = CodeFixAction.SimpleProperty

                    'Does not work. Did it work in VB2013?
                    'Dim codeFixAction = CodeFixAction.SimpleProperty

                    'We don't do it on static fields (or fields in modules).
                    If innerContext2.Symbol.IsStatic Then
                        Return
                    End If

                    Dim contType = innerContext2.Symbol.ContainingType

                    If contType IsNot Nothing Then
                        If contType.IsAnonymousType Then
                            Return
                        End If
                    End If

                    If DirectCast(innerContext2.Symbol, IFieldSymbol).
                                DeclaredAccessibility = Accessibility.Private Then
                        myFieldList.DefinedFields.Add(DirectCast(innerContext2.Symbol, IFieldSymbol))
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
                                myFieldList.ReferenceFields.Add(DirectCast(actualSymbol, IFieldSymbol))
                            End If
                        End If
                    End Sub, SyntaxKind.IdentifierName, SyntaxKind.SimpleMemberAccessExpression)

                innerContext.RegisterCompilationEndAction(
                    Sub(innerInnercontext As CompilationAnalysisContext)
                        For Each fieldSymbolItem In myFieldList.DefinedFields
                            'This is done at the very end: If there are DefinedFields for which
                            'there are no fields in the reference fields, those fields are not in use.
                            If Not myFieldList.ReferenceFields.Contains(fieldSymbolItem) Then
                                Dim diagToReport = Diagnostic.Create(UnusedFieldsAnalyzer.Rule,
                                      fieldSymbolItem.Locations.First,
                                      fieldSymbolItem.Name)

                                innerInnercontext.ReportDiagnostic(diagToReport)

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

Public Enum CodeFixAction
    SimpleProperty
    ManualRaiseNotifyPropertyChanged
    UseSetPropertyOfBaseClass
End Enum

Public Class ActionCascadeManager


    Sub Register(context As AnalysisContext, innerContext As CompilationStartAnalysisContext)

    End Sub

    Private Sub SymbolActionProc()

    End Sub
End Class
