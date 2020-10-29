using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Keywords;
using DM.MovieApi.MovieDb.TV;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.TV
{
    [TestClass]
    public class ApiTVShowRequestTests
    {
        private IApiTVShowRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiTVShowRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiTVShowRequest ) );
        }

        [TestMethod]
        public async Task SearchByNameAsync_The_Nanny_Returns_ExpectedResults()
        {
            const string theNanny = "The Nanny";

            ApiSearchResponse<TVShowInfo> response = await _api.SearchByNameAsync( theNanny );

            AssertTheNanny( response, theNanny );
        }

        [TestMethod]
        public async Task SearchByNameAsync_The_Nanny_Returns_ExpectedResults_InGerman()
        {
            const string theNanny = "Die Nanny";

            ApiSearchResponse<TVShowInfo> response = await _api.SearchByNameAsync( theNanny, language: "de" );

            ApiResponseUtil.AssertErrorIsNull( response );

            AssertTheNanny( response, theNanny );
        }

        private void AssertTheNanny( ApiSearchResponse<TVShowInfo> response, string theNanny )
        {
            const int theNannyId = 2352;
            const string us = "US";
            const string lang = "en";
            var firstAirDate = new DateTime( 1993, 11, 03 );

            Assert.IsTrue( response.TotalResults > 0 );
            Assert.IsTrue( response.Results.Count > 0 );

            TVShowInfo show = response.Results.Single( x => x.Id == theNannyId );

            Assert.AreEqual( theNanny, show.Name );

            Assert.AreEqual( 1, show.OriginCountry.Count );

            string country = show.OriginCountry.Single();

            Assert.AreEqual( us, country );

            Assert.AreEqual( lang, show.OriginalLanguage );

            Assert.AreEqual( firstAirDate.Date, show.FirstAirDate.Date );
        }

        [TestMethod]
        public async Task SearchByNameAsync_GameOfThrones_Returns_PopulatedGenres()
        {
            const string query = "Game of Thrones";

            ApiSearchResponse<TVShowInfo> response = await _api.SearchByNameAsync( query );

            ApiResponseUtil.AssertErrorIsNull( response );

            TVShowInfo gameOfThrones = response.Results.Single();
            Genre[] expGenres =
            {
                GenreFactory.SciFiAndFantasy(),
                GenreFactory.ActionAndAdventure(),
                GenreFactory.Drama(),
                GenreFactory.Mystery()
            };

            CollectionAssert.AreEquivalent( expGenres, gameOfThrones.Genres.ToArray(),
                string.Join( ", ", gameOfThrones.Genres.Select( x => x.Name ) ) );
        }

        [TestMethod]
        public async Task SearchByNameAsync_CanPageResults()
        {
            const string query = "full";
            const int minimumPageCount = 4;
            const int minimumTotalResultsCount = 61;

            await ApiResponseUtil.AssertCanPageSearchResponse( query, minimumPageCount, minimumTotalResultsCount,
                ( search, pageNumber ) => _api.SearchByNameAsync( search, pageNumber ), x => x.Id );
        }

        [TestMethod]
        public async Task FindById_GameOfThrones_ReturnsAllValues()
        {
            var expFirstAirDate = new DateTime( 2011, 04, 17 );
            const string expHomepage = "http://www.hbo.com/game-of-thrones";
            const string expName = "Game of Thrones";
            const string expOriginalLanguage = "en";

            ApiQueryResponse<TVShow> response = await _api.FindByIdAsync( 1399 );

            ApiResponseUtil.AssertErrorIsNull( response );

            TVShow show = response.Item;

            TVShowCreator[] expCreatedBy =
            {
                new TVShowCreator(9813, "David Benioff", "/8CuuNIKMzMUL1NKOPv9AqEwM7og.jpg"),
                new TVShowCreator(228068, "D. B. Weiss", "/caUAtilEe06OwOjoQY3B7BgpARi.jpg"),
            };

            CollectionAssert.AreEquivalent( expCreatedBy, show.CreatedBy.ToArray() );

            var expRunTime = new[] { 60 };

            CollectionAssert.AreEquivalent( expRunTime, show.EpisodeRunTime.ToArray() );

            Assert.AreEqual( expFirstAirDate.Date, show.FirstAirDate.Date );

            Genre[] expGenres =
            {
                GenreFactory.SciFiAndFantasy(),
                GenreFactory.ActionAndAdventure(),
                GenreFactory.Drama(),
                GenreFactory.Mystery()
            };

            CollectionAssert.AreEquivalent( expGenres, show.Genres.ToArray(),
                string.Join( ", ", show.Genres.Select( x => x.Name ) ) );

            Assert.AreEqual( expHomepage, show.Homepage );

            var expLanguages = new[] { "en" };

            CollectionAssert.AreEquivalent( expLanguages, show.Languages.ToArray(),
                string.Join( ", ", show.Languages ) );

            Assert.AreEqual( expName, show.Name );

            Network[] expNetworks =
            {
                new Network(49, "HBO")
            };

            CollectionAssert.AreEquivalent( expNetworks, show.Networks.ToArray() );

            var expCountryCodes = new[] { "US" };

            CollectionAssert.AreEquivalent( expCountryCodes, show.OriginCountry.ToArray() );

            Assert.AreEqual( expOriginalLanguage, show.OriginalLanguage );

            ApiResponseUtil.AssertImagePath( show.BackdropPath );
            ApiResponseUtil.AssertImagePath( show.PosterPath );

            ProductionCompanyInfo[] expProductionCompanies =
            {
                new ProductionCompanyInfo( 5820, "Generator Entertainment" ),
                new ProductionCompanyInfo( 12525, "Television 360" ),
                new ProductionCompanyInfo( 12526, "Bighead Littlehead" ),
                new ProductionCompanyInfo( 76043, "Revolution Sun Studios" )
            };
            CollectionAssert.AreEquivalent( expProductionCompanies, show.ProductionCompanies.ToArray(),
                string.Join( ", ", show.ProductionCompanies ) );

            Keyword[] expKeywords =
            {
                new Keyword(6091, "war"),
                new Keyword(818, "based on novel or book"),
                new Keyword(4152, "kingdom"),
                new Keyword(12554, "dragon"),
                new Keyword(13084, "king"),
                new Keyword(34038, "intrigue"),
                new Keyword(170362, "fantasy world"),
            };
            CollectionAssert.AreEquivalent( expKeywords, show.Keywords.ToArray(),
                string.Join( ", ", show.Keywords ) );
        }

        [TestMethod]
        public async Task GetLatestAsync_Returns_ValidResult()
        {
            ApiQueryResponse<TVShow> response = await _api.GetLatestAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            TVShow show = response.Item;

            Assert.IsNotNull( show );

            Assert.IsTrue( show.Id > 0 );
            Assert.IsFalse( string.IsNullOrEmpty( show.Name ) );
        }

        [TestMethod]
        public async Task GetTopRatedAsync_Returns_ValidResult()
        {
            ApiSearchResponse<TVShowInfo> response = await _api.GetTopRatedAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<TVShowInfo> results = response.Results;

            ApiResponseUtil.AssertTVShowInformationStructure( results );
        }

        [TestMethod]
        public async Task GetTopRatedAsync_CanPageResults()
        {
            const int minimumPageCount = 5;
            const int minimumTotalResultsCount = 100;

            await ApiResponseUtil.AssertCanPageSearchResponse( "none", minimumPageCount, minimumTotalResultsCount,
                ( str, page ) => _api.GetTopRatedAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetPopularAsync_Returns_ValidResult()
        {
            ApiSearchResponse<TVShowInfo> response = await _api.GetPopularAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<TVShowInfo> results = response.Results;

            ApiResponseUtil.AssertTVShowInformationStructure( results );
        }

        [TestMethod]
        public async Task GetPopularAsync_CanPageResults()
        {
            const int minimumPageCount = 5;
            const int minimumTotalResultsCount = 100;

            await ApiResponseUtil.AssertCanPageSearchResponse( "none", minimumPageCount, minimumTotalResultsCount,
                ( str, page ) => _api.GetPopularAsync( page ), x => x.Id );
        }
    }
}
