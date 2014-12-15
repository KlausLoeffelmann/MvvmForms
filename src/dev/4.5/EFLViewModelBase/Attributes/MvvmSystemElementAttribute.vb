
''' <summary>
''' Definiert eine Eigenschaft als Infrastruktur oder System-Eigenschaft, sodass diese bei bestimmten 
''' Designern in der Spaltenauswahl nicht mit angezeigt oder berücksichtigt wird.
''' </summary>
Public Class MvvmSystemElementAttribute
    Inherits Attribute

    Sub New()
        MyBase.New()
    End Sub

End Class
