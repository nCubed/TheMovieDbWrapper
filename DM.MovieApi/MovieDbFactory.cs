using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.Shims;

[assembly: InternalsVisibleTo( "DM.MovieApi.IntegrationTests" )]

namespace DM.MovieApi
{
    /// <summary>
    /// Note: one of the RegisterSettings must be called before the Factory can Create anything.
    /// </summary>
    public static class MovieDbFactory
    {
        /// <include file='ApiDocs.xml' path='Doc/ApiSettings/ApiUrl/*'/>
        public const string TheMovieDbApiUrl = "http://api.themoviedb.org/3/";

        /// <summary>
        /// Determines if the underlying factory has been created.
        /// </summary>
        public static bool IsFactoryComposed => Settings != null;

        internal static IApiSettings Settings { get; private set; }

        /// <summary>
        /// Registers themoviedb.org settings for use with the internal DI container.
        /// </summary>
        /// <param name="bearerToken">
        /// <include file='ApiDocs.xml' path='Doc/ApiSettings/BearerToken/summary/*'/>
        /// </param>
        public static void RegisterSettings( string bearerToken )
        {
            ResetFactory();

            if( bearerToken is null || bearerToken.Length <= 200 )
            {
                // v3 access key was approx 33 chars; v4 bearer is approx 212 chars.
                throw new ArgumentException(
                    $"Must provide a valid TheMovieDb.org Bearer token. Invalid: {bearerToken}. " +
                    "A valid token can be found in your account page, under the API section. " +
                    "You will see a new key listed under the header \"API Read Access Token\".", bearerToken );
            }

            Settings = new MovieDbSettings( TheMovieDbApiUrl, bearerToken );
        }

        /// <summary>
        /// <para>Creates the specific API requested.</para>
        /// <para><inheritdoc cref="MovieDbFactory" path="/summary"/></para>
        /// </summary>
        public static Lazy<T> Create<T>() where T : IApiRequest
        {
            ContainerGuard();

            var requestResolver = new ApiRequestResolver();

            return new Lazy<T>( requestResolver.Get<T> );
        }

        /// <summary>
        /// <para>Creates a global instance exposing all API interfaces against themoviedb.org
        /// that are currently available in this release. Each API is exposed via a Lazy property
        /// ensuring no objects are created until they are needed.</para>
        /// <para><inheritdoc cref="MovieDbFactory" path="/summary"/></para>
        /// </summary>
        public static IMovieDbApi GetAllApiRequests()
        {
            ContainerGuard();

            // Note: the concrete implementation is currently excluded from the .csproj, but is still included in source control.

            string msg = $"{nameof( GetAllApiRequests )} has been temporarily disabled due to porting the code base to Asp.Net Core to provide support for portable library projects.";
            throw new NotImplementedException( msg );
        }

        /// <summary>
        /// Clears all factory settings; forces the next call to be RegisterSettings.
        /// before <see cref="Create{T}"/> can be called.
        /// </summary>
        public static void ResetFactory()
        {
            Settings = null;
        }

        private static void ContainerGuard()
        {
            if( !IsFactoryComposed )
            {
                throw new InvalidOperationException( $"{nameof( RegisterSettings )} must be called before the Factory can Create anything." );
            }
        }

        private class MovieDbSettings : IApiSettings
        {
            public string ApiUrl { get; }
            public string BearerToken { get; }

            public MovieDbSettings( string apiUrl, string bearerToken )
            {
                ApiUrl = apiUrl;
                BearerToken = bearerToken;
            }
        }

        private class ApiRequestResolver
        {
            private static readonly IReadOnlyDictionary<Type, Func<object>> SupportedDependencyTypeMap;
            private static readonly ConcurrentDictionary<Type, ConstructorInfo> TypeCtorMap;

            static ApiRequestResolver()
            {
                SupportedDependencyTypeMap = new Dictionary<Type, Func<object>>
                {
                    {typeof(IApiSettings), () => Settings},
                    {typeof(IApiGenreRequest), () => new ApiGenreRequest( Settings )}
                };

                TypeCtorMap = new ConcurrentDictionary<Type, ConstructorInfo>();
            }

            public T Get<T>() where T : IApiRequest
            {
                ConstructorInfo ctor = TypeCtorMap.GetOrAdd( typeof( T ), GetConstructor );

                ParameterInfo[] param = ctor.GetParameters();

                if( param.Length == 0 )
                {
                    return ( T )ctor.Invoke( null );
                }

                var paramObjects = new List<object>( param.Length );
                foreach( ParameterInfo p in param )
                {
                    if( SupportedDependencyTypeMap.ContainsKey( p.ParameterType ) == false )
                    {
                        throw new InvalidOperationException( $"{p.ParameterType.FullName} is not a supported dependency type for {typeof( T ).FullName}." );
                    }

                    Func<object> typeResolver = SupportedDependencyTypeMap[p.ParameterType];

                    paramObjects.Add( typeResolver() );
                }

                return ( T )ctor.Invoke( paramObjects.ToArray() );
            }

            private ConstructorInfo GetConstructor( Type t )
            {
                ConstructorInfo[] ctors = GetAvailableConstructors( t.GetTypeInfo() );

                if( ctors.Length == 0 )
                {
                    throw new InvalidOperationException( $"No public constructors found for {t.FullName}." );
                }

                if( ctors.Length == 1 )
                {
                    return ctors[0];
                }

                var importingCtors = ctors
                    .Where( x => x.IsDefined( typeof( ImportingConstructorAttribute ) ) )
                    .ToArray();

                if( importingCtors.Length != 1 )
                {
                    throw new InvalidOperationException( "Multiple public constructors found. " +
                                                         $"One must be decorated with the {nameof( ImportingConstructorAttribute )}." );
                }

                return importingCtors[0];
            }

            private ConstructorInfo[] GetAvailableConstructors( TypeInfo typeInfo )
            {
                TypeInfo[] implementingTypes = typeInfo.Assembly.DefinedTypes
                    .Where( x => x.IsAbstract == false )
                    .Where( x => x.IsInterface == false )
                    .Where( typeInfo.IsAssignableFrom )
                    .ToArray();

                if( implementingTypes.Length == 0 )
                {
                    throw new NotSupportedException( $"{typeInfo.Name} must have a concrete implementation." );
                }

                if( implementingTypes.Length != 1 )
                {
                    throw new NotSupportedException( $"Requested type: {typeInfo.Name}. " +
                                                     "Multiple implementations per request interface is not supported." );
                }

                return implementingTypes[0].DeclaredConstructors.ToArray();
            }
        }
    }
}
