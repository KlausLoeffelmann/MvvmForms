namespace MvvmFormsCSharpDemos
{
    using System;
    using System.Collections.Generic;

    public class Product
    {

        public int IDPurchasedBy { get; set; }
        public string ProductName { get; set; }
        public string ProductNo { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }

        public override string ToString()
        {
            return this.ProductNo + ": " + this.ProductName;
        }

        public static List<Product> RandomProducts(List<Contact> Kontakte)
        {

            Random tmpRandom = new Random(42);
            List<Product> tmpListOfProducts = new List<Product>();

            string[] tmpProductMasterData = {
            "DVD|Catch me if you can|1-234",
            "DVD/Blue Ray|Being John Malkovich|2-134",
            "DVD/Blue Ray|Bodyguard|3-123",
            "DVD/Blue Ray|Castaway|9-646",
            "DVD/Blue Ray|The Maiden Heist|3-534",
            "DVD/Blue Ray|Transporter 3|4-324",
            "DVD/Blue Ray|The Social Network|9-423",
            "DVD/Blue Ray|Runaway Jury|5-554",
            "DVD/Blue Ray|24 - Season 7|2-424",
            "Books, IT|Parallel Programming with Microsoft Visual Studio 2010 Step by Step|5-506",
            "Books, IT|Visual Basic 2010 - Developer's Handbook|5-506",
            "Books, IT|Microsoft Visual C# 2010 - Developer's Handbook|3-543",
            "Books, IT|How We Test Software at Microsoft|5-401",
            "Books, IT|Microsoft SQL Server 2008 R2 - Developer's Handbook|5-513",
            "Audibooks|Harry Potter and the Deathly Hallows| 4-444",
            "Audibooks|The Jungle Book|2-321",
            "Audibooks|A tale of two cities|9-009",
            "Audibooks|Pride and prejudice|7-321",
            "Books, Novels|Eclipse (The Twilight Saga, Book 3)|9-445",
            "Books, Novels|The Cathedral of the Sea|5-436",
            "Books, Novels|The Da Vinci Code|4-444",
            "Books, Novels|Der Schwarm (German Edition)|3-333",
            "Books, Novels|The Rose Killer|6-666"
        };

            Product tmpProduct = null;

            //Everybody purchased something! :-)
            foreach (var adrItem_loopVariable in Kontakte)
            {
                var adrItem = adrItem_loopVariable;
                //Every customer purchased between one and 20 products.
                for (int anzahlGekaufterArtikel = 1;
                            anzahlGekaufterArtikel <= tmpRandom.Next(1, 10);
                            anzahlGekaufterArtikel++)
                {
                    tmpProduct = new Product();

                    var tmpStr = tmpProductMasterData[tmpRandom.Next(0, tmpProductMasterData.Length - 1)].Split('|');

                    tmpProduct.IDPurchasedBy = adrItem.IDContact;
                    tmpProduct.ProductName = tmpStr[1];
                    tmpProduct.ProductNo = tmpStr[2];
                    tmpProduct.Amount = tmpRandom.Next(1, 4);
                    tmpProduct.UnitPrice = (tmpRandom.Next(1, 20) * 5) - 0.05m;
                    tmpProduct.Category = tmpStr[0];
                    tmpListOfProducts.Add(tmpProduct);
                }
            }
            return tmpListOfProducts;
        }
    }

    public class Address
    {

        public Address(string Straße, string Plz, string Ort)
        {
            this.Street = Straße;
            this.ZIP = Plz;
            this.City = Ort;
        }

        public string Street { get; set; }
        public string ZIP { get; set; }
        public virtual string City { get; set; }

        public override string ToString()
        {
            return ZIP + ": " + City + " - " + Street;
        }

        public static List<Address> RandomAddresses(int Count)
        {

            List<Address> tmpListOfAddresses = new List<Address>();
            Random tmpRandom = new Random(42);

            string[] tmpStreetNames = {
            "Wiedenbrückerstr.",
            "Stauffenberg Ave.",
            "Broadway",
            "Parkstr.",
            "Kurgartenweg",
            "Alter Postweg",
            "Long Turnpike",
            "Zzyzx Rd.",
            "Main Street",
            "Streetway",
            "Postplatz",
            "Beamer Place",
            "Mercedes Way",
            "Porsche Drive",
            "Weidering",
            "One Way",
            "Endof Rd.",
            "Gotlost Way",
            "Satnav Rd."
        };

            string[] tmpCities = {
            "Bellevue",
            "Dortmund",
            "Lippstadt",
            "Redmond",
            "Los Angeles",
            "Las Vegas",
            "Seattle",
            "New York",
            "Berlin",
            "Bielefeld",
            "Braunschweig",
            "Munich",
            "Cologne",
            "Hamburg",
            "Bad Waldliesborn",
            "Bremen",
            "Encinitas",
            "Anaheim"
        };

            for (int i = 1; i <= Count; i++)
            {
                tmpListOfAddresses.Add(new Address(tmpStreetNames[tmpRandom.Next(tmpStreetNames.Length - 1)],
                    tmpRandom.Next(99999).ToString("00000"),
                    tmpCities[tmpRandom.Next(tmpCities.Length - 1)]));
            }
            return tmpListOfAddresses;
        }
    }

    public class Contact
    {

        public Contact(int ID, string Name, string Vorname, string Straße, string Plz, string Ort)
        {
            this.IDContact = ID;
            this.LastName = Name;
            this.FirstName = Vorname;
            this.Street = Straße;
            this.ZIP = Plz;
            this.City = Ort;
        }

        public int IDContact { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public string ZIP { get; set; }
        public virtual string City { get; set; }

        public override string ToString()
        {
            return "\"" + LastName + ", " + FirstName + "\"";
        }

        public static List<Contact> RandomContacts(int Count)
        {

            List<Contact> tmpListOfAddresses = new List<Contact>();
            Random tmpRandom = new Random(42);

            string[] tmpLastNames = {
            "Heckhuis",
            "Löffelmann",
            "Jones",
            "Lowel",
            "Ardelean",
            "Beckham",
            "Baur",
            "Picard",
            "Trouv",
            "Feigenbaum",
            "Miller",
            "Wallace",
            "Merkel",
            "Spooner",
            "Spoonman",
            "Huffman",
            "Rode",
            "Trouw",
            "Schindler",
            "Brown",
            "Walker",
            "Cruise",
            "Meier",
            "Maier",
            "Mayer",
            "Tinoco",
            "O'Reilly",
            "O'Donnell",
            "Ó Briain",
            "Russel",
            "English",
            "Clarke",
            "Schumacher"
        };

            string[] tmpStreetNames = {
            "Wiedenbrückerstr.",
            "Stauffenberg Ave.",
            "Broadway",
            "Parkstr.",
            "Kurgartenweg",
            "Alter Postweg",
            "Long Turnpike",
            "Zzyzx Rd.",
            "Main Street",
            "Streetway",
            "Postplatz",
            "Beamer Place",
            "Mercedes Way",
            "Porsche Drive",
            "Weidering",
            "One Way",
            "Endof Rd.",
            "Gotlost Way",
            "Satnav Rd."
        };

            string[] tmpFirstNames = {
            "Jürgen",
            "Gabriele",
            "Dianne",
            "Katrin",
            "Jack",
            "Arnold",
            "Christian",
            "Frank",
            "Curt",
            "Peter",
            "Anne",
            "Anja",
            "Theo",
            "Bob",
            "Katrin",
            "Guido",
            "Barbara",
            "Bernhard",
            "Margarete",
            "Alfred",
            "Melanie",
            "Britta",
            "José",
            "Thomas",
            "Dara",
            "Klaus",
            "Axel",
            "Gabby",
            "Gareth",
            "Bob",
            "Denise",
            "Kristen"
        };

            string[] tmpCities = {
            "Bellevue",
            "Dortmund",
            "Lippstadt",
            "Redmond",
            "Los Angeles",
            "Las Vegas",
            "Seattle",
            "New York",
            "Berlin",
            "Bielefeld",
            "Braunschweig",
            "Munich",
            "Cologne",
            "Hamburg",
            "Bad Waldliesborn",
            "Bremen",
            "Encinitas",
            "Anaheim"
        };

            for (int i = 1; i <= Count; i++)
            {
                string tmpLastName = null;
                string tmpFirstName = null;
                tmpLastName = tmpLastNames[tmpRandom.Next(tmpLastNames.Length - 1)];
                tmpFirstName = tmpFirstNames[tmpRandom.Next(tmpLastNames.Length - 1)];
                tmpListOfAddresses.Add(new Contact(i, tmpLastName, tmpFirstName,
                    tmpStreetNames[tmpRandom.Next(tmpStreetNames.Length - 1)],
                    tmpRandom.Next(99999).ToString("00000"),
                    tmpCities[tmpRandom.Next(tmpCities.Length - 1)]));
            }
            return tmpListOfAddresses;
        }

        public static void PrintContacts(List<Contact> Contacts)
        {
            //Option Infer ist 'On', deswegen wird
            //Item automatisch zum Typ 'Adresse'
            foreach (var Item_loopVariable in Contacts)
            {
                var Item = Item_loopVariable;
                Console.WriteLine(Item);
            }
        }

    }
}


