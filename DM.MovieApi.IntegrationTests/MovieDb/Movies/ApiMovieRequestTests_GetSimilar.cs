namespace DM.MovieApi.IntegrationTests.MovieDb.Movies;

[TestClass]
public class GetSimilarTests
{
    private IApiMovieRequest _api;

    [TestInitialize]
    public void TestInit()
    {
        ApiResponseUtil.ThrottleTests();

        _api = MovieDbFactory.Create<IApiMovieRequest>().Value;
    }

    [TestMethod]
    public async Task GetSimilarAsync_Returns_ValidResults()
    {
        const int movieIdRunLolaRun = 104;

        ApiSearchResponse<MovieInfo> response = await _api.GetSimilarAsync( movieIdRunLolaRun );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        // get similar will return the max number of results
        Assert.AreEqual( 20, response.Results.Count );
        Assert.AreEqual( 500, response.TotalPages );
        Assert.AreEqual( 10000, response.TotalResults );
        Assert.AreEqual( 1, response.PageNumber );
    }

    [TestMethod]
    public async Task GetSimilarAsync_CanPageResults()
    {
        const int movieId = 104; // run lola run
        const int minimumPageCount = 10;

        await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount,
            ( _, pageNumber ) => _api.GetSimilarAsync( movieId, pageNumber ), x => x.Id );
    }

    [TestMethod]
    public async Task GetSimilarAsync_HasError_InvalidMovieId()
    {
        const int movieId = 1;

        ApiSearchResponse<MovieInfo> response = await _api.GetSimilarAsync( movieId );
        Assert.IsNotNull( response.Error );
    }
}
