using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MvvmCalculator
{
    public partial class FunctionPlotterView : Form
    {
        public FunctionPlotterView()
        {
            InitializeComponent();
        }
        public object DataContext
        {
            get { return mvvmManager1.DataContext; }
            set { mvvmManager1.DataContext = value; }
        }
    }
}
