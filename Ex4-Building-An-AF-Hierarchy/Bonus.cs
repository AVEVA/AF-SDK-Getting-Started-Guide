using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

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

        public static AFDatabase GetOrCreateDatabase(string servername, string databasename)
        {
            AFDatabase database = null;

            // Your code here
            return database;
        }

        public static void CreateCategories(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }

        public static void CreateEnumerationSets(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }

        public static void CreateTemplates(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }

        // Helper method for CreateTemplates
        public static AFElementTemplate CreateMeterBasicTemplate(AFDatabase database)
        {
            if (database == null) return null;

            AFElementTemplate meterBasicTemplate = database.ElementTemplates["MeterBasic"];
            if (meterBasicTemplate != null)
                return meterBasicTemplate;

            // Your code here
            return meterBasicTemplate;
        }

        public static void CreateMeterAdvancedTemplate(AFElementTemplate meterBasicTemplate)
        {
            // Your code here
        }

        public static void CreateCityTemplate(AFDatabase database)
        {
            // Your code here
        }

        public static void CreateElements(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }

        public static void SetAttributeValues(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }

        public static void CreateCityElements(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }

        public static void CreateWeakReferences(AFDatabase database)
        {
            if (database == null) return;

            // Your code here
        }
    }
}
