Imports System.Windows.Forms
Imports System.Drawing
Imports System.ComponentModel

''' <summary>
''' Steuerelement, mit dessen Hilfe unterschiedliche Views in Abhängigkeit des an die DataContext-Property 
''' zugewiesene ViewModel und dessen MvvmViewAttribute angezeigt wird.
''' </summary>
''' <remarks>Es gibt Anforderungen, bei denen durch Auswahl anderer Typen eines ViewModels entsprechend 
''' andere Views zur Anzeige in denselben Container kommen müssen. Diese Anforderung wird durch dieses 
''' Steuerelement abgedeckt. Es verfügt über eine DataContext-Eigenschaft, der verschiedene ViewModel-Typen 
''' zugewiesen werden. Diese Unterschiedlichen ViewModels tragen über ihrer Klassesdefinition ein MvvmViewAttribute, 
''' das die Relation zu einer View (Einem Formular oder einem UserControl herstellt). Jenachdem, welches ViewModel 
''' diesem Steuerelement dann zur Laufzeit zugewiesen wird, sucht es dann die korrelierende View und blendet sie 
''' in ihrem Child-Bereich an.</remarks>
Public Class MvvmViewSelectorContainer
    Inherits ContainerControl

    Private myDataContext As Object
    Private myAnchorView As Boolean = True
    Private myInitialViewSize As Size = New Size(0, 0)

    Public Event DataContextChanged(sender As Object, e As EventArgs)

    Public Property DataContext As Object
        Get
            Return myDataContext
        End Get
        Set(value As Object)
            If Not Object.Equals(value, myDataContext) Then
                myDataContext = value
                HandleDataContextChanged()
                OnDataContextChanged(EventArgs.Empty)
            End If
        End Set
    End Property

    Protected Overridable Sub OnDataContextChanged(e As EventArgs)
        RaiseEvent DataContextChanged(Me, e)
    End Sub

    Private Sub HandleDataContextChanged()
        'Untersuchen, ob der neue DataContext Nothing ist.

        'ALLE Controls entfernen. Es sollte sich nur ein Control hier befinden,
        'aber falls der Entwickler sich mit der Reflection hier eingeklinkt hat,
        'wird aufjeden Fall ALLES entsorgt.
        If Me.Controls.Count > 0 Then
            Do
                Dim control = Me.Controls(Controls.Count - 1)
                Me.Controls.Remove(control)
                control.Dispose()
            Loop Until Controls.Count = 0
        End If

        If DataContext Is Nothing Then
            Return
        End If

        'Untersuchen, ob es ein entsprechendes Attribut über der neuen Klasse gibt.
        Dim vAtt = DirectCast((From atItem In DataContext.GetType.GetCustomAttributes(True)
                               Where GetType(MvvmViewAttribute).IsAssignableFrom(atItem.GetType)).FirstOrDefault, MvvmViewAttribute)

        If vAtt Is Nothing Then
            Throw New MissingViewAttributeException("Dem zugewiesenen ViewModel ist kein MvvmViewAttribute zugeordnet, deswegen kann keine View zur Anzeige gefunden werden." & vbNewLine &
                                                    "Implementieren Sie eine View in Form eines UserControls oder einer Form, und bestimmen Sie sie mithilfe des MvvmViewAttributes," & vbNewLine &
                                                    "das Sie über der ViewModel-Klasse zuordnen.")
        End If

        'Jetzt die entsprechende View per Reflection finden.
        Dim actualViewType = Type.GetType(vAtt.ViewTypeName)
        If actualViewType Is Nothing Then
            Throw New MissingViewAttributeException("Die View mit dem Namen '" & vAtt.ViewTypeName & "' konnte nicht" & vbNewLine &
                                                    "gefunden werden. Stellen Sie sicher, dass Sie den Namen des View-Typs ausreichend qualifiziert" & vbNewLine &
                                                    "angeben, also etwa im Format 'MainNameSpace.SubNameSpace.Typename, Assemblyname'.")
        End If

        'View muss von Control abgeleitet sein:
        If Not GetType(Control).IsAssignableFrom(actualViewType) Then
            Throw New MissingViewAttributeException("Die View mit dem Namen '" & vAtt.ViewTypeName & "' wurde zwar gefunden, " & vbNewLine &
                                                    "ist aber nicht von 'Control' oder 'Form' abgeleitet, und kann deswegen" & vbNewLine &
                                                    "nicht verwendet werden.")
        End If

        'Hier können wir jetzt die View versuchen, zu erstellen:
        Dim actualView As Control
        Try
            actualView = DirectCast(Activator.CreateInstance(actualViewType), Control)
        Catch ex As Exception
            Throw New MissingViewAttributeException("Die View mit dem Namen '" & vAtt.ViewTypeName & "' wurde zwar gefunden," & vbNewLine &
                                                    "konnte aber nicht erstellt werden. Mehr Informationen finden Sie in der InnerException.", ex)
        End Try

        'Falls die View von Form abgeleitet ist, dann müssen wir sie zum Control "umbauen":
        If GetType(System.Windows.Forms.Form).IsAssignableFrom(actualViewType) Then
            DirectCast(actualView, Form).TopLevel = False
            actualView.Visible = True
        End If

        'Grundeigenschaften setzen, und View hinzufügen.
        actualView.Location = New Point(0, 0)

        'InitialContentSize berücksichtigen
        If Me.InitialViewSize <> New Size(0, 0) Then
            actualView.Size = InitialViewSize
        End If

        'Anchor-Eigenschaft im Bedarfsfall berücksichtigen:
        If Me.AnchorView Then
            actualView.Size = Me.ClientSize
            actualView.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        End If
        Me.Controls.Add(actualView)

        'Finden des (hoffentlich) einzigen(/ersten) MVVM-Managers
        Dim actualViewWithMvvmManager = TryCast(actualView, IWinFormsMvvmView)
        If actualViewWithMvvmManager IsNot Nothing Then
            actualViewWithMvvmManager.GetMvvmController.DataContext = myDataContext
        End If
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die View auf die inneren Ausmaße des Containers 
    ''' angepasst und an allen vier Seiten verankert werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, ob der Inhalt auf die inneren Ausmaße der View angepasst und an allen vier Seiten verankert werden soll."),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(True)>
    Public Property AnchorView As Boolean
        Get
            Return myAnchorView
        End Get
        Set(value As Boolean)
            If value <> myAnchorView Then
                myAnchorView = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Bestimmt oder ermittelt, auf welche Ausmaße die View beim Initialisieren gesetzt werden soll.
    ''' (Standard ist (0,0), die vordefinierte Größe der View wird dabei beibehalten.)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Description("Bestimmt oder ermittelt, auf welche Ausmaße die View beim Initialisieren gesetzt werden soll. (Standard ist (0,0), die vordefinierte Größe der View wird dabei beibehalten.)"),
     Category("Verhalten"),
     EditorBrowsable(EditorBrowsableState.Always),
     Browsable(True), DefaultValue(True)>
    Public Property InitialViewSize As Size
        Get
            Return myInitialViewSize
        End Get
        Set(value As Size)
            If value <> myInitialViewSize Then
                myInitialViewSize = value
            End If
        End Set
    End Property

End Class
