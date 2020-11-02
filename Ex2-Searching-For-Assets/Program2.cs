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

namespace Ex2SearchingForAssets
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void FindMetersByTemplate(AFDatabase database, string templateName)
        {
            /// Your code here

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void FindMetersBySubstation(AFDatabase database, string substationLocation)
        {
            /// Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void FindMetersAboveUsage(AFDatabase database, double val)
        {
            /// Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void FindBuildingInfo(AFDatabase database, string templateName)
        {
            /// Your code here
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
