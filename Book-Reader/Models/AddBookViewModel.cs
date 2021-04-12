using Microsoft.AspNetCore.Mvc;

namespace Book_Reader.Models
{
    public class AddBookViewModel
    {
        [HiddenInput]
        public int AuthorId { get; set; }
    }
}
