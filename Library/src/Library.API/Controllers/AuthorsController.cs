using AutoMapper;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ILibraryRepository libraryRepository,
            IMapper mapper)
        {
            _libraryRepository = libraryRepository ??
                throw new ArgumentNullException(nameof(libraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors()
        {
            var authorsFromRepo = _libraryRepository.GetAuthors();
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var authorFromRepo = _libraryRepository.GetAuthor(authorId);
            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }
    }
}
