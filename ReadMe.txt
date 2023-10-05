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

2) In the DataAccess layer, MySql.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Tools
and Microsoft.EntityFrameworkCore.SqlServer packages are downloaded through NuGet. 
The .NET version you are using determines which packages with that version number should be 
downloaded from NuGet. For example, if you are using .NET 7, you should look for packages compatible 
with .NET 7 and install their latest versions. The version number typically corresponds to the major 
version of .NET, so in this case, you should search for packages starting with version 7.x.x.

3) In the DataAccess layer, a context class derived from the DbContext class is created
including the DbSets of type entites of our project. Then a parameterized constructor is created 
which will accept an object of type DbContextOptions containing the connection string information. 
Afterward, in the MVC layer, dependency injection for the class derived from the DbContext 
is configured in the IoC (Inversion of Control) Container in the Program.cs file.

4) In the MVC layer, Microsoft.EntityFrameworkCore.Design package is downloaded through NuGet.
If you are using Visual Studio instead of Visual Studio Code, you should set the MVC project 
as the start up project.

5) For Entity Framework Code First migration terminal commands:
5.1) First "dotnet tool install --global dotnet-ef" command should be run.
5.2) Then you need to change directory to the DataAccess folder entering "cd dataaccess".
A new database migration (version) can be added with "dotnet-ef --startup-project ../MVC/ migrations add v1" 
command, v1 can be any unique name which hasn't been used before.
5.3) Then the database migration is applied to the database by running 
"dotnet-ef --startup-project ../MVC/ database update" command.

6) For scaffolding:
6.1) In the MVC layer, Microsoft.VisualStudio.Web.CodeGeneration.Design package is downloaded.
6.2) The Templates folder under the MVC project folder should be copied to your MVC Web Application project folder.
6.3) In the DataAccess layer, a class called DbFactory should be created and only connection string of the database 
should be modified in the CreateDbContext method. This class will be used for scaffolding operations (recommended to use).
6.4) "dotnet tool install -g dotnet-aspnet-codegenerator" command should be run in the terminal.
6.5) Change directory to MVC by "cd mvc" terminal command, then for creating the Users controller, its actions and views: 
"dotnet aspnet-codegenerator controller -name UsersController --relativeFolderPath Controllers --useDefaultLayout --dataContext Db --model User"
command should be run in the terminal. Now you can see UsersController under the Controllers folder and Users view folder under the Views folder
of the MVC project.

7.1) In the Business layer, Services folder is created and under this folder service classes with their interfaces 
for CRUD and other operations are created. Since this is a small project, the interfaces and classes of the
services will be in the same file but generally a Bases folder is created under the Services folder and
abstract classes or interfaces are created in the Base folder and concrete classes which inherit the abstract
classes or implement the interfaces are created in the Services folder.
Services are used for conversion of database table raw data from entities to the models and like versa 
which will be used for user interaction in the MVC project.

The data flow from the user to the database or from database to the user can be shown as below:
View <-> Controller (Action) <-> Service (Model) <-> DbContext (Entity) <-> Database

7.2) In the Business layer, Models folder is created and under this folder models for entities are created
firstly by copying the primitive type properties (not reference type) to the model class 
(or this can be defined as copying the properties which have columns in the entity related table from 
entity class to the model class).
