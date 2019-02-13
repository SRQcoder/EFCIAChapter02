using DataLayer.EfCLasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.BookServices.QueryObjects
{
    public static class BookListDtoSelect
    {
        public static IQueryable<BookListDTO> MapBookToDTO(this IQueryable<Book> books)
        {
            return books.Select(p => new BookListDTO
            {
                BookId = p.BookId, 
                Title = p.Title,
                Price = p.Price,
                PublishedOn = p.PublishedOn,
                ActualPrice = p.Promotion == null ? p.Price : p.Promotion.NewPrice,
                PromotionPromotionalText = p.Promotion == null ? null : p.Promotion.PromotionalText,
                AuthorsOrdered = string.Join(", ", p.AuthorsLink.OrderBy(q => q.Order).Select(q => q.Author.Name)),
                ReviewsCount = p.Reviews.Count,
                ReviewsAverageVotes = p.Reviews.Select(y => (double)y.NumStars).Average()
            });
        }
    }
}
