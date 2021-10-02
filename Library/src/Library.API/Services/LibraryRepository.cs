using Library.API.DBContext;
using Library.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.API.Services
{
    public class LibraryRepository : ILibraryRepository, IDisposable
    {
        private LibraryContext _context;

        public LibraryRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            foreach (var book in author.Books)
            {
                book.Id = Guid.NewGuid();
            }

            _context.Authors.Add(author);
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            // always set the AuthorId to the passed-in authorId
            book.AuthorId = authorId;
            _context.Books.Add(book);
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            _context.Books.Remove(book);
        }

        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList<Author>();
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            return _context.Authors
                .Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList<Author>();
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (bookId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookId));
            }

            return _context.Books
              .FirstOrDefault(b => b.AuthorId == authorId && b.Id == bookId);
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Books
              .Where(b => b.AuthorId == authorId).OrderBy(b => b.Title).ToList<Book>();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}
