using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace MRWebApiSelfHost
{
    class ServiceController
    {
        private IDisposable myWebApp;
        private const string BASE_ADDRESS = "Http://localhost:9000/";

        public void Start()
        {
            myWebApp=WebApp.Start<Startup>(BASE_ADDRESS);
        }

        public void Stop()
        {
            if (myWebApp != null)
                myWebApp.Dispose();
        }
    }
}
