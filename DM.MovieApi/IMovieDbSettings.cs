namespace DM.MovieApi
{
    /// <summary>
    /// <para>Interface consumers must implement to access any of the API's exposed against themoviedb.org.</para>
    /// <para>The concrete implementation can be used with <see cref="DM.MovieApi.MovieDbFactory"/> to register your specific settings.</para>
    /// <para>Alternatively, you can use MEF to expose your settings and import as needed. See our online documentation for more information.</para>
    /// </summary>
    public interface IMovieDbSettings
    {
        // TODO: allow the ApiUrl to be empty and fallback to the MovieDbFactory.TheMovieDbApiUrl const.
        /// <include file='ApiDocs.xml' path='Doc/ApiSettings/ApiUrl/*'/>
        string ApiUrl { get; }

        /// <include file='ApiDocs.xml' path='Doc/ApiSettings/BearerToken/*'/>
        string ApiBearerToken { get; }
    }
}
