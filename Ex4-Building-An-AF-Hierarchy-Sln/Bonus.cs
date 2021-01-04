using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;

namespace Ex4BuildingAnAFHierarchySln
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

        private static AFDatabase GetOrCreateDatabase(string servername, string databasename)
        {
            PISystem assetServer = new PISystems()[servername];
            if (assetServer == null)
                return null;
            AFDatabase database = assetServer.Databases[databasename];
            if (database == null)
                database = assetServer.Databases.Add(databasename);
            return database;
        }

        private static void CreateCategories(AFDatabase database)
        {
            if (database == null) return;
            var items = new List<string>
            {
                "Measures Energy",
                "Shows Status",
                "Building Info",
                "Location",
                "Time-Series Data",
                "Energy Usage",
            };
            foreach (var item in items)
            {
                if (!database.ElementCategories.Contains(item))
                    database.ElementCategories.Add(item);
                if (!database.AttributeCategories.Contains(item))
                    database.AttributeCategories.Add(item);
            }

            if (database.IsDirty)
                database.CheckIn();
        }


        private static void CreateEnumerationSets(AFDatabase database)
        {
            if (database == null) return;

            if (!database.EnumerationSets.Contains("Building Type"))
            {
                AFEnumerationSet bTypeEnum = database.EnumerationSets.Add("Building Type");
                bTypeEnum.Add("Residential", 0);
                bTypeEnum.Add("Business", 1);
            }

            if (!database.EnumerationSets.Contains("Meter Status"))
            {
                AFEnumerationSet mStatusEnum = database.EnumerationSets.Add("Meter Status");
                mStatusEnum.Add("Good", 0);
                mStatusEnum.Add("Bad", 1);
            }

            if (database.IsDirty)
                database.CheckIn();
        }


        private static void CreateTemplates(AFDatabase database)
        {
            if (database == null) return;
            AFElementTemplate meterBasicTemplate = CreateMeterBasicTemplate(database);
            CreateMeterAdvancedTemplate(meterBasicTemplate);
            CreateCityTemplate(database);
            if (database.IsDirty)
                database.CheckIn();
        }

        private static AFElementTemplate CreateMeterBasicTemplate(AFDatabase database)
        {
            AFElementTemplate meterBasicTemplate = database.ElementTemplates["MeterBasic"];
            if (meterBasicTemplate != null)
                return meterBasicTemplate;

            UOM uom = database.PISystem.UOMDatabase.UOMs["kilowatt hour"];
            AFCategory mEnergyE = database.ElementCategories["Measures Energy"];
            AFCategory bInfoA = database.AttributeCategories["Building Info"];
            AFCategory locationA = database.AttributeCategories["Location"];
            AFCategory tsDataA = database.AttributeCategories["Time-Series Data"];
            AFEnumerationSet bTypeNum = database.EnumerationSets["Building Type"];
            AFPlugIn PIPoint = database.PISystem.DataReferencePlugIns["PI Point"];

            meterBasicTemplate = database.ElementTemplates.Add("MeterBasic");
            meterBasicTemplate.Categories.Add(mEnergyE);

            AFAttributeTemplate substationAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Substation");
            substationAttrTemp.Type = typeof(string);

            AFAttributeTemplate usageLimitAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Usage Limit");
            usageLimitAttrTemp.Type = typeof(string);
            usageLimitAttrTemp.DefaultUOM = uom;

            AFAttributeTemplate buildingAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Building");
            buildingAttrTemp.Type = typeof(string);
            buildingAttrTemp.Categories.Add(bInfoA);

            AFAttributeTemplate bTypeAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Building Type");
            bTypeAttrTemp.TypeQualifier = bTypeNum;
            bTypeAttrTemp.Categories.Add(bInfoA);

            AFAttributeTemplate cityAttrTemp = meterBasicTemplate.AttributeTemplates.Add("City");
            cityAttrTemp.Type = typeof(string);
            cityAttrTemp.Categories.Add(locationA);

            AFAttributeTemplate energyUsageAttrTemp = meterBasicTemplate.AttributeTemplates.Add("Energy Usage");
            energyUsageAttrTemp.Type = typeof(Single);
            energyUsageAttrTemp.Categories.Add(tsDataA);
            energyUsageAttrTemp.DefaultUOM = uom;
            energyUsageAttrTemp.DataReferencePlugIn = PIPoint;
            energyUsageAttrTemp.ConfigString = @"\\%@\Configuration|PIDataArchiveName%\%Element%.%Attribute%;UOM=kWh";

            return meterBasicTemplate;
        }

        private static void CreateMeterAdvancedTemplate(AFElementTemplate meterBasicTemplate)
        {
            AFDatabase database = meterBasicTemplate.Database;
            AFElementTemplate meterAdvancedTemplate = database.ElementTemplates["MeterAdvanced"];
            if (meterAdvancedTemplate == null)
                meterAdvancedTemplate = database.ElementTemplates.Add("MeterAdvanced");

            AFCategory tsDataA = database.AttributeCategories["Time-Series Data"];
            AFEnumerationSet mStatusEnum = database.EnumerationSets["Meter Status"];

            meterAdvancedTemplate.BaseTemplate = meterBasicTemplate;

            AFAttributeTemplate statusAttrTemp = meterAdvancedTemplate.AttributeTemplates["Status"];
            if (statusAttrTemp == null)
                statusAttrTemp = meterAdvancedTemplate.AttributeTemplates.Add("Status");
            statusAttrTemp.TypeQualifier = mStatusEnum;
            if (!statusAttrTemp.Categories.Contains(tsDataA))
                statusAttrTemp.Categories.Add(tsDataA);
            statusAttrTemp.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];
            statusAttrTemp.ConfigString = @"\\%@\Configuration|PIDataArchiveName%\%Element%.%Attribute%";
        }

        private static void CreateCityTemplate(AFDatabase database)
        {
            AFElementTemplate cityTemplate = database.ElementTemplates["City"];
            if (cityTemplate != null)
                return;

            cityTemplate = database.ElementTemplates.Add("City");

            AFAttributeTemplate cityEnergyUsageAttrTemp = cityTemplate.AttributeTemplates.Add("Energy Usage");
            cityEnergyUsageAttrTemp.Type = typeof(Single);
            cityEnergyUsageAttrTemp.DefaultUOM = database.PISystem.UOMDatabase.UOMs["kilowatt hour"];
            cityEnergyUsageAttrTemp.DataReferencePlugIn = database.PISystem.DataReferencePlugIns["PI Point"];
            cityEnergyUsageAttrTemp.ConfigString = @"\\%@\Configuration|PIDataArchiveName%\%Element%.%Attribute%";
        }

        private static void CreateElements(AFDatabase database)
        {
            if (database == null) return;

            // here we create the configuration element
            // we do a small exception creating an attribute in this method.
            AFElement configuration;
            if (!database.Elements.Contains("Configuration"))
            {
                configuration = database.Elements.Add("Configuration");
                AFAttribute name = configuration.Attributes.Add("PIDataArchiveName");
                name.SetValue(new AFValue(database.DefaultPIServerName));
            }

            AFElement meters = database.Elements["Meters"];
            if (meters == null)
                meters = database.Elements.Add("Meters");

            AFElementTemplate basic = database.ElementTemplates["MeterBasic"];
            AFElementTemplate advanced = database.ElementTemplates["MeterAdvanced"];

            foreach (int i in Enumerable.Range(1, 12))
            {
                string name = "Meter" + i.ToString("D3");
                if (!meters.Elements.Contains(name))
                {
                    AFElementTemplate eTemp = i <= 8 ? basic : advanced;
                    AFElement e = meters.Elements.Add(name, eTemp);
                }
            }

            if (database.IsDirty)
                database.CheckIn();
        }

        private static void SetAttributeValues(AFDatabase database)
        {
            if (database == null) return;

            AFElement meter001 = database.Elements["Meters"].Elements["Meter001"];
            meter001.Attributes["Substation"].SetValue(new AFValue("SSA-01"));
            meter001.Attributes["Usage Limit"].SetValue(new AFValue(350));
            meter001.Attributes["Building"].SetValue(new AFValue("The Shard"));

            AFEnumerationValue bTypeValue = database.EnumerationSets["Building Type"]["Residential"];
            meter001.Attributes["Building Type"].SetValue(new AFValue(bTypeValue));
            meter001.Attributes["City"].SetValue(new AFValue("London"));
        }

        private static void CreateCityElements(AFDatabase database)
        {
            if (database == null) return;

            if (!database.Elements.Contains("Geographical Locations"))
            {
                AFElement geoLocations = database.Elements.Add("Geographical Locations");
                AFElementTemplate cityTemplate = database.ElementTemplates["City"];

                geoLocations.Elements.Add("London", cityTemplate);
                geoLocations.Elements.Add("Montreal", cityTemplate);
                geoLocations.Elements.Add("San Francisco", cityTemplate);
            }

            if (database.IsDirty)
                database.CheckIn();
        }

        private static void CreateWeakReferences(AFDatabase database)
        {
            if (database == null) return;

            AFReferenceType weakRefType = database.ReferenceTypes["Weak Reference"];
            AFElement meters = database.Elements["Meters"];
            AFElement locations = database.Elements["Geographical Locations"];
            AFElementTemplate cityTemplate = database.ElementTemplates["City"];

            foreach (AFElement meter in meters.Elements)
            {
                string cityName = meter.Attributes["City"].GetValue().ToString();
                if (string.IsNullOrEmpty(cityName))
                    continue;
                AFElement city = locations.Elements[cityName];
                if (city == null)
                    locations.Elements.Add(cityName, cityTemplate);
                if (!city.Elements.Contains(meter.Name))
                    city.Elements.Add(meter, weakRefType);
            }

            if (database.IsDirty)
                database.CheckIn();
        }
    }
}
