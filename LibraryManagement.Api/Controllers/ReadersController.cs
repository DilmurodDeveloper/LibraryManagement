//-----------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Build Reliable Library Management Solutions
//-----------------------------------------------------------

using LibraryManagement.Api.Models.Foundations.Readers;
using LibraryManagement.Api.Models.Foundations.Readers.Exceptions;
using LibraryManagement.Api.Services.Foundations.Readers;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : RESTFulController
    {
        private readonly IReaderService readerService;

        public ReadersController(IReaderService readerService)
        {
            this.readerService = readerService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Reader>> PostReaderAsync(Reader reader)
        {
            try
            {
                Reader postedReader =
                    await this.readerService.AddReaderAsync(reader);

                return Created(postedReader);
            }
            catch (ReaderValidationException readerValidationException)
            {
                return BadRequest(readerValidationException.InnerException);
            }
            catch (ReaderDependencyValidationException readerDependencyValidationException)
                when (readerDependencyValidationException.InnerException is AlreadyExistsReaderException)
            {
                return Conflict(readerDependencyValidationException.InnerException);
            }
            catch (ReaderDependencyValidationException readerDependencyValidationException)
            {
                return BadRequest(readerDependencyValidationException.InnerException);
            }
            catch (ReaderDependencyException readerDependencyException)
            {
                return InternalServerError(readerDependencyException.InnerException);
            }
            catch (ReaderServiceException readerServiceException)
            {
                return InternalServerError(readerServiceException.InnerException);
            }
        }

        [HttpGet("all")]
        public ActionResult<IQueryable<Reader>> GetAllReaders()
        {
            try
            {
                IQueryable<Reader> allReaders =
                    this.readerService.RetrieveAllReaders();

                return Ok(allReaders);
            }
            catch (ReaderDependencyException readerDependencyException)
            {
                return InternalServerError(readerDependencyException.InnerException);
            }
            catch (ReaderServiceException readerServiceException)
            {
                return InternalServerError(readerServiceException.InnerException);
            }
        }

        [HttpGet("{readerId}")]
        public async ValueTask<ActionResult<Reader>> GetReaderByIdAsync(Guid readerId)
        {
            try
            {
                Reader getReaderById =
                    await this.readerService.RetrieveReaderByIdAsync(readerId);

                return Ok(getReaderById);
            }
            catch (ReaderValidationException readerValidationException)
                when (readerValidationException.InnerException is InvalidReaderException)
            {
                return BadRequest(readerValidationException.InnerException);
            }
            catch (ReaderValidationException readerValidationException)
                when (readerValidationException.InnerException is NotFoundReaderException)
            {
                return NotFound(readerValidationException.InnerException);
            }
            catch (ReaderDependencyException readerDependencyException)
            {
                return InternalServerError(readerDependencyException.InnerException);
            }
            catch (ReaderServiceException readerServiceException)
            {
                return InternalServerError(readerServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Reader>> PutReaderAsync(Reader reader)
        {
            try
            {
                Reader modifyReader =
                    await this.readerService.ModifyReaderAsync(reader);

                return Ok(modifyReader);
            }
            catch (ReaderValidationException readerValidationException)
                when (readerValidationException.InnerException is NotFoundReaderException)
            {
                return NotFound(readerValidationException.InnerException);
            }
            catch (ReaderValidationException readerValidationException)
            {
                return BadRequest(readerValidationException.InnerException);
            }
            catch (ReaderDependencyValidationException readerDependencyValidationException)
            {
                return Conflict(readerDependencyValidationException.InnerException);
            }
            catch (ReaderDependencyException readerDependencyException)
            {
                return InternalServerError(readerDependencyException.InnerException);
            }
            catch (ReaderServiceException readerServiceException)
            {
                return InternalServerError(readerServiceException.InnerException);
            }
        }
    }
}
