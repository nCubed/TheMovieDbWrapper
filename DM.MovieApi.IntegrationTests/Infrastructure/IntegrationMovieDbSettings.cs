using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests.Infrastructure
{
    internal class IntegrationMovieDbSettings : IMovieDbSettings
    {
        public const string FileName = "ApiCreds.xml";
        public readonly string FilePath = Path.Combine( InitDirectory, FileName );

        public string ApiKey { get; private set; }

        public string ApiUrl { get; private set; }

        /// <summary>
        /// Loads the API credentials from the _init\ApiCreds.xml.
        /// </summary>
        public IntegrationMovieDbSettings()
        {
            Hydrate();
        }

        /// <summary>
        /// Loads the API credentials from the provided values. 
        /// </summary>
        public IntegrationMovieDbSettings( string apiKey, string apiUrl )
        {
            ApiKey = apiKey;
            ApiUrl = apiUrl;
        }

        private void Hydrate()
        {
            Assert.IsTrue( File.Exists( FilePath ), FileName + " does not exist. Did you forget to set the xml file to be copied to the output directory?" );

            var doc = XDocument.Load( FilePath );

            // ReSharper disable PossibleNullReferenceException
            ApiKey = doc.Root.Element( "ApiKey" ).Value;
            ApiUrl = doc.Root.Element( "ApiUrl" ).Value;
            // ReSharper restore PossibleNullReferenceException
        }


        // ReSharper disable InconsistentNaming
        private const string _initDirectoryName = "_init";

        private static readonly Lazy<string> _rootDirectory = new Lazy<string>( GetRootDirectory );
        private static readonly Lazy<string> _initDirectory = new Lazy<string>( GetInitDirectory );

        internal static string RootDirectory => _rootDirectory.Value;
        internal static string InitDirectory => _initDirectory.Value;


        private static string GetRootDirectory()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string dir = Path.GetDirectoryName( location );

            Assert.IsFalse( string.IsNullOrEmpty( dir ) );
            Assert.IsTrue( Directory.Exists( dir ) );

            return dir;
        }

        private static string GetInitDirectory()
        {
            string dir = Path.Combine( RootDirectory, _initDirectoryName );

            Assert.IsFalse( string.IsNullOrEmpty( dir ) );
            Assert.IsTrue( Directory.Exists( dir ) );

            return dir;
        }

    }
}
