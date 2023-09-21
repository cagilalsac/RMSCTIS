#nullable disable

using DataAccess.Enums; // since Statuses enum is under different folder therefore different namespace,
                        // we need to include the namespace with using (similar to import in Java)

namespace DataAccess.Entities;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool IsActive { get; set; } // boolean data type which can have the value of true or false

    // Way 1: we can use the Statuses enum type through its namespace
    // public DataAccess.Enums.Statuses Status { get; set; }
    // Way 2: we can use the Statuses enum type directly after adding the using line by its namespace on top of the file
    public Statuses Status { get; set; }

    // class has-a relationship (Roles table is the one side)
    public Role Role { get; set; }
    
    // tables one to many relationship 
    public int RoleId { get; set; }

    // class has a relationship for many to many tables relationship (UserResources relational table is the many side)
    public List<UserResource> UserResources { get; set; }
}
