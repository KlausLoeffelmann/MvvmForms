using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MvvmFormsCSharpDemos
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rxCollectionViewDemoTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RxCollectionView.frmRxCollectionView frmView = new RxCollectionView.frmRxCollectionView();
            frmView.ShowDialog();
        }

        private void AllowFormulaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numValueField.AllowFormular = AllowFormulaCheckBox.Checked;
        }
    }
}
