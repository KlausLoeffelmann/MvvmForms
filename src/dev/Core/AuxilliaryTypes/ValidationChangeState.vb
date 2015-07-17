'*****************************************************************************************
'                                    ValidationChangeState.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'                    Copyright -2015 by Klaus Loeffelmann
'
'    This program is free software; you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation; either version 2 of the License, or
'    (at your option) any later version.
'
'    This program is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty Of
'    MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License For more details.
'
'    You should have received a copy of the GNU General Public License along
'    with this program; if not, write to the Free Software Foundation, Inc.,
'    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
'
'    MvvmForms is dual licenced. A permissive licence can be obtained - CONTACT INFO:
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

Public Class ValueValidationStateStore(Of NullableType As {Structure, IComparable})

    Private Shared myPredefinedValidatingInstance As ValueValidationStateStore(Of NullableType)
    Private Shared myPredefinedValidatedInstance As ValueValidationStateStore(Of NullableType)

    Public Property EventType As ValidateEventType
    Public Property ValidationEventArgs As NullableValueValidationEventArgs(Of NullableType?)
    Public Property RaiseValueValidationStateChangedEventUnconditionally As Boolean

    Public Shared Function GetPredefinedValidatingInstance() As ValueValidationStateStore(Of NullableType)
        If myPredefinedValidatingInstance Is Nothing Then
            myPredefinedValidatingInstance = New ValueValidationStateStore(Of NullableType) With {.EventType = ValidateEventType.Validating,
                                                                                             .RaiseValueValidationStateChangedEventUnconditionally = True}
        End If
        Return myPredefinedValidatingInstance
    End Function

    Public Shared Function GetPredefinedValidatedInstance() As ValueValidationStateStore(Of NullableType)
        If myPredefinedValidatedInstance Is Nothing Then
            myPredefinedValidatedInstance = New ValueValidationStateStore(Of NullableType) With {.EventType = ValidateEventType.Validated,
                                                                                            .RaiseValueValidationStateChangedEventUnconditionally = True}
        End If
        Return myPredefinedValidatedInstance
    End Function
End Class

Public Enum ValidateEventType
    Validating
    Validated
End Enum