using Book_Reader.Data.Models;
using Book_Reader.Domain.BaseTypes;
using Book_Reader.Domain.Queries;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Book_Reader.Domain.Handlers.Queries.Library
{
    public class AuthorQuery : IRequest<AuthorQueryResponse>, IQuery
    {
        public AuthorQuery(int id)
        {
            Id = id;
        }

        public int Id { get;}
    }

    public class AuthorQueryResponse : QueryResponse
    {
        public Author Author { get; set; }
    }

    public interface IAuthorQueryHandler : IRequestHandler<AuthorQuery, AuthorQueryResponse>
    {
    }

    public class AuthorQueryHandler : IAuthorQueryHandler
    {
        private readonly ConnectionString _connectionString;

        public AuthorQueryHandler(ILogger<AuthorQueryHandler> logger, ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<AuthorQueryResponse> Handle(AuthorQuery query, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_connectionString.Db))
            {
                conn.Open();

                var builder = new SqlBuilder();
                var selector = builder.AddTemplate(@"
SELECT      A.Id AuthorId,
            A.FirstName,
            A.MiddleName,
            A.LastName,
			B.Id BookId,
			B.Title,
			B.IsAnthology
FROM        Authors A
left join   AuthorBook AB on A.Id = AB.AuthorsAuthorId
left join   Books B on AB.BooksBookId = B.Id
WHERE       A.Id = @Id
                    ");

                builder.AddParameters(new
                {
                    Id = query.Id

                });

                var result = (await conn.QueryAsync<Author, Book, Author>(selector.RawSql, (author, book) =>
                                        {
                                            if (book is not null)
                                                author.Books.Add(book);
                                            return author;
                                        },
                                        selector.Parameters,
                                        splitOn: "BookId"));

                var authorQueryResponse = new AuthorQueryResponse { Author = null };

                var author = result.GroupBy(a => a.AuthorId).Select(g =>
                {
                    var groupedAuthor = g.FirstOrDefault();
                    if (groupedAuthor.Books.Any())
                        groupedAuthor.Books = g.Select(b => b.Books.Single()).ToList();
                    return groupedAuthor;
                }).FirstOrDefault();
                return new AuthorQueryResponse { Author = author }; ;
            }
        }
    }
}
