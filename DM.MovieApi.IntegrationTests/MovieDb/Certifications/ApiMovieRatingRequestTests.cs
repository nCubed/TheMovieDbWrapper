using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Certifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Certifications
{
    [TestClass]
    public class ApiMovieRatingRequestTests
    {
        private IApiMovieRatingRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiMovieRatingRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiMovieRatingRequest ) );
        }

        [TestMethod]
        public async Task GetMovieRatingsAsync_Returns_Ratings_InCorrectOrder()
        {
            ApiQueryResponse<MovieRatings> response = await _api.GetMovieRatingsAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            AssertOrderedRatings( 8, response.Item.Australia );
            AssertOrderedRatings( 5, response.Item.Canada );
            AssertOrderedRatings( 5, response.Item.France );
            AssertOrderedRatings( 5, response.Item.Germany );
            AssertOrderedRatings( 3, response.Item.India );
            AssertOrderedRatings( 8, response.Item.NewZealand );
            AssertOrderedRatings( 7, response.Item.UnitedKingdom );
            AssertOrderedRatings( 6, response.Item.UnitedStates );
        }

        private void AssertOrderedRatings( int expectedCount, IReadOnlyList<Certification> items )
        {
            Assert.AreEqual( expectedCount, items.Count );

            int previousOrder = -1;

            foreach( Certification item in items )
            {
                Assert.IsTrue( item.Order > previousOrder,
                    $"{item.Rating} item.Order: {item.Order} previousOrder: {previousOrder}");

                previousOrder = item.Order;
            }
        }
    }
}
