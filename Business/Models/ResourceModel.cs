#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class ResourceModel
    {
        #region Properties copied from the related entity
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }

        public string Content { get; set; }

		// if we use decimal, model validation will show the result "The value '' is invalid." for this property,
		// if we use decimal? with Required, model validation will show the result "Score is required!"
		//public decimal Score { get; set; } 
        [Required(ErrorMessage = "{0} is required!")]
        public decimal? Score { get; set; } 
        
        public DateTime? Date { get; set; }
        #endregion



        #region Extra properties required for the views
        [DisplayName("Score")]
        public string ScoreOutput { get; set; }

        [DisplayName("Date")]
        public string DateOutput { get; set; }

		[DisplayName("User Count")]
		public int UserCountOutput { get; set; }

        [DisplayName("Users")]
        //[Required(ErrorMessage = "{0} must be selected!")] // if users must be selected, uncomment this line
        public List<int> UserIdsInput { get; set; }

        [DisplayName("Users")]
        public string UserNamesOutput { get; set; }
        #endregion
    }
}
