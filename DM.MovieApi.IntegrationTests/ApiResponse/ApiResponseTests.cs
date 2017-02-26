using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.IntegrationTests.Infrastructure;
using DM.MovieApi.MovieDb.Configuration;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.ApiResponse
{
    [TestClass]
    public class ApiResponseBaseTests
    {
        private IntegrationApiRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();
            _api = new IntegrationApiRequest( AssemblyInit.Settings );
        }

        [TestMethod]
        public async Task ApiQueryResponse_Includes_CommandText()
        {
            const string command = "configuration";

            ApiQueryResponse<ApiConfiguration> response = await _api.QueryAsync<ApiConfiguration>( command );

            ApiResponseUtil.AssertErrorIsNull( response );

            string actualCommandText = $"Actual: {response.CommandText}";

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( AssemblyInit.Settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( AssemblyInit.Settings.ApiUrl ), actualCommandText );
        }

        [TestMethod]
        public async Task ApiQueryResponse_Includes_CommandText_OnError()
        {
            const string command = "invalid/request";

            ApiQueryResponse<ApiConfiguration> response = await _api.QueryAsync<ApiConfiguration>( command );

            Assert.IsNotNull( response.Error );

            string actualCommandText = $"Actual: {response.CommandText}";

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( AssemblyInit.Settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( AssemblyInit.Settings.ApiUrl ), actualCommandText );
        }

        [TestMethod]
        public async Task ApiSearchAsyncResponse_Includes_CommandText()
        {
            const string command = "search/movie";

            var param = new Dictionary<string, string>
            {
                {"query", "Run Lola Run"},
            };

            ApiSearchResponse<MovieInfo> response = await _api.SearchAsync<MovieInfo>( command, param );

            ApiResponseUtil.AssertErrorIsNull( response );

            AssertResponseIncludesCommandText( response, command );
        }

        [TestMethod]
        public async Task ApiSearchAsyncResponse_Includes_CommandText_OnError()
        {
            const string command = "search/invalid";

            var param = new Dictionary<string, string>
            {
                {"query", "Run Lola Run"},
            };

            ApiSearchResponse<MovieInfo> response = await _api.SearchAsync<MovieInfo>( command, param );

            Assert.IsNotNull( response.Error );

            AssertResponseIncludesCommandText( response, command );
        }

        [TestMethod]
        public async Task ApiSearchAsyncResponse_Includes_Json()
        {
            const string command = "search/movie";

            var param = new Dictionary<string, string>
            {
                {"query", "Run Lola Run"},
            };

            ApiSearchResponse<MovieInfo> response = await _api.SearchAsync<MovieInfo>( command, param );

            ApiResponseUtil.AssertErrorIsNull( response );

            AssertReponseIncludesJson( response );
        }

        [TestMethod]
        public async Task ApiQueryAsyncResponse_Includes_Json()
        {
            // Run Lola Run MovieId=104
            const string command = "movie/104";

            ApiQueryResponse<Movie> response = await _api.QueryAsync<Movie>( command );

            AssertReponseIncludesJson( response );
        }

        // ReSharper disable once UnusedParameter.Local
        private void AssertResponseIncludesCommandText( ApiResponseBase response, string command )
        {
            string actualCommandText = $"Actual: {response.CommandText}";

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( "&page=" ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( "&query=Run Lola Run" ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( AssemblyInit.Settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( AssemblyInit.Settings.ApiUrl ), actualCommandText );
        }

        private void AssertReponseIncludesJson( ApiResponseBase response )
        {
            string actualJson = $"Actual: {response.Json}";

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.IsTrue( response.Json.Contains( "\"release_date\":\"1998-08-20\"" ), actualJson );
            Assert.IsTrue( response.Json.Contains( "\"original_title\":\"Lola rennt\"" ), actualJson );
            Assert.IsTrue( response.Json.Contains( "\"title\":\"Run Lola Run\"" ), actualJson );
            Assert.IsTrue( response.Json.Contains( "\"original_language\":\"de\"" ), actualJson );
            Assert.IsTrue( response.Json.Contains( "\"id\":104" ), actualJson );
        }
    }
}
