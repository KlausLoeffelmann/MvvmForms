'*****************************************************************************************
'                                         StackedValue.vb
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

''' <summary>
''' Stellt einen Datentyp zur Verfügung, der sich durch seine Value-Eigenschaft 
''' beim Zuweisen und Auslesen wie ein Stapel verhält.
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
