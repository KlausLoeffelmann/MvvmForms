using System.Data.Entity;

namespace MRWebApiSelfHost
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MRWebApiSelfHost.DataLayer.DataObjects;
    using System.Collections.Generic;

    public class MRModel : DbContext
    {
        // Der Kontext wurde für die Verwendung einer MRModel-Verbindungszeichenfolge aus der 
        // Konfigurationsdatei ('App.config' oder 'Web.config') der Anwendung konfiguriert. Diese Verbindungszeichenfolge hat standardmäßig die 
        // Datenbank 'MRWebApiSelfHost.MRModel' auf der LocalDb-Instanz als Ziel. 
        // 
        // Wenn Sie eine andere Datenbank und/oder einen anderen Anbieter als Ziel verwenden möchten, ändern Sie die MRModel-Zeichenfolge 
        // in der Anwendungskonfigurationsdatei.
        public MRModel()
            : base("name=MRModel")
        {
            Database.SetInitializer<MRModel>(new MRDBInitializer());
        }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen 
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        public virtual DbSet<ContactItem> ContactItems { get; set; }
        public virtual DbSet<BuildingItem> BuildingItems { get; set; }
        public virtual DbSet<MeterItem> MeterItems { get; set; }
        public virtual DbSet<ReadingItem> ReadingItems { get; set; }
    }

    public class MRDBInitializer : CreateDatabaseIfNotExists<MRModel>
    {
        // Creating sample data.
        protected override void Seed(MRModel context)
        {
            List<ContactItem> sampleContacts = new List<ContactItem>()
            { new ContactItem { id=Guid.NewGuid(), Lastname="Löffelmann", Firstname="Adriana", Phone="00495551212" },
              new ContactItem { id=Guid.NewGuid(), Lastname="Urgien", Firstname="Stephan", Phone="0049555333222" },
              new ContactItem { id=Guid.NewGuid(), Lastname="Grottendieck", Firstname="Urgien", Phone="0049555333222" },
              new ContactItem { id=Guid.NewGuid(), Lastname="Belke", Firstname="Andreas", Phone="0049444333222" }
            };

            var cheffin = sampleContacts.First();

            List<BuildingItem> sampleBuildings = new List<BuildingItem>()
            { new BuildingItem { id=Guid.NewGuid(), idNum=1000, BuildYear=1969, City="Lippstadt", LocationAddressLine1="Bremer Str. 4", Zip="59555",Country="Germany", Owner=cheffin },
              new BuildingItem { id=Guid.NewGuid(), idNum=1010, BuildYear=1938, City="Soest", LocationAddressLine1="Westenhellweg 4", Zip="59494",Country="Germany",Owner=cheffin },
              new BuildingItem { id=Guid.NewGuid(), idNum=4030, BuildYear=1938, City="Pleasanton", LocationAddressLine1="776 Stoneridge Mall Rd.", Zip="CA 94488",Country="USA",Owner=cheffin },
              new BuildingItem { id=Guid.NewGuid(), idNum=1020, BuildYear=1976, City="Paderborn", LocationAddressLine1="An der Synergoge", Zip="34383",Country="Germany",Owner=cheffin }
            };

            List<MeterItem> sampleMeters = new List<MeterItem>()
            {
                new MeterItem { id=Guid.NewGuid(), MeterId="AA365BF4Pow",
                    LocationDescription ="Basement, First Room/left side",
                    BelongsTo=sampleBuildings[0] },
                new MeterItem { id=Guid.NewGuid(), MeterId="DA365BD4Wat",
                    LocationDescription ="Basement, First Room/left side",
                    BelongsTo=sampleBuildings[0] },
                new MeterItem { id=Guid.NewGuid(), MeterId="DC73BF4Pow",
                    LocationDescription ="Basement, last room down hall",
                    BelongsTo=sampleBuildings[1] },
                new MeterItem { id=Guid.NewGuid(), MeterId="FFAB5BF4Pow",
                    LocationDescription ="Basement, last room down hall",
                    BelongsTo=sampleBuildings[1] },
                new MeterItem { id=Guid.NewGuid(), MeterId="3DEF5532Pow",
                    LocationDescription ="Bathroom, first floor",
                    BelongsTo=sampleBuildings[2] },
                new MeterItem { id=Guid.NewGuid(), MeterId="3DEF5533Wat",
                    LocationDescription ="Bathroom, first floor",
                    BelongsTo=sampleBuildings[2] },
            };

            foreach (var item in sampleContacts)
                context.ContactItems.Add(item);

            foreach (var item in sampleBuildings)
                context.BuildingItems.Add(item);

            foreach (var item in sampleMeters)
                context.MeterItems.Add(item);

            base.Seed(context);
        }
    }
}