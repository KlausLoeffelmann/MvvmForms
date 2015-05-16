'*****************************************************************************************
'                                IFormsToBusinessClassManager.vb
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

Imports System.Data.Objects

''' <summary>
''' Kennzeichnet ein Formular, dass mit einer Instanz einer <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see> verwaltet wird.
''' </summary>
''' <remarks>Diese Schnittstelle muss in ein Formular eingebunden werden, damit Extender-Methoden des 
''' <see cref="EntitiesFormsLib.FormToBusinessClassManagerExtender">FormToBusinessClassManagerExtender-Modules</see> verwendet werden können.</remarks>
Public Interface IFormToBusinessClassManagerHost
    ''' <summary>
    ''' Ermittelt die <see cref="EntitiesFormsLib.FormToBusinessClassManager">FormToBusinessClassManager-Komponente</see>, 
    ''' mit der dieses Formular seine Business-Klasse verwaltet.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property FormToBusinessClassManager As FormToBusinessClassManager

    ''' <summary>
    ''' Bestimmt oder ermittelt den Objekt-Kontext, der benötigt wird, um Navigation-Properties aus DataFieldName-Eigenschaften zu setzen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TargetObjectContext As ObjectContext

End Interface

