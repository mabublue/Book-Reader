using Microsoft.AspNetCore.Mvc;

namespace Book_Reader.Models
{
    public class AddBookModel
    {
        [HiddenInput]
        public int AuthorId { get; set; }
        public string Title { get; set; }
    }
}
