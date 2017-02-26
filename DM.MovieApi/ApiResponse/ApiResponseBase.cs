namespace DM.MovieApi.ApiResponse
{
    /// <summary>
    /// Base class for all API responses from themoviedb.org.
    /// </summary>
    public abstract class ApiResponseBase
    {
        /// <summary>
        /// Contains specific error information if an error was encountered during the API call to themoviedb.org.
        /// </summary>
        public ApiError Error { get; internal set; }

        /// <summary>
        /// Contains the current rate limits from your most recent API call to themoviedb.org.
        /// </summary>
        public ApiRateLimit RateLimit { get; internal set; }

        /// <summary>
        /// The API command text used for the API call to themoviedb.org.
        /// </summary>
        public string CommandText { get; internal set; }

        /// <summary>
        /// The JSON returned from themoviedb.org based on the <see cref="CommandText"/> query.
        /// </summary>
        public string Json { get; internal set; }

        public override string ToString()
            => CommandText;
    }
}
