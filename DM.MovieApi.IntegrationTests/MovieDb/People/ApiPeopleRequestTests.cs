using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.People;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.People
{
    [TestClass]
    public class ApiPeopleRequestTests
    {
        // ReSharper disable InconsistentNaming
        const int PersonId_MillaJovovich = 63;
        const int PersonId_KevinBacon = 4724;
        // ReSharper restore InconsistentNaming

        private IApiPeopleRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiPeopleRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiPeopleRequest ) );
        }

        [TestMethod]
        public async Task FindByIdAync_MillaJovovich_Returns_ExpectedValues()
        {
            const string expectedName = "Milla Jovovich";
            const string expectedBiography = "Milla Jovovich, born as Milica Natasha Jovovich, is an Ukrainian-born actress, an American supermodel, musician, and fashion designer."; // truncated
            DateTime expectedBirthday = DateTime.Parse( "1975-12-17" );
            const Gender expectedGender = Gender.Female;
            const string expectedHomepage = "http://www.millaj.com";
            const string expectedImdbId = "nm0000170";
            const string expectedPlaceOfBirth = "Kiev, Ukraine";

            ApiQueryResponse<Person> response = await _api.FindByIdAsync( PersonId_MillaJovovich );

            ApiResponseUtil.AssertErrorIsNull( response );

            Person person = response.Item;

            Assert.AreEqual( expectedName, person.Name );
            Assert.IsTrue( person.Biography.StartsWith( expectedBiography ) );
            Assert.IsFalse( person.IsAdultFilmStar );
            Assert.AreEqual( 0, person.AlsoKnownAs.Count );
            Assert.AreEqual( expectedBirthday, person.Birthday );
            Assert.AreEqual( expectedGender, person.Gender );
            Assert.AreEqual( null, person.Deathday );
            Assert.AreEqual( expectedHomepage, person.Homepage );
            Assert.AreEqual( expectedImdbId, person.ImdbId );
            Assert.AreEqual( expectedPlaceOfBirth, person.PlaceOfBirth );
            Assert.IsTrue( person.Popularity > 10 );
            Assert.IsNotNull( person.ProfilePath );
        }

        [TestMethod]
        public async Task FindByIdAsync_KevinBacon_Returns_ExpectedValues()
        {
            const string expectedName = "Kevin Bacon";
            const string expectedBiography = "Kevin Norwood Bacon (born July 8, 1958, height 5' 10\" (1,78 m)) is an American film and theater actor"; // truncated
            DateTime expectedBirthday = DateTime.Parse( "1958-07-08" );
            const Gender expectedGender = Gender.Male;
            const string expectedHomepage = "http://baconbros.com/";
            const string expectedImdbId = "nm0000102";
            const string expectedPlaceOfBirth = "Philadelphia, Pennsylvania, USA";

            ApiQueryResponse<Person> response = await _api.FindByIdAsync( PersonId_KevinBacon );

            ApiResponseUtil.AssertErrorIsNull( response );

            Person person = response.Item;

            Assert.AreEqual( expectedName, person.Name );
            Assert.IsTrue( person.Biography.StartsWith( expectedBiography ) );
            Assert.IsFalse( person.IsAdultFilmStar );
            Assert.AreEqual( 1, person.AlsoKnownAs.Count );
            Assert.AreEqual( "Kevin Norwood Bacon", person.AlsoKnownAs.Single() );
            Assert.AreEqual( expectedBirthday, person.Birthday );
            Assert.AreEqual( expectedGender, person.Gender );
            Assert.AreEqual( null, person.Deathday );
            Assert.AreEqual( expectedHomepage, person.Homepage );
            Assert.AreEqual( expectedImdbId, person.ImdbId );
            Assert.AreEqual( expectedPlaceOfBirth, person.PlaceOfBirth );
            Assert.IsTrue( person.Popularity > 3 );
            Assert.IsNotNull( person.ProfilePath );
        }

        [TestMethod]
        public async Task GetMovieCreditsAsync_KevinBacon_Returns_ExpectedValues()
        {
            ApiQueryResponse<PersonMovieCredit> response = await _api.GetMovieCreditsAsync( PersonId_KevinBacon );

            ApiResponseUtil.AssertErrorIsNull( response );

            PersonMovieCredit credits = response.Item;

            Assert.AreEqual( PersonId_KevinBacon, credits.PersonId );
            Assert.IsTrue( credits.CastRoles.Count > 70 );
            Assert.IsTrue( credits.CrewRoles.Count >= 5 );

            PersonMovieCastMember castMember = credits.CastRoles.SingleOrDefault( x => x.Title == "Footloose" );
            Assert.IsNotNull( castMember );

            Assert.AreEqual( 1788, castMember.MovieId );
            Assert.IsFalse( castMember.IsAdultThemed );
            Assert.AreEqual( "Footloose", castMember.Title );
            Assert.AreEqual( "Footloose", castMember.OriginalTitle );
            Assert.AreEqual( "Ren McCormack", castMember.Character );
            Assert.AreEqual( "52fe4315c3a36847f8038fc3", castMember.CreditId );
            Assert.AreEqual( DateTime.Parse( "1984-02-17" ), castMember.ReleaseDate );
            Assert.IsNotNull( castMember.PosterPath );


            PersonMovieCrewMember crewMember = credits.CrewRoles.SingleOrDefault( x => x.Title == "Wild Things" );
            Assert.IsNotNull( crewMember );

            Assert.AreEqual( 617, crewMember.MovieId );
            Assert.IsFalse( crewMember.IsAdultThemed );
            Assert.AreEqual( "Wild Things", crewMember.Title );
            Assert.AreEqual( "Wild Things", crewMember.OriginalTitle );
            Assert.AreEqual( "Production", crewMember.Department );
            Assert.AreEqual( "Executive Producer", crewMember.Job );
            Assert.AreEqual( "52fe425ec3a36847f8018f2d", crewMember.CreditId );
            Assert.IsNotNull( crewMember.PosterPath );
            Assert.AreEqual( DateTime.Parse( "1998-03-20" ), crewMember.ReleaseDate );

            var expectedCastRoles = new Dictionary<string, string>
            {
                {"Footloose", "Ren McCormack"},
                {"Animal House", "Chip Diller"},
                {"Hollow Man", "Sebastian Caine"},
                {"Wild Things", "Sergeant Ray Duquette"},
            };
            foreach( KeyValuePair<string, string> role in expectedCastRoles )
            {
                PersonMovieCastMember cast = credits
                    .CastRoles
                    .SingleOrDefault( x => x.Title == role.Key && x.Character == role.Value );

                Assert.IsNotNull( cast );
            }

            var expectedCrewRoles = new Dictionary<string, string>
            {
                {"Losing Chase", "Director"},
                {"Loverboy", "Director"},
                {"Wild Things", "Executive Producer"},
                {"The Woodsman", "Executive Producer"},
                {"Cop Car", "Executive Producer"},
            };
            foreach( KeyValuePair<string, string> role in expectedCrewRoles )
            {
                PersonMovieCrewMember cast = credits
                    .CrewRoles
                    .SingleOrDefault( x => x.Title == role.Key && x.Job == role.Value );

                Assert.IsNotNull( cast );
            }
        }
    }
}
