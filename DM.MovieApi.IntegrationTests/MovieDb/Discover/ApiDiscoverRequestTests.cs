using DM.MovieApi.IntegrationTests.MovieDb.People;
using DM.MovieApi.MovieDb.Discover;

namespace DM.MovieApi.IntegrationTests.MovieDb.Discover;

[TestClass]
public class ApiDiscoverRequestTests
{
    private IApiDiscoverRequest _api;
    private IApiMovieRequest _movie;

    [TestInitialize]
    public void TestInit()
    {
        ApiResponseUtil.ThrottleTests();

        _api = MovieDbFactory.Create<IApiDiscoverRequest>().Value;
        _movie = MovieDbFactory.Create<IApiMovieRequest>().Value;

        Assert.IsInstanceOfType( _api, typeof( ApiDiscoverRequest ) );
    }

    [TestMethod]
    public async Task WithCrew()
    {
        int directorId = 66212;

        var builder = new DiscoverMovieParameterBuilder().WithCrew( directorId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );
    }

    [TestMethod]
    public async Task WithCrew_HasNoResult_InvalidPersonId()
    {
        var builder = new DiscoverMovieParameterBuilder().WithCrew( 0 );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertNoSearchResults( response );
    }

    [DataRow( ApiPeopleRequestTests.PersonId_MillaJovovich )]
    [DataRow( ApiPeopleRequestTests.PersonId_KevinBacon )]
    [DataRow( ApiPeopleRequestTests.PersonId_CourteneyCox )]
    [TestMethod]
    public async Task WithCast( int personId )
    {
        var builder = new DiscoverMovieParameterBuilder().WithCast( personId );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        foreach( MovieInfo info in response.Results )
        {
            MovieCredit credits = (await _movie.GetCreditsAsync( info.Id )).Item;

            Assert.IsTrue( credits.CastMembers.Any( x => x.PersonId == personId ) );
        }
    }

    [TestMethod]
    public async Task WithCast_HasNoResult_InvalidPersonId()
    {
        var builder = new DiscoverMovieParameterBuilder().WithCast( 0 );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertNoSearchResults( response );
    }

    [DynamicData( nameof( GetGenreData ), DynamicDataSourceType.Method )]
    [TestMethod]
    public async Task WithGenre( Genre genre )
    {
        var builder = new DiscoverMovieParameterBuilder().WithGenre( genre );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        foreach( MovieInfo info in response.Results )
        {
            Assert.IsTrue( info.GenreIds.Contains( genre.Id ) );
            Assert.IsTrue( info.Genres.Contains( genre ) );
        }
    }

    [DynamicData( nameof( GetGenreData ), DynamicDataSourceType.Method )]
    [TestMethod]
    public async Task ExcludeGenre( Genre genre )
    {
        var builder = new DiscoverMovieParameterBuilder().ExcludeGenre( genre );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        foreach( MovieInfo info in response.Results )
        {
            Assert.IsFalse( info.GenreIds.Contains( genre.Id ) );
            Assert.IsFalse( info.Genres.Contains( genre ) );
        }
    }

    [DataRow( "es" )]
    [DataRow( "de" )]
    [DataRow( "fi" )]
    [TestMethod]
    public async Task WithOriginalLanguage( string originalLanguage )
    {
        var builder = new DiscoverMovieParameterBuilder().WithOriginalLanguage( originalLanguage );

        ApiSearchResponse<MovieInfo> response = await _api.DiscoverMoviesAsync( builder );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        foreach( MovieInfo info in response.Results )
        {
            Movie m = (await _movie.FindByIdAsync( info.Id )).Item;

            Assert.AreEqual( originalLanguage, m.OriginalLanguage, $"{m.OriginalLanguage} - {m}" );
        }
    }

    private static IEnumerable<object[]> GetGenreData()
    {
        yield return new object[] { GenreFactory.Comedy() };
        yield return new object[] { GenreFactory.Action() };
        yield return new object[] { GenreFactory.ScienceFiction() };
    }
}
