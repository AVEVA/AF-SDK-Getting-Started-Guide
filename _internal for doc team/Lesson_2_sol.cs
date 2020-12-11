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
using System.Collections.Generic;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Search;

namespace Ex2_Searching_For_Assets_Sln
{
  class Program2
  {
    static void Main(string[] args)
    {
      AFDatabase database = GetDatabase("PISRV01", "Green Power Company");
      
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

    static void FindMetersByName(AFDatabase database, string elementNameFilter)
    {
      Console.WriteLine("Find Meters by Name: {0}", elementNameFilter);

      // Default search is as an element name string mask.
      var queryString = $"\"{elementNameFilter}\"";
      using (AFElementSearch elementQuery = new AFElementSearch(database, "ElementSearch", queryString))
      {
        elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
        foreach (AFElement element in elementQuery.FindElements())
        {
          Console.WriteLine("Element: {0}, Template: {1}, Categories: {2}",
            element.Name,
            element.Template.Name,
            element.CategoriesString);
        }
      }
      Console.WriteLine();
    }

    static void FindMetersByTemplate(AFDatabase database, string templateName)
    {
      Console.WriteLine("Find Meters by Template: {0}", templateName);

      using (AFElementSearch elementQuery = new AFElementSearch(database, "TemplateSearch", string.Format("template:\"{0}\"", templateName)))
      {
        elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
        int countDerived = 0;
        foreach (AFElement element in elementQuery.FindElements())
        {
          Console.WriteLine("Element: {0}, Template: {1}", element.Name, element.Template.Name);
          if (element.Template.Name != templateName)
            countDerived++;
        }

        Console.WriteLine("   Found {0} derived templates", countDerived);
        Console.WriteLine();
      }
    }

    static void FindMetersBySubstation(AFDatabase database, string substationLocation)
    {
      Console.WriteLine("Find Meters by Substation: {0}", substationLocation);

      string templateName = "MeterBasic";
      string attributeName = "Substation";
      using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeValueEQSearch",
        string.Format("template:\"{0}\" \"|{1}\":\"{2}\"", templateName, attributeName, substationLocation)))
      {
        elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
        int countNames = 0;
        foreach (AFElement element in elementQuery.FindElements())
        {
          Console.Write("{0}{1}", countNames++ == 0 ? string.Empty : ", ", element.Name);
        }

        Console.WriteLine("\n");
      }
    }

    static void FindMetersAboveUsage(AFDatabase database, double val)
    {
      Console.WriteLine("Find Meters above Usage: {0}", val);

      string templateName = "MeterBasic";
      string attributeName = "Energy Usage";
      using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeValueGTSearch",
        string.Format("template:\"{0}\" \"|{1}\":>{2}", templateName, attributeName, val)))
      {
        elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
        int countNames = 0;
        foreach (AFElement element in elementQuery.FindElements())
        {
          Console.Write("{0}{1}", countNames++ == 0 ? string.Empty : ", ", element.Name);
        }

        Console.WriteLine("\n");
      }
    }

    static void FindBuildingInfo(AFDatabase database, string templateName)
    {
      Console.WriteLine("Find Building Info: {0}", templateName);

      AFElementTemplate elemTemp = database.ElementTemplates[templateName];
      AFCategory buildingInfoCat = database.AttributeCategories["Building Info"];
      AFNamedCollectionList<AFAttribute> foundAttributes = new AFNamedCollectionList<AFAttribute>();

      using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeCattegorySearch", string.Format("template:\"{0}\"", templateName)))
      {
        elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
        foreach (AFElement element in elementQuery.FindElements())
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
  }
}
