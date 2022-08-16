using DM.MovieApi.MovieDb.Discover;

namespace DM.MovieApi.IntegrationTests.MovieDb.Discover;

[TestClass]
public class ApiDiscoverRequestTests
{
    private IApiDiscoverRequest _api;

    [TestInitialize]
    public void TestInit()
    {
        ApiResponseUtil.ThrottleTests();

        _api = MovieDbFactory.Create<IApiDiscoverRequest>().Value;

        Assert.IsInstanceOfType( _api, typeof( ApiDiscoverRequest ) );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithCrew()
    {
        int directorId = 66212;

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithCrew( directorId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithCrew_HasNoResult_InvalidPersonId()
    {
        int personId = 0;

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithCrew( personId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertNoSearchResults( response );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithCast()
    {
        int actorId = 66462;

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithCast( actorId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithCast_HasNoResult_InvalidPersonId()
    {
        int personId = 0;

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithCast( personId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertNoSearchResults( response );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithGenre()
    {
        int genreId = 28;

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithGenre( genreId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        Assert.IsTrue( response.Results
            .All( r => r.Genres.Any( g => g.Id == genreId ) ), "No results with genre" );
    }

    [TestMethod]
    public async Task DiscoverMovies_ExcludeGenre()
    {
        int genreId = 28;

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.ExcludeGenre( genreId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        Assert.IsTrue( response.Results
            .All( r => r.Genres.All( g => g.Id != genreId ) ), "Genre found in results" );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithOriginalLanguage_InFinnish()
    {
        int directorId = 66212;
        string originalLanguage = "fi";

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithOriginalLanguage( originalLanguage ).WithCrew( directorId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );
    }

    [TestMethod]
    public async Task DiscoverMovies_WithOriginalLanguage_InGerman()
    {
        int directorId = 66212;
        string originalLanguage = "de";

        IDiscoverMovieParameterBuilder builder = CreateBuilder();
        builder.WithOriginalLanguage( originalLanguage ).WithCrew( directorId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );
    }

    private IDiscoverMovieParameterBuilder CreateBuilder()
        => new DiscoverMovieParameterBuilder();
}
