#nullable disable

using System.ComponentModel;

namespace Business.Models
{
	public class RoleModel
	{
		#region Properties copied from the related entity
		public int Id { get; set; } 

		public string Name { get; set; }
        #endregion



        #region Extra properties required for the views
        [DisplayName("User Count")]
        public int UserCountOutput { get; set; }
        #endregion
    }
}
