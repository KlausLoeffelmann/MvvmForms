using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActiveDevelop.MvvmBaseLib.Mvvm;

namespace MvvmFormsCSharpDemos.RxCollectionView
{
    public partial class frmRxCollectionView : Form
    {
        ObservableCollection<SimpleContact> myDemoData;
        ObservableCollection<TinyContact> myResultingFilter1Collection;

        RxCollectionView<SimpleContact, TinyContact> myFilter1;

        public frmRxCollectionView()
        {
            InitializeComponent();
            myDemoData = SimpleContact.GetDemoData();

            myFilter1 = new RxCollectionView<SimpleContact, TinyContact>(myDemoData);
            myFilter1.Query = from item in myFilter1.Source
                              where item.City.Equals("Lippstadt")
                              select new TinyContact
                              {
                                  FirstName = item.FirstName,
                                  LastName = item.LastName
                              };
            
            myResultingFilter1Collection = myFilter1.ResultingCollection;
            myResultingFilter1Collection.CollectionChanged += MyResultingFilter1Collection_CollectionChanged;
        }

        private void MyResultingFilter1Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                Debug.Print(item.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myDemoData.Add(new SimpleContact { LastName = "Belke", FirstName = "Andreas", Address = "Rüdenkuhle", City = "München", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            myDemoData.Add(new SimpleContact { LastName = "Lippmann", FirstName = "Andreas", Address = "Rüdenkuhle", City = "Lippstadt", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
        }

    }
}

