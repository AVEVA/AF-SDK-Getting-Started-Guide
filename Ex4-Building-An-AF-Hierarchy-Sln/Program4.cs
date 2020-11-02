#region Copyright
//  Copyright 2016, 2017  OSIsoft, LLC
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
#endregion
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using OSIsoft.AF;
using OSIsoft.AF.Asset;

namespace Ex4BuildingAnAFHierarchySln
{
    public static class Program4
    {
        private static IConfiguration _config;

        public static string AFServer { get; set; }
        public static string Database { get; set; }

        static void Main()
        {
            Setup();
            AFDatabase database = GetDatabase(AFServer, Database);

            CreateElementTemplate(database);
            CreateFeedersRootElement(database);
            CreateFeederElements(database);
            CreateWeakReference(database);

            Console.WriteLine("Completed - Press ENTER key to close");
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

        public static void CreateFeedersRootElement(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            Console.WriteLine("Creating the Feeders root Element");
            if (database.Elements.Contains("Feeders"))
                return;

            database.Elements.Add("Feeders");
            database.CheckIn();
        }

        public static void CreateElementTemplate(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            Console.WriteLine("Creating the element template: FeederTemplate");
            string templateName = "FeederTemplate";
            AFElementTemplate feederTemplate;
            if (database.ElementTemplates.Contains(templateName))
                return;
            else
                feederTemplate = database.ElementTemplates.Add(templateName);

            AFAttributeTemplate cityAttributeTemplate = feederTemplate.AttributeTemplates.Add("City");
            cityAttributeTemplate.Type = typeof(string);

            AFAttributeTemplate power = feederTemplate.AttributeTemplates.Add("Power");
            power.Type = typeof(Single);

            power.DefaultUOM = database.PISystem.UOMDatabase.UOMs["watt"];
            power.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];

            database.CheckIn();
        }

        public static void CreateFeederElements(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            Console.WriteLine("Creating a feeder element under the Feeders Element");
            AFElementTemplate template = database.ElementTemplates["FeederTemplate"];

            AFElement feeders = database.Elements["Feeders"];
            if (template == null || feeders == null) return;

            if (feeders.Elements.Contains("Feeder001")) return;
            AFElement feeder001 = feeders.Elements.Add("Feeder001", template);

            AFAttribute city = feeder001.Attributes["City"];
            if (city != null) city.SetValue(new AFValue("London"));

            AFAttribute power = feeder001.Attributes["Power"];
            power.ConfigString = @"%@\Configuration|PIDataArchiveName%\SINUSOID";

            if (database.IsDirty)
                database.CheckIn();
        }

        public static void CreateWeakReference(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));

            Console.WriteLine("Adding a week reference of the Feeder001 under London");
            AFReferenceType weakRefType = database.ReferenceTypes["Weak Reference"];

            AFElement london = database.Elements["Geographical Locations"].Elements["London"];
            AFElement feeder0001 = database.Elements["Feeders"].Elements["Feeder001"];
            if (london == null || feeder0001 == null) return;

            if (!london.Elements.Contains(feeder0001))
                london.Elements.Add(feeder0001, weakRefType);
            if (database.IsDirty)
                database.CheckIn();
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
