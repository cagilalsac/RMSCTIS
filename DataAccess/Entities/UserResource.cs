#nullable disable

using DataAccess.Records.Bases;

namespace DataAccess.Entities;

public class UserResource : Record // many to many tables relationship relational entity
{
    // Way 1:
    //public int Id { get; set; }

    // Way 2: Id property should be inherited from the Record abstract base class



    // Tables many to many relationship.
    public int UserId { get; set; }

    // class has a relationship for many to many tables relationship (Users table is the one side)
    public User User { get; set; }

    public int ResourceId { get; set; }

    // class has a relationship for many to many tables relationship (Resources table is the one side)
    public Resource Resource { get; set; }
}
