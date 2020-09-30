#region Copyright
//  Copyright 2016  OSIsoft, LLC
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
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using System.Collections.Generic;

namespace Ex4BuildingAnAFHierarchy
{
    public static class Bonus
    {
        public static void Run()
        {
            Program4.Setup();

            AFDatabase database = GetOrCreateDatabase(Program4.AFServer, "Ethical Power Company");

            CreateCategories(database);
            CreateEnumerationSets(database);
            CreateTemplates(database);
            CreateElements(database);
            SetAttributeValues(database);
            CreateCityElements(database);
            CreateWeakReferences(database);
        }

        static AFDatabase GetOrCreateDatabase(string servername, string databasename)
        {
            AFDatabase database = null;
            // Your code here

            return database;
        }

        static void CreateCategories(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateEnumerationSets(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        private static void CreateTemplates(AFDatabase database)
        {
            if (database == null) return;
            // Your code here

        }

        // Helper method for CreateTemplates
        private static AFElementTemplate CreateMeterBasicTemplate(AFDatabase database)
        {
            AFElementTemplate meterBasicTemplate = database.ElementTemplates["MeterBasic"];
            if (meterBasicTemplate != null)
                return meterBasicTemplate;

            // Your code here

            return meterBasicTemplate;
        }

        private static void CreateMeterAdvancedTemplate(AFElementTemplate meterBasicTemplate)
        {
            // Your code here
        }

        private static void CreateCityTemplate(AFDatabase database)
        {
            // Your code here
        }

        static void CreateElements(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void SetAttributeValues(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateCityElements(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }

        static void CreateWeakReferences(AFDatabase database)
        {
            if (database == null) return;
            // Your code here
        }
    }
}
