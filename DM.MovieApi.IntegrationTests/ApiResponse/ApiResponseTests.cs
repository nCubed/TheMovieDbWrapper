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
        private readonly IMovieDbSettings _settings = new IntegrationMovieDbSettings();

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();
            _api = new IntegrationApiRequest( _settings );
        }

        [TestMethod]
        public async Task ApiQueryResponse_Includes_CommandText()
        {
            const string command = "configuration";

            ApiQueryResponse<ApiConfiguration> response = await _api.QueryAsync<ApiConfiguration>( command );

            ApiResponseUtil.AssertErrorIsNull( response );

            string actualCommandText = string.Format( "Actual: {0}", response.CommandText );

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiUrl ), actualCommandText );
        }

        [TestMethod]
        public async Task ApiQueryResponse_Includes_CommandText_OnError()
        {
            const string command = "invalid/request";

            ApiQueryResponse<ApiConfiguration> response = await _api.QueryAsync<ApiConfiguration>( command );

            Assert.IsNotNull( response.Error );

            string actualCommandText = string.Format( "Actual: {0}", response.CommandText );

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiUrl ), actualCommandText );
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

            string actualCommandText = string.Format( "Actual: {0}", response.CommandText );

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( "&page=" ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( "&query=Run Lola Run" ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiUrl ), actualCommandText );
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

            string actualCommandText = string.Format( "Actual: {0}", response.CommandText );

            Assert.IsTrue( response.CommandText.Contains( command ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( "&page=" ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( "&query=Run Lola Run" ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiKey ), actualCommandText );
            Assert.IsTrue( response.CommandText.Contains( _settings.ApiUrl ), actualCommandText );
        }
    }
}
