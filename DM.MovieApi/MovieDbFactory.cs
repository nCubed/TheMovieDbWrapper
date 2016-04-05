using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using DM.MovieApi.ApiRequest;

namespace DM.MovieApi
{
    /// <summary>
    /// Note: <see cref="RegisterSettings"/> must be called before the Factory can Create anything.
    /// </summary>
    public static class MovieDbFactory
    {
        private static CompositionContainer _container;

        /// <summary>
        /// Determines if the underlying MEF factory has been created.
        /// </summary>
        public static bool IsFactoryComposed { get { return _container != null; } }

        /// <summary>
        /// Registers themoviedb.org settings for use with the MEF container.
        /// </summary>
        /// <param name="settings">The implementation of <see cref="IMovieDbSettings"/> containing
        /// the themoviedb.org credentials to use when connecting to the service.</param>
        public static void RegisterSettings( IMovieDbSettings settings )
        {
            _container = CreateContainer();

            _container.ComposeExportedValue( settings );
        }

        /// <summary>
        /// Registers themoviedb.org settings for use with the MEF container.
        /// </summary>
        /// <param name="apiKey">Private key required to query themoviedb.org API.</param>
        /// <param name="apiUrl">URL used for api calls to themoviedb.org.</param>
        public static void RegisterSettings( string apiKey, string apiUrl )
        {
            var settings = new MovieDbSettings( apiKey, apiUrl );

            RegisterSettings( settings );
        }

        /// <summary>
        /// <para>Creates the specific API requested.</para>
        /// <para>Note: <see cref="RegisterSettings"/> must be called before the Factory can Create anything.</para>
        /// </summary>
        public static Lazy<T> Create<T>() where T : IApiRequest
        {
            ContainerGuard();

            return _container.GetExport<T>();
        }

        /// <summary>
        /// <para>Creates the global instance exposing all API interfaces against themoviedb.org
        /// that are currently available in this release.</para>
        /// <para>Note: <see cref="RegisterSettings"/> must be called before the Factory can Create anything.</para>
        /// </summary>
        public static IMovieDbApi GetAllApiRequests()
        {
            ContainerGuard();

            Lazy<IMovieDbApi> api = _container.GetExport<IMovieDbApi>();

            // ReSharper disable once PossibleNullReferenceException
            return api.Value;
        }

        /// <summary>
        /// Clears the MEF container; forces the next call to be <see cref="RegisterSettings"/>
        /// before <see cref="Create{T}"/> can be called.
        /// </summary>
        public static void ResetFactory()
        {
            if( _container != null )
            {
                _container.Dispose();
                _container = null;
            }
        }

        private static CompositionContainer CreateContainer()
        {
            Assembly exe = Assembly.GetExecutingAssembly();

            var referenced = exe.GetReferencedAssemblies()
                .Where( x => x.FullName.StartsWith( "DM.", StringComparison.OrdinalIgnoreCase ) );

            var assemblies = new List<Assembly>( referenced.Select( Assembly.Load ) ) { exe };

            var parts = assemblies.Select( x => new AssemblyCatalog( x ) );

            var catalog = new AggregateCatalog( parts );

            var container = new CompositionContainer( catalog );

            return container;
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
            public string ApiKey { get; private set; }
            public string ApiUrl { get; private set; }

            public MovieDbSettings( string apiKey, string apiUrl )
            {
                ApiKey = apiKey;
                ApiUrl = apiUrl;
            }
        }
    }
}
