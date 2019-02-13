using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.BookServices
{
    public class ListBookService
    {
        private readonly EFCoreContext Context;

        public ListBookService(EFCoreContext context)
        {
            Context = context;
        }

        public IQueryable<BookListDTO> SortFilterPage(SortFilterPageOptions options)
        {
            var booksQuery = Context.Books.AsNoTracking();

            return null; //booksQuery;
        }
    }
}
