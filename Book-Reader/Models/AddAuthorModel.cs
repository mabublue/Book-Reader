using System.ComponentModel.DataAnnotations;

namespace Book_Reader.Models
{
    public class AddAuthorModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
