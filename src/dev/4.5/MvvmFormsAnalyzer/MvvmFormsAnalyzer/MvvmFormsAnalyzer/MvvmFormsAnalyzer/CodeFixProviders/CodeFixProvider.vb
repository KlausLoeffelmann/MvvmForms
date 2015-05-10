Imports System.Collections.Immutable
Imports Microsoft.CodeAnalysis.Editing
Imports Microsoft.CodeAnalysis.Formatting
Imports Microsoft.CodeAnalysis.Rename

<ExportCodeFixProvider(LanguageNames.VisualBasic, Name:=NameOf(MvvmFormsAnalyzerCodeFixProvider)), [Shared]>
Public Class MvvmFormsAnalyzerCodeFixProvider
    Inherits CodeFixProvider

    Public NotOverridable Overrides ReadOnly Property FixableDiagnosticIds As ImmutableArray(Of String)
        Get
            Return ImmutableArray.Create(UnusedFieldsAnalyzer.DiagnosticId)
        End Get
    End Property

    Public NotOverridable Overrides Function GetFixAllProvider() As FixAllProvider
        Return WellKnownFixAllProviders.BatchFixer
    End Function

    Public NotOverridable Overrides Async Function RegisterCodeFixesAsync(context As CodeFixContext) As Task

        Dim root = Await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(False)

        Dim semModel = Await context.Document.GetSemanticModelAsync
        Dim syntaxTrees = semModel.Compilation.SyntaxTrees


        Dim diagnostic = context.Diagnostics.First()
        Dim diagnosticSpan = diagnostic.Location.SourceSpan

        ' Find the type statement identified by the diagnostic.
        Dim declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType(Of FieldDeclarationSyntax)().First()

        ' Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create("Create Property.",
                              Function(c) InsertPropertyAsync(context.Document, declaration, c)),
            diagnostic)
    End Function

    Private Async Function InsertPropertyAsync(document As Document,
                                               fieldDeclaration As FieldDeclarationSyntax,
                                               cancellationToken As CancellationToken) As Task(Of Document)

        Dim fieldname = fieldDeclaration.Declarators(0).Names(0).ToString
        Dim propertyName As String = ""

        'Derive Propertyname from Fieldname.
        If fieldname.StartsWith("_") Then
            propertyName = fieldname.Substring(1, 1).ToUpper & fieldname.Substring(2)
        ElseIf fieldname.StartsWith("my") Then
            propertyName = fieldname.Substring(2, 1).ToUpper & fieldname.Substring(3)
        Else
            propertyName = fieldname.Substring(0, 1).ToUpper & fieldname.Substring(1) &
                           fieldname & "Property"
        End If

        Dim docRoot = Await document.GetSyntaxRootAsync

        Dim generator As SyntaxGenerator = SyntaxGenerator.GetGenerator(document.Project.Solution.Workspace, document.Project.Language)

        Dim setterStatement = generator.AssignmentStatement(generator.IdentifierName(fieldname),
                                                            generator.IdentifierName("value"))

        Dim getterStatement = generator.ReturnStatement(generator.IdentifierName(fieldname))

        Dim propGeneration As SyntaxNode =
                    generator.PropertyDeclaration(propertyName,
                                                  fieldDeclaration.Declarators(0).AsClause.Type,
                                                  Accessibility.Public,
                                                  DeclarationModifiers.None,
                                                  {getterStatement},
                                                  {setterStatement})

        propGeneration = propGeneration.WithAdditionalAnnotations(Formatter.Annotation)

        'Find end of field declaration
        Dim classNode = fieldDeclaration.Parent
        Dim childAndTokens = classNode.ChildNodesAndTokens
        Dim endClassNode As SyntaxNode = DirectCast(classNode, ClassBlockSyntax).EndClassStatement


        Dim newroot = docRoot.InsertNodesAfter(fieldDeclaration, {propGeneration})

        Return document.WithSyntaxRoot(newroot)

    End Function
End Class

'If contType.Interfaces.Where(
'                    Function(item) item.Name = "INotifyPropertyChanged").FirstOrDefault IsNot Nothing Then
'CodeFixAction = CodeFixAction.ManualRaiseNotifyPropertyChanged
'End If

'Dim basetype = contType.BaseType
'Dim methods = basetype.GetMembers

'If basetype IsNot Nothing AndAlso basetype.Name <> "Object" Then
'Debug.WriteLine($"Basetype found: {basetype.ToString}")
'If basetype.HasTypeInInheritanceHierarchy("BindableBase") Then
'Dim setProp = TryCast(basetype.GetMethodOrInheritedMethod("SetProperty"), IMethodSymbol)
'If setProp IsNot Nothing Then
'If setProp.IsGenericMethod And setProp.Parameters.Count > 0 Then
'If setProp.Parameters(0).RefKind = RefKind.Out Then
''Here we can be comparatively sure, we got the right method for raising the PropChange-Event.
'CodeFixAction = CodeFixAction.UseSetPropertyOfBaseClass
'End If
'End If
'End If
'End If
'End If
'Else
''we should never be here.
'Return
'End If
