using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Reader.Data.Models
{
    public class Book
    {
        public Book(int bookId, string title, bool isAnthology = false)
        {
            BookId = bookId;
            Title = title;
            IsAnthology = isAnthology;
            Authors = new List<Author>();
        }

        public Book(string title, bool isAnthology = false)
        {
            Title = title;
            IsAnthology = isAnthology;
            Authors = new List<Author>();
        }

        public Book()
        {
            Authors = new List<Author>();
        }

        [Column("Id")]
        public int BookId { get; private set; }
        [MaxLength(100)]
        public string Title { get; set; }
        public bool IsAnthology { get; set; }

        public ICollection<Author> Authors { get; set; }
    }
}
