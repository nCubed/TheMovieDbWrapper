using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.People
{
    /// <summary>
    /// Interface for retrieving information about People.
    /// </summary>
    public interface IApiPeopleRequest : IApiRequest
    {
        /// <summary>
        /// Gets all the information about a specific Person.
        /// </summary>
        /// <param name="personId">The person Id is typically found from a more generic query such as movie or television or search.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<Person>> FindByIdAsync( int personId, string language = "en" );

        /// <summary>
        /// Searches for People by name.
        /// </summary>
        /// <param name="query">The query to search for People.</param>
        /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiSearchResponse<PersonInfo>> SearchByNameAsync( string query, int pageNumber = 1, string language = "en" );

        /// <summary>
        /// Get the movie credits for a specific person id. Includes movie cast and crew information for the person.
        /// </summary>
        /// <param name="personId">The person Id is typically found from a more generic query such as movie or television or search.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<PersonMovieCredit>> GetMovieCreditsAsync( int personId, string language = "en" );

        /// <summary>
        /// Get the television credits for a specific person id. Includes TV cast and crew information for the person.
        /// </summary>
        /// <param name="personId">The person Id is typically found from a more generic query such as movie or television or search.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<PersonTVCredit>> GetTVCreditsAsync( int personId, string language = "en" );

        /// <summary>
        /// Gets all images for a specific Person.
        /// </summary>
        /// <param name="personId">The person Id is typically found from a more generic query such as movie or television or search.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<Images>> GetImagesAsync(int personId, string language = "en");
    }
}
