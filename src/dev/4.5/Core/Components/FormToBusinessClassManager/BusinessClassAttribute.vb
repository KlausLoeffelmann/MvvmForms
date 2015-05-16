'*****************************************************************************************
'                                    BusinessClassAttribute.vb
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
''' Bestimmt, dass eine Klasse eine Businesslogik enthält, und für den FormToBusinessClass-Manager verwendet werden kann.
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Class)>
Public Class BusinessClassAttribute
    Inherits Attribute

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        MyBase.new()
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und bestimmt, ob alle Eigenschaften der definierenden Klasse standardmäßig einbezogen oder ausgeschlossen werden sollen.
    ''' </summary>
    ''' <param name="options"></param>
    ''' <remarks></remarks>
    Sub New(options As BusinessClassAttributeOptions)
        Me.Options = options
    End Sub

    ''' <summary>
    ''' Ermittelt oder definiert, ob alle Eigenschaften der definierenden Klasse standardmäßig einbezogen oder ausgeschlossen werden sollen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Options As BusinessClassAttributeOptions

End Class

''' <summary>
''' Bestimmt, das eine Eigenschaft einer Businessklasse als Pfad-Wurzel für das DataBinding verwendet werden kann.
''' </summary>
''' <remarks></remarks>
<AttributeUsage(AttributeTargets.Property)>
Public Class BusinessClassPropertyAttribute
    Inherits Attribute

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        MyBase.new()
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse und weist die Eigenschaft einer Kategorie zu.
    ''' </summary>
    ''' <param name="category">Kategorie, die der Eigenschaft über der dieses Attribut steht, zugewiesen wird.</param>
    ''' <remarks></remarks>
    Sub New(category As String)
        Me.Category = category
    End Sub

    ''' <summary>
    ''' Erstellt eine Instanz dieser Klasse, weist die Eigenschaft einer Kategorie zu und definiert 
    ''' zusätzliche Optionen, die bei der Steuerelementanordnung berücksichtigt werden sollen.
    ''' </summary>
    ''' <param name="category">Kategorie, die der Eigenschaft über der dieses Attribut steht, zugewiesen wird.</param>
    ''' <param name="options">Definiert Optionen, über die bestimmte Regeln für die Steuerelementeerstellung codiert werden können. (Für zukünftige Erweiterungen).</param>
    ''' <remarks></remarks>
    Sub New(category As String, options As BusinessPropertyAttributeOptions)
        Me.Category = category
        Me.Options = options
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="category">Kategorie, die der Eigenschaft über der dieses Attribut steht, zugewiesen wird.</param>
    ''' <param name="options">Definiert Optionen, über die bestimmte Regeln für die Steuerelementeerstellung codiert werden können. (Für zukünftige Erweiterungen).</param>
    ''' <param name="column">Definiert Spaltenzuweisungen, für Regeln bei Layouting der Steuerelementerstellung. (Für zukünftige Erweiterungen).</param>
    ''' <remarks></remarks>
    Sub New(category As String, options As BusinessPropertyAttributeOptions, column As Integer)
        Me.Category = category
        Me.Options = options
        Me.Column = column
    End Sub

    ''' <summary>
    ''' Definiert einen Eigenschaften-Kategorie, um sie später bei der Codegenerierung oder 
    ''' dynamischen Formularerstellung einfacher auf andere Container-Steuerelemente zu verteilen.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Category As String

    ''' <summary>
    ''' Definiert Optionen, über die bestimmte Regeln für die Steuerelementeerstellung codiert werden können. (Für zukünftige Erweiterungen).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Für zukünftige Erweiterungen reserviert.</remarks>
    Property Options As BusinessPropertyAttributeOptions

    ''' <summary>
    ''' Definiert für ein Layouting die Spaltennummer der Spalte, auf der das Steuerelement platziert werden soll. (Für zukünftige Erweiterungen).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Für zukünftige Erweiterungen reserviert.</remarks>
    Property Column As Integer
End Class

''' <summary>
''' Bestimmt, ob alle Eigenschaften der definierenden Klasse standardmäßig einbezogen oder ausgeschlossen werden sollen.
''' </summary>
''' <remarks></remarks>
Public Enum BusinessClassAttributeOptions As Integer
    IncludeAllPropertiesByDefault = 0
    ExcludeAllPropertiesByDefault = 1
End Enum


''' <summary>
''' Definiert eine Reihe von Optionen, die bei der Codegenerierung oder der dynamischen Erstellung von Formularen Beachtung finden sollen.
''' </summary>
''' <remarks>Für zukünftige Erweiterungen reserviert.</remarks>
<Flags()>
Public Enum BusinessPropertyAttributeOptions As Long
    IncludeProperty = 0
    ExcludeProperty = 1
    UseTableLayoutPanelLayouting = 3
    PlaceInPanel = 5
    PlaceInGroupBox = 9
    PlaceInTab = 17
    PlaceInCustomContainer = 33
End Enum
