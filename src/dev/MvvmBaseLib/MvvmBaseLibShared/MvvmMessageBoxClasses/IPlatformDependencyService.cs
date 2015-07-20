using System;
using System.Threading.Tasks;

using ActiveDevelop.MvvmBaseLib.Mvvm;
using System.IO;
using System.Collections.Generic;

namespace ActiveDevelop.MvvmBaseLib
{
    public interface IPlatformDependencyService
	{

		MvvmDialogResult ShowMessageBox(string text, string caption = null, MvvMessageBoxButtons buttons = MvvMessageBoxButtons.OK, MvvmMessageBoxDefaultButton defaultButton = MvvmMessageBoxDefaultButton.Button1, MvvmMessageBoxIcon icon = MvvmMessageBoxIcon.None);

		Task<MvvmDialogResult> ShowMessageBoxAsync(string text, string caption = null, MvvMessageBoxButtons buttons = MvvMessageBoxButtons.OK, MvvmMessageBoxDefaultButton defaultButton = MvvmMessageBoxDefaultButton.Button1, MvvmMessageBoxIcon icon = MvvmMessageBoxIcon.None);
		Task<MvvmDialogResult> ShowDialogAsync<t>(t viewModel, string dialogTitel = null, MvvmDialogType DialogType = MvvmDialogType.Default, Func<Task<bool>> validationCallbackAsync = null, object parameters = null) where t: BindableBase;
		MvvmDialogResult ShowDialog<t>(t viewModel, string dialogTitel = null, MvvmDialogType DialogType = MvvmDialogType.Default, Func<Task<bool>> validationCallbackAsync = null, object parameters = null) where t: BindableBase;
		Task NavigateToAsync<t>(t viewModel, string pageTitel = null) where t: MvvmBase;
        Task<string> SaveStringToPickableFileAsync(string pickerTitel, IDictionary<String, IList<String>> defaultExtensions,
                                                   string commitButtonText, String data );
        Task<string> SaveStringToFileAsync(string filename, string data);

		bool RequestAppBarOpen();
		bool RequestAppBarClose();
		bool IsDesignMode {get;}

	}

}