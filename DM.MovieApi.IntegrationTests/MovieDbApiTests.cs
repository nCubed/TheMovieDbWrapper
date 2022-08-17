using System.Reflection;

namespace DM.MovieApi.IntegrationTests;

[TestClass]
public class MovieDbApiTests
{
    /// <summary>
    /// All types implementing <see cref="IApiRequest"/>.
    /// </summary>
    public static readonly Type[] Implementations = GetImplementations();

    [TestMethod]
    public void Ensure_Implementations_AreConsistent()
    {
        Assert.IsTrue( Implementations.Length > 5, $"Actual: {Implementations.Length}" );

        foreach( Type t in Implementations )
        {
            ApiResponseUtil.Log( t.Name, nameof( IApiRequest ) );

            Assert.IsTrue( t.IsClass );
            Assert.IsFalse( t.IsAbstract );
            Assert.IsFalse( t.IsPublic );
            Assert.IsTrue( t.IsNotPublic );

            Assert.IsTrue( t.Name.StartsWith( "Api", StringComparison.Ordinal ) );
            Assert.IsTrue( t.Name.EndsWith( "Request", StringComparison.Ordinal ) );
        }
    }

    [TestMethod]
    public void Ensure_MovieDbApi_HasAllParts()
    {
        PropertyInfo[] dbApi = typeof( IMovieDbApi )
            .GetProperties()
            .Where( x => typeof( IApiRequest ).IsAssignableFrom( x.PropertyType ) )
            .ToArray();

        Assert.AreEqual( Implementations.Length, dbApi.Length );

        foreach( PropertyInfo pi in dbApi )
        {
            Assert.IsTrue( pi.CanRead );
            Assert.IsFalse( pi.CanWrite );
            Assert.IsTrue( pi.PropertyType.IsPublic );
            Assert.IsTrue( pi.PropertyType.IsInterface );

            Type match = Implementations.SingleOrDefault( x => pi.PropertyType.IsAssignableFrom( x ) );
            Assert.IsNotNull( match );
        }
    }

    private static Type[] GetImplementations()
    {
        // alternatively: Assembly.Load( "DM.MovieApi" )

        Type[] types = typeof( IApiRequest )
            .Assembly
            .GetTypes()
            .Where( x => x.IsClass )
            .Where( x => typeof( IApiRequest ).IsAssignableFrom( x ) )
            .OrderBy( x => x.Name )
            .ToArray();

        return types;
    }
}
