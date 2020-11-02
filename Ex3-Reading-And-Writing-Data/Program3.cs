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
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Search;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ex3ReadingAndWritingData
{
    public static class Program3
    {
        private static IConfiguration _config;

        public static string AFServer { get; set; }
        public static string Database { get; set; }

        static void Main()
        {
            Setup();
            AFDatabase database = GetDatabase(AFServer, Database);

            PrintHistorical(database, "Meter001", "*-30s", "*");
            PrintInterpolated(database, "Meter001", "*-30s", "*", TimeSpan.FromSeconds(10));
            PrintHourlyAverage(database, "Meter001", "y", "t");
            PrintEnergyUsageAtTime(database, "t+10h");
            PrintDailyAverageEnergyUsage(database, "t-7d", "t");
            SwapValues(database, "Meter001", "Meter002", "y", "y+1h");

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void PrintHistorical(AFDatabase database, string meterName, string startTime, string endTime)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            Console.WriteLine(string.Format("Print Historical Values - Meter: {0}, Start: {1}, End: {2}", meterName, startTime, endTime));

            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", database);

            AFTime start = new AFTime(startTime);
            AFTime end = new AFTime(endTime);
            AFTimeRange timeRange = new AFTimeRange(start, end);
            AFValues vals = attr.Data.RecordedValues(
                timeRange: timeRange,
                boundaryType: AFBoundaryType.Inside,
                desiredUOM: database.PISystem.UOMDatabase.UOMs["kilojoule"],
                filterExpression: null,
                includeFilteredValues: false);

            foreach (AFValue val in vals)
            {
                Console.WriteLine("Timestamp (UTC): {0}, Value (kJ): {1}", val.Timestamp.UtcTime, val.Value);
            }

            Console.WriteLine();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintInterpolated(AFDatabase database, string meterName, string startTime, string endTime, TimeSpan timeSpan)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", database);

            // Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintHourlyAverage(AFDatabase database, string meterName, string startTime, string endTime)
        {
            AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", database);

            // Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintEnergyUsageAtTime(AFDatabase database, string timeStamp)
        {
            Console.WriteLine("Print Energy Usage at Time: {0}", timeStamp);
            AFAttributeList attrList = new AFAttributeList();

            // Use this method if you get stuck trying to find attributes
            // attrList = GetAttributes();

            // Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void PrintDailyAverageEnergyUsage(AFDatabase database, string startTime, string endTime)
        {
            Console.WriteLine(string.Format("Print Daily Energy Usage - Start: {0}, End: {1}", startTime, endTime));
            AFAttributeList attrList = new AFAttributeList();

            // Use this method if you get stuck trying to find attributes
            // attrList = GetAttributes();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void SwapValues(AFDatabase database, string meter1, string meter2, string startTime, string endTime)
        {
            Console.WriteLine(string.Format("Swap values for meters: {0}, {1} between {2} and {3}", meter1, meter2, startTime, endTime));
            // Your code here
        }


        // Helper method used in PrintEnergyUsageAtTime() and PrintDailyAverageEnergyUseage 
        // Note that this is an optional method, it is used in the solutions, but it is possible
        // to get a valid solution without using this method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static AFAttributeList GetAttributes(AFDatabase database, string templateName, string attributeName)
        {
            AFAttributeList attrList = new AFAttributeList();

            using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeSearch", string.Format("template:\"{0}\"", templateName)))
            {
                elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
                foreach (AFElement element in elementQuery.FindObjects())
                {
                    foreach (AFAttribute attr in element.Attributes)
                    {
                        if (attr.Name.Equals(attributeName))
                        {
                            attrList.Add(attr);
                        }
                    }
                }
            }

            return attrList;
        }

        static void Setup()
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
