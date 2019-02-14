using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Services.DatabaseServices.Concrete
{
    public enum DbStartupModes { UseExisting, EnsureCreated, EnsureDeletedCreated, UseMigrations }

    public static class SetupHelper
    {
        private const string SeedDataSearchName = "apressBooks.json";
        public const string SeedFileSubDirectory = "seedData";

        public static int SeedDatabase(this EFCoreContext context, string dataDictionary)
        {
            if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                throw new InvalidOperationException("The database does not exist. If you are using Migrations then run PMC command update-database to create it");
            }
            var numBooks = context.Books.Count();
            if (numBooks == 0)
            {
                var books = BookJsonLoader.LoadBooks(Path.Combine(dataDictionary, SeedFileSubDirectory), SeedDataSearchName).ToList();
                context.Books.AddRange(books);
                context.SaveChanges();

            }
            return numBooks;



        }
    }
}
