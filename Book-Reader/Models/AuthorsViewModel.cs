using Book_Reader.Data.Models;
using System.Collections.Generic;

namespace Book_Reader.Models
{
    public class AuthorsViewModel
    {
        public IEnumerable<Author> Authors { get; set; }
    }
}
