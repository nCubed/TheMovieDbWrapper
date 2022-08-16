namespace DM.MovieApi.MovieDb.Discover;

/// <summary>
/// Interface for discovering movies based on the filter provided by the parameter builder.
/// </summary>
public interface IApiDiscoverRequest : IApiRequest
{
    /// <summary>
    /// Allows for the discovery of movies by various types of data provided to the <paramref name="builder"/> parameter.
    /// </summary>
    /// <param name="builder">Provides a method of adding several types of parameters to filter the query.</param>
    /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
    /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
    Task<ApiSearchResponse<MovieInfo>> DiscoverMoviesAsync( IDiscoverMovieParameterBuilder builder, int pageNumber = 1, string language = "en" );
}
