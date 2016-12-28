using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;

namespace DM.MovieApi.MovieDb.Genres
{
    /// <summary>
    /// Interface representing Movie and TV genres.
    /// </summary>
    public interface IApiGenreRequest : IApiRequest
    {
        /// <summary>
        /// Provides a cache of all the genres (language='en') returned from <see cref="GetAllAsync"/>.
        /// As the Genres do not change much, if any, the cache is never evicted.
        /// </summary>
        IReadOnlyList<Genre> AllGenres { get; }

        /// <summary>
        /// Gets all the information about a specific Genre.
        /// </summary>
        /// <param name="genreId">The genre Id is typically found from a more generic Movie or TV query.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<Genre>> FindByIdAsync( int genreId, string language = "en" );

        /// <summary>
        /// <para>It is recommended to use the <see cref="AllGenres"/> property, unless a
        /// language specific parameter other than 'en' is provided.</para>
        /// <para>
        /// themoviedb.org api mixes tv and movie genres into their movies and tv titles.
        /// Use this method to ensure all genres are accounted for when attempting to join
        /// on Genre.Id from a search result; by default, search results only contain genre
        /// id and excludes the name.
        /// </para>
        /// <para>
        /// In some rare cases, a genre is not included in the movie or tv genres list; when this
        /// occurs, use the <see cref="FindByIdAsync"/> method to find a matching genre.
        /// </para>
        /// </summary>
        /// <returns>The merged set of Movie and TV Genres.</returns>
        Task<ApiQueryResponse<IReadOnlyList<Genre>>> GetAllAsync( string language = "en" );

        /// <summary>
        /// Gets all movie related Genres.
        /// </summary>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<IReadOnlyList<Genre>>> GetMoviesAsync( string language = "en" );

        /// <summary>
        /// Gets all tv related Genres.
        /// </summary>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<IReadOnlyList<Genre>>> GetTelevisionAsync( string language = "en" );

        /// <summary>
        /// Finds all movies related to a genre, where the Id passed to this method is a genere Id, not a movie Id.
        /// </summary>
        /// <param name="genreId">The genre Id is typically found through from a related Movie request or from any of the Genre API methods.</param>
        /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiSearchResponse<MovieInfo>> FindMoviesByIdAsync( int genreId, int pageNumber = 1, string language = "en" );
    }
}
