Imports System.ComponentModel
Imports System.Text
Imports ActiveDevelop.MvvmBaseLib.Mvvm
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()> Public Class TestEventBinding

    <TestMethod>
    Public Sub WeakEventManager_BasicHandler()
        Dim sender = New TestViewModelSender()

        WeakEventManager.[AddHandler](Of PropertyChangedEventArgs)(sender, Function(s, e)
                                                                               If sender <> s Then
                                                                                   Assert.Fail("Der übergebene Sender ist nicht korrekt!")

                                                                               End If


                                                                               If e Is Nothing Then
                                                                                   Assert.Fail("Die EventArgs sind nicht gefüllt!")

                                                                               End If

                                                                           End Function, NameOf(TestViewModelSender.PropertyChanged))

    End Sub

    <TestMethod>
    Public Sub WeakEventManager_CallHandler()
        Dim test = New EventTest()

        GC.Collect()
        GC.WaitForFullGCComplete()

        If Not test.Listener.HandlerCalled Then

            Assert.Fail("Handler wurde nicht aufgerufen!")

        End If
    End Sub

    <TestMethod>
    Public Sub WeakEventManager_ReleaseSenderObject()
        Dim test = New EventTest()

        test.Sender = Nothing

        GC.Collect()
        GC.WaitForFullGCComplete()

        If test.WeakSender.IsAlive Then

            Assert.Fail("Der Sender darf nicht mehr gehalten werden!")

        End If
    End Sub

    <TestMethod>
    Public Sub WeakEventManager_ReleaseListenerObject()
        Dim test = New EventTest()

        test.Listener = Nothing

        GC.Collect()
        GC.WaitForFullGCComplete()

        If test.WeakListener.IsAlive Then

            Assert.Fail("Der Listener darf nicht mehr gehalten werden!")

        End If

        test.Sender.CallPropertyChanged()
    End Sub

    <TestMethod>
    Public Sub WeakEventManager_TestRemoveHandler()

        Dim test = New EventTest()


        test.Listener.HandlerCalled = False
        test.Listener.RemoveHandlerWeak(test.Sender)

        test.Sender.CallPropertyChanged()

        Assert.IsFalse(test.Listener.HandlerCalled, "Der Handler darf nach RemoveHandlerWeak nicht mehr aufgerufen werden!")

        test.Listener.HandlerCalled = False
        test.Listener.AddHandlerWeak(test.Sender)

        test.Listener.AddHandlerWeak(test.Sender)
        test.Listener.RemoveHandlerWeak(test.Sender)

        test.Sender.CallPropertyChanged()

        Assert.IsTrue(test.Listener.HandlerCalled, "Der Handler muss einmal aufgerufen werden!")

        'check that there are no more proxyinstances
    End Sub

    <TestMethod>
    Public Sub WeakEventManager_TestPerformanceAddHandler()
        Dim sender = New TestViewModelSender()
        Dim listener = New TestViewModelListener()

        Dim swNormal = Stopwatch.StartNew()

        listener.AddHandlerNormal(sender)
        swNormal.[Stop]()

        sender = New TestViewModelSender()
        listener = New TestViewModelListener()

        Dim swWeak = Stopwatch.StartNew()

        listener.AddHandlerWeak(sender)
        swWeak.[Stop]()

        If swWeak.ElapsedMilliseconds > swNormal.ElapsedMilliseconds + 5 Then
            Assert.Fail("Performance ist schlechter als normal: {swNormal.ElapsedMilliseconds} ms für normales binding und {swWeak.ElapsedMilliseconds} ms für weak.")

        End If

    End Sub

    <TestMethod>
    Public Sub WeakEventManager_TestPerformanceCallHandler()
        Dim interations = 5000


        Dim sender = New TestViewModelSender()
        Dim listener = New TestViewModelListener()

        listener.AddHandlerWeak(sender)

        Dim swWeak = Stopwatch.StartNew()

        For i As Integer = 0 To interations - 1

            sender.CallPropertyChanged()
        Next

        swWeak.[Stop]()

        listener.RemoveHandlerWeak(sender)

        listener.AddHandlerWeakWPF(sender)

        Dim swNormal = Stopwatch.StartNew()

        For i As Integer = 0 To interations - 1

            sender.CallPropertyChanged()
        Next

        swNormal.[Stop]()

        If swWeak.ElapsedTicks > swNormal.ElapsedTicks * 2 Then
            Assert.Fail("Performance ist schlechter als normal: {swNormal.ElapsedMilliseconds} ms für normales binding und {swWeak.ElapsedMilliseconds} ms für weak.")

        End If

    End Sub

    <TestMethod>
    Public Sub WeakEventManager_TestPerformanceRemoveHandler()
        Dim sender = New TestViewModelSender()
        Dim listener = New TestViewModelListener()

        listener.AddHandlerNormal(sender)
        Dim swNormal = Stopwatch.StartNew()

        listener.RemoveHandlerWeak(sender)
        swNormal.[Stop]()

        listener.AddHandlerNormal(sender)
        Dim swWeak = Stopwatch.StartNew()

        listener.RemoveHandlerNormal(sender)
        swWeak.[Stop]()

        If swWeak.ElapsedMilliseconds > swNormal.ElapsedMilliseconds + 5 Then
            Assert.Fail("Performance ist schlechter als normal: {swNormal.ElapsedMilliseconds} ms für normales binding und {swWeak.ElapsedMilliseconds} ms für weak.")

        End If

    End Sub


    Private Class EventTest
        Public WeakSender As WeakReference
        Public WeakListener As WeakReference

        Public Sub New()
            Listener = New TestViewModelListener()
            Sender = New TestViewModelSender()

            WeakSender = New WeakReference(Sender)
            WeakListener = New WeakReference(Listener)


            Listener.AddHandlerWeak(Sender)
            'Listener.AddHandlerNormal(Sender);
            Sender.CallPropertyChanged()
        End Sub

        Property Listener As TestViewModelListener

        Property Sender As TestViewModelSender
    End Class
End Class