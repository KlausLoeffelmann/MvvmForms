'*****************************************************************************************
'                                     HistoricalBoolean.vb
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
''' Stellt einen Boolean-Datentyp zur Verfügung, der erst dann wieder FALSE zurückliefert, 
''' wenn ihm FALSE sooft zugewiesen wurde, wie ihm zuvor TRUE zugewiesen wurde (und umgekehrt).
''' </summary>
''' <remarks></remarks>
Public Class HistoricalBoolean

    Private truecount As Integer

    Property Value As Boolean
        Get
            Return truecount > 0
        End Get
        Set(ByVal value As Boolean)
            truecount += If(value, 1, -1)
            truecount = If(truecount < 0, 0, truecount)
        End Set
    End Property
End Class
