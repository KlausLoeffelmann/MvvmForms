using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MRWebApiSelfHost.DataLayer.DataAccess;

namespace MRWebApiSelfHost.Controller
{
    public class MeterController : ApiController
    {
        [Route("api/meter/getmetersforbuilding"), HttpGet]
        public async Task<IHttpActionResult> GetMetersForBuilding(Guid idBuilding)
        {
            var meterData = new MeterData();
            return Ok(await meterData.GetMetersForBuilding(idBuilding));
        }
    }
}
