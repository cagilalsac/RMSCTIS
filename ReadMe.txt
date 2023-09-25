Visual Studio Code Extensions, .NET 7 SDK and MySQL:
Here's the list of extensions required for Visual Studio Code:

1) C#

2) C# Dev Kit

3) NuGet Package Manager GUI



You also need to install .NET 7 SDK which can be downloaded from the link below:

https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.401-windows-x64-installer



If you don't have MySQL installed, you can download and install the WampServer from the link below:

Please select MySQL version starting with 5 during the installation.

https://www.wampserver.com/en/#wampserver-64-bits-php-5-6-25-php-7



After creating the MVC (ASP.NET Core Web App Model View Controller), 
Business (Class Library) and DataAccess (Class Library) projects, 
the solution is built, and the following references are added to the projects:

1) DataAccess is referenced in the Business project.

2) Business and DataAccess are referenced in the MVC project.



Roadmap:

1) In the DataAccess layer, entities are created.

2) In the DataAccess layer, MySql.EntityFrameworkCore and Microsoft.EntityFrameworkCore.Tools
packages are downloaded through NuGet. The .NET version you are using determines which packages 
with that version number should be downloaded from NuGet. For example, if you are using .NET 7, 
you should look for packages compatible with .NET 7 and install their latest versions. 
The version number typically corresponds to the major version of .NET, so in this case, 
you should search for packages starting with version 7.x.x.

3) In the DataAccess layer, a context class derived from the DbContext class is created
including the DbSets of type entites of our project. Then a parameterized constructor is created 
which will accept an object of type DbContextOptions containing the connection string information. 
Afterward, in the MVC layer, dependency injection for the class derived from the DbContext 
is configured in the IoC (Inversion of Control) Container in the Program.cs file.

4) In the MVC layer, Microsoft.EntityFrameworkCore.Design and Microsoft.VisualStudio.Web.CodeGeneration.Design 
packages are downloaded through NuGet.
If you are using Visual Studio instead of Visual Studio Code, you should set the MVC project 
as the start up project.

5) For Entity Framework Code First migration terminal commands:
5.1) First "dotnet tool install --global dotnet-ef" command should be run only once.
5.2) Then a new database migration (version) can be added with "dotnet-ef migrations add v1" 
command, v1 can be any unique name which hasn't been used before.
5.3) Then the database migration is applied to the database by running
"dotnet-ef database update" command.
