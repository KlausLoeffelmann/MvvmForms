Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.Threading.Tasks

''' <summary>
''' Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
''' </summary>
<DataContract, MvvmSystemElement>
Public MustInherit Class BindableBase
    Implements INotifyPropertyChanged, IEditableObject, INotifyDataErrorInfo

    Private myErrorDictionary As New Dictionary(Of String, String)
    Private myGlobalError As String
    Private myIsPropertyChangeNotificationSuspended As Boolean
    Private myIsPropertyChangingSuspended As Boolean

    ''' <summary>
    ''' Multicast event for property change notifications.
    ''' </summary>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' Wird aufgerufen, wenn das Editieren eines Datensatzes beginnt. Typischerweise beim Editieren in einer Zeile innerhalb eines Grids, wenn die Zeile betreten wurde.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event NotifyBeginEdit(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird aufgerufen, wenn das Editieren eines Datensatzes beendet ist. Typischerweise beim Editieren in einer Zeile innerhalb eines Grids, wenn die Zeile verlassen wurde.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event NotifyEndEdit(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Wird aufgerufen, wenn das Editieren eines Datensatzes beendet ist. Typischerweise beim Editieren in einer Zeile innerhalb eines Grids, wenn die Zeile verlassen wurde.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event NotifyCancelEdit(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Checks if a property already matches a desired value.  Sets the property and notifies
    ''' listeners only when necessary.
    ''' </summary>
    ''' <typeparam name="T">Type of the property.</typeparam>
    ''' <param name="storage">Reference to a property with both getter and setter.</param>
    ''' <param name="value">Desired value for the property.</param>
    ''' <param name="propertyName">Name of the property used to notify listeners.  This value
    ''' is optional and can be provided automatically when invoked from compilers that support
    ''' CallerMemberName.</param>
    ''' <returns>True if the value was changed, false if the existing value matched the
    ''' desired value.</returns>
    ''' <remarks>
    ''' <para>The setting of the properties will only be performed, if the new value does 
    ''' not equal the old one. Property changes can also globally for the class instance be suspended, 
    ''' by calling the method <see cref="SuspendPropertyChanges">SuspendPropertyChanges</see> which causes the 
    ''' <see cref="IsPropertyChangingSuspended">IsPropertyChangingSuspended</see> property to be set.</para>
    ''' <para>For the Windows Forms MvvmManager component, this is implicitely done 
    ''' when assigning the ViewModel to the DataContext property of the source 
    ''' so dependent properties won't overwrite their initial values on assignment. (e.g. ViewModel sets DataSource 
    ''' of a List Control in the View, Control of View sets SelectedItem to nothing, corresponding property in 
    ''' ViewModel becomes nothing, desired Item in Control will not be selected.)</para>
    ''' <para>Call the method <see cref="ResumePropertyChanges">ResumePropertyChanges</see>
    ''' to resume property changes. For the Windows Forms MvvmManager component, this is automatically done for the ViewModel, 
    ''' after it has been assigned to the View's DataContext. 
    ''' If you want to change this default behavior, overwrite the methods <see cref="SuspendPropertyChanges">SuspendPropertyChanges</see> and 
    ''' <see cref="ResumePropertyChanges">ResumePropertyChanges</see>.</para>
    ''' <para>PropertyChange notifications can be suspended by calling the 
    ''' method <see cref="SuspendPropertyChangeNotification">"SuspendPropertyChangeNotification"</see> and can be resumed by calling 
    ''' <see cref="ResumePropertyChangeNotification">ResumePropertyChangeNotification</see></para>.</remarks>
    Protected Function SetProperty(Of T)(ByRef storage As T, value As T,
                                    <CallerMemberName> Optional propertyName As String = Nothing,
                                    Optional actionOnValidate As Func(Of Boolean) = Nothing) As Boolean

        If IsPropertyChangingSuspended Then Return False
        If Object.Equals(storage, value) Then Return False

        'Aufrufen, wenn nicht null.
        Dim dontChangeValue As Boolean = False

        If actionOnValidate IsNot Nothing Then
            dontChangeValue = actionOnValidate()
        End If

        If Not dontChangeValue Then
            storage = value
            If Not IsPropertyChangeNotificationSuspended Then
                Me.OnPropertyChanged(propertyName)
            End If
            Return True
        End If

        Return False
    End Function

    ''' <summary>
    ''' Notifies listeners that a property value has changed.
    ''' </summary>
    ''' <param name="propertyName">Name of the property used to notify listeners.  This value
    ''' is optional and can be provided automatically when invoked from compilers that support
    ''' <see cref="CallerMemberNameAttribute"/>.</param>
    Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    ''' <summary>
    ''' Erstellt eine flache Kopie der Instanz dieser Klasse.
    ''' </summary>
    ''' <typeparam name="bbType"></typeparam>
    ''' <returns></returns>
    Public Function ShallowClone(Of bbType As BindableBase)() As bbType
        Return DirectCast(MyBase.MemberwiseClone, bbType)
    End Function

    ' ''' <summary>
    ' ''' Erstellt eine vollständige Kopie (deep clone) dieser Instanz.
    ' ''' </summary>
    ' ''' <typeparam name="bbType"></typeparam>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    Public Function DeepClone(Of bbType)() As bbType

        Dim memStream As New MemoryStream
        Dim js As New Json.DataContractJsonSerializer(Me.GetType, TypeDetector.GetTypes(Me))
        Dim clonedObject As bbType

        'Als JSon in den Speicher serialisieren
        Try
            js.WriteObject(memStream, Me)
        Catch ex As Exception
            Throw
        End Try

        'Die Kopie wieder zurückserialisieren.
        Try
            memStream.Seek(0, SeekOrigin.Begin)
            clonedObject = DirectCast(js.ReadObject(memStream), bbType)
        Catch ex As Exception
            Throw
        End Try

        Return clonedObject
    End Function

    ''' <summary>
    ''' Erstellt eine Instanz dieses Typs auf Basis einer fremden Datenklasse. Angenommen wird hierbei, 
    ''' dass dieser Typ ein ViewModel des MVVM-Patterns bildet, während die fremde Datenklasse das Model bildet, 
    ''' und es die Anforderung gibt, Model und ViewModel zu kopieren.
    ''' </summary>
    ''' <typeparam name="ViewModelType">Der Typ (vorzugsweise der Typ der Ableitung dieser Klasse), der als neue ViewModel-Instanz 
    ''' fungieren soll, und die Daten aus der Model-Instanz übernimmt.</typeparam>
    ''' <typeparam name="modelType">Der Typ der Instanz der Model-Daten-Klasse, aus der die Eigenschaftenwerte übernommen werden.</typeparam>
    ''' <param name="model">Die Instanz der Model-Klasse, aus der die Daten übernommen werden.</param>
    ''' <returns>Eine neue Instanz der ViewModel-Klasse.</returns>
    ''' <remarks>Wenn Eigenschaften in der ViewModel-Klasse vorhanden sind, die im Model nicht existieren, werden diese 
    ''' Eigenschaften nicht versucht zu kopieren, und es gibt keine Fehlermeldung. Mithilfe des 
    ''' <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes können Sie einen anderen 
    ''' Namen für die dem ViewModel entsprechende Eigenschaft im Model bestimmen. Mit des 
    ''' <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes können Sie bestimmen, dass
    ''' eine Eigenschaft, obwohl sowohl im Model als auch im ViewModel vorhanden, nicht kopiert wird.
    ''' </remarks>
    Public Shared Function FromModel(Of ViewModelType As {New, BindableBase}, modelType)(model As modelType) As ViewModelType
        Dim viewModelToReturn As New ViewModelType
        viewModelToReturn.CopyPropertiesFrom(model)
        Return viewModelToReturn
    End Function

    ''' <summary>
    ''' Erstellt eine Kopie aus Objekten einer Liste eines Models (im MVVM-Pattern), und überträgt die entsprechenden Eigenschaften 
    ''' einer jeden Instanz in Objekte einer Liste eines ViewModels.
    ''' </summary>
    ''' <typeparam name="ViewModelType">Der Typ des ViewModels, aus dem die neue Liste bestehen soll.</typeparam>
    ''' <typeparam name="Modeltype">Der Typ des Models, aus dem die vorhandene Liste besteht.</typeparam>
    ''' <param name="modelCollection">Die Auflistung mit den entsprechenden Model-Objekten.</param>
    ''' <returns>Liste mit Objekten eines ViewModels aus der Liste der Models.</returns>
    ''' <remarks> Diese Methode erstellt eine Liste aus ViewModel-Objekten im MVVM-Pattern auf Basis einer Liste 
    ''' mit Model-Objekten. Dabei verwendet diese Funktion die statische Methode <see cref="FromModel(Of ViewModelType, modelType)(modelType)">FromModel</see>.
    ''' Wenn Eigenschaften in der ViewModel-Klasse vorhanden sind, die im Model nicht existieren, werden diese 
    ''' Eigenschaften nicht versucht zu kopieren, und es gibt keine Fehlermeldung. Mithilfe des 
    ''' <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes können Sie einen anderen 
    ''' Namen für die dem ViewModel entsprechende Eigenschaft im Model bestimmen. Mit des 
    ''' <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes können Sie bestimmen, dass
    ''' eine Eigenschaft, obwohl sowohl im Model als auch im ViewModel vorhanden, nicht kopiert wird.
    ''' </remarks>
    Public Shared Function FromModelList(Of ViewModelType As {New, BindableBase}, Modeltype)(modelCollection As IEnumerable(Of Modeltype)) As IEnumerable(Of ViewModelType)
        Return From modelItem In modelCollection
               Select FromModel(Of ViewModelType, Modeltype)(modelItem)
    End Function

    ''' <summary>
    ''' Kopiert die Inhalte der Eigenschaften von einer anderen Instanz dieser Klasse in die aktuelle. 
    ''' (Soll insbesondere zur Vereinfachung beim Mvvm-Pattern zur Übernahme von Daten eines Models 
    ''' in ein ViewModel verwendet werden).
    ''' </summary>
    ''' <param name="model"></param>
    ''' <remarks>Wenn Eigenschaften in der ViewModel-Klasse vorhanden sind, die im Model nicht existieren, werden diese 
    ''' Eigenschaften nicht versucht zu kopieren, und es gibt keine Fehlermeldung. Mithilfe des 
    ''' <see cref="ModelPropertyNameAttribute">ModelPropertyNameAttribute</see>-Attributes können Sie einen anderen 
    ''' Namen für die dem ViewModel entsprechende Eigenschaft im Model bestimmen. Mit des 
    ''' <see cref="ModelPropertyIgnoreAttribute">ModelPropertyIgnoreAttribute</see>-Attributes können Sie bestimmen, dass
    ''' eine Eigenschaft, obwohl sowohl im Model als auch im ViewModel vorhanden, nicht kopiert wird.
    ''' </remarks>
    Public Overridable Sub CopyPropertiesFrom(model As Object)
        'Properties holen

        If Not Me.GetType.GetTypeInfo.IsAssignableFrom(model.GetType.GetTypeInfo) Then
            CopyPropertiesFromDifferentType(model)
            Return
        End If

        Dim props = model.GetType.GetRuntimeProperties

        'Wir verwenden hier GetRuntimeProperties anstelle von GetProperties. Das wird für 
        '.NET 4.5 beispielsweise per Projection auf GetProperties gemapped (öffentliche Member, aber die reichen), 
        'funktioniert aber auch in WinRt und Windows Phone.
        For Each propItem In props
            Try
                Dim value As Object = propItem.GetValue(model, Nothing)
                propItem.SetValue(Me, value, Nothing)
            Catch ex As Exception
                Dim copPropException As New CopyPropertiesException("Model property '" & propItem.Name &
                                                                    "' could not be copied to ViewModel property '" &
                                                                    propItem.Name & "'." &
                                                                    vbNewLine & "Reason: " & ex.Message)
            End Try
        Next
    End Sub

    Private Sub CopyPropertiesFromDifferentType(model As Object)

        'Liste erstellen, mit ViewModel- und Model-PropertyInfo-Objekten
        'Create a list with ViewModel and Model PropertyInfo objects.

        For Each propToPropItem In From propItem In [GetType].GetRuntimeProperties
                                   Where propItem.GetCustomAttribute(Of ModelPropertyIgnoreAttribute)() Is Nothing
                                   Select New With {
                                        .ViewModelProperty = propItem,
                                        .ModelProperty = model.GetType.GetRuntimeProperty(
                                            If(propItem.GetCustomAttribute(Of ModelPropertyNameAttribute)() Is Nothing,
                                                propItem.Name,
                                                propItem.GetCustomAttribute(Of ModelPropertyNameAttribute).PropertyName))}

            'Falls es die entsprechende Eigenschaft im Model nicht gibt, dann überspringen.
            'Skip the ViewModel property, if a corresponding property does not exist in the model.
            If propToPropItem.ModelProperty Is Nothing Then
                Continue For
            End If

            'Liste Element für Element abarbeiten, und Werte aus der einen Eigenschaft in die andere Eigenschaft übernehmen.
            'Interate through list item by item and copy the property values from one property to the other.
            Try
                propToPropItem.ViewModelProperty.SetValue(Me,
                            propToPropItem.ModelProperty.GetValue(model))
            Catch ex As Exception
                Dim copPropException As New CopyPropertiesException(
                    "Model property '" & propToPropItem.ModelProperty.Name &
                    "' could not be copied to ViewModel property '" &
                    propToPropItem.ViewModelProperty.Name & "'." &
                    vbNewLine & "Reason: " & ex.Message)
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Kopiert die Inhalte der Eigenschaften von einer anderen Instanz dieser Klasse in die aktuelle.
    ''' </summary>
    ''' <param name="model"></param>
    ''' <remarks></remarks>
    Public Overridable Sub CopyPropertiesTo(model As Object)
        'Properties holen

        If Not Me.GetType.GetTypeInfo.IsAssignableFrom(model.GetType.GetTypeInfo) Then
            CopyPropertiesToDifferentType(model)
            Return
        End If

        Dim props = model.GetType.GetRuntimeProperties

        For Each propItem In props
            Try
                Dim value As Object = propItem.GetValue(Me, Nothing)
                propItem.SetValue(model, value, Nothing)
            Catch ex As Exception
                Dim copPropException As New CopyPropertiesException(
                    "Model property '" & propItem.Name &
                    "' could not be copied to ViewModel property '" &
                    propItem.Name & "'." &
                    vbNewLine & "Reason: " & ex.Message)
            End Try
        Next
    End Sub

    Private Sub CopyPropertiesToDifferentType(model As Object)

        'Liste erstellen, mit ViewModel- und Model-PropertyInfo-Objekten
        'Create a list with ViewModel and Model PropertyInfo objects.
        For Each propToPropItem In From propItem In [GetType].GetRuntimeProperties
                                   Where propItem.GetCustomAttribute(Of ModelPropertyIgnoreAttribute)() Is Nothing
                                   Select New With {
                                        .ViewModelProperty = propItem,
                                        .ModelProperty = model.GetType.GetRuntimeProperty(
                                            If(propItem.GetCustomAttribute(Of ModelPropertyNameAttribute)() Is Nothing,
                                                propItem.Name,
                                                propItem.GetCustomAttribute(Of ModelPropertyNameAttribute).PropertyName))}

            'Falls es die entsprechende Eigenschaft im Model nicht gibt, dann überspringen.
            'Skip the ViewModel property, if a corresponding property does not exist in the model.
            If propToPropItem.ModelProperty Is Nothing Then
                Continue For
            End If

            'Liste Element für Element abarbeiten, und Werte aus der einen Eigenschaft in die andere Eigenschaft übernehmen.
            'Interate through list item by item and copy the property values from one property to the other.
            Try
                propToPropItem.ModelProperty.SetValue(model,
                            propToPropItem.ViewModelProperty.GetValue(Me))
            Catch ex As Exception
                Dim copPropException As New CopyPropertiesException(
                    "Model property '" & propToPropItem.ModelProperty.Name &
                    "' could not be copied to ViewModel property '" &
                    propToPropItem.ViewModelProperty.Name & "'." &
                    vbNewLine & "Reason: " & ex.Message)
            End Try
        Next
    End Sub

    Private Sub BeginEdit() Implements IEditableObject.BeginEdit
        OnNotifyBeginEdit(EventArgs.Empty)
    End Sub

    Private Sub OnNotifyBeginEdit(e As EventArgs)
        RaiseEvent NotifyBeginEdit(Me, e)
    End Sub

    Private Sub CancelEdit() Implements IEditableObject.CancelEdit
        OnNotifyCancelEdit(EventArgs.Empty)
    End Sub

    Private Sub OnNotifyCancelEdit(e As EventArgs)
        RaiseEvent NotifyCancelEdit(Me, e)
    End Sub

    Private Sub EndEdit() Implements IEditableObject.EndEdit
        OnNotifyEndEdit(EventArgs.Empty)
    End Sub

    Private Sub OnNotifyEndEdit(e As EventArgs)
        RaiseEvent NotifyEndEdit(Me, e)
    End Sub

    ''' <summary>
    ''' Unterdrückt die PropertyChange-Benachrichtigung durch NotifyPropertyChanged.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SuspendPropertyChangeNotification()
        If IsPropertyChangeNotificationSuspended Then
            Throw New ArgumentException("Suspension of PropertyChangeNotifications is already in affect.")
        End If
        IsPropertyChangeNotificationSuspended = True
    End Sub

    ''' <summary>
    ''' Stellt die PropertyChange-Benachrichtigung wieder her.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResumePropertyChangeNotification()
        If Not IsPropertyChangeNotificationSuspended Then
            Throw New ArgumentException("Suspension of PropertyChangeNotifications is not in affect.")
        End If
        IsPropertyChangeNotificationSuspended = False
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob die PropertyChange-Benachrichtung aktiviert ist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ModelPropertyIgnore, MvvmSystemElementAttribute>
    Property IsPropertyChangeNotificationSuspended As Boolean
        Get
            Return myIsPropertyChangeNotificationSuspended
        End Get
        Private Set(value As Boolean)
            Me.SetProperty(myIsPropertyChangeNotificationSuspended, value)
        End Set
    End Property

    ''' <summary>
    ''' Unterdrückt das Ändern von Properties.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SuspendPropertyChanges()
        If IsPropertyChangingSuspended Then
            Throw New ArgumentException("Suspension of Property Changes is already in affect.")
        End If
        IsPropertyChangingSuspended = True
    End Sub

    ''' <summary>
    ''' Lässt das Ändern von Properties wieder zu.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResumePropertyChanges()
        If Not IsPropertyChangingSuspended Then
            Throw New ArgumentException("Suspension of Property Changes is not in affect.")
        End If
        IsPropertyChangingSuspended = False
    End Sub

    ''' <summary>
    ''' Bestimmt oder ermittelt, ob das Ändern von Properties derzeit gestattet ist.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ModelPropertyIgnore, MvvmSystemElementAttribute>
    Property IsPropertyChangingSuspended As Boolean
        Get
            Return myIsPropertyChangingSuspended
        End Get
        Private Set(value As Boolean)
            'At this point we MUSN'T use base.SetProperty, since it wouldn't work if suspended! 
            'Nasty little trap!!
            If Not Object.Equals(value, myIsPropertyChangingSuspended) Then
                myIsPropertyChangingSuspended = value
                OnPropertyChanged("IsPropertyChangingSuspended")
            End If
        End Set
    End Property

    Public Event ErrorsChanged(sender As Object, e As DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Protected Overridable Sub OnErrorsChanged(e As DataErrorsChangedEventArgs)
        RaiseEvent ErrorsChanged(Me, e)
    End Sub

    Public Overridable Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        Throw New NotImplementedException("Not yet implemented.")
    End Function

    <ModelPropertyIgnore, MvvmSystemElementAttribute>
    Public Overridable ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return False
        End Get
    End Property
End Class
