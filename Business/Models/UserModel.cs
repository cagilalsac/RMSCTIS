#nullable disable 
// For preventing the usage of ? (nullable) for reference types
// such as strings, arrays, classes, interfaces, etc.
// Should only be used with entity and model classes.

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

	// Way 1: all data annotations can be used with or without ErrorMessage
	//[Required]
	// Way 2: overriding default validation message
	[Required(ErrorMessage = "{0} is required!")] // if there is a display name, {0} is the display name else property name
    // Way 1:
    //[StringLength(10)] // the UserName input can be maximum 10 characters
    // Way 2: overriding default validation message
    //[StringLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
    // Way 3:
    //[StringLength(10, MinimumLength = 3)] // the UserName input can be minimum 3 and maximum 10 characters
    // Way 4: overriding default validation message
    [StringLength(10, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
    // Way 5:
    [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
    [MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
    public string UserName { get; set; }

	[Required(ErrorMessage = "{0} is required!")]
	[StringLength(8, MinimumLength = 5, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
	public string Password { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; } 
    
    public Statuses Status { get; set; }

    [DisplayName("Role")]
    // Way 1: if "The value '' is invalid." validation message is wanted to be shown
    //public int RoleId { get; set; }
    // Way 2: if the validation message is wanted to be customized
    [Required(ErrorMessage = "{0} is required!")]
    public int? RoleId { get; set; }
    #endregion

    #region Extra properties required for the views
    [DisplayName("Active")]
    public string IsActiveOutput { get; set; }

    [DisplayName("Role")]
    public string RoleNameOutput { get; set; }
    #endregion
}
