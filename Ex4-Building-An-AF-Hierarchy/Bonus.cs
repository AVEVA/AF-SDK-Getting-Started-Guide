
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
