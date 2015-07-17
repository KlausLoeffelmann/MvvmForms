Imports System.Windows.Forms

''' <summary>
''' Stellt Funktionen zur Verfügung, mit denen Steuerelemente, die die INullableValueDataBinding-Schnittstelle implementieren, 
''' in einem ControlContainer ermittelt und zur Datenübertragung in Datenquellen überprüft werden können.
''' </summary>
''' <remarks></remarks>
Public Class NullableValueDataBindingControlCollection
    Inherits List(Of INullableValueDataBinding)

    ''' <summary>
    ''' Liefert - rekursiv ermittelt - eine Liste aller Steuerelemente eines ContainerControls zurück, die die INullableValue-Schnittstelle implementieren.
    ''' </summary>
    ''' <param name="cControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FromContainerControl(ByVal cControl As Control, ByVal AssignedTo As FormToBusinessClassManager) As NullableValueDataBindingControlCollection
        Return FromContainerControlInternal(New NullableValueDataBindingControlCollection, cControl, AssignedTo)
    End Function

    Private Shared Function FromContainerControlInternal(ByVal nullableControls As NullableValueDataBindingControlCollection, ByVal cControl As Control,
                                                         ByVal AssignedTo As FormToBusinessClassManager) As NullableValueDataBindingControlCollection

        For Each c As Control In cControl.Controls
            If c.GetType.GetInterface(GetType(INullableValueDataBinding).Name) IsNot Nothing Then
                'Changed on April 4th 2010: Absofort können die Steuerelemente verschiedenen Controls zugeordnet werden.
                If DirectCast(c, INullableValueDataBinding).AssignedManagerControl Is AssignedTo Then
                    nullableControls.Add(DirectCast(c, INullableValueDataBinding))
                End If
            End If
            If c.HasChildren Then
                FromContainerControlInternal(nullableControls, DirectCast(c, Control), AssignedTo)
            End If
        Next
        Return nullableControls
    End Function
End Class

''' <summary>
''' Stellt Funktionen zur Verfügung, mit denen Steuerelemente, die die INullableValueDataBinding-Schnittstelle implementieren, 
''' in einem ControlContainer ermittelt und zur Datenübertragung in Datenquellen überprüft werden können.
''' </summary>
''' <remarks></remarks>
Public Class NullableValueEditorControlCollection
    Inherits List(Of INullableValueEditor)

    ''' <summary>
    ''' Liefert - rekursiv ermittelt - eine Liste aller Steuerelemente eines ContainerControls zurück, die die INullableValueEditor-Schnittstelle implementieren.
    ''' </summary>
    ''' <param name="cControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FromContainerControl(ByVal cControl As Control,
                                                ByVal AssignedTo As FormToBusinessClassManager) As NullableValueEditorControlCollection
        Return FromContainerControlInternal(New NullableValueEditorControlCollection, cControl, AssignedTo)
    End Function

    Private Shared Function FromContainerControlInternal(ByVal nullableControls As NullableValueEditorControlCollection, ByVal cControl As Control,
                                                         ByVal AssignedTo As FormToBusinessClassManager) As NullableValueEditorControlCollection

        For Each c As Control In cControl.Controls
            If c.GetType.GetInterface(GetType(INullableValueEditor).Name) IsNot Nothing Then
                'Changed on April 4th 2010: Absofort können die Steuerelemente verschiedenen Controls zugeordnet werden.
                If DirectCast(c, INullableValueEditor).AssignedManagerControl Is AssignedTo Then
                    nullableControls.Add(DirectCast(c, INullableValueEditor))
                End If

            End If
            If c.HasChildren Then
                FromContainerControlInternal(nullableControls, DirectCast(c, Control), AssignedTo)
            End If
        Next
        Return nullableControls
    End Function
End Class

''' <summary>
''' Stellt Funktionen zur Verfügung, mit denen Steuerelemente, die die INullableValueDataBinding-Schnittstelle implementieren, 
''' in einem ControlContainer ermittelt und zur Datenübertragung in Datenquellen überprüft werden können.
''' </summary>
''' <remarks></remarks>
Public Class UpdatableInfoControlCollection
    Inherits List(Of IUpdatableInfoControl)

    ''' <summary>
    ''' Liefert - rekursiv ermittelt - eine Liste aller Steuerelemente eines ContainerControls zurück, die die IUpdatableInfoControl-Schnittstelle implementieren.
    ''' </summary>
    ''' <param name="cControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FromContainerControl(ByVal cControl As Control,
                                                ByVal AssignedTo As FormToBusinessClassManager) As UpdatableInfoControlCollection
        Return FromContainerControlInternal(New UpdatableInfoControlCollection, cControl, AssignedTo)
    End Function

    Private Shared Function FromContainerControlInternal(ByVal nullableControls As UpdatableInfoControlCollection, ByVal cControl As Control,
                                                         ByVal AssignedTo As FormToBusinessClassManager) As UpdatableInfoControlCollection

        For Each c As Control In cControl.Controls
            If c.GetType.GetInterface(GetType(IUpdatableInfoControl).Name) IsNot Nothing Then
                If DirectCast(c, IAssignableFormToBusinessClassManager).AssignedManagerControl Is AssignedTo Then
                    nullableControls.Add(DirectCast(c, IUpdatableInfoControl))
                End If

            End If
            If c.HasChildren Then
                FromContainerControlInternal(nullableControls, DirectCast(c, Control), AssignedTo)
            End If
        Next
        Return nullableControls
    End Function
End Class