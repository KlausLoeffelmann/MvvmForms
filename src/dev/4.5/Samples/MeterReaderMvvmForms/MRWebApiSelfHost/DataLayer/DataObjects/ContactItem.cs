using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRWebApiSelfHost.DataLayer.DataObjects
{
    public class ContactItem
    {
        public Guid id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Phone { get; set; }

    }
}
