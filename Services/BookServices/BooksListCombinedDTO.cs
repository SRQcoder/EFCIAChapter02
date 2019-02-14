using System;
using System.Collections.Generic;
using System.Text;

namespace Services.BookServices
{
    public class BooksListCombinedDTO
    {
        public BooksListCombinedDTO(IEnumerable<BookListDTO> booksList)
        {
            BooksList = booksList;
        }

        public IEnumerable<BookListDTO> BooksList { get; private set; }
    }
}
