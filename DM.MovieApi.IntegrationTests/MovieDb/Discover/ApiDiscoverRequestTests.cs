using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.MovieDb.Discover;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Discover
{
    [TestClass]
    public class ApiDiscoverRequestTests
    {
        private IApiDiscoverRequest _api;
        
        
        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiDiscoverRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiDisoverRequest ) );
        }

        [TestMethod]
        public async Task DiscoverMovies_WithCrew()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var directorId=66212;
            paramBuilder.WithCrew(directorId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count>0, "No results returned");

        } 

        [TestMethod]
        public async Task DiscoverMovies_WithCrew_HasNoResult_InvalidPersonId()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var directorId=0;
            paramBuilder.WithCrew(directorId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count==0, "Results returned");

        } 

        [TestMethod]
        public async Task DiscoverMovies_WithCast()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var actorId=66462;
            paramBuilder.WithCast(actorId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count>0, "No results returned");

        }

        [TestMethod]
        public async Task DiscoverMovies_WithCast_HasNoResult_InvalidPersonId()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var actorId=0;
            paramBuilder.WithCast(actorId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count==0, "Results returned");

        }

        [TestMethod]
        public async Task DiscoverMovies_WithGenre()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var genreId=28;
            paramBuilder.WithGenre(genreId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count>0, "No results returned");
            Assert.IsTrue(response.Results.All(r=>r.Genres.Any(g=>g.Id==genreId)), "No results with genre");
        }

        [TestMethod]
        public async Task DiscoverMovies_WithoutGenre()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var genreId=28;
            paramBuilder.WithoutGenre(genreId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count>0, "No results returned");
            Assert.IsTrue(response.Results.All(r=>r.Genres.All(g=>g.Id!=genreId)), "Genre found in results");

        }

        [TestMethod]
        public async Task DiscoverMovies_WithOriginalLanguage_InFinnish()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var directorId=66212;
            var originalLanguage="fi";
            paramBuilder.WithOriginalLanguage(originalLanguage).WithCrew(directorId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count>0, "No results returned");
            //Assert.IsTrue(response.Results.All(r=>r.OriginalLanguage==language), "No results with language");
        }

        [TestMethod]
        public async Task DiscoverMovies_WithOriginalLanguage_HasNoResuts_InGerman()
        {
            var paramBuilder= new DiscoverMovieParameterBuilder();
            var directorId=66212;
            var originalLanguage="de";
            paramBuilder.WithOriginalLanguage(originalLanguage).WithCrew(directorId);
            var response= await _api.DiscoverMoviesAsync(paramBuilder); 
            ApiResponseUtil.AssertErrorIsNull(response);
            Assert.IsTrue(response.Results.Count>0, "No results returned");
            //Assert.IsTrue(response.Results.All(r=>r.OriginalLanguage==language), "No results with language");
        }
    }    
}