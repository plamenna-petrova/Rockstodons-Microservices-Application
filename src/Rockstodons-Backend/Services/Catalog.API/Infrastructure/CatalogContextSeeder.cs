using Catalog.API.DataModels;
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
            CatalogDbContext catalogContext,
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

                if (!catalogContext.Genres.Any())
                {
                    await catalogContext.Genres.AddRangeAsync(useCustomizationData
                        ? GetGenresFromCSVFile(contentRootPath, logger)
                        : GetPreconfiguredGenres());

                    await catalogContext.SaveChangesAsync();
                }

                if (!catalogContext.AlbumTypes.Any())
                {
                    await catalogContext.AlbumTypes.AddRangeAsync(useCustomizationData
                        ? GetAlbumTypesFromCSVFile(contentRootPath, logger)
                        : GetPreconfiguredAlbumTypes());

                    await catalogContext.SaveChangesAsync();
                }

                if (!catalogContext.Albums.Any())
                {
                    await catalogContext.Albums.AddRangeAsync(useCustomizationData
                        ? GetAlbumsFromCSVFile(contentRootPath, catalogContext, logger)
                        : GetPreconfiguredAlbums());

                    await catalogContext.SaveChangesAsync();
                }

                return Task.CompletedTask;
            });
        }

        private IEnumerable<Genre> GetGenresFromCSVFile(string contentRootPath, ILogger<CatalogContextSeeder> logger)
        {
            string genresCSVFile = Path.Combine(contentRootPath, "Setup", "Genres.csv");

            if (!File.Exists(genresCSVFile))
            {
                return GetPreconfiguredGenres();
            }

            string[] genresCSVHeaders;

            try
            {
                string[] requiredHeaders = { "cataloggenre" };
                genresCSVHeaders = GetHeaders(genresCSVFile, requiredHeaders);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "EXCEPTION ERROR: {Message}", exception.Message);
                return GetPreconfiguredGenres();
            }

            return File.ReadAllLines(genresCSVFile)
                        .Skip(1)
                        .TrySelect(g => CreateGenre(g))
                        .OnCaughtException(ex => { 
                            logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); 
                            return null!; }
                        )
                        .Where(x => x != null);
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

        public IEnumerable<AlbumType> GetAlbumTypesFromCSVFile(
            string contentRootPath, ILogger<CatalogContextSeeder> logger
        )
        {
            string albumTypesCSVFile = Path.Combine(contentRootPath, "Setup", "AlbumTypes.csv");

            if (!File.Exists(albumTypesCSVFile))
            {
                return GetPreconfiguredAlbumTypes();
            }

            string[] albumTypesCSVHeaders;

            try
            {
                string[] requiredHeaders = { "catalogalbumtype" };
                albumTypesCSVHeaders = GetHeaders(albumTypesCSVFile, requiredHeaders);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "EXCEPTION ERROR: {Message}", exception.Message);
                return GetPreconfiguredAlbumTypes();
            }

            return File.ReadAllLines(albumTypesCSVFile)
                            .Skip(1)
                            .TrySelect(at => CreateAlbumType(at))
                            .OnCaughtException(ex => { 
                                logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); 
                                return null!; }
                            )
                            .Where(x => x != null);
        }

        private AlbumType CreateAlbumType(string albumType)
        {
            albumType = albumType.Trim('"').Trim();

            if (string.IsNullOrWhiteSpace(albumType))
            {
                throw new Exception("Album Type is empty");
            }

            return new AlbumType { Name = albumType };
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

        private IEnumerable<Album> GetAlbumsFromCSVFile(
            string contentRootPath,
            CatalogDbContext catalogContext, 
            ILogger<CatalogContextSeeder> logger
        )
        {
            string albumsCSVFile = Path.Combine(contentRootPath, "Setup", "Albums.csv");

            if (!File.Exists(albumsCSVFile))
            {
                return GetPreconfiguredAlbums();
            }

            string[] albumsCSVHeaders;

            try
            {
                string[] requiredHeaders = 
                { 
                    "albumtype", 
                    "genre", 
                    "description", 
                    "name", 
                    "price", 
                    "picturefilename" 
                };
                string[] optionalHeaders = 
                { 
                    "availablestock", 
                    "restockthreshold", 
                    "maxstockthreshold", 
                    "onreorder" 
                };

                albumsCSVHeaders = GetHeaders(albumsCSVFile, requiredHeaders, optionalHeaders);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "EXCEPTION ERROR: {Message}", exception.Message);
                return GetPreconfiguredAlbums();
            }

            var albumTypeIdLookup = catalogContext.AlbumTypes.ToDictionary(at => at.Name, at => at.Id);
            var genreIdLookup = catalogContext.Genres.ToDictionary(g => g.Name, g => g.Id);

            return File.ReadAllLines(albumsCSVFile)
                        .Skip(1)
                        .Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                        .TrySelect(columns => CreateAlbum(
                            columns, albumsCSVHeaders, albumTypeIdLookup, genreIdLookup)
                        )
                        .OnCaughtException(ex => { 
                            logger.LogError("EXCEPTION ERROR {Message}", ex.Message); 
                            return null!; }
                        )
                        .Where(x => x != null);
        }

        private Album CreateAlbum(
            string[] columns, 
            string[] headers, 
            Dictionary<string, int> albumTypeIdLookup, 
            Dictionary<string, int> genreIdLookup
        )
        {
            if (columns.Length != headers.Length)
            {
                throw new Exception($"The columns count '{columns.Length}' " +
                    $"is not the same as the headers count '{headers.Length}'");
            }

            string albumType = columns[Array.IndexOf(headers, "albumtype")].Trim('"').Trim();

            if (!albumTypeIdLookup.ContainsKey(albumType))
            {
                throw new Exception($"Such album type {albumType} does not exist");
            }

            string genre = columns[Array.IndexOf(headers, "genre")].Trim('"').Trim();

            if (!genreIdLookup.ContainsKey(genre))
            {
                throw new Exception($"Such genre {genre} does not exist");
            }

            string priceString = columns[Array.IndexOf(headers, "price")].Trim('"').Trim();

            if (!decimal.TryParse(
                priceString, 
                NumberStyles.AllowDecimalPoint, 
                CultureInfo.InvariantCulture, 
                out decimal price
            ))
            {
                throw new Exception($"The price {priceString} is not a valid decimal number");
            }

            var album = new Album
            {
                AlbumTypeId = albumTypeIdLookup[albumType],
                GenreId = genreIdLookup[genre],
                Description = columns[Array.IndexOf(headers, "description")].Trim('"').Trim(),
                Name = columns[Array.IndexOf(headers, "name")].Trim('"').Trim(),
                Price = price,
                PictureFileName = columns[Array.IndexOf(headers, "picturefilename")].Trim('"').Trim()
            };

            int availableStockIndex = Array.IndexOf(headers, "availablestock");

            if (availableStockIndex != -1)
            {
                string availableStockString = columns[availableStockIndex].Trim('"').Trim();

                if (!string.IsNullOrEmpty(availableStockString))
                {
                    if (int.TryParse(availableStockString, out int availableStock))
                    {
                        album.AvailableStock = availableStock;
                    }
                    else
                    {
                        throw new Exception($"Available Stock = {availableStockString} is not a valid integer");
                    }
                }
            }

            int restockThresholdIndex = Array.IndexOf(headers, "restockthreshold");

            if (restockThresholdIndex != -1)
            {
                string restockThresholdString = columns[restockThresholdIndex].Trim('"').Trim();

                if (!string.IsNullOrEmpty(restockThresholdString))
                {
                    if (int.TryParse(restockThresholdString, out int restockThreshold))
                    {
                        album.RestockThreshold = restockThreshold;  
                    }
                    else
                    {
                        throw new Exception($"Restock Threshold = {restockThreshold} is not a valid integer");
                    }
                }
            }

            int maxStockThresholdIndex = Array.IndexOf(headers, "maxstockthreshold");

            if (maxStockThresholdIndex != -1)
            {
                string maxStockThresholdString = columns[maxStockThresholdIndex].Trim('"').Trim();

                if (!string.IsNullOrEmpty(maxStockThresholdString))
                {
                    if (int.TryParse(maxStockThresholdString, out int maxStockThreshold))
                    {
                        album.MaxStockThreshold = maxStockThreshold;    
                    }
                    else
                    {
                        throw new Exception($"Max Stock Threshold = {maxStockThreshold} is not a valid integer");
                    }
                }
            }

            int onReorderIndex = Array.IndexOf(headers, "onreorder");

            if (onReorderIndex != -1)
            {
                string onReorderString = columns[onReorderIndex].Trim('"').Trim();

                if (!string.IsNullOrEmpty(onReorderString))
                {
                    if (!bool.TryParse(onReorderString, out bool onReorder))
                    {
                        album.OnReorder = onReorder;
                    }
                    else
                    {
                        throw new Exception($"On Reorder = {onReorderString} is not a valid boolean");
                    }
                }
            }

            return album;
        }

        private IEnumerable<Album> GetPreconfiguredAlbums()
        {
            return new List<Album>()
            {
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 2, 
                    AvailableStock = 100, 
                    Description = "Crusader", 
                    Name = "Crusader", 
                    Price = 19.5M, 
                    PictureFileName = "1.png" 
                },
                new() 
                { 
                    AlbumTypeId = 1, 
                    GenreId = 2, 
                    AvailableStock = 100, 
                    Description = "Forever Free", 
                    Name = "Forever Free", 
                    Price= 8.50M, 
                    PictureFileName = "2.png" 
                },
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 5, 
                    AvailableStock = 100, 
                    Description = "Ride The Lightning", 
                    Name = "Ride The Lightning", 
                    Price = 12, 
                    PictureFileName = "3.png" 
                },
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 2, 
                    AvailableStock = 100, 
                    Description = "Magma", 
                    Name = "Magma", 
                    Price = 12, 
                    PictureFileName = "4.png" 
                },
                new() 
                { 
                    AlbumTypeId = 3,
                    GenreId = 5, 
                    AvailableStock = 100, 
                    Description = "Fortitude", 
                    Name = "Fortitude", 
                    Price = 8.5M, 
                    PictureFileName = "5.png" 
                },
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 2, 
                    AvailableStock = 100, 
                    Description = "The Way Of All Flesh", 
                    Name = "The Way Of All Flesh", 
                    Price = 12, 
                    PictureFileName = "6.png" 
                },
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 5, 
                    AvailableStock = 100, 
                    Description = "Lateralus", 
                    Name = "Lateralus", 
                    Price = 12, 
                    PictureFileName = "7.png" 
                },
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 5, 
                    AvailableStock = 100, 
                    Description = "Leviathan", 
                    Name = "Leviathan", 
                    Price = 8.5M, 
                    PictureFileName = "8.png" 
                },
                new() 
                { 
                    AlbumTypeId = 1, 
                    GenreId = 5, 
                    AvailableStock = 100, 
                    Description = "Blood Mountain", 
                    Name = "Bloo Mountain", 
                    Price = 12, 
                    PictureFileName = "9.png" 
                },
                new() 
                { 
                    AlbumTypeId = 3, 
                    GenreId = 2, 
                    AvailableStock = 100, 
                    Description = "The Hunter", 
                    Name = "The Hunter", 
                    Price = 12, 
                    PictureFileName = "10.png" 
                },
                new() 
                { 
                    AlbumTypeId = 3, 
                    GenreId = 2, 
                    AvailableStock = 100, 
                    Description = "Sounds of a Playground Fading", 
                    Name = "Sounds of a Playground Fading", 
                    Price = 8.5M, 
                    PictureFileName = "11.png" 
                },
                new() 
                { 
                    AlbumTypeId = 2, 
                    GenreId = 5, 
                    AvailableStock = 100, 
                    Description = "A Sense Of Purpose", 
                    Name = "A Sense Of Purpose", 
                    Price = 12, 
                    PictureFileName = "12.png" 
                }
            };
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

        private string[] GetHeaders(string csvFilePath, string[] requiredHeaders, string[]? optionalHeaders = null)
        {
            string[] csvHeaders = File.ReadAllLines(csvFilePath).First().ToLowerInvariant().Split(',');

            if (csvHeaders.Length < requiredHeaders.Length)
            {
                throw new Exception($"The required headers count '{requiredHeaders.Length}' " +
                    $"is bigger than the actual headers count '{csvHeaders.Length}'");
            }

            if (optionalHeaders != null)
            {
                if (csvHeaders.Length > (requiredHeaders.Length + optionalHeaders.Length))
                {
                    throw new Exception($"The csv headers count '{csvHeaders.Length}' " +
                        $"is larger than the required headers count '{requiredHeaders.Length}' + " +
                        $"the optional headers count '{optionalHeaders.Length}'");
                }
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvHeaders.Contains(requiredHeader))
                {
                    throw new Exception($"The required header '{requiredHeader} is missing'");
                }
            }

            return csvHeaders;
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
