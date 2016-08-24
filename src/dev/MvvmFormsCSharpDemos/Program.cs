using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActiveDevelop.EntitiesFormsLib;

namespace MvvmFormsCSharpDemos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ActiveDevelop.EntitiesFormsLib.NullableControlManager.RequestNullableControlDefaultValue += NullableControlManager_RequestNullableControlDefaultValue;
            Application.Run(new frmMain());
        }

        private static void NullableControlManager_RequestNullableControlDefaultValue(object sender, ActiveDevelop.EntitiesFormsLib.RequestNullableControlDefaultValueEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                if (sender.GetType() == typeof(NullableNumValue))
                {
                    if (e.PropertyName == nameof(NullableNumValue.AllowFormular))
                    {
                        e.Value = false;
                    }
                }
            }
        }
    }
}
