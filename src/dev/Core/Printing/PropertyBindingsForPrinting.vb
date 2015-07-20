Public Class PropertyBindingsForPrinting

    ''' <summary>
    ''' Funktion, welche die Bindungsinformationen anhand des mvvmManagers ermittelt und als List(Of BindingInformationForReport) zurückgibt.
    ''' </summary>
    ''' <param name="mvvmManager"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getBindingInformation(mvvmManager As ActiveDevelop.EntitiesFormsLib.MvvmManager) As List(Of BindingInformationForPrinting)
        Dim mvvmBindings = mvvmManager.MvvmBindings
        Dim bindingInformationlist = New List(Of BindingInformationForPrinting)

        For Each mvvmBinding In mvvmBindings
            If mvvmBinding.Data.PropertyBindings IsNot Nothing Then
                For Each propBinding In mvvmBinding.Data.PropertyBindings
                    bindingInformationlist.Add(New BindingInformationForPrinting(mvvmBinding, propBinding))
                Next
            Else
                bindingInformationlist.Add(New BindingInformationForPrinting(mvvmBinding, Nothing))
            End If
        Next

        Return bindingInformationlist
    End Function


End Class
