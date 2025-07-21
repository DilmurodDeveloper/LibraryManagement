//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.ReaderBooks;
using LibraryManagement.Api.Services.Foundations.ReaderBooks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderBooksController : RESTFulController
    {
        private readonly IReaderBookService readerBookService;

        public ReaderBooksController(IReaderBookService readerBookService) =>
            this.readerBookService = readerBookService;

        [HttpGet("{readerId}")]
        public async ValueTask<ActionResult<ReaderBook>> GetReaderBookByIdAsync(Guid readerId)
        {
            try
            {
                ReaderBook readerBook =
                    await this.readerBookService.RetrieveReaderBookByIdAsync(readerId);

                return Ok(readerBook);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
    }
}
