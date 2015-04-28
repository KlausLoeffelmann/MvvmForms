using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRWebApiSelfHost.DataLayer.DataObjects
{
    public class MeterItem
    {
        public Guid id { get; set; }
        public BuildingItem BelongsTo { get; set; }
        public string MeterId { get; set; }
        public string LocationDescription { get; set; }
    }
}
