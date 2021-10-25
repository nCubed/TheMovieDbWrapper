using System;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.IntegrationTests.Infrastructure;
using DM.MovieApi.MovieDb.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    [TestClass]
    internal class AssemblyInit
    {
        public static readonly IMovieDbSettings Settings = new IntegrationMovieDbSettings();

        [AssemblyInitialize]
        public static async Task Init( TestContext context )
        {
            NCrunchGuard();

            await ValidateApiKey();

            RegisterFactorySettings();
        }

        [TestMethod]
        public void DebugMethod()
        {
        }

        /// <summary>
        /// Registers the <see cref="IMovieDbSettings"/> for the <see cref="MovieDbFactory"/>
        /// with the credentials from api.creds.json.
        /// </summary>
        public static void RegisterFactorySettings()
        {
            MovieDbFactory.RegisterSettings( Settings );
        }

        /// <summary>
        /// NCrunch has issues with trying to be too clever when running tests concurrently. 
        /// Since this is integration testing and we don't need immediate feedback, use the R# or MS
        /// test runner. And remember to ignore the integration tests in the NCrunch configuration.
        /// </summary>
        private static void NCrunchGuard()
        {
            if( Environment.GetEnvironmentVariable( "NCrunch" ) == "1" )
            {
                throw new NotSupportedException( "NCrunch is not supported for integration testing. Use the R# or MS test runner." );
            }
        }

        private static async Task ValidateApiKey()
        {
            var request = new IntegrationApiRequest( Settings );
            var response = await request.QueryAsync<ApiConfiguration>( "configuration" );

            Assert.IsNull( response.Error, $"{response.Error} Query: {response.CommandText}" );

            ApiConfiguration api = response.Item;
            Assert.IsNotNull( api );

            string[] someChangeKeys = { "crew", "cast", "episode", "title", "overview", "runtime", "adult", "season" };
            CollectionAssert.IsSubsetOf( someChangeKeys, api.ChangeKeys.ToArray() );
        }
    }
}
