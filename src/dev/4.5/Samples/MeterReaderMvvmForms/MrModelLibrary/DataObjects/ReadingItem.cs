using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRWebApiSelfHost.DataLayer.DataObjects
{
    public class ReadingItem
    {
        public Guid id { get; set; }
        public MeterItem Meter { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset ReadingDate { get; set; }
    }
}
