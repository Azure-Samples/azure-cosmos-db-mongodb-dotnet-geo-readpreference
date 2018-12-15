using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace GeoSampleCSharp
{
    class GeoSample
    {
        public Random RndGenerator;

        public List<String> names;

        public const int InsertBatchSize = 10;

        GeoSample()
        {
            RndGenerator = new Random();
            names = new List<String>();
            names.Add("Value1");
            names.Add("Value2");
            names.Add("Value3");
        }

        /*
         * Sample document class with some fields
         */
        public class Doc
        {
            public ObjectId Id { get; set; }

            public String LastName { get; set; } 

            public int Age { get; set; } 

            public String FirstName { get; set; }
        }

        Doc GetDoc()
        {
            Doc aDoc = new Doc();
            aDoc.Id = new ObjectId();
            aDoc.LastName = names[RndGenerator.Next(0, 3)];
            aDoc.Age = RndGenerator.Next(1, 101);
            return aDoc;
        }

        //helper to create client
        MongoClient GetMongoClient(string connectionString)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            return mongoClient;
        }

        //function to create initial setup: db, collection
        IMongoCollection<Doc> GetCollection(string dbname, string collname, MongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(dbname);
            IMongoCollection<Doc> coll = db.GetCollection<Doc>(collname);
            return coll;
        }

        //function to insert docs
        async Task InsertDocsAsync(IMongoCollection<Doc> coll)
        {

            Console.WriteLine("Insert docs started");

            for (int i = 0; i < GeoSample.InsertBatchSize; i++)
            {
                await coll.InsertOneAsync(GetDoc());
                Console.WriteLine("Inserts completed: " + i);
            }


            Console.WriteLine("Insert complete");
        }

        //function to read docs
        async Task ReadDocsAsync(IMongoCollection<Doc> coll, ReadPreference readPref)
        {

            Console.WriteLine("Reading docs started with ReadPreference: "+readPref.ToString());

            var result = await coll.WithReadPreference(readPref).FindAsync<Doc>(FilterDefinition<Doc>.Empty);
            int rCount = result.ToList<Doc>().Count;


            Console.WriteLine("Reading docs complete. Found: "+rCount+" docs.");
        }

        async Task RunAsync()
        {
            string connStr = ConfigurationSettings.AppSettings["connectionString"];
            string readRegion = ConfigurationSettings.AppSettings["readTargetRegion"];
            
            Console.WriteLine("Started Running Sample Against: "+ connStr);
            string dbname = "testdb";
            string collname = "testcoll";
            MongoClient client = GetMongoClient(connStr);

            IMongoCollection<Doc> coll = GetCollection(dbname, collname, client);

            await InsertDocsAsync(coll);

            await Task.Delay(5000);

            //Reading from Write region if it is available, otherwise fallback to Read region
            await ReadDocsAsync(coll, ReadPreference.PrimaryPreferred);

            //Reading from Secondary region if it is available, otherwise fallback to Write region
            await ReadDocsAsync(coll, ReadPreference.SecondaryPreferred);

            //Reading from Nearest region
            await ReadDocsAsync(coll, ReadPreference.Nearest);

            //Read using tags
            Tag tg = new Tag("region", readRegion);
            List<Tag> tgs = new List<Tag>();
            tgs.Add(tg);
            List<TagSet> tgSets = new List<TagSet>();
            tgSets.Add(new TagSet(tgs));
            ReadPreference rp = new ReadPreference(ReadPreferenceMode.SecondaryPreferred, tgSets);

            await ReadDocsAsync(coll, rp);
            Console.WriteLine("Sample run completed.");
        }

        static void Main(string[] args)
        {
            GeoSample sample = new GeoSample();
            sample.RunAsync().Wait();
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }
    }
}
