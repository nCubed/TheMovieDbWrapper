using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Genres
{
    [TestClass]
    public class ApiGenreRequestTests
    {
        private IApiGenreRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiGenreRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiGenreRequest ) );

            ( (ApiGenreRequest)_api ).ClearAllGenres();
        }

        [TestMethod]
        public async Task FindById_Returns_Foreign_Genre_NotInGetAll()
        {
            const int id = 10769;
            const string name = "Foreign";

            CollectionAssert.DoesNotContain( _api.AllGenres.ToList(), new Genre( id, name ) );

            // FindById will add to AllGenres when it does not exist
            ApiQueryResponse<Genre> response = await _api.FindByIdAsync( id );

            ApiResponseUtil.AssertErrorIsNull( response );

            Genre genre = response.Item;

            Assert.AreEqual( id, genre.Id );
            Assert.AreEqual( name, genre.Name );

            CollectionAssert.Contains( _api.AllGenres.ToList(), new Genre( id, name ) );
            CollectionAssert.Contains( _api.AllGenres.ToList(), genre );
        }

        [TestMethod]
        public async Task AllGenres_And_GetAllAsync_Return_Same_Results()
        {
            IReadOnlyList<Genre> allGenres = _api.AllGenres;

            ApiQueryResponse<IReadOnlyList<Genre>> response = await _api.GetAllAsync();

            CollectionAssert.AreEqual( allGenres.ToArray(), response.Item.ToArray() );
        }

        [TestMethod]
        public async Task GetAllAsync_Returns_AllResults_With_Id_and_Name()
        {
            ApiQueryResponse<IReadOnlyList<Genre>> response = await _api.GetAllAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            foreach( Genre genre in response.Item )
            {
                Assert.IsTrue( genre.Id > 0, genre.ToString() );
                Assert.IsNotNull( genre.Name, genre.ToString() );
                Assert.IsTrue( genre.Name.Length >= 3, genre.ToString() );
            }
        }

        [TestMethod]
        public async Task GetAllAsync_Returns_Known_Genres()
        {
            ApiQueryResponse<IReadOnlyList<Genre>> response = await _api.GetAllAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            IReadOnlyList<Genre> knownGenres = GenreFactory.GetAll();

            CollectionAssert.AreEquivalent( knownGenres.ToList(), response.Item.ToList() );
        }

        [TestMethod]
        public async Task GetMoviesAsync_Returns_19_Results()
        {
            ApiQueryResponse<IReadOnlyList<Genre>> response = await _api.GetMoviesAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.IsTrue( response.Item.Any() );

            Assert.AreEqual( 19, response.Item.Count );
        }

        [TestMethod]
        public async Task GetTelevisionAsync_Returns_16_Results()
        {
            ApiQueryResponse<IReadOnlyList<Genre>> response = await _api.GetTelevisionAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.IsTrue( response.Item.Any() );

            Assert.AreEqual( 16, response.Item.Count );
        }

        [TestMethod]
        public async Task GetMoviesAsync_IsSubset_OfGetAll()
        {
            ApiQueryResponse<IReadOnlyList<Genre>> movies = await _api.GetMoviesAsync();

            ApiResponseUtil.AssertErrorIsNull( movies );

            ApiQueryResponse<IReadOnlyList<Genre>> all = await _api.GetAllAsync();

            ApiResponseUtil.AssertErrorIsNull( all );

            Assert.IsTrue( all.Item.Count > movies.Item.Count );

            CollectionAssert.IsSubsetOf( movies.Item.ToList(), all.Item.ToList() );
        }

        [TestMethod]
        public async Task GetTelevisionAsync_IsSubset_OfGetAll()
        {
            ApiQueryResponse<IReadOnlyList<Genre>> tv = await _api.GetTelevisionAsync();

            ApiResponseUtil.AssertErrorIsNull( tv );

            ApiQueryResponse<IReadOnlyList<Genre>> all = await _api.GetAllAsync();

            ApiResponseUtil.AssertErrorIsNull( all );

            Assert.IsTrue( all.Item.Count > tv.Item.Count );

            CollectionAssert.IsSubsetOf( tv.Item.ToList(), all.Item.ToList() );
        }

        [TestMethod]
        public async Task FindMoviesByIdAsync_Returns_ValidResult()
        {
            int genreId = GenreFactory.Comedy().Id;

            ApiSearchResponse<MovieInfo> response = await _api.FindMoviesByIdAsync( genreId );

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( 20, response.Results.Count );

            var expectedGenres = new[] { GenreFactory.Comedy() };

            foreach( MovieInfo info in response.Results )
            {
                CollectionAssert.IsSubsetOf( expectedGenres, info.Genres.ToList() );
            }
        }

        [TestMethod]
        public async Task FindMoviesByIdAsync_CanPageResults()
        {
            int genreId = GenreFactory.Comedy().Id;
            // Comedy has upwards of 2k pages.
            const int minimumPageCount = 5;
            const int minimumTotalResultsCount = 100; // 20 results per page x 5 pages = 100

            await ApiResponseUtil.AssertCanPageSearchResponse( genreId, minimumPageCount, minimumTotalResultsCount,
                ( id, page ) => _api.FindMoviesByIdAsync( id, page ), x => x.Id );
        }
    }
}
