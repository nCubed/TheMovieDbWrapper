using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.IntegrationTests.Infrastructure;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.ApiResponse
{
    [TestClass]
    public class TmdbStatusCodeTests
    {
        /// <summary>
        /// <see cref="IntegrationApiRequest"/> is a private class to TmdbStatusCodeTests. Uses all the same plumbing
        /// that every other IApiRequest uses to query themoviedb.org api.
        /// </summary>
        private IntegrationApiRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();
            _api = new IntegrationApiRequest( AssemblyInit.Settings );
        }

        [TestMethod]
        public async Task InvalidApiKey()
        {
            var api = new IntegrationApiRequest( new IntegrationMovieDbSettings( "xxx", AssemblyInit.Settings.ApiUrl ) );

            ApiQueryResponse<InvalidObject> result = await api.QueryAsync<InvalidObject>( "junk" );

            AssertErrorCode( result, TmdbStatusCode.InvalidApiKey );
        }

        [TestMethod]
        public async Task ResourceNotFound()
        {
            ApiQueryResponse<InvalidObject> result = await _api.QueryAsync<InvalidObject>( "invalid/service" );

            AssertErrorCode( result, TmdbStatusCode.ResourceNotFound );
        }

        [TestMethod]
        public async Task ResourceNotFound_MovieById()
        {
            const int validId = 104;
            const int invalidId = 0;

            // Step 1: Ensure there is a valid result

            ApiQueryResponse<Movie> validResult = await _api.QueryAsync<Movie>( $"movie/{validId}" );
            Assert.IsNull( validResult.Error );
            Assert.AreEqual( validId, validResult.Item.Id );
            Assert.AreEqual( "Run Lola Run", validResult.Item.Title );

            // Step 2: Ensure there is an invalid result

            ApiQueryResponse<Movie> invalidResult = await _api.QueryAsync<Movie>( $"movie/{invalidId}" );
            AssertErrorCode( invalidResult, TmdbStatusCode.ResourceNotFound );
        }

        [Ignore]
        [TestMethod]
        public async Task InvalidPage_LessThanOne_SearchMovie()
        {
            // TODO: (K. Chase) [2016-04-04] InvalidPage status codes are expected to fail for now; see AssertInvalidPage.

            const string invalidPage = "0";

            var param = new Dictionary<string, string>
            {
                {"query", "Run Lola Run"},
                {"page", invalidPage},
            };

            ApiSearchResponse<MovieInfo> result = await _api.SearchAsync<MovieInfo>( "search/movie", param );

            AssertInvalidPage( result );
        }

        [Ignore]
        [TestMethod]
        public async Task InvalidPage_GreaterThanOneThousand_SearchMovie()
        {
            // TODO: (K. Chase) [2016-04-04] InvalidPage status codes are expected to fail for now; see AssertInvalidPage.

            const string invalidPage = "1001";

            var param = new Dictionary<string, string>
            {
                {"query", "Run Lola Run"},
                {"page", invalidPage},
            };

            ApiSearchResponse<MovieInfo> result = await _api.SearchAsync<MovieInfo>( "search/movie", param );

            AssertInvalidPage( result );
        }

        [Ignore]
        [TestMethod]
        public async Task InvalidPage_PageNotAnInteger_SearchMovie()
        {
            // TODO: (K. Chase) [2016-04-04] InvalidPage status codes are expected to fail for now; see AssertInvalidPage.

            const string invalidPage = "One";

            var param = new Dictionary<string, string>
            {
                {"query", "Run Lola Run"},
                {"page", invalidPage},
            };

            ApiSearchResponse<MovieInfo> result = await _api.SearchAsync<MovieInfo>( "search/movie", param );

            AssertInvalidPage( result );
        }

        [TestMethod]
        public async Task InvalidPage_LessThanOne_MoviesByGenre()
        {
            const string invalidPage = "0";

            var param = new Dictionary<string, string>
            {
                {"page", invalidPage},
            };

            string command = $"genre/{GenreFactory.Comedy().Id}/movies";

            ApiSearchResponse<MovieInfo> result = await _api.SearchAsync<MovieInfo>( command, param );

            AssertInvalidPage( result );
        }

        [TestMethod]
        public async Task InvalidPage_GreaterThanOneThousand_MoviesByGenre()
        {
            const string invalidPage = "1001";

            var param = new Dictionary<string, string>
            {
                {"page", invalidPage},
            };

            string command = $"genre/{GenreFactory.Comedy().Id}/movies";

            ApiSearchResponse<MovieInfo> result = await _api.SearchAsync<MovieInfo>( command, param );

            AssertInvalidPage( result );
        }

        [TestMethod]
        public async Task InvalidPage_PageNotAnInteger_MoviesByGenre()
        {
            const string invalidPage = "One";

            var param = new Dictionary<string, string>
            {
                {"page", invalidPage},
            };

            string command = $"genre/{GenreFactory.Comedy().Id}/movies";

            ApiSearchResponse<MovieInfo> result = await _api.SearchAsync<MovieInfo>( command, param );

            AssertInvalidPage( result );
        }

        private void AssertErrorCode<T>( ApiQueryResponse<T> result, TmdbStatusCode expectedCode )
        {
            Assert.IsNull( result.Item );
            Assert.IsNotNull( result.Error );
            Assert.AreEqual( expectedCode, result.Error.TmdbStatusCode, result.Error.ToString() );
        }

        private void AssertInvalidPage( ApiSearchResponse<MovieInfo> result )
        {
            const string note =
                "InvalidPage status codes are expected to fail for now. TheMovieDb.org api is currenlty being updated. See: https://plus.google.com/u/0/+KindlerChase/posts/5CqrtakFTGS";

            if( result.Error == null || result.Error.TmdbStatusCode == TmdbStatusCode.Unknown )
            {
                Assert.Fail( note );
            }

            Assert.IsNotNull( result.Error );
            Assert.AreEqual( TmdbStatusCode.InvalidPage, result.Error.TmdbStatusCode, result.Error.ToString() );
            Assert.IsNull( result.Results );
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class InvalidObject
        { }
    }
}
