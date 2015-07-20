using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MrModelLibrary.DataObjects;

namespace MRWebApiSelfHost.DataLayer.DataAccess
{
    class BuildingData
    {
        public async Task<IEnumerable<BuildingItem>> GetAllBuildings()
        {
            MRModel context = new MRModel();

            var toReturn = await (from item in context.BuildingItems
                                  orderby item.BuildYear descending
                                  select item).ToListAsync();

            return toReturn;
        }
    }
}
