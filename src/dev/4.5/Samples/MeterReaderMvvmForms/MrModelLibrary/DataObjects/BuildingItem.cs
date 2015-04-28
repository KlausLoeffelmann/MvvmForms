using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRWebApiSelfHost.DataLayer.DataObjects
{
    public class BuildingItem
    {
        public Guid id { get; set; }
        public int idNum { get; set; }
        public ContactItem Owner { get; set; }
        public int BuildYear { get; set; }
        public string LocationAddressLine1 { get; set;}
        public string LocationAddressLine2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }
}
