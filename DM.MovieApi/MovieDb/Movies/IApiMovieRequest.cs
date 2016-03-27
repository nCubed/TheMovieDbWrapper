using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.Movies
{
    /// <summary>
    /// Interface for retrieving infromation about Movies.
    /// </summary>
    public interface IApiMovieRequest : IApiRequest
    {
        /// <summary>
        /// Gets all the information about a specific Movie.
        /// </summary>
        /// <param name="movieId">The movie Id is typically found from a more generic Movie query.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<Movie>> FindByIdAsync( int movieId, string language = "en" );

        /// <summary>
        /// Searches for Movies by title.
        /// </summary>
        /// <param name="query">The query to search for Movies.</param>
        /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiSearchResponse<MovieInfo>> SearchByTitleAsync( string query, int pageNumber = 1, string language = "en" );

        /// <summary>
        /// Gets the most recent movie that has been added to TheMovieDb.org.
        /// </summary>
        Task<ApiQueryResponse<Movie>> GetLatestAsync();

        /// <summary>
        /// Gets the list of movies playing that have been, or are being released this week.
        /// </summary>
        /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiSearchResponse<Movie>> GetNowPlayingAsync( int pageNumber = 1, string language = "en" );

        /// <summary>
        /// Gets the list of upcoming movies by release date.
        /// </summary>
        /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiSearchResponse<Movie>> GetUpcomingAsync( int pageNumber = 1, string language = "en" );
    }
}