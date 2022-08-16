﻿using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace DM.MovieApi.IntegrationTests.Infrastructure;

internal class IntegrationMovieDbSettings : IApiSettings
{
    public const string FileName = "api.creds.json";
    public readonly string FilePath = Path.Combine( RootDirectory, FileName );

    public string ApiUrl => MovieDbFactory.TheMovieDbApiUrl;

    public string BearerToken { get; private set; }

    /// <summary>
    /// Loads the API credentials from api.creds.json in the root of the test project.
    /// </summary>
    public IntegrationMovieDbSettings()
    {
        Hydrate();
    }

    /// <summary>
    /// Loads the API credentials from the provided values. 
    /// </summary>
    public IntegrationMovieDbSettings( string apiBearerToken )
    {
        BearerToken = apiBearerToken;
    }

    private void Hydrate()
    {
        var anon = new
        {
            BearerToken = "your-v4-bearer-token-here"
        };

        if( File.Exists( FilePath ) == false )
        {
            string structure = JsonConvert.SerializeObject( anon, Formatting.Indented );

            Assert.Fail( $"The file {FileName} does not exist. Expected to find it here:\r\n{FilePath}\r\n\r\n" +
                         $"{FileName} must exist in the root of the integration project with your API Key " +
                         "from themoviedb.org. When the project is built, the json file will be output to " +
                         $"the directory listed above. Use the following json structure:\r\n{structure}\r\n" );
        }

        var json = File.ReadAllText( FilePath );
        var api = JsonConvert.DeserializeAnonymousType( json, anon );

        BearerToken = api.BearerToken;
    }


    // ReSharper disable InconsistentNaming
    private static readonly Lazy<string> _rootDirectory = new( GetRootDirectory );

    internal static string RootDirectory => _rootDirectory.Value;

    private static string GetRootDirectory()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        string dir = Path.GetDirectoryName( location );

        Assert.IsFalse( string.IsNullOrEmpty( dir ) );
        Assert.IsTrue( Directory.Exists( dir ) );

        return dir;
    }
}
