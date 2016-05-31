using DM.MovieApi.MovieDb.Certifications;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Configuration;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.IndustryProfessions;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.TV;

namespace DM.MovieApi
{
    /// <summary>
    /// Global interface exposing all API interfaces against themoviedb.org that are
    /// currently available in this release.
    /// </summary>
    public interface IMovieDbApi
    {
        /// <summary>
        /// Provides access for retrieving production company information.
        /// </summary>
        IApiCompanyRequest Companies { get; }

        /// <summary>
        /// Provides access for retrieving themoviedb.org configuration information.
        /// </summary>
        IApiConfigurationRequest Configuration { get; }

        /// <summary>
        /// Provides access for retrieving Movie and TV genres.
        /// </summary>
        IApiGenreRequest Genres { get; }

        /// <summary>
        /// Provides access for retrieving information about Movie/TV industry specific professions.
        /// </summary>
        IApiProfessionRequest IndustryProfessions { get; }

        /// <summary>
        /// Provides access for retrieving information about Movies.
        /// </summary>
        IApiMovieRequest Movies { get; }

        /// <summary>
        /// Provides access for retrieving movie rating information.
        /// </summary>
        IApiMovieRatingRequest MovieRatings { get; }

        /// <summary>
        /// Provides access for retrieving information about Television shows.
        /// </summary>
        IApiTVShowRequest Television { get; }
    }
}