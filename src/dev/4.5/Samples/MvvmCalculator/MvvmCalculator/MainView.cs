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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void calcCommandButton_Click(object sender, EventArgs e)
        {
            resultLabel.Text = "This is the result, world.";
        }
    }
}
