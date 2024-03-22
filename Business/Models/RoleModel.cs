#nullable disable

using DataAccess.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
	public class RoleModel : Record
	{
        #region Properties copied from the related entity
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(5, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Name { get; set; }
        #endregion



        #region Extra properties required for the views
        [DisplayName("User Count")]
        public int UserCountOutput { get; set; }
        #endregion
    }
}
