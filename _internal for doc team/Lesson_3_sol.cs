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
using OSIsoft.AF.Data;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Search;

namespace Ex3_Reading_And_Writing_Data_Sln
{
  class Program3
  {
    static void Main(string[] args)
    {
      AFDatabase database = GetDatabase("PISRV01", "Green Power Company");
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

    static void PrintHistorical(AFDatabase database, string meterName, string startTime, string endTime)
    {
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

    static void PrintInterpolated(AFDatabase database, string meterName, string startTime, string endTime, TimeSpan timeSpan)
    {
      Console.WriteLine(string.Format("Print Interpolated Values - Meter: {0}, Start: {1}, End: {2}", meterName, startTime, endTime));

      AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", database);

      AFTime start = new AFTime(startTime);
      AFTime end = new AFTime(endTime);
      AFTimeRange timeRange = new AFTimeRange(start, end);

      AFTimeSpan interval = new AFTimeSpan(timeSpan);

      AFValues vals = attr.Data.InterpolatedValues(
        timeRange: timeRange,
        interval: interval,
        desiredUOM: null,
        filterExpression: null,
        includeFilteredValues: false);

      foreach (AFValue val in vals)
      {
        Console.WriteLine("Timestamp (Local): {0}, Value: {1:0.00} {2}", val.Timestamp.LocalTime, val.Value, val?.UOM.Abbreviation);
      }
      Console.WriteLine();
    }

    static void PrintHourlyAverage(AFDatabase database, string meterName, string startTime, string endTime)
    {
      Console.WriteLine(string.Format("Print Hourly Average - Meter: {0}, Start: {1}, End: {2}", meterName, startTime, endTime));

      AFAttribute attr = AFAttribute.FindAttribute(@"\Meters\" + meterName + @"|Energy Usage", database);

      AFTime start = new AFTime(startTime);
      AFTime end = new AFTime(endTime);
      AFTimeRange timeRange = new AFTimeRange(start, end);

      IDictionary<AFSummaryTypes, AFValues> vals = attr.Data.Summaries(
        timeRange: timeRange,
        summaryDuration: new AFTimeSpan(TimeSpan.FromHours(1)),
        summaryType: AFSummaryTypes.Average,
        calcBasis: AFCalculationBasis.TimeWeighted,
        timeType: AFTimestampCalculation.EarliestTime);


      foreach (AFValue val in vals[AFSummaryTypes.Average])
      {
        Console.WriteLine("Timestamp (Local): {0:yyyy-MM-dd HH\\h}, Value: {1:0.00} {2}", val.Timestamp.LocalTime, val.Value,val?.UOM.Abbreviation);
      }

      Console.WriteLine();
    }

    static void PrintEnergyUsageAtTime(AFDatabase database, string timeStamp)
    {
      Console.WriteLine("Print Energy Usage at Time: {0}", timeStamp);

      AFAttributeList attrList = GetAttributes(database, "MeterBasic", "Energy Usage");

      AFTime time = new AFTime(timeStamp);

      IList<AFValue> vals = attrList.Data.RecordedValue(time);

      foreach (AFValue val in vals)
      {
        Console.WriteLine("Meter: {0}, Timestamp (Local): {1:yyyy-MM-dd HH\\h}, Value: {2:0.00} {3}",val.Attribute.Element.Name,val.Timestamp.LocalTime,val.Value,val?.UOM.Abbreviation);
      }
      Console.WriteLine();
    }

    static void PrintDailyAverageEnergyUsage(AFDatabase database, string startTime, string endTime)
    {
      Console.WriteLine(string.Format("Print Daily Energy Usage - Start: {0}, End: {1}", startTime, endTime));

      AFAttributeList attrList = GetAttributes(database, "MeterBasic", "Energy Usage");

      AFTime start = new AFTime(startTime);
      AFTime end = new AFTime(endTime);
      AFTimeRange timeRange = new AFTimeRange(start, end);

      // Ask for 100 PI Points at a time
      PIPagingConfiguration pagingConfig = new PIPagingConfiguration(PIPageType.TagCount, 100);

      IEnumerable<IDictionary<AFSummaryTypes, AFValues>> summaries = attrList.Data.Summaries(
        timeRange: timeRange,
        summaryDuration: new AFTimeSpan(TimeSpan.FromDays(1)),
        summaryTypes: AFSummaryTypes.Average,
        calculationBasis: AFCalculationBasis.TimeWeighted,
        timeType: AFTimestampCalculation.EarliestTime,
        pagingConfig: pagingConfig);

      // Loop through attributes
      foreach (IDictionary<AFSummaryTypes, AFValues> dict in summaries)
      {
        AFValues values = dict[AFSummaryTypes.Average];
        Console.WriteLine("Averages for Meter: {0}", values.Attribute.Element.Name);

        // Loop through values per attribute
        foreach (AFValue val in dict[AFSummaryTypes.Average])
        {
          Console.WriteLine("Timestamp (Local): {0:yyyy-MM-dd}, Avg. Value: {1:0.00} {2}",val.Timestamp.LocalTime,val.Value,val?.UOM.Abbreviation);
        }
        Console.WriteLine();

      }
      Console.WriteLine();
    }

    static void SwapValues(AFDatabase database, string meter1, string meter2, string startTime, string endTime)
    {
      Console.WriteLine(string.Format("Swap values for meters: {0}, {1} between {2} and {3}", meter1, meter2, startTime, endTime));

      // NOTE: This method does not ensure that there is no data loss if there is failure.
      // Persist the data first in case you need to rollback.

      AFAttribute attr1 = AFAttribute.FindAttribute(@"\Meters\" + meter1 + @"|Energy Usage", database);
      AFAttribute attr2 = AFAttribute.FindAttribute(@"\Meters\" + meter2 + @"|Energy Usage", database);

      AFTime start = new AFTime(startTime);
      AFTime end = new AFTime(endTime);
      AFTimeRange timeRange = new AFTimeRange(start, end);

      // Get values to delete for meter1
      AFValues valsToRemove1 = attr1.Data.RecordedValues(
        timeRange: timeRange,
        boundaryType: AFBoundaryType.Inside,
        desiredUOM: null,
        filterExpression: null,
        includeFilteredValues: false);

      // Get values to delete for meter2
      AFValues valsToRemove2 = attr2.Data.RecordedValues(
        timeRange: timeRange,
        boundaryType: AFBoundaryType.Inside,
        desiredUOM: null,
        filterExpression: null,
        includeFilteredValues: false);

      List<AFValue> valsToRemove = valsToRemove1.ToList();
      valsToRemove.AddRange(valsToRemove2.ToList());

      // Remove the values
      AFListData.UpdateValues(valsToRemove, AFUpdateOption.Remove);

      // Create new AFValues from the other meter and assign them to this meter
      List<AFValue> valsToAdd1 = valsToRemove2.Select(v => new AFValue(attr1, v.Value, v.Timestamp)).ToList();
      List<AFValue> valsToAdd2 = valsToRemove1.Select(v => new AFValue(attr2, v.Value, v.Timestamp)).ToList();

      List<AFValue> valsCombined = valsToAdd1;
      valsCombined.AddRange(valsToAdd2);

      AFListData.UpdateValues(valsCombined, AFUpdateOption.Insert);
      Console.WriteLine();
    }


    // Helper method used in PrintEnergyUsageAtTime() and PrintDailyAverageEnergyUseage
    static AFAttributeList GetAttributes(AFDatabase database, string templateName, string attributeName)
    {
      AFAttributeList attrList = new AFAttributeList();

      using (AFElementSearch elementQuery = new AFElementSearch(database, "AttributeSearch", string.Format("template:\"{0}\"", templateName)))
      {
        elementQuery.CacheTimeout = TimeSpan.FromMinutes(5);
        foreach (AFElement element in elementQuery.FindElements())
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
  }
}
