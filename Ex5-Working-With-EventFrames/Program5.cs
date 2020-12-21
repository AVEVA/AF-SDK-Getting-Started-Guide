using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;
using OSIsoft.AF.Time;

namespace Ex5WorkingWithEventFrames
{
    public static class Program5
    {
        private static IConfiguration _config;

        public static string AFServer { get; set; }
        public static string Database { get; set; }

        static void Main()
        {
            Setup();
            AFDatabase database = GetDatabase(AFServer, Database);

            if (database == null) throw new NullReferenceException("Database is null");

            AFElementTemplate eventFrameTemplate = CreateEventFrameTemplate(database);
            CreateEventFrames(database, eventFrameTemplate);
            CaptureValues(database, eventFrameTemplate);
            PrintReport(database, eventFrameTemplate);

            Console.WriteLine("Press ENTER key to close");
            Console.ReadLine();
        }

        static AFDatabase GetDatabase(string serverName, string databaseName)
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

        public static AFElementTemplate CreateEventFrameTemplate(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            AFElementTemplate eventFrameTemplate = database.ElementTemplates["Daily Usage"];
            if (eventFrameTemplate != null)
                return eventFrameTemplate;

            eventFrameTemplate = database.ElementTemplates.Add("Daily Usage");
            eventFrameTemplate.InstanceType = typeof(AFEventFrame);
            eventFrameTemplate.NamingPattern = @"%TEMPLATE%-%ELEMENT%-%STARTTIME:yyyy-MM-dd%-EF*";

            AFAttributeTemplate usage = eventFrameTemplate.AttributeTemplates.Add("Average Energy Usage");
            usage.Type = typeof(Single);
            usage.DataReferencePlugIn = AFDataReference.GetPIPointDataReference();
            usage.ConfigString = @".\Elements[.]|Energy Usage;TimeRangeMethod=Average";
            usage.DefaultUOM = database.PISystem.UOMDatabase.UOMs["kilowatt hour"];

            if (database.IsDirty)
                database.CheckIn();

            return eventFrameTemplate;
        }

        public static void CreateEventFrames(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (eventFrameTemplate == null) throw new ArgumentNullException(nameof(eventFrameTemplate));
            // Your code here
        }

        public static void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (eventFrameTemplate == null) throw new ArgumentNullException(nameof(eventFrameTemplate));
            // Your code here
        }

        public static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (eventFrameTemplate == null) throw new ArgumentNullException(nameof(eventFrameTemplate));
            // Your code here
        }

        public static void Setup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _config = builder.Build();

            // ==== Client constants ====
            AFServer = _config["AFServer"];
            Database = _config["Database"];
        }
    }
}
