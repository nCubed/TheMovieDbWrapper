using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Movies
{
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
        public async Task GetSimilarAsync_CanPage()
        {
            const int movieIdRunLolaRun = 104;

            int num = 0;
            var ids = new HashSet<int>();

            for( int i = 1; i <= 10; i++ )
            {
                int pageNumber = i;
                ApiSearchResponse<MovieInfo> response = await _api.GetSimilarAsync( movieIdRunLolaRun, pageNumber );

                ApiResponseUtil.AssertErrorIsNull( response );
                ApiResponseUtil.AssertMovieInformationStructure( response.Results );

                // get similar will return the max number of results
                Assert.AreEqual( 20, response.Results.Count );
                Assert.AreEqual( 500, response.TotalPages );
                Assert.AreEqual( 10000, response.TotalResults );
                Assert.AreEqual( pageNumber, response.PageNumber );

                foreach( MovieInfo m in response.Results )
                {
                    ids.Add( m.Id );
                    num++;
                }
            }

            // api tends to return duplicate results when paging
            // shouldn't be more than 2 or 3 per page; at 20 per page,
            // that's approximately 4-6; let's target 20% (4/page)
            int min = (int)(num * 0.8);
            Assert.IsTrue( ids.Count > min, $"Actual: {ids.Count} vs {min}" );
        }

        [TestMethod]
        public async Task GetSimilarAsync_HasError_InvalidMovieId()
        {
            const int movieId = 1;

            ApiSearchResponse<MovieInfo> response = await _api.GetSimilarAsync( movieId );
            Assert.IsNotNull( response.Error );
        }
    }
}
