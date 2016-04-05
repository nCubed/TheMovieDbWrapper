using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.IndustryProfessions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.MovieDb.Professions
{
    [TestClass]
    public class ApiProfessionRequestTests
    {
        private IApiProfessionRequest _api;

        [TestInitialize]
        public void TestInit()
        {
            ApiResponseUtil.ThrottleTests();

            _api = MovieDbFactory.Create<IApiProfessionRequest>().Value;

            Assert.IsInstanceOfType( _api, typeof( ApiProfessionRequest ) );
        }

        [TestMethod]
        public async Task GetAllAsync_Returns_ValidResults()
        {
            const int expectedCount = 12;

            ApiQueryResponse<IReadOnlyList<Profession>> response = await _api.GetAllAsync();

            ApiResponseUtil.AssertErrorIsNull( response );

            Assert.AreEqual( expectedCount, response.Item.Count );

            foreach( Profession pro in response.Item )
            {
                Assert.IsTrue( pro.Department.Length >= 3, pro.Department ); // Art
                Assert.IsNotNull( pro.Jobs );
                Assert.IsTrue( pro.Jobs.Count >= 5, string.Format( "Actual Count: {0}", pro.Jobs.Count ) );

                foreach( string job in pro.Jobs )
                {
                    Assert.IsTrue( job.Length >= 4, job ); // Idea
                }
            }
        }
    }
}
