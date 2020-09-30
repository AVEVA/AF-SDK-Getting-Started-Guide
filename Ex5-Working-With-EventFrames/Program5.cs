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

            AFElementTemplate eventFrameTemplate = CreateEventFrameTemplate(database);
            CreateEventFrames(database, eventFrameTemplate);
            CaptureValues(database, eventFrameTemplate);
            PrintReport(database, eventFrameTemplate);

            Console.WriteLine("Press ENTER key to close");
            Console.ReadLine();
        }

        static AFDatabase GetDatabase(string serverName, string databaseName)
        {
            PISystem assetServer = GetPISystem(null, serverName);
            if (!string.IsNullOrEmpty(databaseName))
                return assetServer.Databases[databaseName];
            else
                return assetServer.Databases.DefaultDatabase;
        }

        static PISystem GetPISystem(PISystems systems = null, string systemName = null)
        {
            systems = systems == null ? new PISystems() : systems;
            if (!string.IsNullOrEmpty(systemName))
                return systems[systemName];
            else
                return systems.DefaultPISystem;
        }

        public static AFElementTemplate CreateEventFrameTemplate(AFDatabase database)
        {
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
            // Your code here
        }

        public static void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
            // Your code here
        }

        public static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
        {
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
