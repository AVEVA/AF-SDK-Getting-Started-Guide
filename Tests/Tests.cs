using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;
using OSIsoft.AF.Time;
using Xunit;

namespace Tests
{
    public class Tests
    {
        public Tests()
        {
            Setup();
            Database = new PISystems()[AFServer].Databases[DatabaseString];
        }

        public static AFDatabase Database { get; set; }
        public static string AFServer { get; set; }
        public static string DatabaseString { get; set; }

        [Fact]
        public static void Setup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();

            // ==== Client constants ====
            AFServer = config["AFServer"];
            DatabaseString = config["Database"];
            (config as ConfigurationRoot).Dispose();
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        public void PrintRootElementsEx1()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasics.Program1.PrintRootElements(Database);
                var actual = sw.ToString();
                string expected = "Print Root Elements: 3\r\n  Configuration\r\n  Geographical Locations\r\n  Meters\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        public void PrintElementTemplatesEx1()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasics.Program1.PrintElementTemplates(Database);
                var actual = sw.ToString();
                string expected = "Print Element Templates\r\nName: City; Categories: \r\nName: MeterAdvanced; Categories: Shows Status;\r\nName: MeterBasic; Categories: Measures Energy;\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        public void PrintAttributeTemplatesEx1()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasics.Program1.PrintAttributeTemplates(Database, "MeterAdvanced");
                var actual = sw.ToString();
                string expected = "Print Attribute Templates for Element Template: MeterAdvanced\r\nName: Status, DRPlugin: PI Point\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        public void PrintEnergyUOMsEx1()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasics.Program1.PrintEnergyUOMs(Database.PISystem);
                var actual = sw.ToString();
                string expected = "Print Energy UOMs\r\nUOM: gigawatt hour, Abbreviation: GWh\r\nUOM: megawatt hour, Abbreviation: MWh\r\nUOM: watt hour, Abbreviation: Wh\r\nUOM: joule, Abbreviation: J\r\nUOM: British thermal unit, Abbreviation: Btu\r\nUOM: calorie, Abbreviation: cal\r\nUOM: gigajoule, Abbreviation: GJ\r\nUOM: kilojoule, Abbreviation: kJ\r\nUOM: kilowatt hour, Abbreviation: kWh\r\nUOM: megajoule, Abbreviation: MJ\r\nUOM: watt second, Abbreviation: Ws\r\nUOM: kilocalorie, Abbreviation: kcal\r\nUOM: million calorie, Abbreviation: MMcal\r\nUOM: million British thermal unit, Abbreviation: MM Btu\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        public void PrintEnumerationSetsEx1()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasics.Program1.PrintEnumerationSets(Database);
                var actual = sw.ToString();
                string expected = "Print Enumeration Sets\r\nBuilding Type\r\n0 - A\r\n1 - B\r\n\r\nMeter Status\r\n0 - Good\r\n1 - Bad\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        public void PrintCategoriesEx1()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasics.Program1.PrintCategories(Database);
                var actual = sw.ToString();
                string expected = "Print Categories\r\nElement Categories\r\nMeasures Energy\r\nShows Status\r\n\r\nAttribute Categories\r\nBuilding Info\r\nLocation\r\nTime-Series Data\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        [Trait("Category", "Solution")]
        public void PrintRootElementsEx1Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasicsSln.Program1.PrintRootElements(Database);
                var actual = sw.ToString();
                string expected = "Print Root Elements: 4\r\n";
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        [Trait("Category", "Solution")]
        public void PrintElementTemplatesEx1Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasicsSln.Program1.PrintElementTemplates(Database);
                var actual = sw.ToString();
                string expected = "Print Element Templates\r\nName: City; Categories: \r\nName: MeterAdvanced; Categories: Shows Status;\r\nName: MeterBasic; Categories: Measures Energy;\r\n";
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        [Trait("Category", "Solution")]
        public void PrintAttributeTemplatesEx1Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasicsSln.Program1.PrintAttributeTemplates(Database, "MeterAdvanced");
                var actual = sw.ToString();
                string expected = "Print Attribute Templates for Element Template: MeterAdvanced\r\nName: Status, DRPlugin: PI Point\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        [Trait("Category", "Solution")]
        public void PrintEnergyUOMsEx1Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasicsSln.Program1.PrintEnergyUOMs(Database.PISystem);
                var actual = sw.ToString();
                string expected = "Print Energy UOMs\r\nUOM: gigawatt hour, Abbreviation: GWh\r\nUOM: megawatt hour, Abbreviation: MWh\r\nUOM: watt hour, Abbreviation: Wh\r\nUOM: joule, Abbreviation: J\r\nUOM: British thermal unit, Abbreviation: Btu\r\nUOM: calorie, Abbreviation: cal\r\nUOM: gigajoule, Abbreviation: GJ\r\nUOM: kilojoule, Abbreviation: kJ\r\nUOM: kilowatt hour, Abbreviation: kWh\r\nUOM: megajoule, Abbreviation: MJ\r\nUOM: watt second, Abbreviation: Ws\r\nUOM: kilocalorie, Abbreviation: kcal\r\nUOM: million calorie, Abbreviation: MMcal\r\nUOM: million British thermal unit, Abbreviation: MM Btu\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        [Trait("Category", "Solution")]
        public void PrintEnumerationSetsEx1Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasicsSln.Program1.PrintEnumerationSets(Database);
                var actual = sw.ToString();
                string expected = "Print Enumeration Sets\r\nBuilding Type\r\n0 - A\r\n1 - B\r\n\r\nMeter Status\r\n0 - Good\r\n1 - Bad\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise1")]
        [Trait("Category", "Solution")]
        public void PrintCategoriesEx1Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex1ConnectionAndHierarchyBasicsSln.Program1.PrintCategories(Database);
                var actual = sw.ToString();
                string expected = "Print Categories\r\nElement Categories\r\nMeasures Energy\r\nShows Status\r\n\r\nAttribute Categories\r\nBuilding Info\r\nLocation\r\nTime-Series Data\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        [Trait("Category", "Solution")]
        public void FindMetersByNameEx2Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssetsSln.Program2.FindMetersByName(Database, "Meter00*");
                var actual = sw.ToString();
                string expected = "Find Meters by Name: Meter00*\r\nElement: Meter001, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter002, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter003, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter004, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter005, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter006, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter007, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter008, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter009, Template: MeterAdvanced, Categories: Measures Energy;Shows Status;\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        [Trait("Category", "Solution")]
        public void FindMetersByTemplateEx2Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssetsSln.Program2.FindMetersByTemplate(Database, "MeterBasic");
                var actual = sw.ToString();
                string expected = "Find Meters by Template: MeterBasic\r\nElement: Meter001, Template: MeterBasic\r\nElement: Meter002, Template: MeterBasic\r\nElement: Meter003, Template: MeterBasic\r\nElement: Meter004, Template: MeterBasic\r\nElement: Meter005, Template: MeterBasic\r\nElement: Meter006, Template: MeterBasic\r\nElement: Meter007, Template: MeterBasic\r\nElement: Meter008, Template: MeterBasic\r\nElement: Meter009, Template: MeterAdvanced\r\nElement: Meter010, Template: MeterAdvanced\r\nElement: Meter011, Template: MeterAdvanced\r\nElement: Meter012, Template: MeterAdvanced\r\n   Found 4 derived templates\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        [Trait("Category", "Solution")]
        public void FindMetersBySubstationEx2Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssetsSln.Program2.FindMetersBySubstation(Database, "SSA*");
                var actual = sw.ToString();
                string expected = "Find Meters by Substation: SSA*\r\nMeter001, Meter005, Meter009";
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        [Trait("Category", "Solution")]
        public void FindMetersAboveUsageEx2Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssetsSln.Program2.FindMetersAboveUsage(Database, 300);
                var actual = sw.ToString();
                string expected = "Find Meters above Usage: 300";
                Assert.Contains(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        [Trait("Category", "Solution")]
        public void FindBuildingInfoEx2Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssetsSln.Program2.FindBuildingInfo(Database, "MeterAdvanced");
                var actual = sw.ToString();
                string expected = "Find Building Info: MeterAdvanced\r\nFound 8 attributes.\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        public void FindMetersByNameEx2()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssets.Program2.FindMetersByName(Database, "Meter00*");
                var actual = sw.ToString();
                string expected = "Find Meters by Name: Meter00*\r\nElement: Meter001, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter002, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter003, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter004, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter005, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter006, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter007, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter008, Template: MeterBasic, Categories: Measures Energy;\r\nElement: Meter009, Template: MeterAdvanced, Categories: Measures Energy;Shows Status;\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        public void FindMetersByTemplateEx2()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssets.Program2.FindMetersByTemplate(Database, "MeterBasic");
                var actual = sw.ToString();
                string expected = "Find Meters by Template: MeterBasic\r\nElement: Meter001, Template: MeterBasic\r\nElement: Meter002, Template: MeterBasic\r\nElement: Meter003, Template: MeterBasic\r\nElement: Meter004, Template: MeterBasic\r\nElement: Meter005, Template: MeterBasic\r\nElement: Meter006, Template: MeterBasic\r\nElement: Meter007, Template: MeterBasic\r\nElement: Meter008, Template: MeterBasic\r\nElement: Meter009, Template: MeterAdvanced\r\nElement: Meter010, Template: MeterAdvanced\r\nElement: Meter011, Template: MeterAdvanced\r\nElement: Meter012, Template: MeterAdvanced\r\n   Found 4 derived templates\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        public void FindMetersBySubstationEx2()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssets.Program2.FindMetersBySubstation(Database, "SSA*");
                var actual = sw.ToString();
                string expected = "Find Meters by Substation: SSA*\r\nMeter001, Meter005, Meter009\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        public void FindMetersAboveUsageEx2()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssets.Program2.FindMetersAboveUsage(Database, 300);
                var actual = sw.ToString();
                string expected = "Find Meters above Usage: 300\r\n\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise2")]
        public void FindBuildingInfoEx2()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex2SearchingForAssets.Program2.FindBuildingInfo(Database, "MeterAdvanced");
                var actual = sw.ToString();
                string expected = "Find Building Info: MeterAdvanced\r\nFound 8 attributes.\r\n\r\n";
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        [Trait("Category", "Solution")]
        public void PrintHistoricalEx3Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintHistorical(Database, "Meter001", "*-30s", "*");
                var actual = sw.ToString();

                Assert.Contains("Print Historical Values - Meter: Meter001, Start: *-30s, End: *\r\n", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        [Trait("Category", "Solution")]
        public void PrintInterpolatedEx3Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintInterpolated(Database, "Meter001", "*-30s", "*", TimeSpan.FromSeconds(10));
                var actual = sw.ToString();

                Assert.Contains("Print Interpolated Values - Meter: Meter001, Start: *-30s, End: *\r\nTimestamp (Local):", actual);
                Assert.Contains(", Value: ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        [Trait("Category", "Solution")]
        public void PrintHourlyAverageEx3Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintHourlyAverage(Database, "Meter001", "y", "t");
                var actual = sw.ToString();

                Assert.Contains("Print Hourly Average - Meter: Meter001, Start: y, End: t\r\nTimestamp (Local):", actual);
                Assert.Contains(", Value: ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        [Trait("Category", "Solution")]
        public void PrintEnergyUsageAtTimeEx3Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintEnergyUsageAtTime(Database, "t+10h");
                var actual = sw.ToString();

                Assert.Contains("Print Energy Usage at Time: t+10h\r\nMeter: Meter001, Timestamp (Local): ", actual);
                Assert.Contains("Meter003, ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        [Trait("Category", "Solution")]
        public void PrintDailyAverageEnergyUsageEx3Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintDailyAverageEnergyUsage(Database, "t-7d", "t");
                var actual = sw.ToString();

                Assert.Contains("Print Daily Energy Usage - Start: t-7d, End: t\r\nAverages for Meter: Meter001\r\nTimestamp (Local):", actual);
                Assert.Contains(", Avg. Value:", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        [Trait("Category", "Solution")]
        public void SwapValuesEx3Sln()
        {
            var meter1 = "Meter001";
            var meter2 = "Meter002";
            var startDate = "*-1h";
            var endDate = "*";

            AFAttribute attr1 = AFAttribute.FindAttribute(@"\Meters\" + meter1 + @"|Energy Usage", Database);
            AFAttribute attr2 = AFAttribute.FindAttribute(@"\Meters\" + meter2 + @"|Energy Usage", Database);
            var valAtt1Before = attr1.GetValue(new AFTime(startDate));
            var valAtt2Before = attr2.GetValue(new AFTime(startDate));

            string actual = null;
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Ex3ReadingAndWritingDataSln.Program3.SwapValues(Database, meter1, meter2, startDate, endDate);

                actual = sw.ToString();
                Assert.Contains("Swap values for meters: ", actual);
            }

            var valAtt1After = attr1.GetValue(new AFTime(startDate));
            var valAtt2After = attr2.GetValue(new AFTime(startDate));

            // adding console logging to see what is going on here, because this fails occassionally
            {
                var standardOutput = new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true,
                };
                Console.SetOut(standardOutput);

                Console.WriteLine(actual);
                Console.WriteLine($"{valAtt1Before.Value}, {valAtt1After.Value}");
                Console.WriteLine($"{valAtt2Before.Value}, {valAtt2After.Value}");
            }

            Assert.Equal(valAtt1Before.Value.ToString(), valAtt2After.Value.ToString());
            Assert.Equal(valAtt2Before.Value.ToString(), valAtt1After.Value.ToString());
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        public void PrintHistoricalEx3()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintHistorical(Database, "Meter001", "*-30s", "*");
                var actual = sw.ToString();

                Assert.Contains("Print Historical Values - Meter: Meter001, Start: *-30s, End: *\r\nTimestamp (UTC):", actual);
                Assert.Contains(", Value (kJ): ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        public void PrintInterpolatedEx3()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintInterpolated(Database, "Meter001", "*-30s", "*", TimeSpan.FromSeconds(10));
                var actual = sw.ToString();

                Assert.Contains("Print Interpolated Values - Meter: Meter001, Start: *-30s, End: *\r\nTimestamp (Local):", actual);
                Assert.Contains(", Value: ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        public void PrintHourlyAverageEx3()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintHourlyAverage(Database, "Meter001", "y", "t");
                var actual = sw.ToString();

                Assert.Contains("Print Hourly Average - Meter: Meter001, Start: y, End: t\r\nTimestamp (Local):", actual);
                Assert.Contains(", Value: ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        public void PrintEnergyUsageAtTimeEx3()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintEnergyUsageAtTime(Database, "t+10h");
                var actual = sw.ToString();

                Assert.Contains("Print Energy Usage at Time: t+10h\r\nMeter: Meter001, Timestamp (Local): ", actual);
                Assert.Contains("Meter003, ", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        public void PrintDailyAverageEnergyUsageEx3()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Ex3ReadingAndWritingDataSln.Program3.PrintDailyAverageEnergyUsage(Database, "t-7d", "t");
                var actual = sw.ToString();

                Assert.Contains("Print Daily Energy Usage - Start: t-7d, End: t\r\nAverages for Meter: Meter001\r\nTimestamp (Local):", actual);
                Assert.Contains(", Avg. Value:", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise3")]
        public void SwapValuesEx3()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var meter1 = "Meter001";
                var meter2 = "Meter002";
                var startDate = "y";

                AFAttribute attr1 = AFAttribute.FindAttribute(@"\Meters\" + meter1 + @"|Energy Usage", Database);
                AFAttribute attr2 = AFAttribute.FindAttribute(@"\Meters\" + meter2 + @"|Energy Usage", Database);
                var valAtt1Before = attr1.GetValue(new AFTime(startDate));
                var valAtt2Before = attr2.GetValue(new AFTime(startDate));

                Ex3ReadingAndWritingDataSln.Program3.SwapValues(Database, meter1, meter2, startDate, "y+1h");

                var valAtt1After = attr1.GetValue(new AFTime(startDate));
                var valAtt2After = attr2.GetValue(new AFTime(startDate));

                var actual = sw.ToString();

                Assert.Contains("Swap values for meters: ", actual);
                Assert.Equal(valAtt1Before.Value.ToString(), valAtt2After.Value.ToString());
                Assert.Equal(valAtt2Before.Value.ToString(), valAtt1After.Value.ToString());
            }
        }

        [Fact]
        [Trait("Category", "Exercise4")]
        public void Ex4()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true,
            };
            Console.SetOut(standardOutput);

            // note this method either creates or ensures it was created.  That is what we will test
            Ex4BuildingAnAFHierarchy.Program4.CreateElementTemplate(Database);
            Assert.True(Database.ElementTemplates.Contains("FeederTemplate"));

            Ex4BuildingAnAFHierarchy.Program4.CreateFeedersRootElement(Database);
            Assert.True(Database.Elements.Contains("Feeders"));

            Ex4BuildingAnAFHierarchy.Program4.CreateFeederElements(Database);
            Assert.True(Database.Elements["Feeders"].Elements.Contains("Feeder001"));

            Ex4BuildingAnAFHierarchy.Program4.CreateWeakReference(Database);

            AFElement feeder0001 = Database.Elements["Feeders"].Elements["Feeder001"];
            var reftype = Database.Elements["Geographical Locations"].Elements["London"].GetReferenceTypes(feeder0001);

            Assert.Equal("Weak Reference", reftype[0].Name);
        }

        [Fact]
        [Trait("Category", "Exercise4")]
        [Trait("Category", "Solution")]
        public void Ex4Sln()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true,
            };
            Console.SetOut(standardOutput);

            // note this method either creates or ensures it was created.  That is what we will test
            Ex4BuildingAnAFHierarchySln.Program4.CreateElementTemplate(Database);
            Assert.True(Database.ElementTemplates.Contains("FeederTemplate"));

            // note this method either creates or ensures it was created.  That is what we will test
            Ex4BuildingAnAFHierarchySln.Program4.CreateFeedersRootElement(Database);
            Assert.True(Database.Elements.Contains("Feeders"));

            // note this method either creates or ensures it was created.  That is what we will test
            Ex4BuildingAnAFHierarchySln.Program4.CreateFeederElements(Database);
            Assert.True(Database.Elements["Feeders"].Elements.Contains("Feeder001"));

            Ex4BuildingAnAFHierarchySln.Program4.CreateWeakReference(Database);

            AFElement feeder0001 = Database.Elements["Feeders"].Elements["Feeder001"];
            var reftype = Database.Elements["Geographical Locations"].Elements["London"].GetReferenceTypes(feeder0001);

            Assert.Equal("Weak Reference", reftype[0].Name);
        }

        [Fact]
        [Trait("Category", "Exercise4")]
        public void Ex4Bonus()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true,
            };
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            // note this method either creates or ensures it was created.  That is what we will test
            Ex4BuildingAnAFHierarchy.Bonus.Run();
            var newDB = Database.PISystem.Databases["Ethical Power Company"];
            Assert.NotNull(newDB);

            AFElement meters = Database.Elements["Meters"];
            var meter = meters.Elements.First();
            var reftype = newDB.Elements["Geographical Locations"].Elements["London"].GetReferenceTypes(meter);

            Assert.Equal("Weak Reference", reftype[0].Name);
        }

        [Fact]
        [Trait("Category", "Exercise4")]
        [Trait("Category", "Solution")]
        public void Ex4BonusSln()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true,
            };
            Console.SetOut(standardOutput);

            // note this method either creates or ensures it was created.  That is what we will test
            Ex4BuildingAnAFHierarchySln.Bonus.Run();
            var newDB = Database.PISystem.Databases["Ethical Power Company"];
            Assert.NotNull(newDB);

            AFElement meters = newDB.Elements["Meters"];
            var meter = meters.Elements["Meter001"];
            var reftype = newDB.Elements["Geographical Locations"].Elements["London"].GetReferenceTypes(meter);

            Assert.Equal("Weak Reference", reftype[0].Name);
        }

        [Fact]
        [Trait("Category", "Exercise5")]
        [Trait("Category", "Solution")]
        public void Ex5Sln()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var eventFrameTemplate = Ex5WorkingWithEventFramesSln.Program5.CreateEventFrameTemplate(Database);
                Assert.NotNull(eventFrameTemplate);
                Ex5WorkingWithEventFramesSln.Program5.CreateEventFrames(Database, eventFrameTemplate);

                AFTime startTime = DateTime.Today.AddDays(-7);
                string queryString = $"template:\"{eventFrameTemplate.Name}\"";
                using (AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(Database, "EventFrame Captures", AFEventFrameSearchMode.ForwardFromStartTime, startTime, queryString))
                {
                    eventFrameSearch.CacheTimeout = TimeSpan.FromMinutes(5);
                    var resp = eventFrameSearch.FindObjects();
                    Assert.True(resp.Count() > 0);
                }

                Ex5WorkingWithEventFramesSln.Program5.CaptureValues(Database, eventFrameTemplate);
                Ex5WorkingWithEventFramesSln.Program5.PrintReport(Database, eventFrameTemplate);
                var actual = sw.ToString();
                Assert.Contains("Daily Usage-Meter", actual);
                Assert.Contains(", Meter003,", actual);
            }
        }

        [Fact]
        [Trait("Category", "Exercise5")]
        public void PrintReportEx5()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                var eventFrameTemplate = Ex5WorkingWithEventFramesSln.Program5.CreateEventFrameTemplate(Database);
                Assert.NotNull(eventFrameTemplate);
                Ex5WorkingWithEventFramesSln.Program5.CreateEventFrames(Database, eventFrameTemplate);

                AFTime startTime = DateTime.Today.AddDays(-7);
                string queryString = $"template:\"{eventFrameTemplate.Name}\"";
                using (AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(Database, "EventFrame Captures", AFEventFrameSearchMode.ForwardFromStartTime, startTime, queryString))
                {
                    eventFrameSearch.CacheTimeout = TimeSpan.FromMinutes(5);
                    var resp = eventFrameSearch.FindObjects();
                    Assert.True(resp.Count() > 0);
                }

                Ex5WorkingWithEventFramesSln.Program5.CaptureValues(Database, eventFrameTemplate);
                Ex5WorkingWithEventFramesSln.Program5.PrintReport(Database, eventFrameTemplate);
                var actual = sw.ToString();
                Assert.Contains("Daily Usage-Meter", actual);
                Assert.Contains(", Meter003,", actual);
            }
        }

        [Fact]
        [Trait("Category", "Delete")]
        public void Cleanup()
        {
            Database.PISystem.Databases.Remove("Ethical Power Company");
            Database.PISystem.Databases.Remove(Database);
        }
    }
}
