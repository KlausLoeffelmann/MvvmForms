using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmFormsCSharpDemos
{
    class SimpleContact
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public DateTime DateOfBirth { get; set; }

        public static ObservableCollection<SimpleContact> GetDemoData()
        {
            var ret = new ObservableCollection<SimpleContact>();
            ret.Add(new SimpleContact { LastName = "Löffelmann", FirstName = "Adriana", Address = "Rüdenkuhle", City = "Lippstadt", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Löffelmann", FirstName = "Klaus", Address = "Rüdenkuhle", City = "Soest", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Urgien", FirstName = "Stephan", Address = "Rüdenkuhle", City = "Paderborn", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Belke", FirstName = "Andreas", Address = "Rüdenkuhle", City = "München", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Wischik", FirstName = "Lucian", Address = "Rüdenkuhle", City = "Paderborn", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Müller", FirstName = "Peter", Address = "Rüdenkuhle", City = "München", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Meyer", FirstName = "Peter", Address = "Rüdenkuhle", City = "Lippstadt", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            ret.Add(new SimpleContact { LastName = "Meier", FirstName = "Peter", Address = "Rüdenkuhle", City = "Lippstadt", Zip = "59555", DateOfBirth = new DateTime(1979, 12, 12) });
            return ret;
        }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}: {City}";
        }
    }

    class TinyContact
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public override string ToString()
        {
            return $"{LastName}, {FirstName}";
        }
    }
}
