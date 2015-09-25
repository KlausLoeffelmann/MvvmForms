Imports ActiveDevelop.MvvmBaseLib.Mvvm

Public Class CircTestPerson
    Inherits MvvmViewModelBase

    Property Name As String

    Property Anschrift As CircTestAnschrift
End Class

Public Class CircTestAnschrift
    Inherits MvvmViewModelBase

    Property Person As CircTestPerson
End Class