'*****************************************************************************************
'                                         StringValue.vb
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
''' String, der als ValueType ausgelegt ist, um der Verarbeitung als Nullable zu genügen.
''' </summary>
''' <remarks></remarks>
Public Structure StringValue
    Implements IComparable

    Private myValue As String

    Public Sub New(ByVal value As String)
        myValue = value
    End Sub

    Public Property Value() As String
        Get
            If myValue Is Nothing Then
                Return ""
            End If
            Return myValue.ToString
        End Get
        Set(ByVal value As String)
            myValue = value
        End Set
    End Property

    Public ReadOnly Property IsEmpty() As Boolean
        Get
            If Value.Length = 0 Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Value
    End Function

    Public Shared Operator +(ByVal value1 As StringValue, ByVal value2 As String) As StringValue
        Return New StringValue(value1.Value & value2)
    End Operator

    Public Shared Operator +(ByVal value1 As StringValue, ByVal value2 As StringValue) As StringValue
        Return New StringValue(value1.Value & value2.Value)
    End Operator

    Public Shared Widening Operator CType(ByVal value1 As String) As StringValue
        Return New StringValue(value1)
    End Operator

    Public Shared Widening Operator CType(ByVal value As StringValue) As String
        Return value.Value
    End Operator

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If obj Is Nothing AndAlso myValue Is Nothing Then Return 0
        If obj Is Nothing Then Throw New NullReferenceException("The object to compare with must not be null!")
        Return Value.CompareTo(obj.ToString)
    End Function
End Structure