using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActiveDevelop.EntitiesFormsLib;

namespace MvvmFormsCSharpDemos
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            SetupDemoData();
        }

        private void SetupDemoData()
        {
            var myDemoContacts = Contact.RandomContacts(10000);

            
            nullableValueRelationPopup.DataSource = myDemoContacts;
            nullableValueRelationPopup.DisplayMember = "\"{0:0000}: {1}, {2}\",{IDContact},{LastName},{FirstName}";
            nullableValueRelationPopup.SearchPattern = "\"{0:0000}: {1}, {2}, {3}\",{IDContact},{LastName},{FirstName},{City}";
            nullableValueRelationPopup.PreferredVisibleColumnsOnOpen = 4;
            nullableValueRelationPopup.PreferredVisibleRowsOnOpen = 10;
            nullableValueRelationPopup.ValueMember = "IDContact";
            nullableValueRelationPopup.Select();
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

        private void nullableValueRelationPopup1_GetColumnSchema(object sender, ActiveDevelop.EntitiesFormsLib.GetColumnSchemaEventArgs e)
        {
            var fn = new DataGridViewColumnFieldnames();
            fn.Add("LastName", "Lastname");
            fn.Add("FirstName", "Firstname");
            fn.Add("Street", "Address");
            fn.Add("Zip", "Zip");
            fn.Add("City", "City");
            e.SchemaFieldnames = fn;
        }

        private async void aSyncShowDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new TestViewWithFocusableFields();
            commandButton1.Enabled = false;
            await Task.WhenAll(LongLastingOperationAsync(),
                               dialog.ShowDialogAsync());
            commandButton1.Enabled = true;

        }

        private async Task LongLastingOperationAsync()
        {
            await Task.Delay(2000);
            toolStripStatusLabel1.Text = "Long lasting operation done";
        }

        private void nullableOptionButton1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.Print(nullableOptionButton1.Value.Value.ToString());
        }

        private void ncbAllowFormular_CheckedChanged(object sender, EventArgs e)
        {
            if (ncbAllowFormular.Value.HasValue)
                numValueField.AllowFormular = ncbAllowFormular.Value.Value;
        }

        private void nullableCheckBox1_ValueChanged(object sender, ValueChangedEventArgs e)
        {

        }

        private void setDateValueCommandButton_Click(object sender, EventArgs e)
        {
            nullableDateValue1.Value = DateTime.Now.Date;
        }
    }
}
