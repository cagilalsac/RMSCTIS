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



After creating the 
I) MVC (ASP.NET Core Web App Model-View-Controller)
by giving the solution name ResourceManagementSystem and project name MVC,
if Visual Studio is used Place solution and project in the same directory
option should not be checked, and project creation should be completed by 
selecting .NET 7.0 framework, None Authentication type, checking Configure for HTTPS, 
not checking Enable Docker and not checking Do not use top-level statements,
II) Business (Class Library) and 
III) DataAccess (Class Library) projects, 
the solution is built, and the following references are added to the projects
by right clicking the mouse on the project in the Solution Explorer and 
selecting Add Project Reference:

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
If you are using Visual Studio instead of Visual Studio Code, right mouse click on the project
in the Solution Explorer and click Manage NuGet Packages. Packages can be installed
after being searched in the Browse tab.

3) In the DataAccess layer, a context class derived from the DbContext class is created
including the DbSets of type entites of our project. Then a parameterized constructor is created 
which will accept an object of type DbContextOptions containing the connection string information. 
Afterward, in the MVC layer, dependency injection for the class derived from the DbContext 
is configured in the IoC (Inversion of Control) Container in the Program.cs file.

4) In the MVC layer, Microsoft.EntityFrameworkCore.Design package is downloaded through NuGet.
If you are using Visual Studio instead of Visual Studio Code, you should set the MVC project 
as the start up project.

5.1) For Entity Framework Code First migration Visual Studio Code terminal commands:
5.1.1) First "dotnet tool install --global dotnet-ef" command should be run.
5.1.2) Then you need to change directory to the DataAccess folder entering "cd dataaccess".
A new database migration (version) can be added with "dotnet-ef --startup-project ../MVC/ migrations add v1" 
command, v1 can be any unique name which hasn't been used before.
5.1.3) Then the database migration is applied to the database by running 
"dotnet-ef --startup-project ../MVC/ database update" command.
5.2) For Entity Framework Code First migration Visual Studio Package Manager Console commands:
5.2.1) Click Tools menu item in the Visual Studio menu then NuGet Package Manager -> Package Manager Console.
5.2.2) Select the Default project where the DbContext class is created, which is DataAccess in our project.
5.2.3) A new database migration (version) can be added with "add-migration v1" command,
v1 can be any unique name which hasn't been used before.
5.2.4) Then the database migration is applied to the database by running 
"update-database" command.

6) For scaffolding:
6.1) In the MVC layer, Microsoft.VisualStudio.Web.CodeGeneration.Design package is downloaded.
6.2) The Templates folder under the MVC project folder should be copied to your MVC Web Application project folder.
6.3) In the DataAccess layer, a class called DbFactory should be created and only connection string of the database 
should be modified in the CreateDbContext method. This class will be used for scaffolding operations (recommended to use).
6.4.1.1) For Visual Studio Code scaffolding terminal commands:
6.4.1.2) "dotnet tool install -g dotnet-aspnet-codegenerator" command should be run in the terminal.
6.4.1.3) Change directory to MVC by "cd mvc" terminal command, then for creating the Users controller, its actions and views: 
"dotnet aspnet-codegenerator controller -name UsersController --relativeFolderPath Controllers --useDefaultLayout --dataContext Db --model User"
command should be run in the terminal. 
6.4.2.1) For Visual Studio scaffolding:
6.4.2.2) Right-click on the /Controllers folder within the MVC project.
6.4.2.3) Select Add and then Controller.
6.4.2.4) In the dialog that appears, select MVC Controller with views, using Entity Framework.
6.4.2.5) Choose the Model class (always an entity class, in this case User).
6.4.2.6) Select the DbContext class which should be a class inheriting from the DbContext (in this case Db).
6.4.2.7) If you want to generate views using the selected entity model, check the Generate views option.
6.4.2.8) If you want to use jQuery Validation for client-side validation in the views, check Reference script libraries. 
If not checked, validation will be done server-side and client-side validation should be added manually for create 
and edit operations if desired. Do not check this option.
6.4.2.9) If you want the generated views to use a layout view defined in _ViewStart.cshtml (typically /Views/Shared/_Layout.cshtml), 
check Use a layout page. You can leave the text box below empty because it is defined in _ViewStart.cshtml in our project.
6.4.2.10) Optionally, you can change the Controller name to specify a different name for the generated controller.
6.5) Now you can see UsersController under the Controllers folder and Users view folder under the Views folder
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

7.2) In the MVC layer, in the Program.cs file, the created service inversion of control must be added
into the IoC Container.

7.3) In the Business layer, Models folder is created and under this folder models for entities are created
firstly by copying the primitive type properties (not reference type) to the model class 
(or this can be defined as copying the properties which have columns in the entity related table from 
entity class to the model class).

7.4) If the view that the model will be used requires formatted or extra data, new properties
should be added to the model class and set in the related service's methods.

7.5) Within the model classes, data annotations (attributes) such as DisplayName, Required, StringLength, etc. 
must be defined above the properties which will be used in the related views.

Here is a list of some data annotations for entities and models:

Note: Data annotations are used for simple validations in models based on model data only. 
For instance, if validation is required based on a table in the database, it should be done in service classes.

Note: Data annotations of entities are used for database table strcutures.

DisplayName: Used for setting the display name to be shown in the views or to be used in other data annotation error messages.
Key (Entity): Indicates that the property is a primary key and creates it as an automatically incremented column in the table.
Required (Entity and Model): Indicates that the property value is required.
Column (Entity): Specifies settings related to the property's column in the database table, such as the column name, data type, 
and order (used for multiple keys).
DataType (Model): Used to specify the data type of the property. For example Text, Date, Time, DateTime, Currency, EmailAddress, 
PhoneNumber, Password, etc.
ReadOnly (Model): Used to make the property read-only therefore its value can't be set.
DisplayFormat (Model): Specifies the format to be used in text data representation, often used for formatting operations for 
date, decimal numbers, etc.
Table (Entity): Used to change the name of the table that will be created in the database.
StringLength (Entity and Model): Used to specify the maximum character length for text properties.
MinLength (Model): Used to specify the minimum character length for text properties.
MaxLength (Model): Used to specify the maximum character length for text properties.
Compare (Model): Used to compare data of the property with another property specified.
RegularExpression (Model): A validation pattern that can be used for more detailed data validation.
Range (Model): Used to specify a range for numerical values.
EmailAddress (Model): Used to ensure that the property data is in e-mail format.
Phone (Model): Used to ensure that the property data is in phone number format.
NotMapped (Entity): Used to prevent the property from being created as a column in the corresponding table in the database.
JsonIgnore (Model): Ensures that the property is not included in the generated JSON data.

7.6) In the MVC layer, the views should be edited according to the view's model such that the model properties 
for user interaction should be used.
The default MVC route is: controller/action/id? (? means optional), for example a request to the
Users controller's GetList action can be sent by writing https://localhost:7275/Users/GetList in the address
of a browser or creating a link in a view using the HTML anchor tag such that <a href="/Users/GetList">User List</a>.
Instead of HTML, HTML Helpers or Tag Helpers should be used.
For example, another request to the Users controller's Details action can be sent by writing
https://localhost:7275/Users/Details/6 in the address of a browser or creating a link in a view using the 
HTML anchor tag such that <a href="/Users/Details/6">Details</a>.

7.7) By scaffolding defaults, the Create, Edit and Delete actions have two overloaded methods.
For example, for create and edit operations the first action is the get action which returns the view containing
the HTML form to the user for editing or entering new data. If required, extra data not related to the model can be
carried to the view by using ViewBag or ViewData. The second action for create and edit operations is the 
post action marked with the HttpPost action method which is used for sending form data to the action 
using the related view's model as parameter. Inside these post actions, after the model data is validated by
ModelState, database create and update operations are performed by the related services. If there are
validation errors catched by the ModelState through model's data annotations, the related view is returned with
the model containing user entered data. For delete operation, the get action returns the model data to the view
to show the details. If the submit button within the form is clicked, the hidden input for id value is sent to the
delete operation's post action and catched by id action parameter, then delete operation is performed through 
the related service by using the id value.