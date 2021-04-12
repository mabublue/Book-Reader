using Book_Reader.Data;
using Book_Reader.Data.Models;
using Book_Reader.Domain.BaseTypes;
using Book_Reader.Domain.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Book_Reader.Domain.Handlers.Commands.Library
{
    public class AddBookCommand : IRequest<CommandResponse>
    {
        public AddBookCommand(int authorId, string title)
        {
            AuthorId = authorId;
            Title = title;
        }

        public int AuthorId { get; }
        public string Title { get; }
    }

    public interface IAddBookCommandHandler : IRequestHandler<AddBookCommand, CommandResponse>
    {
    }

    public class AddBookCommandHandler : IAddBookCommandHandler
    {
        private readonly ILogger<AddBookCommandHandler> _logger;
        private readonly ApplicationDbContext _dbContext;

        public AddBookCommandHandler(ILogger<AddBookCommandHandler> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            using (_dbContext)
            {
                try
                {
                    var author = await _dbContext.Authors.FindAsync(request.AuthorId);
                    if (author is null)
                        throw new Exception($"Author id {request.AuthorId} Not found");
                    var book = new Book(request.Title);
                    if (author.Books is null)
                        author.Books = new List<Book>();
                    author.Books.Add(book);
                    await _dbContext.SaveChangesAsync();
                    response.Data = book.BookId;
                }
                catch (Exception ex)
                {
                    response.ValidationErrors.Add(new ValidationError($"Error Saving Book: {ex.Message}"));
                }
            }

            return response;
        }
    }
}
