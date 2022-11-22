# AFSDK Getting Started Guide

**Version:** 1.1.2

[![Build Status](https://dev.azure.com/osieng/engineering/_apis/build/status/product-readiness/PI-System/AF-SDK-Getting-Started-Guide?branchName=main)](https://dev.azure.com/osieng/engineering/_build/latest?definitionId=2251&branchName=main)

This sample uses the AFSDK to accomplish some common tasks with the PISystem.

## Requirements

The [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/) is referenced in this sample, and should be installed to run the sample from the command line.

Replace the placeholders in the appsettings.placeholder.json file and rename it it to appsettings.json.

This sample is written against AF 2.10.

## Setting up the environment

1. Clone the GitHub repository
1. Start PI System Explorer
1. File > Database
1. New Database
1. Name it: Green Power Company
1. Ok then close
1. File > import from file
1. Select downloaded 'AF Database - Green Power Company.xml'
1. Select create pi points
1. Import the data
1. Navigate to new database
1. Enable Analysis

## Sample

This sample is based off of a PI World class and it includes projects that need to be filled out and completed pojects. The completed solutions to the skeleton projects are just one way you can accomplish the tasks set for the exercise. For instructions about what is expected to be filled go go [here](https://livelibrary.osisoft.com/LiveLibrary/web/pub.xql?c=t&action=home&pub=af-sdk-getting-started-v1&lang=en#addHistory=true&filename=GUID-781248C8-0952-4393-9F44-C9BFCDA54364.xml&docid=GUID-5EEFEEBC-A4EE-40D4-B8F9-21049E7F7C41&inner_id=&tid=&query=&scope=&resource=&toc=false&eventType=lcContent.loadDocGUID-5EEFEEBC-A4EE-40D4-B8F9-21049E7F7C41). Included in this samples are tests you can run against both the completed samples and the skeleton code to be filled in to see if your code works as expected. The tests are reasonably exact in making sure it was followed, given that timestamps and values are not predetermined.

As indicated above, there is a provided AF database, 'AF Database - Green Power Company.xml', that you need to use for this sample.

## Running the Automated Test

From the command line, run:

```shell
dotnet restore
dotnet test
```

This runs all of the tests for all completed solutions. You can also configure this command to run a subset of the tests.

```shell
dotnet restore
dotnet test --filter Category~Solution
```

Depending on the exercise, the test either does a unit test or an integrated test. The unit tests make sure functions behave as expected based on expected inputs and outputs, while integrated tests make sure the whole solution and the underlying functions behave as expected when run in sequence.

---

For the PI System landing page [ReadMe](https://github.com/osisoft/OSI-Samples-PI-System)  
For the OSIsoft Samples landing page [ReadMe](https://github.com/osisoft/OSI-Samples)
