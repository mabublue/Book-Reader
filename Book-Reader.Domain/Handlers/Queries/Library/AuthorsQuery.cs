using Book_Reader.Data.Models;
using Book_Reader.Domain.BaseTypes;
using Book_Reader.Domain.Queries;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Book_Reader.Domain.Handlers.Queries.Library
{
    public class AuthorsQuery : IRequest<AuthorsQueryResponse>, IQuery
    {
    }

    public class AuthorsQueryResponse : QueryResponse
    {
        public IEnumerable<Author> Authors { get; set; }
    }

    public interface IAuthorsQueryHandler : IRequestHandler<AuthorsQuery, AuthorsQueryResponse>
    {
    }

    public class AuthorsQueryHandler : IAuthorsQueryHandler
    {
        private readonly ConnectionString _connectionString;

        public AuthorsQueryHandler(ILogger<AuthorsQueryHandler> logger, ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<AuthorsQueryResponse> Handle(AuthorsQuery query, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_connectionString.Db))
            {
                conn.Open();

                var builder = new SqlBuilder();
                var selector = builder.AddTemplate(@"
SELECT      A.Id AuthorId,
            A.FirstNAme,
            A.MiddleName,
            A.LastName
FROM        Authors A
                    ");
                var result = await conn.QueryAsync<Author>(selector.RawSql);

                return new AuthorsQueryResponse {Authors = result};
            }
        }
    }
}
