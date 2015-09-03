using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Autofac;
using MvvmCalculatorVMLib;
using WinForms.MvvmServiceLocatorBase;

namespace MvvmCalculator
{
    public partial class MainView : Form
    {

        private MainViewModel myViewModel;

        public MainView()
        {
            InitializeComponent();
            RegisterTypes(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //myViewModel= new MainViewModel();
            var scope = WinFormsMvvmPlatformServiceLocator.Container.BeginLifetimeScope();
            myViewModel = scope.Resolve<MainViewModel>();
            this.mvvmManager1.DataContext = myViewModel;
        }

        private void RegisterTypes(Form mainWindow)
        {
            WinFormsMvvmPlatformServiceLocator.
            BeginRegister().
            RegisterPlatformServiceLocator(mainWindow).
            RegisterTypes(new List<Type>{ typeof(MainViewModel)}).
            EndRegister();

            WinFormsMvvmPlatformServiceLocator.ViewToPageResolver =
                (INotifyPropertyChanged viewModel) =>
                {
                    var vmType = viewModel.GetType();

                    if (viewModel.GetType() == typeof(MainViewModel)) return typeof(MainView);
                    if (viewModel.GetType() == typeof(MainViewModel)) return typeof(MainView);

                    return null;

                };

        }
    }
}
