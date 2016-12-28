using System;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.MovieDb.Certifications;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Configuration;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.IndustryProfessions;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.People;
using DM.MovieApi.MovieDb.TV;

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

            Type type = typeof( T );

            if( type == typeof( IApiCompanyRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiCompanyRequest api = new ApiCompanyRequest( _settings, new ApiGenreRequest( _settings ) );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiConfigurationRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiConfigurationRequest api = new ApiConfigurationRequest( _settings );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiGenreRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiGenreRequest api = new ApiGenreRequest( _settings );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiMovieRatingRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiMovieRatingRequest api = new ApiMovieRatingRequest( _settings );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiMovieRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiMovieRequest api = new ApiMovieRequest( _settings, new ApiGenreRequest( _settings ) );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiPeopleRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiPeopleRequest api = new ApiPeopleRequest( _settings, new ApiGenreRequest( _settings ) );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiProfessionRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiProfessionRequest api = new ApiProfessionRequest( _settings );
                     return (T)api;
                 } );
            }

            if( type == typeof( IApiTVShowRequest ) )
            {
                return new Lazy<T>( () =>
                 {
                     IApiTVShowRequest api = new ApiTVShowRequest( _settings, new ApiGenreRequest( _settings ) );
                     return (T)api;
                 } );
            }


            throw new NotImplementedException( $"Factory has not registered for {type.FullName}" );
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

            throw new NotImplementedException();
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
                throw new InvalidOperationException( "RegisterSettings must be called before the Factory can Create anything." );
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
    }
}
