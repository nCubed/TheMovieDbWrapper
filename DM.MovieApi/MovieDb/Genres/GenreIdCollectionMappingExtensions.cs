using System.Collections.Generic;
using System.Linq;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.TV;

namespace DM.MovieApi.MovieDb.Genres
{
    internal static class GenreIdCollectionMappingExtensions
    {
        public static void PopulateGenres( this IEnumerable<MovieInfo> movies, IEnumerable<Genre> allGenres )
        {
            var genres = allGenres.ToArray();

            foreach( MovieInfo movie in movies )
            {
                movie.Genres = MapGenreIdsToGenres( movie.GenreIds, genres );
            }
        }

        public static void PopulateGenres( this IEnumerable<TVShowInfo> tvShows, IEnumerable<Genre> allGenres )
        {
            var genres = allGenres.ToArray();

            foreach( TVShowInfo tvShow in tvShows )
            {
                tvShow.Genres = MapGenreIdsToGenres( tvShow.GenreIds, genres );
            }
        }

        private static IReadOnlyList<Genre> MapGenreIdsToGenres( IEnumerable<int> genreIds, IEnumerable<Genre> allGenres )
        {
            IReadOnlyList<Genre> genres = genreIds
                .Select( x => allGenres.First( y => y.Id == x ) )
                .ToList()
                .AsReadOnly();

            return genres;
        }
    }
}
