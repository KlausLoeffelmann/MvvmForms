using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRWebApiSelfHost
{
    public partial class WebApiSelfHostForm : Form
    {
        private const bool AUTO_START_SERVICE = true;

        private ServiceController myServiceController;
        private bool myStarted = false;

        public WebApiSelfHostForm()
        {
            InitializeComponent();
        }

        private void StartWebApiServiceButton_Click(object sender, EventArgs e)
        {
            HandleWebServiceStart();
        }

        private void HandleWebServiceStart()
        {
            if (myStarted)
            {
                StartWebApiServiceButton.Text = "Start Web Api Service";
                myServiceController.Stop();
                myStarted = false;

            }
            else
            {
                StartWebApiServiceButton.Text = "Stop Web Api Service";
                try
                {
                    myServiceController = new ServiceController();
                    myServiceController.Start();
                }
                catch (Exception)
                {
                    throw;
                }
                myStarted = true;
            }
        }

        private void WebApiSelfHostForm_Load(object sender, EventArgs e)
        {
            if (AUTO_START_SERVICE)
                HandleWebServiceStart();
        }

        private void TestEntityButton_Click(object sender, EventArgs e)
        {
            MRModel context = new MRModel();

            var result = from item in context.ContactItems
                         select item;

            context.ContactItems.Add(new DataLayer.DataObjects.ContactItem
            {
                id = Guid.NewGuid(),
                Firstname = "Klaus",
                Lastname = "Löffelmann",
                Phone="+49 2941 910907"
            });

            context.SaveChanges();
        }
    }
}
