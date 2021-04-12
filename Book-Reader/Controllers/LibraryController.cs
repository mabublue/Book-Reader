using Book_Reader.Domain.Commands.Library;
using Book_Reader.Domain.Handlers.Commands.Library;
using Book_Reader.Domain.Handlers.Queries.Library;
using Book_Reader.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Book_Reader.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public LibraryController(ILogger<LibraryController> logger,
                                 IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Authors");
        }

        // Authors

        [HttpGet]
        public async Task<IActionResult> Authors()
        {
            var query = new AuthorsQuery();
            var queryResult = await _mediator.Send(query);

            var model = new AuthorsViewModel()
            {
                Authors = queryResult.Authors
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Author(int authorId)
        {
            var query = new AuthorQuery(authorId);
            var queryResult = await _mediator.Send(query);

            var model = new AuthorViewModel()
            {
                Author = queryResult.Author
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult AddAuthor(string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AddAuthorModel vm)
        {
            var command = new AddAuthorCommand(vm.FirstName, vm.MiddleName, vm.LastName);
            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(data => RedirectToAction("Author", new { authorId = data }),
                                 errors => RedirectToAction("AddAuthor", new { message = "There were errors" }));
        }

        [HttpPost]
        public IActionResult AddAuthor_Cancel(AddAuthorModel vm)
        {
            return RedirectToAction("Authors");
        }

        // Books

        [HttpGet]
        public async Task<IActionResult> Book(int bookId)
        {
            var query = new BookQuery(bookId);
            var queryResult = await _mediator.Send(query);

            var model = new BookViewModel()
            {
                Book = queryResult.Book
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult AddBook(int authorId, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                ViewBag.Message = message;

            var addBookModel = new AddBookModel()
            {
                AuthorId = authorId
            };

            return View(addBookModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookModel vm)
        {
            var command = new AddBookCommand(vm.AuthorId, vm.Title);
            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(data => RedirectToAction("Book", new { bookId = data }),
                                 errors => RedirectToAction("AddBook", new { message = "There were errors" }));
        }

        [HttpPost]
        public IActionResult AddBook_Cancel(AddBookModel vm)
        {
            return RedirectToAction("Author", new { authorId = vm.AuthorId });
        }
    }
}
