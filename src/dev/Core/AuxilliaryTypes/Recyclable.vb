'*****************************************************************************************
'                                         Recyclable.vb
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

Imports System.Collections
Imports System.Collections.Generic

''' <summary>
''' Interface für die Parameter des Recyclable-Interfaces
''' </summary>
''' <remarks></remarks>
Public Interface IRecyclableParameters
    Inherits IComparable
End Interface

''' <summary>
''' Interface, dass implementiert werden muss, um ein Object recycclingfähig zu machen
''' </summary>
''' <remarks></remarks>
Public Interface IRecyclable
    ''' <summary>
    ''' der Parent ist die Instanz, die diese Parameterklasse (also die Klasse, die dieses Interface implementiert) verwendet
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Parent() As IRecycle

    ''' <summary>
    ''' Legt die Ressourcen eines Recyclables an. Diese Methode wird nur einmal beim Anlegen der 
    ''' Instanz aufgerufen. Die Parameter wurden bereits gesetzt
    ''' </summary>
    ''' <remarks></remarks>
    Sub AllocateRessource(ByVal parameter As IRecyclableParameters)

    ''' <summary>
    ''' Initialisiert die Instanz anhand der bei AllocateRessource übergebenen RecyclableParameters
    ''' Entgegen AllocateRessource, welche nur einmal aufgerufen wird, wird InitializeRecyclable 
    ''' bei jeder Verwendung der Instanz augerufen (durch GetFreeObject)
    ''' </summary>
    ''' <remarks></remarks>
    Sub InitializeRecyclable()

    ''' <summary>
    ''' gibt diese Instanz für die Wiederverwendung frei.
    ''' Muss "Parent.Free(me)" aufrufen
    ''' </summary>
    ''' <remarks></remarks>
    Sub Recycle()
End Interface

''' <summary>
''' Stellt die Recycle-Methode zuur Verfügung
''' Sie Muss als Seperates Interface implementiert werden !
''' Grund: Bei IRecyclables. Parent könnte auch die Recyclables zuurückgeliefert werden. 
''' Leider stören hier jedoch die Generics, denn Parent müsste Recyclables (of type) zurückgeben. 
''' Der ist jedoch im Interface nicht bekannt
''' (man kann ihn auch bekannt geben, es funktioniert trotzdem nicht)
''' </summary>
''' <remarks></remarks>
Public Interface IRecycle
    Sub Recycle(ByVal o As IRecyclable)
End Interface

''' <summary>
''' Verwaltungsklasse für Recyclables.
''' Pro RecyclableType und Parameterliste muss eine Instanz diese Klasse existieren
''' </summary>
''' <typeparam name="RecyclableType"></typeparam>
''' <remarks></remarks>
Public Class Recyclables(Of RecyclableType As {IRecyclable, New})
    Implements IRecycle

    Private _recyclableObject As New Queue(Of RecyclableType)
    'Private myLocker As String = "Locker"
    Private myLocker As New Object
    ''' <summary>
    ''' Legt die angebene Anzahl der Instanzen an. Es müssen hier die Parameter angebene werden, da für jede angelegte Instanz
    ''' die Methode AllocateRessource aufgerufen wird
    ''' </summary>
    ''' <param name="count"></param>
    ''' <param name="parameters"></param>
    ''' <remarks></remarks>
    Sub New(ByVal count As Integer, ByVal parameters As IRecyclableParameters)
        For i As Integer = 1 To count
            'Instanzen anlegen
            Dim tmpnew As New RecyclableType
            tmpnew.Parent = Me
            tmpnew.AllocateRessource(parameters)
            _recyclableObject.Enqueue(tmpnew)
        Next
        GC.Collect()
    End Sub

    ''' <summary>
    ''' Gibt eine Instanz als Recyclefähig frei
    ''' </summary>
    ''' <param name="o"></param>
    ''' <remarks></remarks>
    Sub Recycle(ByVal o As IRecyclable) Implements IRecycle.Recycle
        SyncLock (myLocker)
            _recyclableObject.Enqueue(CType(o, RecyclableType))
        End SyncLock
    End Sub

    ''' <summary>
    ''' Liefert eine wiederverwendbare Instanz aus dem Pool zurück
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetFreeObject() As RecyclableType
        SyncLock (myLocker)
            If _recyclableObject.Count = 0 Then
                Throw New NoFreeObjectException("Es stehen keine Objekte mehr zur Verfügung.")
            End If
            Dim tmpRec As RecyclableType = _recyclableObject.Dequeue
            tmpRec.InitializeRecyclable()
            Return tmpRec
        End SyncLock

    End Function
End Class


''' <summary>
''' Exception, falls kein freies Objekt mehr vorhanden ist.
''' </summary>
''' <remarks></remarks>
Public Class NoFreeObjectException
    Inherits Exception

    Public Sub New(ByVal msg As String)
        MyBase.New(msg)
    End Sub
End Class

''' <summary>
''' Stellt eine Standardimplementierung der IRecyclable-Schnittstelle bereit
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RecyclableBase
    Implements IRecyclable

    ''' <summary>
    ''' Verwaltunginstanz für dieses Objekt
    ''' </summary>
    ''' <remarks></remarks>
    Private _parentQueue As IRecycle

    ''' <summary>
    ''' AllocateRessource muss überschrieben werden um die notwendigen Ressourcen anzulegen
    ''' z.B. Bitmaps
    ''' Als Hilfestellung wird die Parameterliste übergeben
    ''' Die Parameterliste ist für die Lebensdauer des Objektes fest.
    ''' </summary>
    ''' <param name="parameter"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub AllocateRessource(ByVal parameter As IRecyclableParameters) Implements IRecyclable.AllocateRessource

    ''' <summary>
    ''' Initialisiert ein Recyclable
    ''' Diese Standardimplementierung führt keine Aktion durch.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitializeRecyclable() Implements IRecyclable.InitializeRecyclable
        ' Default: Nothing to do
    End Sub

    Public Sub Free() Implements IRecyclable.Recycle
        Parent.Recycle(Me)
    End Sub

    ''' <summary>
    ''' Liefert die Verwaltungsinstanz des Objekts zurck oder legt diese fest.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Parent() As IRecycle Implements IRecyclable.Parent
        Get
            Return _parentQueue
        End Get
        Set(ByVal value As IRecycle)
            _parentQueue = value
        End Set
    End Property
End Class

''' <summary>
''' StandardImplementierung der IRecyclableParameters-Schnittstelle
''' Verwendet für den Parametervergleich den Hashcode
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RecyclableParametersBase
    Implements IRecyclableParameters

    ''' <summary>
    ''' Implementierung der CompareTo-Methode 
    ''' Verleicht die Hashwerte der Objekte
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If obj Is Nothing Then
            'Exception?
        End If
        Return Me.GetHashCode.CompareTo(obj.GetHashCode)
    End Function

    ''' <summary>
    ''' Liefert den Hashcode für das Object
    ''' Diese Standardimplemeniterung liefert den HashCode der Basisklasse zurück
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode()
    End Function
End Class