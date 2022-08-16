namespace DM.MovieApi.MovieDb.Discover;

public interface IDiscoverMovieParameterBuilder
{
    /// <summary>
    /// Builds all parameters for use with an <see cref="ApiRequest.IApiRequest"/>.
    /// Typically called by the internal engine.
    /// </summary>
    Dictionary<string, string> Build();

    /// <summary>
    /// Add for each language version to be returned in the query. May be invoked more than once.
    /// </summary>
    IDiscoverMovieParameterBuilder WithOriginalLanguage( string language );

    /// <summary>
    /// Add for each crew member to be returned in the query. May be invoked more than once.
    /// </summary>
    IDiscoverMovieParameterBuilder WithCrew( int personId );

    /// <summary>
    /// Add for each cast member to be returned in the query. May be invoked more than once.
    /// </summary>
    IDiscoverMovieParameterBuilder WithCast( int personId );

    /// <summary>
    /// Add for each genre to be included in the query. May be invoked more than once.
    /// </summary>
    IDiscoverMovieParameterBuilder WithGenre( int genre );

    /// <summary>
    /// Add for each genre to be excluded from the query. May be invoked more than once.
    /// </summary>
    IDiscoverMovieParameterBuilder ExcludeGenre( int genre );
}
