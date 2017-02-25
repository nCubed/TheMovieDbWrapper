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
        const int PersonId_CourteneyCox = 14405;
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
            const string expectedBiography = "Kevin Norwood Bacon (born July 8, 1958) is an American film and theater actor"; // truncated
            DateTime expectedBirthday = DateTime.Parse( "1958-07-08" );
            const Gender expectedGender = Gender.Male;
            const string expectedHomepage = "http://www.baconbros.com";
            const string expectedImdbId = "nm0000102";
            const string expectedPlaceOfBirth = "Philadelphia, Pennsylvania, USA";

            string[] alsoKnownAs =
            {
                "Kevin Norwood Bacon",
            };

            ApiQueryResponse<Person> response = await _api.FindByIdAsync( PersonId_KevinBacon );

            ApiResponseUtil.AssertErrorIsNull( response );

            Person person = response.Item;

            Assert.AreEqual( expectedName, person.Name );
            Assert.IsTrue( person.Biography.StartsWith( expectedBiography ), $"Actual Biography: {person.Biography}" );
            Assert.IsFalse( person.IsAdultFilmStar );
            Assert.AreEqual( expectedBirthday, person.Birthday );
            Assert.AreEqual( expectedGender, person.Gender );
            Assert.AreEqual( null, person.Deathday );
            Assert.AreEqual( expectedHomepage, person.Homepage );
            Assert.AreEqual( expectedImdbId, person.ImdbId );
            Assert.AreEqual( expectedPlaceOfBirth, person.PlaceOfBirth );
            Assert.IsTrue( person.Popularity > 3 );
            Assert.IsNotNull( person.ProfilePath );

            CollectionAssert.AreEquivalent( alsoKnownAs, person.AlsoKnownAs.ToArray() );
        }

        [TestMethod]
        public async Task FindByIdAsync_CourteneyCox_Returns_ExpectedValues()
        {
            const string expectedName = "Courteney Cox";
            const string expectedBiography = "Courteney Bass Cox (born June 15, 1964) is an American actress"; // truncated
            DateTime expectedBirthday = DateTime.Parse( "1964-06-15" );
            const Gender expectedGender = Gender.Female;
            const string expectedHomepage = "";
            const string expectedImdbId = "nm0001073";
            const string expectedPlaceOfBirth = "Birmingham, Alabama, USA";

            string[] alsoKnownAs =
            {
                "CeCe",
                "Coco",
                "Courtney Cox",
            };

            ApiQueryResponse<Person> response = await _api.FindByIdAsync( PersonId_CourteneyCox );

            ApiResponseUtil.AssertErrorIsNull( response );

            Person person = response.Item;

            Assert.AreEqual( expectedName, person.Name );
            Assert.IsTrue( person.Biography.StartsWith( expectedBiography ) );
            Assert.IsFalse( person.IsAdultFilmStar );
            Assert.AreEqual( expectedBirthday, person.Birthday );
            Assert.AreEqual( expectedGender, person.Gender );
            Assert.AreEqual( null, person.Deathday );
            Assert.AreEqual( expectedHomepage, person.Homepage );
            Assert.AreEqual( expectedImdbId, person.ImdbId );
            Assert.AreEqual( expectedPlaceOfBirth, person.PlaceOfBirth );
            Assert.IsTrue( person.Popularity > 3 );
            Assert.IsNotNull( person.ProfilePath );

            CollectionAssert.AreEquivalent( alsoKnownAs, person.AlsoKnownAs.ToArray() );
        }

        [TestMethod]
        public async Task GetMovieCreditsAsync_KevinBacon_Returns_ExpectedValues()
        {
            ApiQueryResponse<PersonMovieCredit> response = await _api.GetMovieCreditsAsync( PersonId_KevinBacon );

            ApiResponseUtil.AssertErrorIsNull( response );

            PersonMovieCredit credits = response.Item;

            Assert.AreEqual( PersonId_KevinBacon, credits.PersonId );
            Assert.IsTrue( credits.CastRoles.Count > 70, $"Actual Count: {credits.CastRoles.Count}" );
            Assert.IsTrue( credits.CrewRoles.Count >= 5, $"Actual Count: {credits.CrewRoles.Count}" );

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

        [TestMethod]
        public async Task GetTVCreditsAsync_CourteneyCox_Returns_ExpectedValues()
        {
            ApiQueryResponse<PersonTVCredit> response = await _api.GetTVCreditsAsync( PersonId_CourteneyCox );

            ApiResponseUtil.AssertErrorIsNull( response );

            PersonTVCredit credits = response.Item;

            Assert.AreEqual( PersonId_CourteneyCox, credits.PersonId );
            Assert.IsTrue( credits.CastRoles.Count > 20, $"Actual Count: {credits.CastRoles.Count}" );
            Assert.IsTrue( credits.CrewRoles.Count >= 3, $"Actual Count: {credits.CrewRoles.Count}" );

            PersonTVCastMember castMember = credits.CastRoles.SingleOrDefault( x => x.Name == "Friends" );
            Assert.IsNotNull( castMember );

            Assert.AreEqual( 1668, castMember.TVShowId );
            Assert.AreEqual( "525710bc19c295731c032341", castMember.CreditId );
            Assert.AreEqual( 236, castMember.EpisodeCount );
            Assert.AreEqual( DateTime.Parse( "1994-09-22" ), castMember.FirstAirDate );
            Assert.AreEqual( "Friends", castMember.Name );
            Assert.AreEqual( "Friends", castMember.OriginalName );
            Assert.AreEqual( "Monica Geller", castMember.Character );
            Assert.IsNotNull( castMember.PosterPath );


            PersonTVCrewMember crewMember = credits.CrewRoles.SingleOrDefault( x => x.Name == "Dirt" );
            Assert.IsNotNull( crewMember );

            Assert.AreEqual( 284, crewMember.TVShowId );
            Assert.AreEqual( "Production", crewMember.Department );
            Assert.AreEqual( 20, crewMember.EpisodeCount );
            Assert.AreEqual( DateTime.Parse( "2007-01-02" ), crewMember.FirstAirDate );
            Assert.AreEqual( "Producer", crewMember.Job );
            Assert.AreEqual( "Dirt", crewMember.Name );
            Assert.AreEqual( "Dirt", crewMember.OriginalName );
            Assert.AreEqual( "52534b4119c29579400f66b9", crewMember.CreditId );
            Assert.IsNotNull( crewMember.PosterPath );


            var expectedCastRoles = new Dictionary<string, string>
            {
                {"Friends", "Monica Geller"},
                {"The Trouble with Larry", "Gabriella Easden"},
                {"Misfits of Science", "Gloria Dinallo"},
                {"Cougar Town", "Jules Cobb"},
            };
            foreach( KeyValuePair<string, string> role in expectedCastRoles )
            {
                PersonTVCastMember cast = credits
                    .CastRoles
                    .SingleOrDefault( x => x.Name == role.Key && x.Character == role.Value );

                Assert.IsNotNull( cast );
            }

            var expectedCrewRoles = new Dictionary<string, string>
            {
                {"Cougar Town", "Producer"},
                {"Dirt", "Producer"},
                {"Daisy Does America", "Producer"},
            };
            foreach( KeyValuePair<string, string> role in expectedCrewRoles )
            {
                PersonTVCrewMember cast = credits
                    .CrewRoles
                    .SingleOrDefault( x => x.Name == role.Key && x.Job == role.Value );

                Assert.IsNotNull( cast );
            }
        }

        [TestMethod]
        public async Task SearchByNameAsync_Milla_Jovovich_Returns_SingleResult_WithExpectedValues()
        {
            const string millaJovovich = "Milla Jovovich";

            ApiSearchResponse<PersonInfo> response = await _api.SearchByNameAsync( millaJovovich );

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( 1, response.TotalResults );
            Assert.AreEqual( 1, response.Results.Count );

            PersonInfo person = response.Results.Single();

            Assert.AreEqual( PersonId_MillaJovovich, person.Id );
            Assert.AreEqual( millaJovovich, person.Name );
            Assert.IsFalse( person.IsAdultFilmStar );
            Assert.AreEqual( 3, person.KnownFor.Count );

            string[] roles =
            {
                "The Fifth Element",
                "Resident Evil: Retribution",
                "Resident Evil",
            };

            foreach( string role in roles )
            {
                PersonInfoRole info = person.KnownFor.SingleOrDefault( x => x.MovieTitle == role );
                Assert.IsNotNull( info );
                Assert.AreEqual( MediaType.Movie, info.MediaType );
            }
        }

        [TestMethod]
        public async Task SearchByNameAsync_CanPageResults()
        {
            const string query = "Cox";
            const int minimumPageCount = 15;
            const int minimumTotalResultsCount = 300;

            await ApiResponseUtil.AssertCanPageSearchResponse( query, minimumPageCount, minimumTotalResultsCount,
                ( search, pageNumber ) => _api.SearchByNameAsync( search, pageNumber ), null );
        }
    }
}
