Imports System.Reflection

Public Class MvvmFormsPage
    Inherits Page

    Sub New()
        MyBase.New
    End Sub

    Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
        MyBase.OnNavigatedTo(e)
        'TextBox:
        Dim textBoxCol = New List(Of TextBox)
        FindChilden(Of TextBox)(textBoxCol, Me)

        For Each textBoxItem In textBoxCol
            AddHandler textBoxItem.TextChanging, Sub(o, eArgs)
                                                     If IsDirty Then Exit Sub

                                                     IsDirty = True
                                                 End Sub
        Next

        Dim listBoxCol = New List(Of ListViewBase)
        FindChilden(Of ListViewBase)(listBoxCol, Me)

        For Each listBoxItem In listBoxCol
            AddHandler listBoxItem.SelectionChanged, Sub(o, eArgs)
                                                         If IsDirty Then Exit Sub

                                                         IsDirty = True
                                                     End Sub
        Next

    End Sub

    Protected Overrides Sub OnApplyTemplate()
        MyBase.OnApplyTemplate()

    End Sub

    Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
        MyBase.OnNavigatingFrom(e)
        e.Cancel = NavigationBlocked
    End Sub

    Public Shared ReadOnly NavigationBlockedProperty As DependencyProperty =
                    DependencyProperty.Register("NavigationBlocked",
                      GetType(Boolean),
                      GetType(MvvmFormsPage),
                      New PropertyMetadata(Nothing,
                                           Sub(sender, eArgs)
                                           End Sub))

    Public Property NavigationBlocked As Boolean
        Get
            Return CBool(GetValue(NavigationBlockedProperty))
        End Get
        Set(value As Boolean)
            SetValue(NavigationBlockedProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IsDirtyProperty As DependencyProperty =
                    DependencyProperty.Register("IsDirty",
                      GetType(Boolean),
                      GetType(MvvmFormsPage),
                      New PropertyMetadata(Nothing,
                                           Sub(sender, eArgs)
                                           End Sub))

    Public Property IsDirty As Boolean
        Get
            Return CBool(GetValue(IsDirtyProperty))
        End Get
        Set(value As Boolean)
            SetValue(IsDirtyProperty, value)
        End Set
    End Property


    Private Shared Sub FindChilden(Of uiType As DependencyObject)(results As List(Of uiType), startnode As DependencyObject)

        Dim count = VisualTreeHelper.GetChildrenCount(startnode)
        For i = 0 To count - 1
            Dim current = VisualTreeHelper.GetChild(startnode, i)
            If (current.GetType.Equals(GetType(uiType))) OrElse (current.GetType().GetTypeInfo().IsSubclassOf(GetType(uiType))) Then
                Dim asType = DirectCast(current, uiType)
                results.Add(asType)
            End If
            FindChilden(Of uiType)(results, current)
        Next
    End Sub

End Class
