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
        private static IMovieDbSettings _settings;

        /// <summary>
        /// Determines if the underlying factory has been created.
        /// </summary>
        public static bool IsFactoryComposed => _settings != null;

        internal static IMovieDbSettings Settings => _settings;

        /// <summary>
        /// Registers themoviedb.org settings for use with the MEF container.
        /// </summary>
        /// <param name="settings">The implementation of <see cref="IMovieDbSettings"/> containing
        /// the themoviedb.org credentials to use when connecting to the service.</param>
        public static void RegisterSettings( IMovieDbSettings settings )
        {
            ResetFactory();

            _settings = settings;
        }

        /// <summary>
        /// Registers themoviedb.org settings for use with the MEF container.
        /// </summary>
        /// <param name="apiKey">Private key required to query themoviedb.org API.</param>
        /// <param name="apiUrl">URL used for api calls to themoviedb.org.</param>
        public static void RegisterSettings( string apiKey, string apiUrl = "http://api.themoviedb.org/3/" )
        {
            var settings = new MovieDbSettings( apiKey, apiUrl );

            RegisterSettings( settings );
        }

        /// <summary>
        /// <para>Creates the specific API requested.</para>
        /// <para>Note: one of the RegisterSettings must be called before the Factory can Create anything.</para>
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
        /// <para>Note: RegisterSettings must be called before the Factory can Create anything.</para>
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
            _settings = null;
        }

        private static void ContainerGuard()
        {
            if( !IsFactoryComposed )
            {
                throw new InvalidOperationException( $"{nameof( RegisterSettings )} must be called before the Factory can Create anything." );
            }
        }

        private class MovieDbSettings : IMovieDbSettings
        {
            public string ApiKey { get; }
            public string ApiUrl { get; }

            public MovieDbSettings( string apiKey, string apiUrl )
            {
                ApiKey = apiKey;
                ApiUrl = apiUrl;
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
                    {typeof(IMovieDbSettings), () => Settings},
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

                var markedCtors = ctors
                    .Where( x => x.IsDefined( typeof( ImportingConstructorAttribute ) ) )
                    .ToArray();

                if( markedCtors.Length != 1 )
                {
                    throw new InvalidOperationException( "Multiple public constructors found.  Please mark the one to use with the FactoryConstructorAttribute." );
                }

                return markedCtors[0];
            }

            private ConstructorInfo[] GetAvailableConstructors( TypeInfo typeInfo )
            {
                TypeInfo[] implementingTypes = typeInfo.Assembly.DefinedTypes
                    .Where( x => x.IsAbstract == false )
                    .Where( x => x.IsInterface == false )
                    .Where( typeInfo.IsAssignableFrom )
                    .ToArray();

                if( implementingTypes.Length != 1 )
                {
                    throw new NotSupportedException( "Multiple implementing requests per request interface is not currently supported." );
                }

                return implementingTypes[0].DeclaredConstructors.ToArray();
            }
        }
    }
}
