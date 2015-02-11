Imports System.ComponentModel
Imports System.Reflection
Imports System.Windows.Forms

Public Class BoundSourceItemProxyBase
    Implements IDisposable, INotifyAttachOrDetach(Of BoundSourceItemProxyBase)

    Public Event SourceObjectPropertyChanged(sender As Object, e As SourceObjectChangedEventArgs)
    Public Event SourceObjectDataErrorsChanged(sender As Object, e As DataErrorsChangedEventArgs)

    Private mySourceObject As INotifyPropertyChanged

    Public Property TargetObject As Object
    Public Property BindingItem As PropertyBindingItem

    Property SourceObject As INotifyPropertyChanged
        Get
            Return mySourceObject
        End Get
        Set(value As INotifyPropertyChanged)
            If Not Object.Equals(value, mySourceObject) Then
                'Wenn es das Source-Objekt bereits gab, dann das vorhandene PropertyChanged-Ereignis entfernen.
                If mySourceObject IsNot Nothing Then
                    Windows.WeakEventManager(Of INotifyPropertyChanged, PropertyChangedEventArgs).RemoveHandler(
                        mySourceObject, "PropertyChanged", AddressOf PropertyChangedEventHandler)
                End If

                Dim tmpSourceAsINotifyDataErrorInfo = TryCast(mySourceObject, INotifyDataErrorInfo)
                If tmpSourceAsINotifyDataErrorInfo IsNot Nothing Then
                    Windows.WeakEventManager(Of INotifyDataErrorInfo, DataErrorsChangedEventArgs).RemoveHandler(
                        tmpSourceAsINotifyDataErrorInfo, "ErrorsChanged", AddressOf DataErrorsChangedEventHandler)
                End If

                mySourceObject = value

                'Nur wenn nicht Nothing neu zugewiesen wurde, das PropertyChanged des neuen Objektes wieder binden.
                If mySourceObject IsNot Nothing Then
                    Windows.WeakEventManager(Of INotifyPropertyChanged, PropertyChangedEventArgs).AddHandler(
                        mySourceObject, "PropertyChanged", AddressOf PropertyChangedEventHandler)
                End If

                tmpSourceAsINotifyDataErrorInfo = TryCast(mySourceObject, INotifyDataErrorInfo)
                If tmpSourceAsINotifyDataErrorInfo IsNot Nothing Then
                    Windows.WeakEventManager(Of INotifyDataErrorInfo, DataErrorsChangedEventArgs).AddHandler(
                        tmpSourceAsINotifyDataErrorInfo, "ErrorsChanged", AddressOf DataErrorsChangedEventHandler)
                End If
            End If
        End Set
    End Property

    Protected Overridable Sub PropertyChangedEventHandler(sender As Object, e As PropertyChangedEventArgs)
        If BindingItem.ViewModelProperty.PropertyName.EndsWith(e.PropertyName) Then
            Dim eArgs As New SourceObjectChangedEventArgs(DirectCast(sender, INotifyPropertyChanged))
            OnSourceObjectPropertyChanged(eArgs)
        End If
    End Sub

    Private Sub DataErrorsChangedEventHandler(sender As Object, e As DataErrorsChangedEventArgs)
        If BindingItem.ViewModelProperty.PropertyName.EndsWith(e.PropertyName) Then
            OnDataErrorsChanged(sender, e)
        End If
    End Sub

    Protected Overridable Sub OnDataErrorsChanged(sender As Object, e As DataErrorsChangedEventArgs)
        RaiseEvent SourceObjectDataErrorsChanged(sender, e)
    End Sub

    Protected Overridable Sub OnSourceObjectPropertyChanged(e As SourceObjectChangedEventArgs)
        RaiseEvent SourceObjectPropertyChanged(Me, e)
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If mySourceObject IsNot Nothing Then
                    Windows.WeakEventManager(Of INotifyPropertyChanged, PropertyChangedEventArgs).RemoveHandler(
                        mySourceObject, "PropertyChanged", AddressOf PropertyChangedEventHandler)
                End If

                Dim tmpSourceAsINotifyDataErrorInfo = TryCast(mySourceObject, INotifyDataErrorInfo)
                If tmpSourceAsINotifyDataErrorInfo IsNot Nothing Then
                    Windows.WeakEventManager(Of INotifyDataErrorInfo, DataErrorsChangedEventArgs).RemoveHandler(
                        tmpSourceAsINotifyDataErrorInfo, "ErrorsChanged", AddressOf DataErrorsChangedEventHandler)
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Public Property CorrelatingNode As LinkedListNode(Of BoundSourceItemProxyBase) Implements INotifyAttachOrDetach(Of BoundSourceItemProxyBase).CorrelatingNode

    Public ReadOnly Property HasPredecessor As Boolean Implements INotifyAttachOrDetach(Of BoundSourceItemProxyBase).HasPredecessor
        Get
            Return (Me.CorrelatingNode.Previous IsNot Nothing)
        End Get
    End Property

    Protected Overridable Sub NotifyAttached() Implements INotifyAttachOrDetach(Of BoundSourceItemProxyBase).NotifyAttached
    End Sub

    Protected Overridable Sub NotifyDetached() Implements INotifyAttachOrDetach(Of BoundSourceItemProxyBase).NotifyDetached
        If mySourceObject IsNot Nothing Then
            Windows.WeakEventManager(Of INotifyPropertyChanged, PropertyChangedEventArgs).RemoveHandler(
                mySourceObject, "PropertyChanged", AddressOf PropertyChangedEventHandler)

            Dim tmpSourceAsINotifyDataErrorInfo = TryCast(mySourceObject, INotifyDataErrorInfo)
            If tmpSourceAsINotifyDataErrorInfo IsNot Nothing Then
                Windows.WeakEventManager(Of INotifyDataErrorInfo, DataErrorsChangedEventArgs).RemoveHandler(
                    tmpSourceAsINotifyDataErrorInfo, "ErrorsChanged", AddressOf DataErrorsChangedEventHandler)
            End If
        End If
    End Sub

    Public Property ParentLinkedList As ChangeAwareLinkedList(Of BoundSourceItemProxyBase) Implements INotifyAttachOrDetach(Of BoundSourceItemProxyBase).ParentLinkedList
End Class

Public Class PropertyPathBoundSourceItemProxy
    Inherits BoundSourceItemProxyBase

    Private myObjectToObserve As INotifyPropertyChanged
    Private myRemainingPath As String
    Private myControl As Control
    Private myTempParentList As PropertyBindingManager

    Public Sub New(viewModelSource As INotifyPropertyChanged, bindingItem As PropertyBindingItem, targetControl As Control, parentBindingManager As BindingManager)
        If parentBindingManager Is Nothing Then
            Throw New NullReferenceException("Der übergebendene BindingManager darf nicht null (nothing in VB) sein.")
        End If

        Me.SourceObject = viewModelSource
        Me.BindingItem = bindingItem
        Me.TargetControl = targetControl
        Me.ParentBindingManager = parentBindingManager
    End Sub

    Protected Overrides Sub NotifyAttached()
        MyBase.NotifyAttached()
        If myObjectToObserve IsNot Nothing Then
            Me.SourceObject = myObjectToObserve
        End If
    End Sub

    Protected Overrides Sub NotifyDetached()
        MyBase.NotifyDetached()
        Me.SourceObject = Nothing
    End Sub

    Protected Overrides Sub PropertyChangedEventHandler(sender As Object, e As PropertyChangedEventArgs)
        If e.PropertyName = PropertyPathName Then

            Dim eArgs As New SourceObjectChangedEventArgs(DirectCast(sender, INotifyPropertyChanged))
            OnSourceObjectPropertyChanged(eArgs)
        End If
    End Sub

    Protected Overrides Sub OnSourceObjectPropertyChanged(e As SourceObjectChangedEventArgs)
        MyBase.OnSourceObjectPropertyChanged(e)

        'Wenn wir hier landen, muss der untenliegende Pfad neu gebunden werden.
        Dim newViewModelPathObject = DirectCast(e.OriginalObject, INotifyPropertyChanged)

        'Von hier aus alle Elemente der Kette, die den Pfad bis zum Ende bilden lösen
        Do
            If Me.CorrelatingNode.Next IsNot Nothing Then
                Me.ParentLinkedList.Remove(Me.CorrelatingNode.Next.Value)
            Else
                Exit Do
            End If
        Loop

        'Die Elternliste müssen wir temporär zwischenspeichern
        'denn da *dieses* Element ebenfalls aus der Liste raus
        'muss, würden wir sonst die ParentList verlieren,
        'da sie nicht mehr referenzierbar ist.
        myTempParentList = DirectCast(Me.ParentLinkedList, PropertyBindingManager)

        'Diesen Knoten ebenfalls und auf jeden Fall löschen.
        'Jetzt würde aber auch die Parentlist verloren gehen, die 
        'wir deswegen zwischengespeichert haben.
        Me.ParentLinkedList.Remove(Me.CorrelatingNode.Value)

        'Alle Bindingen des Pfades ab "hier" wieder neu setzen.
        WireUpPropertyChangeEvents(newViewModelPathObject, RemainingPath, TargetControl, myTempParentList)

        'Alle Bindungen finden, die diesen ViewModel-Part betreffen und die Werte neu setzen.
        'Dazu benötigen wir zunächst alle Bindungselemente ...
        Dim parentBM = Me.ParentBindingManager
        '...und filtern nun die heraus, die mit dem Bindungspfad enden.
        Dim bindingInfoList = (From bindingInfoItem In parentBM.BindingItems.SelectMany(
                              Function(item) From item2 In item.MvvmItem.PropertyBindings
                                             Select item.Control, item2)
                               Where bindingInfoItem.item2.ViewModelProperty.PropertyName.EndsWith(Me.RemainingPath) And
                                    bindingInfoItem.Control Is TargetControl).
                                    Select(Function(item) item.item2).ToList

        If newViewModelPathObject IsNot Nothing AndAlso
            TargetControl IsNot Nothing AndAlso
                bindingInfoList IsNot Nothing AndAlso
                    bindingInfoList.Count > 0 Then
            parentBM.UpdateControlProperties(TargetControl, bindingInfoList)
        End If
    End Sub

    ''' <summary>
    ''' ?Der verbleibende Teil des Property-Pfades, ab diesem Property-Knotenstück.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RemainingPath As String
        Get
            Return myRemainingPath
        End Get
        Set(value As String)
            myRemainingPath = value
        End Set
    End Property

    Public Property ParentBindingManager As BindingManager
    Public Property TargetControl As Control

    ''' <summary>
    ''' ?Der exakte Teil des Propertypfades, der von diesem Property-Knotenstück verarbeitet werden soll.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PropertyPathName As String

    Private Sub WireUpPropertyChangeEvents(value As Object, PropertyPath As String, targetControl As Control, parentList As PropertyBindingManager)

        'TODO: Sicherstellen, dass beim Initialen binden ebenfalls die Werte neu geschrieben werden, nicht nur beim ersten PropertyChange.
        Dim partObjects = PropertyPath.Split("."c)
        Dim startPropertyPath As String = partObjects(0)
        Dim currentPropertyPath = ""

        For count = 0 To partObjects.GetLength(0) - 1

            'Hier wird der Pfad zur eigentlichen Eigenschaft nach und nach hinzugefügt.
            Dim charsToAdd As String = ""
            If count > 0 Then
                charsToAdd = "."
            End If

            charsToAdd &= partObjects(count)
            currentPropertyPath &= charsToAdd

            Dim temp As Tuple(Of Object, PropertyInfo) = Nothing

            Try
                temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(value, currentPropertyPath)
            Catch ex As NullReferenceException
                'TODO: Bereits vorhandene Bindungen lösen.
                Continue For
            End Try

            If temp Is Nothing OrElse temp.Item1 Is Nothing Then
                Continue For
            End If

            Dim viewModelTargetObject = temp.Item1  ' Objekt, dessen NotifyPropertyChange-Ereignis wir binden wollen
            Dim viewModelPropertyInfo = temp.Item2  ' Die PropertyInfo, aber die benötigen wird nicht.

            Dim remainingPath = ""
            For innerCount = count To partObjects.GetLength(0) - 1
                remainingPath &= partObjects(innerCount)
                If Not innerCount = partObjects.GetLength(0) - 1 Then
                    remainingPath &= "."
                End If
            Next

            If Not count = partObjects.GetLength(0) - 1 Then
                'An dieser Stelle müssen wir versuchen, die PropertyPathChanged-Ereignisse zu binden, also die Changed
                'Ereignisse, die auslösen können, wenn sich nicht die eigentliche Eigenschaft sondern eine Eigenschaft
                'auf dem Weg (Pfad) zur Eigenschaft geändert hat.
                Dim traceInfo = "Start binding PropertyPathChanged Event for Property " & currentPropertyPath &
                                        " for ViewModel " & viewModelTargetObject.GetType.Name

                parentList.Add(New PropertyPathBoundSourceItemProxy(DirectCast(value, INotifyPropertyChanged), BindingItem, targetControl, Me.ParentBindingManager) With
                                                                    {.RemainingPath = remainingPath,
                                                                     .PropertyPathName = partObjects(count)})
            Else
                'An dieser Stelle müssen wir versuchen, die PropertyChanged-Ereignisse zu binden:
                Dim traceInfo = "Start binding PropertyChanged Event for Property " & currentPropertyPath &
                                        " as " & BindingItem.ControlProperty.PropertyType.Name & " for ViewModel " &
                                        value.GetType.Name

                parentList.Add(New PropertyBoundSourceItemProxy(DirectCast(viewModelTargetObject, INotifyPropertyChanged), targetControl, BindingItem))
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        If BindingItem IsNot Nothing Then
            Return BindingItem.ToString & " : RemainingPath: " & RemainingPath
        Else
            Return "* --- *: RemainingPath"
        End If
    End Function
End Class

Public Class PropertyBoundSourceItemProxy
    Inherits BoundSourceItemProxyBase

    Public Sub New(viewModelSource As INotifyPropertyChanged,
            controlTarget As Object, bindingItem As PropertyBindingItem)
        Me.SourceObject = viewModelSource
        Me.TargetObject = controlTarget
        Me.BindingItem = bindingItem
    End Sub

    Protected Overrides Sub PropertyChangedEventHandler(sender As Object, e As PropertyChangedEventArgs)
        MyBase.PropertyChangedEventHandler(sender, e)
    End Sub

    Protected Overrides Sub OnSourceObjectPropertyChanged(e As SourceObjectChangedEventArgs)
        MyBase.OnSourceObjectPropertyChanged(e)

        'Ziel schreiben
        If BindingItem.UpdatingViewmodelInProgress Or e.Cancel Then Return

        'Wichtig: Wir aktualisieren jetzt hier nur ein Objekt inmitten des Pfades, nicht am Ende.
        '(Siehe auch Beschreibung zum letzten Optionalen Parameter.)
        BindingManager.UpdateSourcePropertyInternal(TargetObject, SourceObject, BindingItem,
                                                    True, sender:=BindingItem.PropertyBindingManager.ParentBindingManager)
    End Sub

    Protected Overrides Sub OnDataErrorsChanged(sender As Object, e As DataErrorsChangedEventArgs)
        MyBase.OnDataErrorsChanged(sender, e)

        BindingManager.HandleFullPathDataErrorsChangedInternal(TargetObject, SourceObject, BindingItem,
                                                               True, sender:=BindingItem.PropertyBindingManager.ParentBindingManager)
    End Sub

    Public Overrides Function ToString() As String
        If BindingItem IsNot Nothing Then
            Return BindingItem.ToString
        End If
        Return "* --- *"
    End Function

End Class

''' <summary>
''' Managed die Binding einer 1:1-Beziehung Control.Property - ViewModel.Path.To.Property
''' </summary>
''' <remarks></remarks>
Public Class PropertyBindingManager
    Inherits ChangeAwareLinkedList(Of BoundSourceItemProxyBase)
    Implements IDisposable

    Private myViewModelObject As INotifyPropertyChanged
    Private myControl As Control
    Private myBindingItem As PropertyBindingItem

    ''' <summary>
    ''' Erstellt eine neue Instanz dieser Klasse.
    ''' </summary>
    ''' <param name="ViewModelObject">Instanz des zu bindenden ViewModels.</param>
    ''' <param name="TargetControl">Instanz des zu bindenden Controls.</param>
    ''' <param name="bindingItem">Bindungsinformationen, die bestimmt, wie die Eigenschaften gebunden sein sollen.</param>
    ''' <remarks></remarks>
    Sub New(ViewModelObject As INotifyPropertyChanged, TargetControl As Control, bindingItem As PropertyBindingItem, parentBindingManager As BindingManager)
        MyBase.New()
        If parentBindingManager Is Nothing Then
            Throw New NullReferenceException("Der übergebendene BindingManager darf nicht null (nothing in VB) sein.")
        End If

        'Wichtig: Diese Eigenschaft muss als erstes gesetzt werden,
        'da der Setter von BindingItem sie bereits benötigt. 
        'Reihenfolge in der Zuweisung deswegen nicht ändern!!!
        Me.ParentBindingManager = parentBindingManager

        Me.ViewModelObject = ViewModelObject
        myControl = TargetControl
        Me.BindingItem = bindingItem
    End Sub

    Public Property ParentBindingManager As BindingManager

    ''' <summary>
    ''' Bestimmt die Bindungseigenschaften von ViewModel und Control. Das Setzen dieser Eigenschaft richtet 
    ''' die Infrastruktur des Bindungsmechanismusses ein (auflösen der vorherigen, setzen der neuen PropertyChange-Events).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BindingItem As PropertyBindingItem
        Get
            Return myBindingItem
        End Get

        Set(value As PropertyBindingItem)
            If Not Object.Equals(value, myBindingItem) Then
                If value IsNot Nothing Then
                    myBindingItem = value
                    If Me.ViewModelObject IsNot Nothing Then
                        WireUpPropertyChangeEvents(myViewModelObject, myBindingItem.ViewModelProperty.PropertyName,
                                                   myBindingItem)
                    End If
                Else
                    'TODO: Tritt dieser Fall auf?
                    myBindingItem = Nothing
                End If
            End If
        End Set
    End Property

    Public Property ViewModelObject As INotifyPropertyChanged
        Get
            Return myViewModelObject
        End Get
        Set(value As INotifyPropertyChanged)
            If Not Object.Equals(value, myViewModelObject) Then
                If value IsNot Nothing Then
                    If myViewModelObject IsNot Nothing Then
                        'TODO: Tritt dieser Fall auf?
                    End If
                    myViewModelObject = value
                    If Me.BindingItem IsNot Nothing Then
                        WireUpPropertyChangeEvents(myViewModelObject, myBindingItem.ViewModelProperty.PropertyName,
                                                   myBindingItem)
                    End If
                Else
                    'TODO: Tritt dieser Fall auf?
                    myViewModelObject = Nothing
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Richtet die PropertyChange-Ereignisse für die Bindung dieses Elementes ein. Diese Methode 
    ''' wird von der BindingItem-Eigenschaft verwendet.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="PropertyPath"></param>
    ''' <param name="bindingItem"></param>
    ''' <remarks></remarks>
    Friend Sub WireUpPropertyChangeEvents(value As Object, PropertyPath As String,
                                           Optional bindingItem As PropertyBindingItem = Nothing)

        Dim partObjects = PropertyPath.Split("."c)
        Dim startPropertyPath As String = partObjects(0)
        Dim currentPropertyPath = ""

        For count = 0 To partObjects.GetLength(0) - 1

            'Hier wird der Pfad zur eigentlichen Eigenschaft nach und nach hinzugefügt.
            Dim charsToAdd As String = ""
            If count > 0 Then
                charsToAdd = "."
            End If

            charsToAdd &= partObjects(count)
            currentPropertyPath &= charsToAdd

            Dim temp As Tuple(Of Object, PropertyInfo) = Nothing

            Try
                temp = ObjectAnalyser.PropertyInfoFromNestedPropertyName(myViewModelObject,
                                                                         currentPropertyPath)
            Catch ex As NullReferenceException
                'TODO: Tritt dieser Fall auf?
                Continue For
            End Try

            If temp Is Nothing Then
                Throw New MvvmBindingException("Property '" & currentPropertyPath & "' could not be found on the ViewModel of type '" & myViewModelObject.GetType.ToString & "'." & vbNewLine &
                                               "Please check, if the Propertyname has been renamed, and try to fix the binding with the PropertyBinding-Editor in the property windows.", Nothing)
            End If

            'Da temp ein Tupel(of object, PropertyInfo) ist
            'muss auf dem Weg zum Pfadende ein Objekt nothing gewesen sein.
            'In diesem Fall müssen wir aufhören.
            If temp.Item1 Is Nothing Then
                'TODO: Tritt dieser Fall auf?
                Continue For
            End If

            Dim viewModelTargetObject = temp.Item1  ' Objekt, dessen NotifyPropertyChange-Ereignis wir binden wollen
            Dim viewModelPropertyInfo = temp.Item2  ' Die PropertyInfo, aber die benötigen wird nicht.

            'Feststellen, ob das Objekt bereits gebunden ist. Dann, es nicht nochmal binden.

            If Not count = partObjects.GetLength(0) - 1 Then
                'An dieser Stelle müssen wir versuchen, die PropertyPathChanged-Ereignisse zu binden, also die Changed
                'Ereignisse, die auslösen können, wenn sich nicht die eigentliche Eigenschaft sondern eine Eigenschaft
                'auf dem Weg (Pfad) zur Eigenschaft geändert hat.
                Dim traceInfo = "Start binding PropertyPathChanged Event for Property " & currentPropertyPath &
                                        " for ViewModel " & viewModelTargetObject.GetType.Name

                Dim remainingPath = ""
                For innerCount = count To partObjects.GetLength(0) - 1
                    remainingPath &= partObjects(innerCount)
                    If Not innerCount = partObjects.GetLength(0) - 1 Then
                        remainingPath &= "."
                    End If
                Next
                Me.Add(New PropertyPathBoundSourceItemProxy(DirectCast(viewModelTargetObject, INotifyPropertyChanged),
                                                            bindingItem, myControl, Me.ParentBindingManager) With {.RemainingPath = remainingPath,
                                                                    .PropertyPathName = partObjects(count)})
            Else
                'An dieser Stelle müssen wir versuchen, die PropertyChanged-Ereignisse zu binden:
                Dim traceInfo = "Start binding PropertyChanged Event for Property " & currentPropertyPath &
                                        " as " & bindingItem.ControlProperty.PropertyType.Name & " for ViewModel " &
                                        value.GetType.Name

                Me.Add(New PropertyBoundSourceItemProxy(DirectCast(viewModelTargetObject, INotifyPropertyChanged),
                                                        myControl, bindingItem))
            End If
        Next
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Me.RemoveAll()
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

