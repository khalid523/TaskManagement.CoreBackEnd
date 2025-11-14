Task Management System - Backend API
A complete RESTful API built with .NET 8 and SQL Server for managing tasks with role-based access control.
📋 Table of Contents

Features
Prerequisites
Installation
Configuration
Running the Application
Database Setup
API Documentation
Project Structure
Testing
Troubleshooting
Architecture

✨ Features

User Management: Create, read, update, and delete users
Task Management: Complete CRUD operations for tasks
Role-Based Access Control: Admin and User roles with different permissions
RESTful API: Well-designed endpoints with proper HTTP status codes
SQL Server Database: Persistent data storage with migrations
Swagger Documentation: Interactive API documentation
Comprehensive Testing: Unit tests for services and controllers
Error Handling: Proper exception handling and logging
Seed Data: Pre-populated with 1 admin user and 1 regular user

🔧 Prerequisites
Before you begin, ensure you have installed:

.NET 8 SDK
SQL Server Express (or full SQL Server)
SQL Server Management Studio (optional, for database visualization)
Git

📥 Installation
1. Clone the Repository
bashgit clone https://github.com/yourusername/task-management-system.git
cd task-management-system/Backend
2. Restore Dependencies
bashdotnet restore
3. Build the Solution
bashdotnet build
⚙️ Configuration
appsettings.json Setup
Update TaskManagement.API/appsettings.json with your SQL Server connection string:
json{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskManagementDb;Trusted_Connection=true;Encrypt=false;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
Connection String Guide:

Server=. - Local SQL Server instance
Database=TaskManagementDb - Database name
Trusted_Connection=true - Windows Authentication
Encrypt=false - Disable encryption for local development
TrustServerCertificate=true - Trust self-signed certificate

Alternative Connection Strings
For SQL Server with username/password:
json"DefaultConnection": "Server=YOUR_SERVER;Database=TaskManagementDb;User Id=sa;Password=YourPassword;Encrypt=false;TrustServerCertificate=true"
🚀 Running the Application
Step 1: Create Database and Run Migrations
bashcd TaskManagement.API

# Add migration
dotnet ef migrations add InitialCreate -p ../TaskManagement.Infrastructure

# Update database (creates tables and seed data)
dotnet ef database update -p ../TaskManagement.Infrastructure
Step 2: Start the API
bashdotnet run
Expected Output:
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
      Now listening on: http://localhost:5000
Step 3: Access the API

API Base URL: https://localhost:7001
Swagger UI: https://localhost:7001/swagger
API Version: v1

📊 Database Setup
Automatic Setup
When you run dotnet ef database update, the system automatically:

✅ Creates TaskManagementDb database
✅ Creates Users table
✅ Creates WorkTasks table
✅ Inserts seed data:

Admin User (ID: 1, Email: admin@example.com, Role: Admin)
John Doe (ID: 2, Email: john@example.com, Role: User)
3 Sample Tasks assigned to these users



Verify Database in SQL Server Management Studio

Open SQL Server Management Studio
Connect to: . (localhost)
Navigate: Databases → TaskManagementDb → Tables
Verify:

dbo.Users (2 rows)
dbo.WorkTasks (3 rows)



📚 API Documentation
Swagger UI
Access interactive API documentation at: https://localhost:7001/swagger
User Endpoints
MethodEndpointDescriptionAuthGET/api/usersGet all usersAdminGET/api/users/{id}Get user by IDAllPOST/api/usersCreate new userAdminPUT/api/users/{id}Update userAdminDELETE/api/users/{id}Delete userAdmin
Task Endpoints
MethodEndpointDescriptionAuthGET/api/tasksGet all tasksAdminGET/api/tasks/{id}Get task by IDAdmin/OwnerGET/api/tasks/user/{userId}Get user's tasksAllPOST/api/tasksCreate new taskAdminPUT/api/tasks/{id}Update taskAdmin/Owner (status only)DELETE/api/tasks/{id}Delete taskAdmin
Authentication Headers
All requests must include role headers:
bashcurl -X GET "https://localhost:7001/api/users" \
  -H "X-User-Role: Admin" \
  -H "X-User-Id: 1" \
  -H "Content-Type: application/json"
Headers:

X-User-Role: "Admin" or "User"
X-User-Id: User ID (1 or 2)

Example Requests
Get All Users (Admin Only)
bashcurl -X GET "https://localhost:7001/api/users" \
  -H "X-User-Role: Admin" \
  -H "X-User-Id: 1"
Get User Tasks
bashcurl -X GET "https://localhost:7001/api/tasks/user/2" \
  -H "X-User-Role: User" \
  -H "X-User-Id: 2"
Create New Task (Admin Only)
bashcurl -X POST "https://localhost:7001/api/tasks" \
  -H "X-User-Role: Admin" \
  -H "X-User-Id: 1" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "New Feature",
    "description": "Implement new feature",
    "assignedToUserId": 2
  }'
Update Task Status
bashcurl -X PUT "https://localhost:7001/api/tasks/1" \
  -H "X-User-Role: User" \
  -H "X-User-Id: 2" \
  -H "Content-Type: application/json" \
  -d '{
    "status": "InProgress"
  }'
📁 Project Structure
TaskManagement/
├── TaskManagement.API/
│   ├── Controllers/
│   │   ├── UsersController.cs
│   │   └── TasksController.cs
│   ├── Services/
│   │   ├── UserService.cs
│   │   └── WorkTaskService.cs
│   ├── Middleware/
│   │   └── RoleBasedAccessMiddleware.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
│
├── TaskManagement.Core/
│   ├── Entities/
│   │   ├── User.cs
│   │   └── WorkTask.cs
│   └── Interfaces/
│       ├── IUserRepository.cs
│       ├── IUserService.cs
│       ├── IWorkTaskRepository.cs
│       └── IWorkTaskService.cs
│
├── TaskManagement.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Migrations/
│   └── Repositories/
│       ├── UserRepository.cs
│       └── WorkTaskRepository.cs
│
└── TaskManagement.Tests/
    ├── ServiceTests/
    │   └── UserServiceTests.cs
    └── ControllerTests/
        └── UsersControllerTests.cs
🧪 Testing
Run All Tests
bashdotnet test
Run Specific Test File
bashdotnet test TaskManagement.Tests/ServiceTests/UserServiceTests.cs
Run Tests with Coverage
bashdotnet test /p:CollectCoverage=true
Test Details
Service Tests (UserServiceTests.cs):

✅ GetUserByIdAsync with valid ID
✅ GetUserByIdAsync with invalid ID
✅ CreateUserAsync with valid data
✅ CreateUserAsync with invalid role
✅ UpdateUserAsync with valid ID
✅ DeleteUserAsync functionality

Controller Tests (UsersControllerTests.cs):

✅ GetAll returns list of users
✅ GetById returns single user
✅ GetById returns NotFound for invalid ID
✅ Create returns CreatedAtAction
✅ Delete returns NoContent

🐛 Troubleshooting
SQL Server Connection Issues
Error: "Cannot connect to SQL Server"
bash# Check if SQL Server is running (Windows)
sc query MSSQLSERVER

# Start SQL Server service
net start MSSQLSERVER
Error: "Login failed for user"

Verify connection string in appsettings.json
Ensure SQL Server is using Windows Authentication
Check SQL Server service is running

Migration Issues
Error: "Migration command not found"
bash# Install Entity Framework Core tools
dotnet tool install --global dotnet-ef

# Or update existing installation
dotnet tool update --global dotnet-ef
Error: "Cannot drop database"
bash# Remove the last migration
dotnet ef migrations remove -p ../TaskManagement.Infrastructure

# Create new migration
dotnet ef migrations add InitialCreate -p ../TaskManagement.Infrastructure

# Update database
dotnet ef database update -p ../TaskManagement.Infrastructure
Port Already in Use
If port 7001 is already in use, modify Properties/launchSettings.json:
json"applicationUrl": "https://localhost:7002;http://localhost:5002"
Clear Database
bash# Drop and recreate database
cd TaskManagement.API

# Remove all migrations
dotnet ef migrations remove -p ../TaskManagement.Infrastructure --force

# Recreate migration
dotnet ef migrations add InitialCreate -p ../TaskManagement.Infrastructure

# Update database
dotnet ef database update -p ../TaskManagement.Infrastructure
🏗️ Architecture
Layered Architecture
┌─────────────────────────────────────┐
│  API Layer (Controllers)            │
│  - HTTP endpoints                   │
│  - Request/Response handling        │
└────────────────┬────────────────────┘
                 │
┌────────────────▼────────────────────┐
│  Service Layer                      │
│  - Business logic                   │
│  - Data validation                  │
└────────────────┬────────────────────┘
                 │
┌────────────────▼────────────────────┐
│  Repository Layer                   │
│  - Data access                      │
│  - CRUD operations                  │
└────────────────┬────────────────────┘
                 │
┌────────────────▼────────────────────┐
│  Data Layer (Entity Framework)      │
│  - SQL Server Database              │
│  - DbContext & Migrations           │
└─────────────────────────────────────┘
Design Patterns Used

Repository Pattern: Abstraction over data access
Dependency Injection: IoC container for loose coupling
Service Layer: Business logic separation
Entity Framework Core: ORM for database operations
Middleware Pattern: Cross-cutting concerns (logging, authentication)

🔐 Role-Based Access Control
Admin Permissions

✅ View all users
✅ Create new users
✅ Update user details
✅ Delete users
✅ View all tasks
✅ Create new tasks
✅ Update all task fields
✅ Delete tasks

User Permissions

✅ View own profile
✅ View assigned tasks only
✅ Update task status only
❌ Cannot create tasks
❌ Cannot delete tasks
❌ Cannot manage users

📝 Default Credentials
For testing purposes, two users are pre-seeded:
UserEmailRoleIDAdmin Useradmin@example.comAdmin1John Doejohn@example.comUser2
🚀 Deployment
Build for Release
bashdotnet publish -c Release -o ./publish
Run Published Application
bash./publish/TaskManagement.API.exe
Docker Support (Optional)
Create Dockerfile:
dockerfileFROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /src/TaskManagement.API/bin/Release/net8.0 .
EXPOSE 7001
ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]
Build and run:
bashdocker build -t task-management-api .
docker run -p 7001:7001 task-management-api
📚 Additional Resources

.NET 8 Documentation
Entity Framework Core Docs
SQL Server Documentation
RESTful API Best Practices
