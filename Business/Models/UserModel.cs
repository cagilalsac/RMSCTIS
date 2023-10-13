#nullable disable 
// For preventing the usage of ? (nullable) for reference types
// such as strings, arrays, classes, interfaces, etc.
// Should only be used with entity and model classes.

using System.ComponentModel;
using DataAccess.Enums;

namespace Business;

public class UserModel
{
    #region Properties copied from the related entity
    public int Id { get; set; }

    [DisplayName("User Name")] // DisplayName data annotation (attribute) for "DisplayNameFor" HTML Helper
                               // or "label asp-for" Tag Helper in views.
                               // If no DisplayName is defined, "UserName" will be written, else "User Name"
                               // will be written in the view.
    public string UserName { get; set; }

    public string Password { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; } 
    
    public Statuses Status { get; set; }
    
    [DisplayName("Role")]
    public int RoleId { get; set; }
    #endregion

    #region Extra properties required for the views
    [DisplayName("Active")]
    public string IsActiveOutput { get; set; }

    [DisplayName("Role")]
    public string RoleNameOutput { get; set; }
    #endregion
}
