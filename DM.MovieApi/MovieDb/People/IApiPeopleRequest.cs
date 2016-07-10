using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.People
{
    public interface IApiPeopleRequest : IApiRequest
    {
        /// <summary>
        /// Gets all the information about a specific Person.
        /// </summary>
        /// <param name="personId">The person Id is typically found from a more generic query such as movie or television or search.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<Person>> FindByIdAsync( int personId, string language = "en" );


        /// <summary>
        /// Get the movie credits for a specific person id. Includes movie cast and crew information for the person.
        /// </summary>
        /// <param name="personId">The person Id is typically found from a more generic query such as movie or television or search.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        Task<ApiQueryResponse<PersonMovieCredit>> GetMovieCreditsAsync( int personId, string language = "en" );


        // TODO: (K. Chase) [2016-07-09] Add GetTelevisionCreditsAsync
        // TODO: (K. Chase) [2016-07-09] Add SearchAsync
    }
}
