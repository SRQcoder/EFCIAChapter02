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

        public static void SeedDatabase(this EFCoreContext context, string dataDictionary)
        {
            if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                var numbooks = context.Books.Count();
                if (numbooks == 0)
                {
                    var books = BookJsonLoader.LoadBooks(Path.Combine(dataDictionary, SeedFileSubDirectory), SeedDataSearchName).ToList();
                }
            }
        }
    }
}
