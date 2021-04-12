using Book_Reader.Data;
using Book_Reader.Data.Models;
using Book_Reader.Domain.BaseTypes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Book_Reader.Domain.Commands.Library
{
    public class AddAuthorCommand : IRequest<CommandResponse>
    {
        public AddAuthorCommand(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }
    }

    public interface IAddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, CommandResponse>
    {
    }

    public class AddAuthorCommandHandler : IAddAuthorCommandHandler
    {
        private readonly ILogger<AddAuthorCommandHandler> _logger;
        private readonly ApplicationDbContext _dbContext;

        public AddAuthorCommandHandler(ILogger<AddAuthorCommandHandler> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            using (_dbContext)
            {
                try
                {
                    var result = await _dbContext.Authors.AddAsync(new Author(request.FirstName, request.MiddleName, request.LastName));
                    await _dbContext.SaveChangesAsync();
                    response.Data = result.Entity.AuthorId;
                }
                catch (Exception ex)
                {
                    response.ValidationErrors.Add(new ValidationError($"Error Saving Author: {ex.Message}"));
                }
            }

            return response;
        }
    }
}
