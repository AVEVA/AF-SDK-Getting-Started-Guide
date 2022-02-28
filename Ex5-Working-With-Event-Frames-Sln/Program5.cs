using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;
using OSIsoft.AF.Time;

namespace Ex5WorkingWithEventFramesSln
{
    public static class Program5
    {
        public static string AFServer { get; set; }
        public static string DatabaseString { get; set; }

        public static void Main()
        {
            Setup();
            AFDatabase database = GetDatabase(AFServer, DatabaseString);

            if (database == null) throw new NullReferenceException("Database is null");

            AFElementTemplate eventFrameTemplate = CreateEventFrameTemplate(database);
            CreateEventFrames(database, eventFrameTemplate);
            CaptureValues(database, eventFrameTemplate);
            PrintReport(database, eventFrameTemplate);

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
            string queryString = "Template:MeterBasic";
            {
                // This method returns the collection of AFBaseElement objects that were created with this template.
                using (AFElementSearch elementQuery = new AFElementSearch(database, "Meters", queryString))
                {
                    DateTime timeReference = DateTime.Today.AddDays(-7);
                    int count = 0;
                    foreach (AFElement meter in elementQuery.FindObjects())
                    {
                        foreach (int day in Enumerable.Range(1, 7))
                        {
                            AFTime startTime = new AFTime(timeReference.AddDays(day - 1));
                            AFTime endTime = new AFTime(startTime.LocalTime.AddDays(1));
                            AFEventFrame ef = new AFEventFrame(database, "*", eventFrameTemplate);
                            ef.SetStartTime(startTime);
                            ef.SetEndTime(endTime);
                            ef.PrimaryReferencedElement = meter;

                            // It is good practice to periodically check in the database
                            if (++count % 500 == 0)
                                database.CheckIn();
                        }
                    }
                }
            }

            if (database.IsDirty)
                database.CheckIn();
        }

        public static void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (eventFrameTemplate == null) throw new ArgumentNullException(nameof(eventFrameTemplate));

            // Formulate search constraints on time and template
            AFTime startTime = DateTime.Today.AddDays(-7);
            string queryString = $"template:\"{eventFrameTemplate.Name}\"";
            using (AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(database, "EventFrame Captures", AFEventFrameSearchMode.ForwardFromStartTime, startTime, queryString))
            {
                eventFrameSearch.CacheTimeout = TimeSpan.FromMinutes(5);
                int count = 0;
                foreach (AFEventFrame item in eventFrameSearch.FindObjects())
                {
                    item.CaptureValues();
                    if ((count++ % 500) == 0)
                        database.CheckIn();
                }

                if (database.IsDirty)
                    database.CheckIn();
            }
        }

        public static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (eventFrameTemplate == null) throw new ArgumentNullException(nameof(eventFrameTemplate));
            AFTime startTime = DateTime.Today.AddDays(-7);
            AFTime endTime = startTime.LocalTime.AddDays(+8); // Or DateTime.Today.AddDays(1);
            string queryString = $"template:'{eventFrameTemplate.Name}' ElementName:Meter003";
            using (AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(database, "EventFrame Captures", AFSearchMode.StartInclusive, startTime, endTime, queryString))
            {
                eventFrameSearch.CacheTimeout = TimeSpan.FromMinutes(5);
                foreach (AFEventFrame ef in eventFrameSearch.FindObjects())
                {
                    Console.WriteLine("{0}, {1}, {2}",
                        ef.Name,
                        ef.PrimaryReferencedElement.Name,
                        ef.Attributes["Average Energy Usage"].GetValue().Value);
                }
            }
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
