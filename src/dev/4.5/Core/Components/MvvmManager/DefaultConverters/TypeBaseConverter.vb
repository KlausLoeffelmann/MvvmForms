'*****************************************************************************************
'                                    TypeBaseConverter.vb
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

Imports System.Windows.Data
Imports System.Runtime.CompilerServices
Imports System.Globalization
Imports System.Linq.Expressions
Imports System.Windows.Forms
Imports System.Drawing


''' <summary>
''' Kapselt IValueConverter-Funktionalität in einer Basisklasse, die dann vereinfachend mit den statischen Methoden ToThisType und ToThatType aufgerufen werden kann.
''' </summary>
''' <typeparam name="ThisType">Typ, von dem konvertiert wird.</typeparam>
''' <typeparam name="ThatType">Typ, in den konvertiert wird.</typeparam>
''' <typeparam name="t">Typ der ableitenden Klasse.</typeparam>
''' <remarks></remarks>
Public MustInherit Class TypeBaseConverter(Of ThisType, ThatType, t As {New, TypeBaseConverter(Of ThisType, ThatType, t)})
    Implements IValueConverter

    Private Shared myOnlyThisInstanceShallExists As t

    Protected Sub New()
    End Sub

    Public Shared Function GetInstance() As t
        If myOnlyThisInstanceShallExists Is Nothing Then
            myOnlyThisInstanceShallExists = New t
        End If
        Return myOnlyThisInstanceShallExists
    End Function

    Public Shared Function ToThisType(valueGetter As Func(Of ThatType),
                                        Optional parameter As Object = Nothing) As ThisType
        Try
            Return CType(GetInstance.ConvertBack(valueGetter.Invoke, Nothing, parameter, CultureInfo.CurrentCulture), ThisType)
        Catch ex As NullReferenceException
            If ReturnNothingOnNullReferenceException Then
                TraceEx.TraceError("'ToThisType' generated a NullReferenceException for Class '" & GetType(t).ToString & "'.")
                Return Nothing
            Else
                Throw
            End If
        End Try
    End Function

    Public Shared Function ToThatType(valueGetter As Func(Of ThisType),
                                        Optional parameter As Object = Nothing) As ThatType
        Try
            'For debugging purposes:
            Dim tmp = valueGetter.Invoke
            Return CType(GetInstance.Convert(tmp, Nothing, parameter, CultureInfo.CurrentCulture), ThatType)
        Catch ex As Exception
            If ReturnNothingOnNullReferenceException Then
                TraceEx.TraceError("'ToThatType' generated a NullReferenceException for Class '" & GetType(t).ToString & "'.")
                Return Nothing
            Else
                Throw
            End If
        End Try
    End Function

    Public MustOverride Function Convert(value As Object, targetType As Type,
                                         parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
    Public MustOverride Function ConvertBack(value As Object, targetType As Type,
                                             parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack

    Public Shared Property ReturnNothingOnNullReferenceException As Boolean = True

End Class

''' <summary>
''' Methodensammlung, um Werte aus Property-Pfaden zu ermitteln oder zu setzen, und Null-Referenz-Exception dabei zu unterdrücken.
''' </summary>
''' <remarks>HINWEIS: Diese Hilfsmethoden dürfen nur im Performance-unkritischen Kontext zur Arbeitserleichterung zum Einsatz kommen. 
''' Sie dienen dazu, beim Verarbeiten von Property-Pfaden zu verhindern, dass auf halben Weg eines Property-Pfades (z.B. Mitarbeiter.Adresse.Kommunikation.PrivateEmail) 
''' NullReference-Exceptions auftreten, weil bereits eine Instanz "auf dem Weg" zur eigentlichen Eigenschaft Null (Nothing in VB) war. 
''' Die Methoden selber fangen dabei NullReferenceException lediglich ab; sie werten den Pfad nicht wirklich aus. 
''' <para>Angewendet wird diese Hilfsmethode durch Übergabe eines Lambdas, also beispielsweise:</para>
''' <code>
''' Dim eMailAdresse = PropPath.GetValue(Function() MitarbeiterInstanz.AdressDaten.EmailAdresse)
''' </code>
''' <para>Sollte in diesem Fall AdressDaten null sein (Nothing in VB), wird standardmäßig keine Exception ausgelöst. 
''' Das kann geändert werden, indem die globale Variable SupressException auf False gesetzt wird (beispielsweise zu Debugging-Zwecken).</para>
''' </remarks>
Public Class PropPath

    Public Shared Function GetValue(Of PropType)(valueGetter As Func(Of PropType)) As PropType
        Try
            Return valueGetter.Invoke()
        Catch ex As Exception
            If SuppressException Then
                TraceEx.TraceError("'GetValue' generated a NullReferenceException for Class '" & GetType(PropType).ToString & "'.")
                Return Nothing
            Else
                Throw
            End If
        End Try
    End Function

    Public Shared Sub SetValue(Of PropType)(valueSetter As Func(Of PropType))
        Try
            valueSetter.Invoke()
        Catch ex As Exception
            If SuppressException Then
                TraceEx.TraceError("'SetValue' generated a NullReferenceException for Class '" & GetType(PropType).ToString & "'.")
            Else
                Throw
            End If
        End Try
    End Sub

    Public Shared Property SuppressException As Boolean = True

End Class
