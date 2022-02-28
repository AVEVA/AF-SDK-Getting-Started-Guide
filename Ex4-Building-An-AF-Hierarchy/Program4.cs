using System;
using System.IO;
using System.Text.Json;
using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace Ex4BuildingAnAFHierarchy
{
    public static class Program4
    {
        public static string AFServer { get; set; }
        public static string DatabaseString { get; set; }

        public static void Main()
        {
            Setup();
            AFDatabase database = GetDatabase(AFServer, DatabaseString);

            if (database == null) throw new NullReferenceException("Database is null");

            CreateElementTemplate(database);
            CreateFeedersRootElement(database);
            CreateFeederElements(database);
            CreateWeakReference(database);

            Console.WriteLine("Press ENTER key to close");
            Console.ReadLine();
        }

        public static AFDatabase GetDatabase(string serverName, string databaseName)
        {
            PISystems systems = new PISystems();
            PISystem assetServer;

            if (!string.IsNullOrEmpty(serverName))
                assetServer = systems[serverName];
            else
                assetServer = systems.DefaultPISystem;

            if (!string.IsNullOrEmpty(databaseName))
                return assetServer.Databases[databaseName];
            else
                return assetServer.Databases.DefaultDatabase;
        }

        public static void CreateElementTemplate(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            string templateName = "FeederTemplate";
            AFElementTemplate feederTemplate;
            if (database.ElementTemplates.Contains(templateName))
                return;
            else
                feederTemplate = database.ElementTemplates.Add(templateName);

            AFAttributeTemplate cityattributeTemplate = feederTemplate.AttributeTemplates.Add("City");
            cityattributeTemplate.Type = typeof(string);

            AFAttributeTemplate power = feederTemplate.AttributeTemplates.Add("Power");
            power.Type = typeof(Single);

            power.DefaultUOM = database.PISystem.UOMDatabase.UOMs["watt"];
            power.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];

            database.CheckIn();
        }

        public static void CreateFeedersRootElement(AFDatabase database)
        {
            // Your code here
        }

        public static void CreateFeederElements(AFDatabase database)
        {
            // Your code here
        }

        public static void CreateWeakReference(AFDatabase database)
        {
            // Your code here
        }

        public static void Setup()
        {
            AppSettings settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(Directory.GetCurrentDirectory() + "/appsettings.json"));

            // ==== Client constants ====
            AFServer = settings.AFServerName;
            DatabaseString = settings.AFDatabaseName;
        }
    }
}
