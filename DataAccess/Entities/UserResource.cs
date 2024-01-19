#nullable disable

namespace DataAccess.Entities;

public class UserResource // many to many tables relationship relational entity
{
    public int Id { get; set; }

    

    // Tables many to many relationship.
    public int UserId { get; set; }

    // class has a relationship for many to many tables relationship (Users table is the one side)
    public User User { get; set; }

    public int ResourceId { get; set; }

    // class has a relationship for many to many tables relationship (Resources table is the one side)
    public Resource Resource { get; set; }
}
