using System.Collections.Generic;
using System.Linq;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.Movies
{
    internal static class MovieInfoExtensions
    {
        public static void PopulateGenres( this IEnumerable<MovieInfo> movies, IEnumerable<Genre> allGenres )
        {
            foreach( MovieInfo info in movies )
            {
                info.Genres = info.GenreIds
                    .Select( x => allGenres.First( y => y.Id == x ) )
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}