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
            // To introduce a new ViewModel/View relation...

            // 1. Register the ViewModel with RegisterTypes here, and ...
            WinFormsMvvmPlatformServiceLocator.
            BeginRegister().
            RegisterPlatformServiceLocator(mainWindow).
            RegisterTypes(new List<Type>{ typeof(MainViewModel),
                                          typeof(FunctionPlotterViewModel)}).
            EndRegister();

            WinFormsMvvmPlatformServiceLocator.ViewModelToPageResolver =
                (INotifyPropertyChanged viewModel) =>
                {
                    var vmType = viewModel.GetType();

                    // 2. create the assignment, which ViewModel should return which view ...
                    if (viewModel.GetType() == typeof(MainViewModel)) return typeof(MainView);
                    if (viewModel.GetType() == typeof(FunctionPlotterViewModel)) return typeof(FunctionPlotterView);

                    return null;

                };

        }
    }
}
