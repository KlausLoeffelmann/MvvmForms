using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrModelLibrary.DataObjects;

namespace MRWebApiSelfHost.DataLayer.DataAccess
{
    public class MeterData
    {
        public async Task<IEnumerable<MeterItem>> GetMetersForBuilding(Guid idBuilding)
        {
            MRModel context = new MRModel();

            var toReturn = await (from item in context.MeterItems
                                  where item.BelongsTo.id.Equals(idBuilding)
                                  orderby item.MeterId ascending
                                  select item).ToListAsync();

            return toReturn;
        }
    }
}
