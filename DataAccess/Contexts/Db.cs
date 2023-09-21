namespace DataAccess.Contexts;

public class Db : DbContext // Db is-an Entity Framework DbContext which will add
                            // database operations functionality through Entity Framework
{
    // DbSet typed properties are related to the database tables for CRUD operations
    public DbSet<User> Users { get; set; } // Users table

    public DbSet<Role> Roles { get; set; } // Roles table

    public DbSet<Resource> Resources { get; set; } // Resources table

    public DbSet<UserResource> UserResources { get; set; } // UserResources table
}
