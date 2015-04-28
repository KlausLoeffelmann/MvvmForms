using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MRWebApiSelfHost.DataLayer.DataAccess;

namespace MRWebApiSelfHost.Controller
{
    public class ContactController : ApiController
    {
        [Route("api/contact/getallcontacts"), HttpGet]
        public async Task<IHttpActionResult> GetAllContacts()
        {
            var contactData = new ContactData();
            return Ok(await contactData.GetAllContacts());
        }
    }
}
