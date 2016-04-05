using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DM.MovieApi.ApiRequest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    [TestClass]
    public class MovieDbApiTests
    {
        [TestMethod]
        public void Ensure_MovieDbApi_HasAllParts()
        {
            Assembly load = Assembly.Load( "DM.MovieApi" );

            List<Type> apiRequests = load.GetTypes()
                .Where( x => typeof( IApiRequest ).IsAssignableFrom( x ) )
                .Where( x => x.IsClass )
                .Distinct()
                .ToList();

            Assert.IsTrue( apiRequests.Count > 5 );

            foreach( Type type in apiRequests )
            {
                Assert.IsFalse( type.IsPublic );
            }

            List<PropertyInfo> dbApi = typeof( IMovieDbApi )
                .GetProperties()
                .Where( x => typeof( IApiRequest ).IsAssignableFrom( x.PropertyType ) )
                .Distinct()
                .ToList();

            Assert.AreEqual( apiRequests.Count, dbApi.Count );

            foreach( PropertyInfo pi in dbApi )
            {
                Assert.IsTrue( pi.CanRead );
                Assert.IsFalse( pi.CanWrite );
                Assert.IsTrue( pi.PropertyType.IsPublic );
                Assert.IsTrue( pi.PropertyType.IsInterface );

                Type match = apiRequests.SingleOrDefault( x => pi.PropertyType.IsAssignableFrom( x ) );
                Assert.IsNotNull( match );
            }
        }
    }
}
