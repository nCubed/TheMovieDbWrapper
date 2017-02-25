using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Movies
{
    [TestClass]
    public class GetCreditsTests
    {
        private IApiMovieRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiMovieRequest>().Value;
        }

        [TestMethod]
        public async Task GetCreditsAsync_Returns_ValidResults()
        {
            const int movieIdRunLolaRun = 104;

            ApiQueryResponse<MovieCredit> response = await _api.GetCreditsAsync( movieIdRunLolaRun );

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( movieIdRunLolaRun, response.Item.MovieId );
            Assert.AreEqual( 23, response.Item.CastMembers.Count );
            Assert.AreEqual( 37, response.Item.CrewMembers.Count );
        }

        [TestMethod]
        public async Task GetCreditsAsync_ReturnsCastMembers()
        {
            const int movieIdRunLolaRun = 104;

            ApiQueryResponse<MovieCredit> response = await _api.GetCreditsAsync( movieIdRunLolaRun );

            ApiResponseUtil.AssertErrorIsNull( response );

            MovieCredit credit = response.Item;

            MovieCastMember lola = credit.CastMembers.Single( x => x.Character == "Lola" );
            Assert.AreEqual( 679, lola.PersonId );
            Assert.AreEqual( 11, lola.CastId );
            Assert.AreEqual( "52fe4218c3a36847f80038df", lola.CreditId );
            Assert.AreEqual( "Franka Potente", lola.Name );

            foreach( MovieCastMember castMember in credit.CastMembers )
            {
                Assert.IsTrue( castMember.PersonId > 0 );
                Assert.IsTrue( castMember.CastId > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( castMember.CreditId ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( castMember.Name ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( castMember.Character ) );
            }
        }

        [TestMethod]
        public async Task GetCreditsAsync_ReturnsCrewMembers()
        {
            const int movieIdRunLolaRun = 104;

            ApiQueryResponse<MovieCredit> response = await _api.GetCreditsAsync( movieIdRunLolaRun );

            ApiResponseUtil.AssertErrorIsNull( response );

            MovieCredit credit = response.Item;

            MovieCrewMember director = credit.CrewMembers.Single( x => x.Job == "Director" );
            Assert.AreEqual( 1071, director.PersonId );
            Assert.AreEqual( "52fe4218c3a36847f80038ab", director.CreditId );
            Assert.AreEqual( "Directing", director.Department );
            Assert.AreEqual( "Tom Tykwer", director.Name );

            foreach( MovieCrewMember crewMember in credit.CrewMembers )
            {
                Assert.IsTrue( crewMember.PersonId > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.CreditId ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.Department ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.Job ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.Name ) );
            }
        }
    }
}
