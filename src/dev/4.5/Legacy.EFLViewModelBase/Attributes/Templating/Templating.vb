Imports System.ComponentModel
'Namespace Global.System.Runtime.CompilerServices

'    <AttributeUsage(AttributeTargets.Parameter)>
'    Public NotInheritable Class CallerMemberNameAttribute
'        Inherits Attribute

'    End Class

'End Namespace

Public Class TemplateClass(Of t)
    Implements INotifyPropertyChanged

    <TargetRequirement, ApplyTemplatePattern(ApplyInStrings:=True)>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    <ApplyTemplatePattern()>
    Private myPropertyTemplate As t

    <ApplyTemplatePattern()>
    Public Property PropertyTemplate As t
        Get
            Return myPropertyTemplate
        End Get
        Set(value As t)
            If Not Object.Equals(myPropertyTemplate, value) Then
                myPropertyTemplate = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PropertyTemplate"))
            End If
        End Set
    End Property

End Class


Public Class Test

    <CodeTemplate(GetType(TemplateClass(Of String)), "PropertyTemplate")>
    Public Property TestProperty As String

End Class

Public Class CodeTemplateAttribute
    Inherits Attribute

    Sub New(templateClass As Type, codeTemplateIdentifier As String)
        Me.TemplateClass = templateClass
        Me.CodeTemplateIdentifier = codeTemplateIdentifier
    End Sub

    Property TemplateClass As Type
    Property CodeTemplateIdentifier As String

End Class

Public Class TargetRequirementAttribute
    Inherits Attribute

    Sub New()
        MyBase.New
    End Sub

End Class

Public Class ApplyTemplatePatternAttribute
    Inherits Attribute

    Sub New()
        MyBase.New
    End Sub

    Public Property ApplyInStrings As Boolean

End Class


