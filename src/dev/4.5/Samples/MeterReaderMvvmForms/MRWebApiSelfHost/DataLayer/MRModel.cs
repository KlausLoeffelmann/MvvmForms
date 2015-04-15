namespace MRWebApiSelfHost
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MRWebApiSelfHost.DataLayer.DataObjects;

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
        }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen 
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        public virtual DbSet<ContactItem> ContactItems { get; set; }
        public virtual DbSet<BuildingItem> BuildingItems { get; set; }
        public virtual DbSet<MeterItem> MeterItems { get; set; }
        public virtual DbSet<ReadingItem> ReadingItems { get; set; }
    }
}