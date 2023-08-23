# Adding Azure SignalR Service

## Add Azure SignalR Package
Add a reference to the `Microsoft.Azure.SignalR` NuGet package by running the following command:
```sh
dotnet add package Microsoft.Azure.SignalR
```

## Add SignalR ConnectionString for local tests

Instead of set the variables in the `appsettings.json` file you can add a secret to Secret Manager for local tests.

This secret will contain the connection string to access your SignalR Service resource. `Azure:SignalR:ConnectionString` is the default configuration key that SignalR looks for to establish a connection. Replace the value in the following command with the connection string for your SignalR Service resource.

You should run these commands in the same directory as the csproj file:
```sh
dotnet user-secrets init
dotnet user-secrets set Azure:SignalR:ConnectionString "<Your connection string>"
```

# Adding CosmosDB with EF Core

## Add EF for Cosmo DB
Install the `Microsoft.EntityFrameworkCore.Cosmos` NuGet package:
```sh
dotnet add package Microsoft.EntityFrameworkCore.Cosmos
```

## 1. Create Azure CosmosDB
Go to Portal Azure or CLI and create a new CosmosDB resource and get Uri and Read/Write Key

## 2. Add CosmosDB Uri and Key for local tests
Instead of set the variables in the `appsettings.json` file you can add some secrets for local tests.
You should run these commands in the same directory as the csproj file:
```sh
dotnet user-secrets init
dotnet user-secrets set Database:CosmosDB:Uri "<Your ComsosDB Uri>"
dotnet user-secrets set Database:CosmosDB:Key "<Your ComsosDB Key>"
```
 
# Adding SqlServerDB with EF Core

## Add EF for SqlServer DB
Install the `Microsoft.EntityFrameworkCore.SqlServer` NuGet package:
```sh
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## 1. Create Azure SQLServerDB
Go to Portal Azure or CLI and create a new SqlServerDB resource and get ConnectionString and replace the default user password.

## 2. Add SqlServerDB ConnectionString for local tests
Instead of set the variables in the `appsettings.json` file you can add a secret for local tests.
You should run these commands in the same directory as the csproj file:
```sh
dotnet user-secrets init
dotnet user-secrets set Database:SQLServerDB:ConnectionString "<Your SqlServer ConnStr>"
```

# How to inject CosmosDB or SqlServerDB provider
Got o `Program.cs` file and comment/uncomment the respective line:
```c#
// Using Azure CosmosDB
builder.Services.AddDbContext<IDbContext, CosmosDbContext>();

// Using Azure SqlServerDB
builder.Services.AddDbContext<IDbContext, SqlServerDbContext>();
```