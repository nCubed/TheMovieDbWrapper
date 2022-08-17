namespace DM.MovieApi.MovieDb.TV;

/// <summary>
/// Interface for retrieving information about TV shows.
/// </summary>
public interface IApiTVShowRequest : IApiRequest
{
    /// <summary>
    /// Gets all the information about a specific TV show.
    /// </summary>
    /// <param name="tvShowId">The TV show Id which is typically found from a more generic TV show query.</param>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiQueryResponse<TVShow>> FindByIdAsync( int tvShowId, string language = "en" );

    /// <summary>
    /// Searches for TV shows by title.
    /// </summary>
    /// <param name="query">The query to search for TV shows.</param>
    /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiSearchResponse<TVShowInfo>> SearchByNameAsync( string query, int pageNumber = 1, string language = "en" );

    /// <summary>
    /// Gets the latest TV show added to TheMovieDb.org
    /// </summary>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiQueryResponse<TVShow>> GetLatestAsync( string language = "en" );

    /// <summary>
    /// Gets the list of top rated TV shows which is refreshed daily.
    /// </summary>
    /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiSearchResponse<TVShowInfo>> GetTopRatedAsync( int pageNumber = 1, string language = "en" );

    /// <summary>
    /// Gets the list of popular TV shows which is refreshed daily.
    /// </summary>
    /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiSearchResponse<TVShowInfo>> GetPopularAsync( int pageNumber = 1, string language = "en" );

    /// <summary>
    /// Get the TV season details by id.
    /// </summary>
    /// <param name="tvShowId">The TV show Id which is typically found from a more generic TV show query.</param>
    /// <param name="seasonNumber">The Season Number is typically found from a more generic TV show query.</param>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiQueryResponse<SeasonInfo>> GetTvShowSeasonInfoAsync( int tvShowId, int seasonNumber, string language = "en" );
}
