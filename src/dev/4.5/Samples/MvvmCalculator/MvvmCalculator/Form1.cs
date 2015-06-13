using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActiveDevelop.EntitiesFormsLib;
using MvvmCalculatorVMLib;

namespace MvvmCalculator
{
    public partial class Form1 : Form
    {

        private MainViewModel myViewModel = new MainViewModel();

        public Form1()
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
    }
}
