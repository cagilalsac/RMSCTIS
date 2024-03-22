#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class FavoriteModel // we don't need to inherit from the Record abstract base class
                               // since this model is not entity related
    {
        public int ResourceId { get; set; } // the id of the resource added to favorites
        public string UserName { get; set; } // the application user's user name

        [DisplayName("Resource Title")]
        public string ResourceTitle { get; set; } // the title of the resource added to favorites

        public decimal ResourceScore { get; set; } // the score of the resource added to favorites

        [DisplayName("Resource Score")]
        public string ResourceScoreOutput { get; set; } // the number formatted score of the resource added to favorites
    }
}
