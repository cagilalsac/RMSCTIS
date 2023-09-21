#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class UserResource // many to many tables relationship relational entity
{
    // Key and Column are called C# Attributes and may be used on top of properties,
    // fields (class variables), behaviors (methods) or classes.
    // For entity and models C# Attributes are called Data Annotations and they gain new features to the
    // properties, fields, behaviors or classes by the implementation of Aspect Oriented Programming.
    // Tables many to many relationship.
    [Key] // Primary key data annotation for Entity Framework
    [Column(Order = 0)] // The order of the composite primary keys through the indices data annotation, 0: first primary key
    public int UserId { get; set; }

    // class has a relationship for many to many tables relationship (Users table is the one side)
    public User User { get; set; }

    [Key] // Primary key data annotation for Entity Framework
    [Column(Order = 1)] // The order of the composite primary keys through the indices data annotation, 1: second primary key
    public int ResourceId { get; set; }

    // class has a relationship for many to many tables relationship (Resources table is the one side)
    public Resource Resource { get; set; }
}
