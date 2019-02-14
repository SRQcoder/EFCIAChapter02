using DataLayer.EfCLasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Services.DatabaseServices.Concrete
{
    public static class BookJsonLoader
    {
        public static IEnumerable<Book> LoadBooks(string fileDirectory, string fileSearchString)
        {
            var filePath = GetJsonFilePath(fileDirectory, fileSearchString);
            var jsonDecoded = JsonConvert.DeserializeObject<ICollection<BookInfoJson>>(File.ReadAllText(filePath));

            var authorDictionary = new Dictionary<string, Author>();
            foreach (var bookInfoJson in jsonDecoded)
            {
                foreach (var author in bookInfoJson.authors)
                {
                    if (!authorDictionary.ContainsKey(author))
                    {
                        authorDictionary[author] = new Author { Name = author };
                    }
                }
            }

            return jsonDecoded.Select(x => CreateBookWithRefs(x, authorDictionary));
        }


        //  Private Methods
        private static string GetJsonFilePath(string fileDirectory, string searchPattern)
        {
            var fileList = Directory.GetFiles(fileDirectory, searchPattern);
            if (fileList.Length == 0)
            {
                throw new FileNotFoundException($"Could not find a file with the search name of {searchPattern} in directory {fileDirectory}");
            }

            return fileList.ToList().OrderBy(x => x).Last();
        }

        private static Book CreateBookWithRefs(BookInfoJson bookInfoJson, Dictionary<string, Author>  authorDictionary)
        {
            var book = new Book
            {
                Title = bookInfoJson.title,
                Description = bookInfoJson.description,
                PublishedOn = DecodePublishDate(bookInfoJson.publishedDate),
                Publisher = bookInfoJson.publisher,
                Price = (decimal)(bookInfoJson.saleInfoListPriceAmount ?? -1),
                ImageUrl = bookInfoJson.imageLinksThumbnail
            };
            byte i = 0;
            book.AuthorsLink = new List<BookAuthor>();
            foreach (var author in bookInfoJson.authors)
            {
                book.AuthorsLink.Add(new BookAuthor { Book = book, Author = authorDictionary[author], Order = i++ });
            }

            if (bookInfoJson.averageRating != null)
            {
                book.Reviews = CalculateReviewsToMatch((double)bookInfoJson.averageRating, (int)bookInfoJson.ratingsCount);
            }

            return book;
        }

        private static DateTime DecodePublishDate(string publishDate)
        {
            var split = publishDate.Split('-');
            switch (split.Length)
            {
                case 1: return new DateTime(int.Parse(split[0]), 1, 1);
                default:
                    break;
            }
        }

        private static ICollection<Review> CalculateReviewsToMatch(double averageRating, int ratingsCount)
        {
            var reviews = new List<Review>();
            var currentAverage = averageRating;
            for (int i = 0; i < ratingsCount; i++)
            {
                reviews.Add(new Review
                {
                    VoterName = "anonymous",
                    NumStars = (int)(currentAverage > averageRating ? Math.Truncate(averageRating) : Math.Ceiling(averageRating))
                });
                currentAverage = reviews.Average(x => x.NumStars);
            }
            return reviews;
        }
    }
}
