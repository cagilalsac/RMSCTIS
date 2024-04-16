using Business;
using Business.Services;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVC.Settings;

var builder = WebApplication.CreateBuilder(args);

#region Localization 
// Way 1: Managing localization in the MvcControllerBase is a better way
//List<CultureInfo> cultures = new List<CultureInfo>()
//{
//    new CultureInfo("en-US") // for Turkish, "tr-TR" constructor parameter must be used,
//                             // this will be our default culture for our MVC Web Application
//};

//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
//    options.SupportedCultures = cultures;
//    options.SupportedUICultures = cultures;
//});
#endregion

#region AppSettings
// Application configuration settings can be read from sections such as AppSettings in the appsettings.json file.
// Way 1:
//var section = builder.Configuration.GetSection("AppSettings");
// Way 2:
var section = builder.Configuration.GetSection(nameof(MVC.Settings.AppSettings)); // AppSettings class can also be used by
                                                                                  // adding "using MVC.Settings;" directive
section.Bind(new MVC.Settings.AppSettings()); // this method will fill the only one instance of type AppSettings with data of the
                                              // AppSettings section of the appsettings.json file
#endregion

// Add services to the container.
#region IoC (Inversion of Control) Container
// IoC Container manages the initialization operations of the objects which are
// injected to classes by Constructor Injection. Alternatively Autofac or Ninject
// libraries can also be used under the Business layer.
// "Unable to reslove service..." exceptions should be resolved here.
builder.Services.AddDbContext<Db>(options => options // options used in the AddDbContext method is a delegate
                                                     // of type DbContextOptionsBuilder. This delegate
                                                     // is also called an Action which doesn't return anything.
                                                     // Actions are generally used for configurations.
                                                     // Through the Actions properties or methods
                                                     // (such as UseMySql method) can be used therefore
                                                     // the Actions can provide these to the method
                                                     // which they are used in.
                                                     // We are saying that use MySQL with the provided
                                                     // connection string through the options Action
                                                     // to the AddDbContext method which uses the type of Db,
                                                     // therefore we should provide the type of our class
                                                     // inherited from the DbContext as the generic type
                                                     // for AddDbContext method.
    //.UseMySQL("server=127.0.0.1;database=test;user id=std;password=;")); // // we are going to use Microsoft SQL Server LocalDB from now on
    //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=RMSCTISDB;trusted_connection=true;"));
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // appsettings.json -> ConnectionString section
    // Connection string should be read from the ConnectionString section of the appsettings.json file by using the
    // connection string name (DefaultConnection).

// AddScoped: The object's reference (usually an interface or abstract class) is used to instantiate an object
// through constructor injection when a request is received and the object lives until the response is returned.
// AddDbContext is a scoped method by default.
// AddSingleton: The object's reference (usually an interface or abstract class) is used to instantiate an object
// through constructor injection when a request is received and the only one object lives throughout
// the application's lifetime (as long as the application is running and not stopped or restarted for example
// via IIS: Internet Information Services or Kestrel web server applications).
// AddTransient: The object is instantiated every time whenever a constructor injection through the
// object's reference (usually an interface or abstract class) is used, independent from the request.
// Generally the AddScoped method is used.
// Way 1:
// builder.Services.AddSingleton<IUserService, UserService>();
// Way 2:
// builder.Services.AddTransient<IUserService, UserService>();
// Way 3:
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
#endregion

#region Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    // We are adding authentication to the project using default Cookie authentication.

    .AddCookie(config =>
    // We configure the cookie to be created through the config action delegate; unlike func delegates, 
	// action delegates do not return a result and they are generally used for configuration operations as seen here.

    {
        //config.LoginPath = "/Account/Home/Login";
        // since we changed the route of Account area's Home controller's Login action, we should set:
        config.LoginPath = "/Login";
        // If an operation is attempted without logging into the system, redirect the user to the 
        // Account area's Home controller's Login action.

        //config.AccessDeniedPath = "/Account/Home/AccessDenied";
        // since we changed the route of Account area's Home controller's AccessDenied action, we should set:
        config.AccessDeniedPath = "/AccessDenied";
        // If an unauthorized operation is attempted after logging into the system, redirect the user to the 
        // Account area's Home controller's AccessDenied action.

        // Way 1:
        //config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        // Way 2: getting minute value from appsettings.json
        config.ExpireTimeSpan = TimeSpan.FromMinutes(AppSettings.CookieExpirationInMinutes);
        // Allow the cookie created after logging into the system to be valid for 30 minutes.

        config.SlidingExpiration = true;
        // When SlidingExpiration is set to true, the user's cookie expiration is extended by a specific duration 
		// every time they perform an action in the system. If set to false, the user's cookie lifespan ends after 
		// a specific duration after the initial login, requiring them to log in again.
    });
#endregion

#region Session
builder.Services.AddSession(config => // we add session to the services through the config action delegate
{
    config.IdleTimeout = TimeSpan.FromMinutes(30); // the session data will be kept during this value as long as the application user interacts,
                                                   // default is 20 minutes
    config.IOTimeout = Timeout.InfiniteTimeSpan; // we set this value for using the only time out value of IdleTimeout
});
#endregion

builder.Services.AddControllersWithViews();

var app = builder.Build();

#region Localization
// Way 1: Managing localization in the MvcControllerBase is a better way
//app.UseRequestLocalization(new RequestLocalizationOptions()
//{
//    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
//    SupportedCultures = cultures,
//    SupportedUICultures = cultures
//});
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region Authentication
app.UseAuthentication();
#endregion

app.UseAuthorization();

#region Session
app.UseSession();
#endregion

// custom conventional routes can be added before the default route so that here for example instead of calling "Home/Privacy" route,
// "About" route can be called when necessary
app.MapControllerRoute(name: "about",
    pattern: "about",
    defaults: new { controller = "Home", action = "Privacy" });

#region Area
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
