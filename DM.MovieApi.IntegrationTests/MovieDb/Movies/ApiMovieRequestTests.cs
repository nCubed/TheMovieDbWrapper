using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.IntegrationTests.Infrastructure;
using DM.MovieApi.MovieDb;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Keywords;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Movies
{
    [TestClass]
    public class ApiMovieRequestTests
    {
        private IApiMovieRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiMovieRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiMovieRequest ) );
        }

        [TestMethod]
        public async Task SearchByTitleAsync_Excludes_AdultFilms()
        {
            // 2 parts to test:
            // 1. manual search to verify adult films
            // 2. api search to verify no adult films

            const string adultFilmTitle = "Debbie Does Dallas";

            var param = new Dictionary<string, string>
            {
                {"query", adultFilmTitle},
                {"include_adult", "true"},
                {"language", "en"},
            };

            string[] expectedAdultTitles =
            {
                "Debbie Does Dallas",
                "Debbie Does Dallas... Again",
                "Debbie Does Dallas: The Revenge",
                "Debbie Does Dallas Part II",
                "Debbie Does Dallas III: The Final Chapter",
            };

            var integrationApi = new IntegrationApiRequest( AssemblyInit.Settings );

            var adult = await integrationApi.SearchAsync<MovieInfo>( "search/movie", param );

            foreach( string title in expectedAdultTitles )
            {
                var adultMovie = adult.Results.SingleOrDefault( x => x.Title == title );
                Assert.IsNotNull( adultMovie );
            }

            ApiSearchResponse<MovieInfo> search = await _api.SearchByTitleAsync( adultFilmTitle );

            // if any results are returned, ensure they are not marked as an adult film
            foreach( MovieInfo movie in search.Results )
            {
                Assert.IsFalse( movie.IsAdultThemed );
            }
        }

        [TestMethod]
        public async Task SearchByTitleAsync_RunLolaRun_Returns_SingleResult_WithExpectedValues()
        {
            const string runLolaRun = "Run Lola Run";

            ApiSearchResponse<MovieInfo> response = await _api.SearchByTitleAsync( runLolaRun );

            AssertRunLolaRun( response, runLolaRun );
        }

        [TestMethod]
        public async Task SearchByTitleAsync_RunLolaRun_Returns_SingleResult_WithExpectedValues_InGerman()
        {
            const string runLolaRun = "Run Lola Run";
            const string expectedTitle = "Lola rennt";

            ApiSearchResponse<MovieInfo> response = await _api.SearchByTitleAsync( runLolaRun, 1, "de" );

            AssertRunLolaRun( response, expectedTitle );
        }

        private void AssertRunLolaRun( ApiSearchResponse<MovieInfo> response, string expectedTitle )
        {
            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( 1, response.TotalResults );
            Assert.AreEqual( 1, response.Results.Count );

            MovieInfo movie = response.Results.Single();

            Assert.AreEqual( 104, movie.Id );

            Assert.AreEqual( expectedTitle, movie.Title );

            Assert.AreEqual( new DateTime( 1998, 08, 20 ), movie.ReleaseDate );

            var expectedGenres = new List<Genre>
            {
                GenreFactory.Action(),
                GenreFactory.Drama(),
                GenreFactory.Thriller(),
            };
            CollectionAssert.AreEquivalent( expectedGenres, movie.Genres.ToList() );
        }

        [TestMethod]
        public async Task SearchByTitleAsync_CanPageResults()
        {
            const string query = "Harry";
            const int minimumPageCount = 8;
            const int minimumTotalResultsCount = 140;

            await ApiResponseUtil.AssertCanPageSearchResponse( query, minimumPageCount, minimumTotalResultsCount,
                ( search, pageNumber ) => _api.SearchByTitleAsync( search, pageNumber ), x => x.Id );
        }

        [TestMethod]
        public async Task FindByIdAsync_StarWarsTheForceAwakens_Returns_AllValues()
        {
            const int id = 140607;
            const string expectedImdbId = "tt2488496";
            const string expectedTitle = "Star Wars: The Force Awakens";
            const string expectedOriginalTitle = "Star Wars: The Force Awakens";
            const string expectedTagline = "Every generation has a story.";
            const string expetedOverview = "Thirty years after defeating the Galactic Empire"; // truncated
            const string expectedOriginalLanguage = "en";
            const string expectedHomepage = "http://www.starwars.com/films/star-wars-episode-vii";
            const string expectedStatus = "Released";
            const int expectedBudget = 200000000;
            const int expectedRuntime = 136;
            var expectedReleaseDate = new DateTime( 2015, 12, 15 );

            ApiQueryResponse<Movie> response = await _api.FindByIdAsync( id );

            ApiResponseUtil.AssertErrorIsNull( response );

            Movie movie = response.Item;

            Assert.AreEqual( id, movie.Id );
            Assert.AreEqual( expectedImdbId, movie.ImdbId );
            Assert.AreEqual( expectedTitle, movie.Title );
            Assert.AreEqual( expectedOriginalTitle, movie.OriginalTitle );
            Assert.AreEqual( expectedTagline, movie.Tagline );
            Assert.AreEqual( expectedOriginalLanguage, movie.OriginalLanguage );
            Assert.AreEqual( expectedHomepage, movie.Homepage );
            Assert.AreEqual( expectedStatus, movie.Status );
            Assert.AreEqual( expectedBudget, movie.Budget );
            Assert.AreEqual( expectedRuntime, movie.Runtime );
            Assert.AreEqual( expectedReleaseDate, movie.ReleaseDate );

            ApiResponseUtil.AssertImagePath( movie.BackdropPath );
            ApiResponseUtil.AssertImagePath( movie.PosterPath );

            Assert.IsTrue( movie.Overview.StartsWith( expetedOverview ) );
            Assert.IsTrue( movie.Popularity > 7, $"Actual: {movie.Popularity}" );
            Assert.IsTrue( movie.VoteAverage > 5 );
            Assert.IsTrue( movie.VoteCount > 1500 );

            // Spoken Languages
            var languages = new[]
            {
                new Language("en", "English"),
            };
            CollectionAssert.AreEqual( languages, movie.SpokenLanguages.ToArray() );

            // Production Companies
            var companies = new[]
            {
                new ProductionCompanyInfo(1, "Lucasfilm"),
                new ProductionCompanyInfo(1634, "Truenorth Productions"),
                new ProductionCompanyInfo(11461, "Bad Robot"),
            };
            CollectionAssert.AreEquivalent( companies, movie.ProductionCompanies.ToArray() );

            // Production Countries
            var countries = new[]
            {
                new Country("US", "United States of America"),
            };
            CollectionAssert.AreEqual( countries, movie.ProductionCountries.ToArray() );

            // Movie Collection
            Assert.IsNotNull( movie.MovieCollectionInfo );
            Assert.AreEqual( 10, movie.MovieCollectionInfo.Id );
            Assert.AreEqual( "Star Wars Collection", movie.MovieCollectionInfo.Name );
            ApiResponseUtil.AssertImagePath( movie.MovieCollectionInfo.BackdropPath );
            ApiResponseUtil.AssertImagePath( movie.MovieCollectionInfo.PosterPath );

            // Genres
            var expectedGenres = new List<Genre>
            {
                GenreFactory.Action(),
                GenreFactory.Adventure(),
                GenreFactory.ScienceFiction(),
                GenreFactory.Fantasy(),
            };
            CollectionAssert.AreEquivalent( expectedGenres, movie.Genres.ToList() );

            // Keywords
            var expectedKeywords = new List<Keyword>
            {
                new Keyword(803, "android"),
                new Keyword(9831, "spaceship"),
                new Keyword(10527, "jedi"),
                new Keyword(161176, "space opera"),
                new Keyword(209714, "3d"),
                new Keyword(229031, "shot on imax cameras"),
            };
            CollectionAssert.AreEquivalent( expectedKeywords, movie.Keywords.ToList() );
        }

        [TestMethod]
        public async Task FindByIdAsync_Returns_German_With_LanguageParameter()
        {
            const int id = 140607;
            const string language = "de";
            const string expectedTitle = "Star Wars: Das Erwachen der Macht";

            ApiQueryResponse<Movie> response = await _api.FindByIdAsync( id, language );

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( id, response.Item.Id );
            Assert.AreEqual( expectedTitle, response.Item.Title );
        }

        [TestMethod]
        public async Task MovieWithLargeRevenue_Will_Deserialize()
        {
            const int id = 19995;
            const string expectedTitle = "Avatar";

            ApiQueryResponse<Movie> response = await _api.FindByIdAsync( id );

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( expectedTitle, response.Item.Title );
            Assert.IsTrue( response.Item.Revenue > int.MaxValue );
        }


        [TestMethod]
        public async Task GetLatestAsync_Returns_ValidResult()
        {
            ApiQueryResponse<Movie> response = await _api.GetLatestAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            ApiResponseUtil.AssertMovieStructure( response.Item );
        }

        [TestMethod]
        public async Task GetNowPlayingAsync_Returns_ValidResults()
        {
            ApiSearchResponse<Movie> response = await _api.GetNowPlayingAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            ApiResponseUtil.AssertMovieStructure( response.Results );
        }

        [TestMethod]
        public async Task GetNowPlayingAsync_CanPageResults()
        {
            // Now Playing typically has 25+ pages.
            const int minimumPageCount = 5;
            const int minimumTotalResultsCount = 100; // 20 results per page x 5 pages = 100

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( str, page ) => _api.GetNowPlayingAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetUpcomingAsync_Returns_ValidResults()
        {
            ApiSearchResponse<Movie> response = await _api.GetUpcomingAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            ApiResponseUtil.AssertMovieStructure( response.Results );
        }

        [TestMethod]
        public async Task GetUpcomingAsync_CanPageResults()
        {
            // Now Playing typically has 5+ pages.
            // - intentionally setting minimumTotalResultsCount at 50; sometimes upcoming movies are scarce.
            const int minimumPageCount = 3;
            const int minimumTotalResultsCount = 50; // 20 results per page x 3 pages = 60

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( str, page ) => _api.GetUpcomingAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetTopRatedAsync_Returns_ValidResults()
        {
            ApiSearchResponse<MovieInfo> response = await _api.GetTopRatedAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<MovieInfo> results = response.Results;

            ApiResponseUtil.AssertMovieInformationStructure( results );
        }

        [TestMethod]
        public async Task GetTopRatedAsync_CanPageResults()
        {
            const int minimumPageCount = 2;
            const int minimumTotalResultsCount = 40;

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( str, page ) => _api.GetTopRatedAsync( page ), x => x.Id );
        }

        [TestMethod]
        public async Task GetPopularAsync_Returns_ValidResults()
        {
            ApiSearchResponse<MovieInfo> response = await _api.GetPopularAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<MovieInfo> results = response.Results;

            ApiResponseUtil.AssertMovieInformationStructure( results );
        }

        [TestMethod]
        public async Task GetPopularAsync_CanPageResults()
        {
            const int minimumPageCount = 2;
            const int minimumTotalResultsCount = 40;

            await ApiResponseUtil.AssertCanPageSearchResponse( "unused", minimumPageCount, minimumTotalResultsCount,
                ( str, page ) => _api.GetPopularAsync( page ), x => x.Id );
        }
    }
}
