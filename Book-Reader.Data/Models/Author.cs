using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Reader.Data.Models
{
    public class Author
    {
        public Author(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Books = new List<Book>();
        }

        public Author()
        {
            // For EF Core
            Books = new List<Book>();
        }

        [Column("Id")]
        public int AuthorId { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; }

        [NotMapped]
        public string FullName => GetFullName();

        private string GetFullName()
        {
            return $"{(string.IsNullOrWhiteSpace(FirstName) ? "" : $"{FirstName} ")}{(string.IsNullOrWhiteSpace(MiddleName) ? "" : $"{MiddleName} ")}{LastName}";
        }
    }
}
