Imports System.ComponentModel
Imports Microsoft.CodeAnalysis.Editing
Imports Microsoft.CodeAnalysis.Formatting

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
        Dim diagnostic = context.Diagnostics.First()
        Dim diagnosticSpan = diagnostic.Location.SourceSpan

        ' Find the type statement identified by the diagnostic.
        Dim declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType(Of FieldDeclarationSyntax)().First()

        ' Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create("Create property from unused field.",
                              Function(c) InsertPropertyAsync(context.Document, declaration, c)),
            diagnostic)
    End Function

    Private Async Function InsertPropertyAsync(document As Document,
                                               fieldDeclaration As FieldDeclarationSyntax,
                                               cancellationToken As CancellationToken) As Task(Of Document)

        Dim enclosingType = DirectCast(
                    (Await document.GetSemanticModelAsync).
                            GetEnclosingSymbol(fieldDeclaration.GetLocation.SourceSpan.Start), INamedTypeSymbol)

        Dim codefixaction = GetCodeFixActionFromClassType(enclosingType)

        Select Case codefixaction
            Case CodeFixAction.SimpleProperty
                Return Await InsertSimpleProperty(document, fieldDeclaration, cancellationToken)
            Case CodeFixAction.ManualRaiseNotifyPropertyChanged
                Return Await InsertManuallyRaisedPropChangedProperty(document, fieldDeclaration, cancellationToken)
            Case CodeFixAction.UseSetPropertyOfBaseClass
                Return Await InsertPropertyWithSetPropertyOfBaseClass(document, fieldDeclaration, cancellationToken)
        End Select

        'we should not ever be here.
        Return Nothing
    End Function

    Private Async Function InsertSimpleProperty(document As Document,
                                                fieldDeclaration As FieldDeclarationSyntax,
                                                cancellationToken As CancellationToken) As Task(Of Document)

        Dim fieldname = fieldDeclaration.Declarators(0).Names(0).ToString
        Dim propertyName As String = GetPropertyNameFromFieldname(fieldname)
        Dim docRoot = Await document.GetSyntaxRootAsync
        Dim generator As SyntaxGenerator = SyntaxGenerator.GetGenerator(document.Project.Solution.Workspace, document.Project.Language)

        Dim commentTrivia = SyntaxFactory.CommentTrivia(vbNewLine & "'--- " & propertyName & "---" & vbNewLine)
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

        propGeneration = propGeneration.WithLeadingTrivia(commentTrivia)
        propGeneration = propGeneration.WithAdditionalAnnotations(Formatter.Annotation)

        'Find last Property Block.
        Dim lastNodeInClass As SyntaxNode = FindLastPropertyNode(fieldDeclaration)

        Dim newroot = docRoot.InsertNodesAfter(lastNodeInClass, {propGeneration})

        Return document.WithSyntaxRoot(newroot)

    End Function

    Private Async Function InsertManuallyRaisedPropChangedProperty(
                          document As Document,
                          fieldDeclaration As FieldDeclarationSyntax,
                          cancellationToken As CancellationToken) As Task(Of Document)

        Dim fieldname = fieldDeclaration.Declarators(0).Names(0).ToString
        Dim propertyName As String = GetPropertyNameFromFieldname(fieldname)
        Dim docRoot = Await document.GetSyntaxRootAsync
        Dim generator As SyntaxGenerator = SyntaxGenerator.GetGenerator(document.Project.Solution.Workspace, document.Project.Language)

        Try
            Dim commentTrivia = SyntaxFactory.CommentTrivia(vbNewLine & "'--- " & propertyName & "---" & vbNewLine)

            Dim eventArgsDeclaration = generator.LocalDeclarationStatement("e",
                   generator.ObjectCreationExpression(
                        generator.IdentifierName("PropertyChangedEventArgs"),
                        generator.Argument(SyntaxFactory.NameOfExpression(SyntaxFactory.IdentifierName(propertyName)))))

            Dim OnxxxMethodStatements = New List(Of SyntaxNode) From
                      {eventArgsDeclaration,
                       generator.RaiseEventStatement("PropertyChanged", "e")}

            Dim OnxxxMethod = generator.MethodDeclaration("On" & propertyName & "Changed",
                                                         accessibility:=Accessibility.Protected,
                                                         modifiers:=DeclarationModifiers.Virtual,
                                                         statements:=OnxxxMethodStatements)
            OnxxxMethod = OnxxxMethod.WithLeadingTrivia(commentTrivia)

            Dim ifTrueStatements = New List(Of SyntaxNode) From
                {generator.AssignmentStatement(
                    generator.IdentifierName(fieldname),
                    generator.IdentifierName("value")),
                 generator.InvocationExpression(
                    generator.MemberAccessExpression(
                    SyntaxFactory.MeExpression(),
                    "On" & propertyName & "Changed"))}

            Dim ifFalseStatements = New List(Of SyntaxNode)

            Dim ifBlock = generator.IfStatement(
            generator.InvocationExpression(
                    generator.MemberAccessExpression(
                            generator.IdentifierName("Object"),
                            "equals"),
                    generator.Argument(
                        generator.IdentifierName(fieldname)),
                    generator.Argument(
                        generator.IdentifierName("value"))),
            ifTrueStatements, ifFalseStatements)

            Dim getterStatement = generator.ReturnStatement(generator.IdentifierName(fieldname))

            Dim propBlock As SyntaxNode =
                    generator.PropertyDeclaration(propertyName,
                                                  fieldDeclaration.Declarators(0).AsClause.Type,
                                                  Accessibility.Public,
                                                  DeclarationModifiers.None,
                                                  {getterStatement},
                                                  {ifBlock})

            OnxxxMethod = OnxxxMethod.WithAdditionalAnnotations(Formatter.Annotation)
            propBlock = propBlock.WithAdditionalAnnotations(Formatter.Annotation)

            'Find last Property Block.
            Dim lastNodeInClass As SyntaxNode = FindLastPropertyNode(fieldDeclaration)

            Dim newroot = docRoot.InsertNodesAfter(lastNodeInClass, {OnxxxMethod, propBlock})

            Return document.WithSyntaxRoot(newroot)
        Catch ex As Exception
            Stop
        End Try

        Return Nothing

    End Function

    Private Async Function InsertPropertyWithSetPropertyOfBaseClass(
                                                document As Document,
                                                fieldDeclaration As FieldDeclarationSyntax,
                                                cancellationToken As CancellationToken) As Task(Of Document)

        Dim fieldname = fieldDeclaration.Declarators(0).Names(0).ToString
        Dim propertyName = GetPropertyNameFromFieldname(fieldname)

        Dim docRoot = Await document.GetSyntaxRootAsync

        Dim generator As SyntaxGenerator = SyntaxGenerator.GetGenerator(document.Project.Solution.Workspace,
                                                                        document.Project.Language)

        Dim setterStatement As SyntaxNode = Nothing
        Dim commentTrivia = SyntaxFactory.CommentTrivia(vbNewLine & "'--- " & propertyName & "---" & vbNewLine)

        setterStatement = generator.InvocationExpression(
            generator.IdentifierName("SetProperty"),
            generator.Argument(RefKind.Out, generator.IdentifierName(fieldname)),
            generator.Argument(RefKind.None, generator.IdentifierName("value")))

        Dim getterStatement = generator.ReturnStatement(generator.IdentifierName(fieldname))

        Dim propGeneration As SyntaxNode =
                    generator.PropertyDeclaration(propertyName,
                                                  fieldDeclaration.Declarators(0).AsClause.Type,
                                                  Accessibility.Public,
                                                  DeclarationModifiers.None,
                                                  {getterStatement},
                                                  {setterStatement})

        propGeneration = propGeneration.WithLeadingTrivia(commentTrivia)
        propGeneration = propGeneration.WithAdditionalAnnotations(Formatter.Annotation)

        'Find last Property Block.
        Dim lastNodeInClass As SyntaxNode = FindLastPropertyNode(fieldDeclaration)

        Dim newroot = docRoot.InsertNodesAfter(lastNodeInClass, {propGeneration})

        Return document.WithSyntaxRoot(newroot)

    End Function

    Private Function GetPropertyNameFromFieldname(fieldname As String) As String
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

        Return propertyName
    End Function

    Private Function FindLastPropertyNode(startNode As SyntaxNode) As SyntaxNode

        Dim parentNode = startNode.Parent

        Dim lastPropBlock As SyntaxNode = Nothing
        Dim lastNode As SyntaxNode = Nothing
        Dim previousNode As SyntaxNode = parentNode

        For Each nodeItem In parentNode.ChildNodes
            Dim tempNode = TryCast(nodeItem, PropertyBlockSyntax)
            If tempNode IsNot Nothing Then
                lastPropBlock = tempNode
            End If
            lastNode = previousNode
            previousNode = nodeItem
        Next

        If lastPropBlock IsNot Nothing Then
            Return lastPropBlock
        Else
            Return lastNode
        End If

    End Function

    Private Function GetCodeFixActionFromClassType(contType As INamedTypeSymbol) As CodeFixAction

        Dim codeFixActionToReturn As CodeFixAction = CodeFixAction.SimpleProperty

        If contType.Interfaces.Where(
                            Function(item) item.Name = "INotifyPropertyChanged").FirstOrDefault IsNot Nothing Then
            codeFixActionToReturn = CodeFixAction.ManualRaiseNotifyPropertyChanged
        End If

        Dim basetype = contType.BaseType

        If basetype IsNot Nothing AndAlso basetype.Name <> "Object" Then
            Debug.WriteLine($"Basetype found: {basetype.ToString}")
            If basetype.HasTypeInInheritanceHierarchy("BindableBase") Then
                Dim setProp = TryCast(basetype.GetMethodOrInheritedMethod("SetProperty"), IMethodSymbol)
                If setProp IsNot Nothing Then
                    If setProp.IsGenericMethod And setProp.Parameters.Count > 0 Then
                        If setProp.Parameters(0).RefKind = RefKind.Ref Then
                            'Here we can be comparatively sure, we got the right method for raising the PropChange-Event.
                            codeFixActionToReturn = CodeFixAction.UseSetPropertyOfBaseClass
                        End If
                    End If
                End If
            End If
        End If

        Return codeFixActionToReturn

    End Function
End Class

Public Class test
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private myField As Integer

    'Dies ist ein Test
    Protected Overridable Sub OnTestChanged()
        Dim e = New PropertyChangedEventArgs(NameOf(Test))
        RaiseEvent PropertyChanged(Me, e)
    End Sub

    Public Property Test As Integer
        Get
            Return myField
        End Get
        Set(value As Integer)
            If Not Object.Equals(myField, value) Then
                myField = value
                OnTestChanged()
            End If
        End Set
    End Property
End Class
