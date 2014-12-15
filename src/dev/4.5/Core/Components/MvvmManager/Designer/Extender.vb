Imports System.Runtime.CompilerServices

Public Module Extender
    <Extension>
    Public Function GetService(Of ty)(sp As IServiceProvider) As ty
        Dim serv = sp.GetService(GetType(ty))
        If serv Is Nothing Then Return Nothing
        Return DirectCast(serv, ty)
    End Function
End Module
