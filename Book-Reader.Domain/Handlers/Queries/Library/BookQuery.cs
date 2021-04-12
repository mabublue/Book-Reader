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
    public class BookQuery : IRequest<BookQueryResponse>, IQuery
    {
        public BookQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class BookQueryResponse : QueryResponse
    {
        public Book Book { get; set; }
    }

    public interface IBookQueryHandler : IRequestHandler<BookQuery, BookQueryResponse>
    {
    }

    public class BookQueryHandler : IBookQueryHandler
    {
        private readonly ConnectionString _connectionString;

        public BookQueryHandler(ILogger<BookQueryHandler> logger, ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<BookQueryResponse> Handle(BookQuery query, CancellationToken cancellationToken)
        {
            using (var conn = new SqlConnection(_connectionString.Db))
            {
                conn.Open();

                var builder = new SqlBuilder();
                var selector = builder.AddTemplate(@"
SELECT      B.Id BookId,
            B.Title,
			B.IsAnthology,
			A.Id AuthorId,
			A.FirstName,
			A.MiddleName,
			A.LastName
FROM        Books AS B
left join	AuthorBook AB on B.Id = AB.BooksBookId
left join   Authors A on AB.AuthorsAuthorId = A.Id
WHERE       B.Id = @Id
                    ");

                builder.AddParameters(new
                {
                    Id = query.Id

                });

                var result = (await conn.QueryAsync<Book, Author, Book>(selector.RawSql, (book, author) =>
                                        {
                                            if (author is not null)
                                                book.Authors.Add(author);
                                            return book;
                                        },
                                        selector.Parameters,
                                        splitOn: "AuthorId")).FirstOrDefault();

                return new BookQueryResponse { Book = result };
            }
        }
    }
}
