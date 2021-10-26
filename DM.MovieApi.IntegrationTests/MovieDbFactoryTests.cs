using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.Shims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    [TestClass]
    public class MovieDbFactoryTests
    {
        [TestCleanup]
        public void TestCleanup()
        {
            // Some of the factory tests will de-register the settings registered in AssemblyInit.
            // Ensure other tests are able to run with the registered settings.
            AssemblyInit.RegisterFactorySettings();
        }

        [TestMethod]
        public async Task RegisterSettings_CanUse_RawStrings()
        {
            MovieDbFactory.ResetFactory();
            MovieDbFactory.RegisterSettings( AssemblyInit.Settings.BearerToken );

            var api = MovieDbFactory.Create<IApiMovieRequest>().Value;

            ApiQueryResponse<Movie> response = await api.GetLatestAsync();

            Assert.IsNull( response.Error );
            Assert.IsNotNull( response.Item );
            Assert.IsTrue( response.Item.Id > 391000 );
        }

        [TestMethod]
        public void Create_Throws_InvalidOperationException_When_SettingsNotRegistered()
        {
            try
            {
                MovieDbFactory.ResetFactory();
                MovieDbFactory.Create<IMockApiRequest>();
            }
            catch( InvalidOperationException ex )
            {
                Assert.IsTrue( ex.Message.StartsWith( "RegisterSettings must be called" ), $"Actual: {ex.Message}" );
                return;
            }

            Assert.Fail( $"{nameof( InvalidOperationException )} was expected, but none were thrown." );
        }

        [TestMethod]
        public void Create_ConstrainedBy_IApiRequest()
        {
            MethodInfo mi = typeof( MovieDbFactory ).GetMethod( nameof( MovieDbFactory.Create ) );
            Assert.IsNotNull( mi );

            Type t1 = mi.GetGenericArguments()
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

            Assert.Fail( $"See the {nameof( GetAllApiRequests_IsDisabled_WithEx )} for expected failure." );

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

        [TestMethod]
        public void GetAllApiRequests_IsDisabled_WithEx()
        {
            try
            {
                MovieDbFactory.GetAllApiRequests();
            }
            catch( NotImplementedException )
            {
                return;
            }

            Assert.Fail( $"{nameof( MovieDbFactory.GetAllApiRequests )} " +
                         $"should be disabled with a {nameof( NotImplementedException )}." );
        }

        [TestMethod]
        public void ImportingConstructorAttribute_IsMissing_ThrowsEx()
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var api = MovieDbFactory.Create<IMockApiRequestWithImplementation>().Value;
            }
            catch( InvalidOperationException ex )
            {
                Assert.IsTrue( ex.Message.StartsWith( "Multiple public constructors found." ),
                    $"Actual: {ex.Message}" );

                Assert.IsTrue( ex.Message.Contains( nameof( ImportingConstructorAttribute ) ),
                    $"Actual: {ex.Message}" );

                return;
            }

            Assert.Fail( $"{nameof( MovieDbFactory.Create )} should throw an exception " +
                         "when multiple ctors are found without an decorated ctor for importing." );
        }

        [TestMethod]
        public void IApiRequest_WithoutImplementation_ThrowsEx()
        {
            try
            {
                // ReSharper disable once UnusedVariable
                var api = MovieDbFactory.Create<IMockApiRequestNotImplemented>().Value;
            }
            catch( NotSupportedException ex )
            {
                Assert.AreEqual( ex.Message,
                    $"{nameof( IMockApiRequestNotImplemented )} must have a concrete implementation." );

                return;
            }

            Assert.Fail( $"{nameof( MovieDbFactory.Create )} should throw an exception " +
                         "when no concrete implementation is found." );
        }


        [SuppressMessage( "ReSharper", "UnusedType.Local" )]
        [SuppressMessage( "ReSharper", "NotAccessedField.Local" )]
        private class MockApiRequestMultipleImportingCtors : IMockApiRequestWithImplementation
        {
            private readonly string _val;

            public MockApiRequestMultipleImportingCtors()
            { }

            public MockApiRequestMultipleImportingCtors( string val )
            {
                _val = val;
            }
        }

        private interface IMockApiRequestWithImplementation : IApiRequest
        { }

        private interface IMockApiRequestNotImplemented : IApiRequest
        { }

        private interface IMockApiRequest : IApiRequest
        { }
    }
}
