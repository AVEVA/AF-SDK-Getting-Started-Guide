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
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex2SearchingForAssetsSln
{
    public static class Program2
    {
        private static IConfiguration _config;

        public static string AFServer { get; set; }
        public static string Database { get; set; }

        static void Main()
        {
            Setup();
            AFDatabase database = GetDatabase(AFServer, Database);

            if (database == null) throw new NullReferenceException("Database is null");

            FindMetersByName(database, "Meter00*");
            FindMetersByTemplate(database, "MeterBasic");
            FindMetersBySubstation(database, "SSA*");
            FindMetersAboveUsage(database, 300);
            FindBuildingInfo(database, "MeterAdvanced");

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

        public static void FindMetersByName(AFDatabase database, string elementNameFilter)
        {
            Console.WriteLine("Find Meters by Name: {0}", elementNameFilter);

            // Default search is as an element name string mask.
            var queryString = $"\"{elementNameFilter}\"";
            using (AFElementSearch elementQuery = new AFElementSearch(database, "ElementSearch", queryString))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                foreach (AFElement element in elementQuery.FindObjects())
                {
                    Console.WriteLine("Element: {0}, Template: {1}, Categories: {2}",
                        element.Name,
                        element.Template.Name,
                        element.CategoriesString);
                }
            }
            Console.WriteLine();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void FindMetersByTemplate(AFDatabase database, string templateName)
        {
            Console.WriteLine("Find Meters by Template: {0}", templateName);

            using (AFElementSearch elementQuery = new AFElementSearch(database, "TemplateSearch", string.Format("template:\"{0}\"", templateName)))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                int countDerived = 0;
                foreach (AFElement element in elementQuery.FindObjects())
                {
                    Console.WriteLine("Element: {0}, Template: {1}", element.Name, element.Template.Name);
                    if (element.Template.Name != templateName)
                        countDerived++;
                }

                Console.WriteLine("   Found {0} derived templates", countDerived);
                Console.WriteLine();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void FindMetersBySubstation(AFDatabase database, string substationLocation)
        {
            Console.WriteLine("Find Meters by Substation: {0}", substationLocation);

            string templateName = "MeterBasic";
            string attributeName = "Substation";
            using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeValueEQSearch",
                string.Format("template:\"{0}\" \"|{1}\":\"{2}\"", templateName, attributeName, substationLocation)))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                int countNames = 0;
                foreach (AFElement element in elementQuery.FindObjects())
                {
                    Console.Write("{0}{1}", countNames++ == 0 ? string.Empty : ", ", element.Name);
                }

                Console.WriteLine("");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void FindMetersAboveUsage(AFDatabase database, double val)
        {
            Console.WriteLine("Find Meters above Usage: {0}", val);

            string templateName = "MeterBasic";
            string attributeName = "Energy Usage";
            using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeValueGTSearch",
                string.Format("template:\"{0}\" \"|{1}\":>{2}", templateName, attributeName, val)))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                int countNames = 0;
                foreach (AFElement element in elementQuery.FindObjects())
                {
                    Console.Write("{0}{1}", countNames++ == 0 ? string.Empty : ", ", element.Name);
                }

                Console.WriteLine("");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void FindBuildingInfo(AFDatabase database, string templateName)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            Console.WriteLine("Find Building Info: {0}", templateName);

            AFCategory buildingInfoCat = database.AttributeCategories["Building Info"];
            AFNamedCollectionList<AFAttribute> foundAttributes = new AFNamedCollectionList<AFAttribute>();

            using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeCattegorySearch", string.Format("template:\"{0}\"", templateName)))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                foreach (AFElement element in elementQuery.FindObjects())
                {
                    foreach (AFAttribute attr in element.Attributes)
                    {
                        if (attr.Categories.Contains(buildingInfoCat))
                        {
                            foundAttributes.Add(attr);
                        }
                    }
                }
            }
            Console.WriteLine("Found {0} attributes.", foundAttributes.Count);
            Console.WriteLine();
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
