Imports ActiveDevelop.EntitiesFormsLib
Imports MeterReaderMvvmForms

Public Class NewEditBuildingView
    Implements IMvvmForm

    Public ReadOnly Property MvvmManager As MvvmManager Implements IMvvmForm.MvvmManager
        Get
            Return MvvmManager1
        End Get
    End Property

    Public ReadOnly Property Self As Form Implements IMvvmForm.Self
        Get
            Return Me
        End Get
    End Property
End Class
