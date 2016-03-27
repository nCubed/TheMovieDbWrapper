namespace DM.MovieApi.ApiResponse
{
    /// <summary>
    /// Standard response from an API call returning a single specific result.
    /// Multiple item based based results (i.e., searches) are returned with an <see cref="ApiQueryResponse{T}"/>.
    /// </summary>
    public class ApiQueryResponse<T> : ApiResponseBase
    {
        /// <summary>
        /// The item returned from the API call.
        /// </summary>
        public T Item { get; internal set; }

        public override string ToString()
        {
            return Item.ToString();
        }
    }
}