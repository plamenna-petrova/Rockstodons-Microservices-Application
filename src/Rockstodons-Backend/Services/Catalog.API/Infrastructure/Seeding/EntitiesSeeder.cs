﻿using Catalog.API.Data.Data.Models;
using Catalog.API.Data.Models;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;
namespace Catalog.API.Infrastructure.Seeding
{
    public class EntitiesSeeder : ISeeder
    {
        private readonly ILogger<EntitiesSeeder> _logger;

        public async Task SeedAsync(
            CatalogDbContext catalogDbContext,
            IServiceProvider serviceProvider
        )
        {
            var policy = CreatePolicy(_logger, nameof(EntitiesSeeder));

            await policy.ExecuteAsync(async () =>
            {
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
                    "Dawson on bass"
                },
                new()
                {
                    Name = "Metallica",
                    Country = "USA",
                    History = "Metallica is an American heavy metal band. " +
                    "The band was formed in 1981 in Los Angeles by vocalist and " +
                    "guitarist James Hetfield and drummer Lars Ulrich, and has " +
                    "been based in San Francisco for most of its career."
                },
                new()
                {
                    Name = "Gojira",
                    Country = "France",
                    History = "Gojira is a French heavy metal band from Ondres. " +
                    "Founded as Godzilla in 1996, the band's lineup—brothers Joe (lead vocals, rhythm guitar) " +
                    "and Mario Duplantier (drums), Christian Andreu (lead guitar), " +
                    "and Jean-Michel Labadie (bass)—has been the same since the band changed its name to Gojira in 2001. "
                },
                new()
                {
                    Name = "Tool",
                    Country = "USA",
                    History = "Tool is an American rock band from Los Angeles. " +
                    "Formed in 1990, the group's line-up includes vocalist Maynard James Keenan, " +
                    "guitarist Adam Jones and drummer Danny Carey. "
                },
                new()
                {
                    Name = "Mastodon",
                    Country = "USA",
                    History = "Mastodon is an American heavy metal band from Atlanta, Georgia. " +
                    "Formed in 2000, the band's lineup of Troy Sanders (bass/vocals), " +
                    "Brent Hinds (lead guitar/vocals), Bill Kelliher (rhythm guitar/backing vocals) and " +
                    "Brann Dailor (drums/vocals) has remained the same since 2001."
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
                    Name = "Crusader",
                    Description = "Crusader",
                    YearOfRelease = 1978,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[1],
                    PerformerId = existingPerformersIds[0]
                },
                new()
                {
                    Description = "Forever Free",
                    Name = "Forever Free",
                    YearOfRelease = 1982,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[1],
                    PerformerId = existingPerformersIds[0]
                },
                new()
                {
                    Name = "Ride The Lightning",
                    Description = "Ride The Lightning",
                    YearOfRelease = 1983,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[3],
                    PerformerId = existingPerformersIds[1]
                },
                new()
                {
                    Name = "Magma",
                    Description = "Magma",
                    YearOfRelease = 2016,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[2]
                },
                new()
                {
                    Name = "Fortitude",
                    Description = "Fortitude",
                    YearOfRelease = 2021,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[2]
                },
                new()
                {
                    Name = "The Way Of All Flesh",
                    Description = "The Way Of All Flesh",
                    YearOfRelease = 2008,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[2]
                },
                new()
                {
                    Name = "Lateralus",
                    Description = "Lateralus",
                    YearOfRelease = 2001,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[2],
                    PerformerId = existingPerformersIds[3]
                },
                new()
                {
                    Name = "Leviathan",
                    Description = "Leviathan",
                    YearOfRelease = 2004,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[4]
                },
                new()
                {
                    Name = "Blood Mountain",
                    Description = "Blood Mountain",
                    YearOfRelease = 2005,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[4]
                },
                new()
                {
                    Name = "The Hunter",
                    Description = "The Hunter",
                    YearOfRelease = 2011,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[4]
                },
                new()
                {
                    Name = "Sounds of a Playground Fading",
                    Description = "Sounds of a Playground Fading",
                    YearOfRelease = 2008,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[5]
                },
                new()
                {
                    Name = "A Sense Of Purpose",
                    Description = "A Sense Of Purpose",
                    YearOfRelease = 2007,
                    AlbumTypeId = existingAlbumTypesIds[1],
                    GenreId = existingGenresIds[4],
                    PerformerId = existingPerformersIds[5]
                }
            };

            return albumsToSeed;
        }

        public AsyncRetryPolicy CreatePolicy(ILogger<EntitiesSeeder> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timespan, retry, context) =>
                    {
                        logger.LogWarning(
                            exception, "[{prefix}] Exception {ExceptionType} with message {Message} " +
                            "detected on attempt {retry} of {retries}",
                            prefix, exception.GetType().Name, exception.Message,
                            retry, retries
                        );
                    }
                );
        }
    }
}
