# ProductApi

Simple ASP.NET Core Web API for Product Management with JWT Authentication, Logging, Exception Handling, and Search/Filter.

## Features

- JWT-based Authentication (Register/Login)
- Product CRUD
- Search/Filter Products
- Global Exception Handling
- Logging via `ILogger`
- Unit Tests with xUnit

---

## Run Locally

### Requirements

- .NET 8 SDK ([download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- SQLite (optional)
- VSCode / Visual Studio / terminal

### Steps

1. Clone the repository
git clone <https://github.com/syahferiaswan25/ProductApiTest.git>
cd ProductApi

2. Restore dependencies
dotnet restore

3. Build project
dotnet build

4. Run project
dotnet run --project ProductApi

5. Access frontend
Open browser and go to:
https://localhost:5001/ProductView/Index
