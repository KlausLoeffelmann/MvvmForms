using MRWebApiSelfHost.DataLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MRWebApiSelfHost.Controller
{
    class BuildingController : ApiController
    {
        [Route("api/building/getallbuildings"), HttpGet]
        public async Task<IHttpActionResult> GetAllBuildings()
        {
            var buildingData = new BuildingData();
            return Ok(await buildingData.GetAllBuildings());
        }
    }
}
