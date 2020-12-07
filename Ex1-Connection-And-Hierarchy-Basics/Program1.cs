using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex1ConnectionAndHierarchyBasics
{
    public static class Program1
    {
        public static void Main()
        {
            AFDatabase database = GetDatabase("PISRV01", "Green Power Company");

            if (database == null) throw new NullReferenceException("Database is null");

            PrintRootElements(database);
            PrintElementTemplates(database);
            PrintAttributeTemplates(database, "MeterAdvanced");
            PrintEnergyUOMs(database.PISystem);
            PrintEnumerationSets(database);
            PrintCategories(database);

            Console.WriteLine("Press ENTER key to close");
            Console.ReadLine();
        }

        static AFDatabase GetDatabase(string server, string database)
        {
            PISystems piSystems = new PISystems();
            PISystem assetServer = piSystems[server];
            AFDatabase afDatabase = assetServer.Databases[database];
            return afDatabase;
        }

        public static void PrintRootElements(AFDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            Console.WriteLine("Print Root Elements: {0}", database.Elements.Count);
            foreach (AFElement element in database.Elements)
            {
                Console.WriteLine("  {0}", element.Name);
            }

            Console.WriteLine();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintElementTemplates(AFDatabase database)
        {
            /// Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintAttributeTemplates(AFDatabase database, string elemTempName)
        {
            /// Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintEnergyUOMs(PISystem system)
        {
            /// Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintEnumerationSets(AFDatabase database)
        {
            /// Your code here
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Skeleton")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "Skeleton")]
        public static void PrintCategories(AFDatabase database)
        {
            /// Your code here
        }
    }
}
