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
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;
using OSIsoft.AF.Time;

namespace Ex5_Working_With_EventFrames_Sln
{
  class Program5
  {
    static void Main(string[] args)
    {
      AFDatabase database = GetDatabase("PISRV01", "Green Power Company");
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

    static AFElementTemplate CreateEventFrameTemplate(AFDatabase database)
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

    static void CreateEventFrames(AFDatabase database, AFElementTemplate eventFrameTemplate)
    {
      string queryString = "Template:MeterBasic";
      {
        // This method returns the collection of AFBaseElement objects that were created with this template.
        using (AFElementSearch elementQuery = new AFElementSearch(database, "Meters", queryString))
        {

          DateTime timeReference = DateTime.Today.AddDays(-7);
          int count = 0;
          foreach (AFElement meter in elementQuery.FindElements())
          {
            foreach (int day in Enumerable.Range(1, 7))
            {
              AFTime startTime = new AFTime(timeReference.AddDays(day - 1));
              AFTime endTime = new AFTime(startTime.LocalTime.AddDays(1));
              AFEventFrame ef = new AFEventFrame(database, "*", eventFrameTemplate);
              ef.SetStartTime(startTime);
              ef.SetEndTime(endTime);
              ef.PrimaryReferencedElement = meter;
              // It is good practice to periodically check in the database
              if (++count % 500 == 0)
                database.CheckIn();
            }
          }
        }
      } 
      if (database.IsDirty)
        database.CheckIn();
    }

    static public void CaptureValues(AFDatabase database, AFElementTemplate eventFrameTemplate)
    {
      // Formulate search constraints on time and template
      AFTime startTime = DateTime.Today.AddDays(-7);
      string queryString = $"template:\"{eventFrameTemplate.Name}\"";
      using (AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(database, "EventFrame Captures", AFEventFrameSearchMode.ForwardFromStartTime, startTime, queryString))
      {
        eventFrameSearch.CacheTimeout = TimeSpan.FromMinutes(5);
        int count = 0;
        foreach (AFEventFrame item in eventFrameSearch.FindEventFrames())
        {
          item.CaptureValues();
          if ((count++ % 500) == 0)
            database.CheckIn();
        }
        if (database.IsDirty)
          database.CheckIn();
      }
    }

    static void PrintReport(AFDatabase database, AFElementTemplate eventFrameTemplate)
    {
      AFTime startTime = DateTime.Today.AddDays(-7);
      AFTime endTime = startTime.LocalTime.AddDays(+8); // Or DateTime.Today.AddDays(1);
      string queryString = $"template:'{eventFrameTemplate.Name}' ElementName:Meter003";
      using (AFEventFrameSearch eventFrameSearch = new AFEventFrameSearch(database, "EventFrame Captures", AFSearchMode.StartInclusive, startTime, endTime, queryString))
      {
        eventFrameSearch.CacheTimeout = TimeSpan.FromMinutes(5);
        foreach (AFEventFrame ef in eventFrameSearch.FindEventFrames())
        {
          Console.WriteLine("{0}, {1}, {2}",
            ef.Name,
            ef.PrimaryReferencedElement.Name,
            ef.Attributes["Average Energy Usage"].GetValue().Value);
        }
      }
    }
  }
}
