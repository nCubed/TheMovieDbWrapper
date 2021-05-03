using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.TV;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.TV
{
    [TestClass]
    public class GetCreditsTests
    {
        private IApiTVShowRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiTVShowRequest>().Value;
        }

        [TestMethod]
        public async Task GetCreditsAsync_Returns_ValidResults()
        {
            const int tvShowRunLolaRun = 106159;


            ApiQueryResponse<TVShowEpisodeCredit> response = await _api.GetCreditsAsync(tvShowRunLolaRun, 1, 1);

            ApiResponseUtil.AssertErrorIsNull( response );

            //Assert.AreEqual(tvShowRunLolaRun, response.Item.TVShowEpisodeId );
            Assert.AreEqual( 4, response.Item.CastMembers.Count );
            Assert.AreEqual( 11, response.Item.CrewMembers.Count );
            Assert.AreEqual( 29, response.Item.GuestStarsMembers.Count );
        }

        [TestMethod]
        public async Task GetCreditsAsync_ReturnsCastMembers()
        {
            const int tvShowRunLolaRun = 106159;

            ApiQueryResponse<TVShowEpisodeCredit> response = await _api.GetCreditsAsync(tvShowRunLolaRun, 1, 1);

            ApiResponseUtil.AssertErrorIsNull( response );

            TVShowEpisodeCredit credit = response.Item;

            TVShowEpisodeCastMember member = credit.CastMembers.Single( x => x.Character == "Bryan Beneventi");
            Assert.AreEqual( 17243, member.PersonId );
            Assert.AreEqual("5f131fbeb87aec00361e2ebd", member.CreditId );
            Assert.AreEqual("Jonathan Tucker", member.Name );

            foreach(TVShowEpisodeCastMember castMember in credit.CastMembers )
            {
                Assert.IsTrue( castMember.PersonId > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( castMember.CreditId ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( castMember.Name ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( castMember.Character ) );
            }
        }

        [TestMethod]
        public async Task GetCreditsAsync_ReturnsCrewMembers()
        {
            const int tvShowRunLolaRun = 106159;

            ApiQueryResponse<TVShowEpisodeCredit> response = await _api.GetCreditsAsync(tvShowRunLolaRun, 1, 1);


            ApiResponseUtil.AssertErrorIsNull( response );

            TVShowEpisodeCredit credit = response.Item;

            TVShowEpisodeCrewMember director = credit.CrewMembers.Single( x => x.Job == "Director" );
            Assert.AreEqual( 37948, director.PersonId );
            Assert.AreEqual("6041cbd4b77d4b004488e000", director.CreditId );
            Assert.AreEqual( "Directing", director.Department );
            Assert.AreEqual("Brad Anderson", director.Name );

            foreach(TVShowEpisodeCrewMember crewMember in credit.CrewMembers )
            {
                Assert.IsTrue( crewMember.PersonId > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.CreditId ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.Department ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.Job ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( crewMember.Name ) );
            }
        }

        [TestMethod]
        public async Task GetCreditsAsync_ReturnsGuestStarsMembers()
        {
            const int tvShowRunLolaRun = 106159;

            ApiQueryResponse<TVShowEpisodeCredit> response = await _api.GetCreditsAsync(tvShowRunLolaRun, 1, 1);


            ApiResponseUtil.AssertErrorIsNull(response);

            TVShowEpisodeCredit credit = response.Item;

            TVShowEpisodeGuestStarsMember member = credit.GuestStarsMembers.Single( x => x.Character == "Isla Vandeberg");
            Assert.AreEqual(1272883, member.PersonId);
            Assert.AreEqual("041caaf5690b50045ea29ae", member.CreditId);
            Assert.AreEqual("Alisha Newton", member.Name);

            foreach (TVShowEpisodeGuestStarsMember guestStarsMember in credit.GuestStarsMembers)
            {
                Assert.IsTrue(guestStarsMember.PersonId > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestStarsMember.CreditId));
                Assert.IsFalse(string.IsNullOrWhiteSpace(guestStarsMember.Name));
                Assert.IsFalse( string.IsNullOrWhiteSpace(guestStarsMember.Character ) );
            }
        }
    }
}
