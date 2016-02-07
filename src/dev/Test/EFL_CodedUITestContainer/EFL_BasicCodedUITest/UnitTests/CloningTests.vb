<TestClass()>
Public Class CloningTests

    <TestMethod()>
    Public Sub BasicCloningTest()

        Dim contacts = ContactViewModel.GetTestData(10)
        Dim contactVm = contacts(0)
        Dim contactModel As New ContactModel

        contactVm.CopyPropertiesTo(contactModel)

        Dim newContactVM As New ContactViewModel
        newContactVM.CopyPropertiesFrom(contactModel)

        Assert.AreNotEqual(contactVm.ID, newContactVM.ID)
        Assert.AreEqual(contactVm.Firstname, newContactVM.Firstname)
        Assert.AreEqual(contactVm.Lastname, newContactVM.Lastname)
        Assert.AreEqual(contactVm.AddressLine1, newContactVM.AddressLine1)
        Assert.AreEqual(contactVm.AddressLine2, newContactVM.AddressLine2)
        Assert.AreEqual(contactVm.City, newContactVM.City)
        Assert.AreEqual(contactVm.Zip, newContactVM.Zip)
    End Sub

End Class
