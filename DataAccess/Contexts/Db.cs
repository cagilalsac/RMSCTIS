using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public class Db : DbContext // Db is-an Entity Framework DbContext which will add
                            // database operations functionality through Entity Framework
{
    // DbSet typed properties are related to the database tables for CRUD operations
    public DbSet<User> Users { get; set; } // Users table

    public DbSet<Role> Roles { get; set; } // Roles table

    public DbSet<Resource> Resources { get; set; } // Resources table

    public DbSet<UserResource> UserResources { get; set; } // UserResources table

    // Dependency (Constructor) Injection:
    // In the Program.cs file of the MVC Web Application project we will manage
    // the initialization operations of the objects of type Db which are injected
    // in other classes through their constructors (such as Service classes)
    // in the IoC (Inversion of Control) Container.
    // options parameter which for example contains the database connection string is provided
    // from the IoC Container in the Program.cs file through a delegate (options) for the AddDbContext
    // method of the builder object's services collection. Therefore the options parameter
    // is sent to the constructor of the base (parent, super in Java) inherited class (DbContext)
    // so that we can manage database operations through our sub (child) class (Db) using the
    // connetion string provided.
    public Db(DbContextOptions options) : base(options)
    {
    }
	
    // We need to override the OnModelCreating virtual method in the base (DbContext) class to
    // configure many to many relationships. We can also configure about anything related
    // to the database structure under this method, but for easier development we configured
    // some in the entities using data annotations. This way is not recommended by SOLID Principles.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // new { e.UserId, e.ResourceId }: anonymous type used for using more then one property with the delegate
        modelBuilder.Entity<UserResource>().HasKey(e =>  new { e.UserId, e.ResourceId }); 
    }
}
