#nullable disable 
// For preventing the usage of ? (nullable) for reference types
// such as strings, arrays, classes, interfaces, etc.
// Should only be used with entity and model classes.

using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities; // namespace is used for grouping the classes according the their similar purposes,
                               // similar to package usage in Java

public class Resource
{
    // integer number data types: int, long, short, byte
    public int Id { get; set; } // value assignment required

    // data annotations can be used both in entities and models
    [Required] // can't be null
    [StringLength(50)] // must have maximum 50 characters
    public string Title { get; set; } // value assignment is not required (can be assigned as null)

    // can be null,
    // will have table column of type nvarchar(MAX), MAX: 4000 characters
    public string Content { get; set; } // value assignment is not required

    // decimal number data types: value assignment is required, can't be null
    // Way 1:
    // public float Score { get; set; } // example value assignment: = 1.2f or 1.2F
    // Way 2:
    // public double Score { get; set; } // example value assignment: = 1.2;
    // Way 3:
    public decimal Score { get; set; } // example value assignment: = 1.2m or 1.2M

    // nullable value types: value assignment is not required, can be null
    // Way 1:
    //public Nullable<DateTime> Date { get; set; }
    // Way 2:
    public DateTime? Date { get; set; }

    // class has a relationship for many to many tables relationship (UserResources table is the many side)
    public List<UserResource> UserResources { get; set; } // value assignment is not required, can be null
}
