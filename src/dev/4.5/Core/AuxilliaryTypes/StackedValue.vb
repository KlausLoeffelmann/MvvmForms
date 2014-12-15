''' <summary>
''' Stellt einen Datentyp zur Verfügung, der sich durch seine Value-Eigenschaft beim Zuweisen und Auslesen wie ein Stapel verhält.
''' </summary>
''' <typeparam name="Type"></typeparam>
''' <remarks></remarks>
Public Structure StackedValue(Of Type)

    Private myStack As Stack(Of Type)

    ''' <summary>
    ''' Bestimmt oder ermittelt den aktuellen Wert. Bei zweimal hintereinander durchgeführtem Zuweisen unterschiedlicher Werte, werden dieselben unterschiedlichen 
    ''' Werte beim hintereinander durchgeführten Auslesen auch wieder zurückgegeben.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Value As Type
        Get
            If myStack Is Nothing Then
                Throw New NullReferenceException("This StackValue has not been assigned a value yet, or more values has been tried to be get than set.")
            Else
                Dim returnValue = myStack.Pop
                If myStack.Count = 0 Then
                    myStack = Nothing
                End If
                Return returnValue
            End If
        End Get
        Set(value As Type)
            If myStack Is Nothing Then
                myStack = New Stack(Of Type)
            End If
            myStack.Push(value)
        End Set
    End Property

    ''' <summary>
    ''' Liefert den aktuellen Wert zurück, der auch mehrfach gelesen werden kann, ohne die Stapelhistorie zu verändern (Peek-Funktion).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CurrentValue As Type
        Get
            If myStack Is Nothing Then
                Throw New NullReferenceException("This StackValue has not been assigned a value yet, or more values has been tried to be get than set.")
            End If
            Return myStack.Peek
        End Get
    End Property
End Structure
