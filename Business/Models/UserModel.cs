#nullable disable // For preventing the usage of ? (nullable) for reference types
                  // such as strings, arrays, classes, interfaces, etc.
                  // Should only be used with entity and model classes.

using DataAccess.Enums;

namespace Business;

public class UserModel
{
    #region Properties copied from the related entity
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool IsActive { get; set; } 
    
    public Statuses Status { get; set; }
    
    public int RoleId { get; set; }
    #endregion
}
