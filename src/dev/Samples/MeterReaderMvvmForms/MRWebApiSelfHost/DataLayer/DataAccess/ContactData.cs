using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MrModelLibrary.DataObjects;

namespace MRWebApiSelfHost.DataLayer.DataAccess
{
    public class ContactData
    {
        public async Task<IEnumerable<ContactItem>> GetAllContacts()
        {
            MRModel context = new MRModel();

            var toReturn = await (from item in context.ContactItems
                                  orderby item.Lastname ascending
                                  select item).ToListAsync();

            return toReturn;
        }

        public async Task<ContactItem> GetContactForBuilding(BuildingItem buildingItem)
        {
            MRModel context = new MRModel();

            var toReturn = await (from item in context.ContactItems
                                  where item.id == buildingItem.Owner.id
                                  select item).SingleOrDefaultAsync();
            return toReturn;
        }
    }
}
