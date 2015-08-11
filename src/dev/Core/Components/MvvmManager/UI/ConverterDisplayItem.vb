'*****************************************************************************************
'                                    ConverterDisplayItem.vb
'                    =======================================================
'
'          Part of MvvmForms - The Component Library for bringing the Model-View-Viewmodel
'                              pattern to Data Centric Windows Forms Apps in an easy,
'                              feasible and XAML-compatible way.
'
'
'    This designer code is proprtiety code. A licence can be obtained - CONTACT INFO, see below.
'    Permission is granted, to use the designer code (in terms of running it for developing purposes)
'    to maintain Open Source Projects according to the OSI (opensource.org). For maintaining 
'    designer code in commercial (propriety) projects, a licence must be obtained.
'
'                       ActiveDevelop
'                       Bremer Str. 4
'                       Lippstadt, DE-59555
'                       Germany
'                       email: mvvmforms at activedevelop . de. 
'*****************************************************************************************

''' <summary>
''' Used to show an entry in the Converter Combo Box in the PropertyBindings Dialog.
''' </summary>
Public Class ConverterDisplayItem
    Property ConverterName As String
    Property ConverterAssembly As String
    Property ConverterType As Type
End Class
