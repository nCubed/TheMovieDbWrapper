﻿namespace DM.MovieApi.IntegrationTests.MovieDb.Movies;

[TestClass]
public class GetRecommendationsTests
{
    private IApiMovieRequest _api;

    [TestInitialize]
    public void TestInit()
    {
        ApiResponseUtil.ThrottleTests();

        _api = MovieDbFactory.Create<IApiMovieRequest>().Value;
    }

    [TestMethod]
    public async Task GetRecommendationsAsync_Returns_ValidResults()
    {
        const int movieIdRunLolaRun = 104;

        ApiSearchResponse<MovieInfo> response = await _api.GetRecommendationsAsync( movieIdRunLolaRun );

        ApiResponseUtil.AssertErrorIsNull( response );
        ApiResponseUtil.AssertMovieInformationStructure( response.Results );

        Assert.IsTrue( response.TotalPages > 1 );
        Assert.IsTrue( response.TotalResults > 20 );
        Assert.AreEqual( 1, response.PageNumber );
    }

    [TestMethod]
    public async Task GetRecommendationsAsync_HasError_InvalidMovieId()
    {
        const int movieId = 1;

        ApiSearchResponse<MovieInfo> response = await _api.GetRecommendationsAsync( movieId );
        Assert.IsNotNull( response.Error );
    }
}
