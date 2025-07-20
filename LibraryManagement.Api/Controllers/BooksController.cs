//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Books;
using LibraryManagement.Api.Models.Foundations.Books.Exceptions;
using LibraryManagement.Api.Services.Foundations.Books;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : RESTFulController
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Book>> PostBookAsync(Book book)
        {
            try
            {
                Book postedBook = await this.bookService.AddBookAsync(book);

                return Created(postedBook);
            }
            catch (BookValidationException bookValidationException)
            {
                return BadRequest(bookValidationException.InnerException);
            }
            catch (BookDependencyValidationException bookDependencyValidationException)
                when (bookDependencyValidationException.InnerException is AlreadyExistsBookException)
            {
                return Conflict(bookDependencyValidationException.InnerException);
            }
            catch (BookDependencyValidationException bookDependencyValidationException)
            {
                return BadRequest(bookDependencyValidationException.InnerException);
            }
            catch (BookDependencyException bookDependencyException)
            {
                return InternalServerError(bookDependencyException.InnerException);
            }
            catch (BookServiceException bookServiceException)
            {
                return InternalServerError(bookServiceException.InnerException);
            }
        }

        [HttpGet("all")]
        public ActionResult<IQueryable<Book>> GetAllBooks()
        {
            try
            {
                IQueryable<Book> allBooks = this.bookService.RetrieveAllBooks();

                return Ok(allBooks);
            }
            catch (BookDependencyException bookDependencyException)
            {
                return InternalServerError(bookDependencyException.InnerException);
            }
            catch (BookServiceException bookServiceException)
            {
                return InternalServerError(bookServiceException.InnerException);
            }
        }

        [HttpGet("{bookId}")]
        public async ValueTask<ActionResult<Book>> GetBookByIdAsync(Guid bookId)
        {
            try
            {
                Book maybeBook =
                    await this.bookService.RetrieveBookByIdAsync(bookId);

                return Ok(maybeBook);
            }
            catch (BookValidationException bookValidationException)
                when (bookValidationException.InnerException is InvalidBookException)
            {
                return BadRequest(bookValidationException.InnerException);
            }
            catch (BookValidationException bookValidationException)
                when (bookValidationException.InnerException is NotFoundBookException)
            {
                return NotFound(bookValidationException.InnerException);
            }
            catch (BookDependencyException bookDependencyException)
            {
                return InternalServerError(bookDependencyException.InnerException);
            }
            catch (BookServiceException bookServiceException)
            {
                return InternalServerError(bookServiceException.InnerException);
            }
        }
    }
}
