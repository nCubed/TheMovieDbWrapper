namespace DM.MovieApi.MovieDb.Discover;

internal class ApiDiscoverRequest : ApiRequestBase, IApiDiscoverRequest
{
    private readonly IApiGenreRequest _genreApi;

    [ImportingConstructor]
    public ApiDiscoverRequest( IApiSettings settings, IApiGenreRequest genreApi )
        : base( settings )
    {
        _genreApi = genreApi;
    }

    public async Task<ApiSearchResponse<MovieInfo>> DiscoverMoviesAsync( DiscoverMovieParameterBuilder builder, int pageNumber = 1, string language = "en" )
    {
        Dictionary<string, string> param = builder.Build();
        param.Add( "language", language );

        const string command = "discover/movie";

        ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

        if( response.Error != null )
        {
            return response;
        }

        response.Results.PopulateGenres( _genreApi );

        return response;
    }
}
