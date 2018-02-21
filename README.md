---
services: cosmos-db
platforms: dotnet
author: viviswan
---

# Using Mongo ReadPreference with Azure Cosmos DB's MongoDB API
Azure Cosmos DB is a globally distributed multi-model database. One of the supported APIs is the MongoDB API, which provides a document model and support for client drivers in many platforms. This sample shows you how to use MongoDB ReadPreference against Azure Cosmos DB with MongoDB API from a .NET application.

## Running this sample

* Before you can run this sample, you must have the following prerequisites:

   * An active Azure account. If you don't have one, you can sign up for a [free account](https://azure.microsoft.com/free/). Alternatively, you can use the [Azure Cosmos DB Emulator](https://docs.microsoft.com/azure/cosmos-db/local-emulator) for this tutorial.
   * Visual Studio 2017 (download and use the free [Visual Studio 2017 Community Edition](https://www.visualstudio.com/downloads/))

* Then, clone this repository.

* Next, substitute the `connectionString`, `readTargetRegion` in *App.Config* with your Cosmos DB account's values. 

* Install the *MongoDB.Driver* library from Visual Studio's Nuget Manager.

* Click *CTRL + F5* to run your application.

## About the code
The code included in this sample is intended to get you quickly started with a .NET application that connects to Azure Cosmos DB with the MongoDB API.

## More information

- [Azure Cosmos DB](https://docs.microsoft.com/azure/cosmos-db/introduction)
- [Azure Cosmos DB : MongoDB API](https://docs.microsoft.com/azure/cosmos-db/mongodb-introduction)
- [MongoDB .NET driver](https://docs.mongodb.com/ecosystem/drivers/csharp/)
- [MongoDB .NET driver documentation](https://mongodb.github.io/mongo-csharp-driver/2.4/)