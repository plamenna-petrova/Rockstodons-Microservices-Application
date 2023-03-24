using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Catalog.API.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Catalog.API.Infrastructure
{
    public class CatalogContextSeeder
    {
        public async Task SeedAsync(
            CatalogDbContext catalogDbContext,
            IWebHostEnvironment webHostEnvironment,
            IOptions<CatalogSettings> catalogSettings,
            ILogger<CatalogContextSeeder> logger
        )
        {
            var policy = CreatePolicy(logger, nameof(CatalogContextSeeder));

            await policy.ExecuteAsync(async () =>
            {
                var useCustomizationData = catalogSettings.Value.UseCustomizationData;
                var contentRootPath = webHostEnvironment.ContentRootPath;
                var picturesPath = webHostEnvironment.WebRootPath;

                if (!catalogDbContext.Genres.Any())
                {
                    await catalogDbContext.Genres.AddRangeAsync(GetPreconfiguredGenres());
                    await catalogDbContext.SaveChangesAsync();
                }

                if (!catalogDbContext.AlbumTypes.Any())
                {
                    await catalogDbContext.AlbumTypes.AddRangeAsync(GetPreconfiguredAlbumTypes());
                    await catalogDbContext.SaveChangesAsync();
                }

                if (!catalogDbContext.Performers.Any())
                {
                    await catalogDbContext.Performers.AddRangeAsync(GetProconfiguredPerformers());
                    await catalogDbContext.SaveChangesAsync();
                }

                if (!catalogDbContext.Albums.Any())
                {
                    await catalogDbContext.Albums.AddRangeAsync(GetPreconfiguredAlbums(catalogDbContext));
                    await catalogDbContext.SaveChangesAsync();
                }

                return Task.CompletedTask;
            });
        }

        private Genre CreateGenre(string genreName)
        {
            genreName = genreName.Trim('"').Trim();

            if (string.IsNullOrEmpty(genreName))
            {
                throw new Exception("Genre Name is empty");
            }

            return new Genre { Name = genreName };
        }

        public IEnumerable<Genre> GetPreconfiguredGenres()
        {
            return new List<Genre>
            {
                new() { Name = "Alternative Rock" },
                new() { Name = "Progressive Metal" },
                new() { Name = "Thrash Metal" },
                new() { Name = "Death Metal" },
                new() { Name = "Metalcore" }
            };
        }

        private IEnumerable<AlbumType> GetPreconfiguredAlbumTypes()
        {
            return new List<AlbumType>
            {
                new() { Name = "Full" },
                new() { Name = "EP" },
                new() { Name = "Live"}
            };
        }

        private IEnumerable<Performer> GetProconfiguredPerformers()
        {
            return new List<Performer>
            {
                new()
                {
                    Name = "Saxon",
                    Country = "England",
                    History = "Saxon was formed in November 1975 by former " +
                    "Coast members Peter \"Biff\" Byford on vocals, Paul Quinn, and former " +
                    "SOB and Blue Condition members Graham Oliver on guitars, Steve \"Dobby\" " +
                    "Dawson on bass;and former Glitter " +
                    "Band member Pete Gill on drums who replaced original drummer John Walker in 1977."
                },
                new()
                {
                    Name = "Metallica",
                    Country = "USA",
                    History = "Metallica is an American heavy metal band. " +
                    "The band was formed in 1981 in Los Angeles by vocalist and " +
                    "guitarist James Hetfield and drummer Lars Ulrich, and has " +
                    "been based in San Francisco for most of its career." +
                    "The band's fast tempos, instrumentals and aggressive musicianship made them one of the " +
                    "founding \"big four\" bands of thrash metal, alongside Megadeth, Anthrax and Slayer. " +
                    "Metallica's current lineup comprises founding members and primary songwriters Hetfield and " +
                    "Ulrich, longtime lead guitarist Kirk Hammett, and bassist Robert Trujillo. "
                },
                new()
                {
                    Name = "Gojira",
                    Country = "France",
                    History = "Gojira is a French heavy metal band from Ondres. " +
                    "Founded as Godzilla in 1996, the band's lineup—brothers Joe (lead vocals, rhythm guitar) " +
                    "and Mario Duplantier (drums), Christian Andreu (lead guitar), " +
                    "and Jean-Michel Labadie (bass)—has been the same since the band changed its name to Gojira in 2001. " +
                    "Gojira has been known for their progressive and technical death metal styles and their spiritual, " +
                    "philosophical, and environmentally-themed lyrics. The band has gone \"from the utmost obscurity " +
                    "during the first half of their career to widespread global recognition in the second\""
                },
                new()
                {
                    Name = "Tool",
                    Country = "USA",
                    History = "Tool is an American rock band from Los Angeles. " +
                    "Formed in 1990, the group's line-up includes vocalist Maynard James Keenan, " +
                    "guitarist Adam Jones and drummer Danny Carey. " +
                    "Justin Chancellor has been the band's bassist since 1995, replacing their original " +
                    "bassist Paul D'Amour. Tool has won four Grammy Awards, performed worldwide tours, " +
                    "and produced albums topping the charts in several countries."
                },
                new()
                {
                    Name = "Mastodon",
                    Country = "USA",
                    History = "Mastodon is an American heavy metal band from Atlanta, Georgia. " +
                    "Formed in 2000, the band's lineup of Troy Sanders (bass/vocals), " +
                    "Brent Hinds (lead guitar/vocals), Bill Kelliher (rhythm guitar/backing vocals) and " +
                    "Brann Dailor (drums/vocals) has remained the same since 2001. Mastodon has released eight studio " +
                    "albums, as well as a number of other releases. The band's 2002 debut album, Remission, " +
                    "garnered significant critical acclaim for its unique sound.[1] Mastodon's second full-length release, " +
                    "Leviathan, is a concept album based on the novel Moby-Dick by Herman Melville. Three magazines " +
                    "awarded the record \"Album of the Year\" in 2004: Revolver, Kerrang! and Terrorizer."
                },
                new()
                {
                    Name = "In Flames",
                    Country = "Sweden",
                    History = "In Flames is a Swedish heavy metal band, formed by guitarist " +
                    "Jesper Strömblad in Gothenburg in 1990. Alongside At the Gates and Dark Tranquillity, " +
                    "In Flames pioneered the genres known as Swedish death metal and melodic death metal."
                }
            };
        }

        private IEnumerable<Album> GetPreconfiguredAlbums(CatalogDbContext catalogDbContext)
        {
            var existingGenresIds = catalogDbContext.Genres.Select(g => g.Id).ToList();
            var existingAlbumTypesIds = catalogDbContext.AlbumTypes.Select(at => at.Id).ToList();
            var existingPerformersIds = catalogDbContext.Performers.Select(p => p.Id).ToList();

            var albumsToSeed = new List<Album>()
            {
                new() 
                {  
                    AvailableStock = 100, 
                    Description = "Crusader", 
                    Name = "Crusader", 
                    Price = 19.5M, 
                    PictureFileName = "1.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[1],
                    PerformerId = existingPerformersIds[0]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Forever Free", 
                    Name = "Forever Free", 
                    Price= 8.50M, 
                    PictureFileName = "2.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[1],
                    PerformerId = existingPerformersIds[0]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Ride The Lightning", 
                    Name = "Ride The Lightning", 
                    Price = 12, 
                    PictureFileName = "3.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[3],
                    PerformerId = existingPerformersIds[1]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Magma", 
                    Name = "Magma", 
                    Price = 12, 
                    PictureFileName = "4.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[2]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Fortitude", 
                    Name = "Fortitude", 
                    Price = 8.5M, 
                    PictureFileName = "5.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[2]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "The Way Of All Flesh", 
                    Name = "The Way Of All Flesh", 
                    Price = 12, 
                    PictureFileName = "6.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[2]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Lateralus", 
                    Name = "Lateralus", 
                    Price = 12, 
                    PictureFileName = "7.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[3]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Leviathan", 
                    Name = "Leviathan", 
                    Price = 8.5M, 
                    PictureFileName = "8.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[4]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Blood Mountain", 
                    Name = "Bloo Mountain", 
                    Price = 12, 
                    PictureFileName = "9.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[4]
                },
                new() 
                {  
                    AvailableStock = 100, 
                    Description = "The Hunter", 
                    Name = "The Hunter", 
                    Price = 12, 
                    PictureFileName = "10.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[4]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "Sounds of a Playground Fading", 
                    Name = "Sounds of a Playground Fading", 
                    Price = 8.5M,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[5]
                },
                new() 
                { 
                    AvailableStock = 100, 
                    Description = "A Sense Of Purpose", 
                    Name = "A Sense Of Purpose", 
                    Price = 12, 
                    PictureFileName = "12.png",
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[5]
                }
            };

            return albumsToSeed;
        }

        private void GetAlbumPictures(string contentRootPath, string picturesPath)
        {
            if (picturesPath != null)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(picturesPath);

                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    fileInfo.Delete();
                }

                string albumPicturesZipFile = Path.Combine(contentRootPath, "Setup", "Albums.zip");
                ZipFile.ExtractToDirectory(albumPicturesZipFile, picturesPath);
            }
        }

        public AsyncRetryPolicy CreatePolicy(ILogger<CatalogContextSeeder> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timespan, retry, context) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} " +
                            "detected on attempt {retry} of {retries}",
                            prefix, exception.GetType().Name, exception.Message,
                            retry, retries
                        );
                    }
                );
        }
    }
}
