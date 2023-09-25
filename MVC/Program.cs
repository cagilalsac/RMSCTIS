using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
    .UseMySQL("server=127.0.0.1;database=RMSDB;user id=std;password=;"));
#endregion

builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
