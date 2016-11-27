namespace DM.MovieApi
{
    /// <summary>
    /// <para>Interface consumers must implement to acccess any of the API's exposed against themoviedb.org.</para>
    /// <para>The concrete implementation can be used with <see cref="DM.MovieApi.MovieDbFactory"/> to register your specific settings.</para>
    /// <para>Alternatively, you can use MEF to expose your settings and import as needed. See our online documentation for more information.</para>
    /// </summary>
    public interface IMovieDbSettings
    {
        /// <summary>
        /// Private key required to query themoviedb.org API.
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// <para>URL used for api calls to themoviedb.org.</para>
        /// <para>Current URL is: http://api.themoviedb.org/3/ </para>
        /// </summary>
        string ApiUrl { get; }
    }
}
