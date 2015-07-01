using System;
using System.Windows.Forms;
using MvvmCalculatorVMLib;

namespace MvvmCalculator
{
    public partial class MainView : Form
    {

        private MainViewModel myViewModel = new MainViewModel();

        public MainView()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.mvvmManager1.DataContext = myViewModel;
        }
    }
}
