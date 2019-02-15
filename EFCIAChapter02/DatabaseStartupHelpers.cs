using DataLayer.EfCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services.DatabaseServices.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EFCIAChapter02
{
    public static class DatabaseStartupHelpers
    {
        private const string WwwRootDirectory = "wwwroot\\";
        private const string FileToRead = @"C:\Users\Stephen\Source\Repos\EFCIAChapter02\EFCIAChapter02\wwwroot\";

        public static string GetWwwRootPath()
        {
            //return Path.Combine(Directory.GetCurrentDirectory(), WwwRootDirectory);
            return FileToRead;
        }

        public static IWebHost SetupDevelopmentDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var context = services.GetRequiredService<EFCoreContext>())
                {
                    try
                    {
                        context.SeedDatabase(GetWwwRootPath());
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }

            return webHost;
        }
    }
}
