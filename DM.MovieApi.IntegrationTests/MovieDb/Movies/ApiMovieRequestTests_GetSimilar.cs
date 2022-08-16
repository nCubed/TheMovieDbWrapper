using System.Collections.Generic;
using System.Linq;
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

            // todo: move to ApiResponseUtil and refactor w/ existing AssertCanPageSearchResponse
            //       * each component may be able to become re-usable methods

            int num = 0;
            var ids = new HashSet<int>();
            var dups = new List<string>();

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
                    num++;
                    if( ids.Add( m.Id ) == false )
                    {
                        dups.Add( m.ToString() );
                    }
                }
            }

            // api tends to return duplicate results when paging
            // shouldn't be more than 2 or 3 per page; at 20 per page,
            // that's approximately 4-6; let's target 25%
            int min = (int)(num * 0.75);
            var d = dups
                .GroupBy( x => x )
                .Select( x => $"{x.Key} (x {x.Count()})" );
            System.Diagnostics.Trace.WriteLine( $"Results: {num}, Dups: {dups.Count}" +
                                                $"\r\n{string.Join( "\r\n", d )}" );

            Assert.IsTrue( ids.Count >= min, $"Total: {num}.\r\nUnique: {ids.Count}, Dup Threshold {min}" );
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
