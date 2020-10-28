using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DM.MovieApi.IntegrationTests.Infrastructure
{
    internal class IntegrationMovieDbSettings : IMovieDbSettings
    {
        public const string FileName = "api.creds.json";
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
            Assert.IsTrue(File.Exists(FilePath),
                $"{FileName} does not exist. Expected to find it here:\r\n{FilePath}\r\n\r\n" +
                "Did you forget to set the file to be copied to the output directory?");

            var anon = new
            {
                ApiKey = "", ApiUrl = ""
            };

            var json = File.ReadAllText(FilePath);
            var api = JsonConvert.DeserializeAnonymousType(json, anon);

            ApiKey = api.ApiKey;
            ApiUrl = api.ApiUrl;
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
            Assert.IsTrue( Directory.Exists( dir ), $"\r\nExpected Directory: {dir}\r\n\r\n" );

            return dir;
        }

    }
}
