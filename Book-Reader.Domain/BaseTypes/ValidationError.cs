using System.Collections.Generic;

namespace Book_Reader.Domain.BaseTypes
{
    public class ValidationError : ValueObject<ValidationError>
    {
        public ValidationError()
        {
        }

        public ValidationError(string field,
                               string message)
        {
            Field = field;
            Message = message;
        }

        public ValidationError(
            string message) : this(string.Empty, message)
        {
        }

        public string Field { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Field) ? Message : $"{Field}: {Message}";
        }

        public List<ValidationError> ToList()
        {
            return new List<ValidationError>
                   {
                       this
                   };
        }
    }
}
