using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    [TestClass]
    public class MovieDbFactoryTests
    {
        [TestMethod]
        public async Task RegisterSettings_CanUse_RawStrings()
        {
            try
            {
                MovieDbFactory.ResetFactory();
                MovieDbFactory.RegisterSettings( AssemblyInit.Settings.ApiKey, AssemblyInit.Settings.ApiUrl );

                var api = MovieDbFactory.Create<IApiMovieRequest>().Value;

                ApiQueryResponse<Movie> response = await api.GetLatestAsync();

                Assert.IsNull( response.Error );
                Assert.IsNotNull( response.Item );
                Assert.IsTrue( response.Item.Id > 391000 );
            }
            finally
            {
                // keeps all other tests running since the settings are registered in AssemblyInit for all tests.
                AssemblyInit.RegisterFactorySettings();
            }
        }

        [TestMethod]
        public void Create_Throws_InvalidOperationException_When_SettingsNotRegistered()
        {
            try
            {
                MovieDbFactory.ResetFactory();
                MovieDbFactory.Create<IMockApiRequest>();
            }
            catch( InvalidOperationException )
            {
                return;
            }
            finally
            {
                // keeps all other tests running since the settings are registered in AssemblyInit for all tests.
                AssemblyInit.RegisterFactorySettings();
            }

            Assert.Fail( "InvalidOperationException was expected." );
        }

        [TestMethod]
        public void Create_ConstrainedBy_IApiRequest()
        {
            Type t1 = typeof( MovieDbFactory )
                .GetMethod( "Create" )
                .GetGenericArguments()
                .Single()
                .GetGenericParameterConstraints()
                .Single();

            Assert.AreEqual( typeof( IApiRequest ), t1 );
        }

        [TestMethod]
        public void Create_CanCreate_Lazy_IApiMovieRequest()
        {
            Lazy<IApiMovieRequest> lazy = MovieDbFactory.Create<IApiMovieRequest>();

            Assert.IsNotNull( lazy );

            Assert.IsFalse( lazy.IsValueCreated );

            IApiMovieRequest api = lazy.Value;

            Assert.IsTrue( lazy.IsValueCreated );

            Assert.IsNotNull( api );
        }

        [TestMethod]
        public void GetAllApiRequests_CanCreate_IMovieApi()
        {
            List<PropertyInfo> dbApi = typeof( IMovieDbApi )
                .GetProperties()
                .Where( x => typeof( IApiRequest ).IsAssignableFrom( x.PropertyType ) )
                .Distinct()
                .ToList();

            Assert.AreEqual( 8, dbApi.Count );

            IMovieDbApi api;

            try
            {
                api = MovieDbFactory.GetAllApiRequests();
            }
            catch( NotImplementedException )
            {
                return;
            }

            Assert.Fail( $"{nameof( MovieDbFactory.GetAllApiRequests )} is not implemented." );

            // ReSharper disable HeuristicUnreachableCode
            Assert.IsNotNull( api );

            foreach( PropertyInfo pi in dbApi )
            {
                var val = pi.GetValue( api ) as IApiRequest;
                Assert.IsNotNull( val );
            }
            // ReSharper restore HeuristicUnreachableCode
        }

        [TestMethod]
        public void ResetFactory_SetsIsFactoryComposed()
        {
            MovieDbFactory.ResetFactory();
            Assert.IsFalse( MovieDbFactory.IsFactoryComposed );

            AssemblyInit.RegisterFactorySettings();
            Assert.IsTrue( MovieDbFactory.IsFactoryComposed );
        }

        private interface IMockApiRequest : IApiRequest
        { }
    }
}
