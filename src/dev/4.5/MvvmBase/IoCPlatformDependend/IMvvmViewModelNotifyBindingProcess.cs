using System;

/// <summary>
/// Interface which can be implemented for only use in WIndows Forms, if the WinForms MvvmManager should inform the class that the binding starts. (Only for backwards compatibility reasons).
/// </summary>
[Obsolete("Don't use this Interface if not absolutely necessary, since it introduces a platform dependency which is hard to dissolve.")]
    public interface IMvvmViewModelNotifyBindingProcess
{
    void BeginBinding();
    void EndBinding();
}
