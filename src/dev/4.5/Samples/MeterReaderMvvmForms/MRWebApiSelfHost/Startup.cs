using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;

namespace MRWebApiSelfHost
{
    class Startup
    {
        public void Configuration(IAppBuilder appbuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi",
                                       "api/{controller}/{id}",
                                       new { ID = RouteParameter.Optional });

            //config.Routes.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.
                SerializerSettings.ReferenceLoopHandling =
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            appbuilder.UseWebApi(config);
        }
    }
}
