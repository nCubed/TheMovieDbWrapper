using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.Certifications
{
    /// <summary>
    /// Interface for retrieving movie rating information.
    /// </summary>
    public interface IApiMovieRatingRequest : IApiRequest
    {
        /// <summary>
        /// Gets the list of supported certifications (movie ratings) for movies.
        /// </summary>
        Task<ApiQueryResponse<MovieRatings>> GetMovieRatingsAsync();
    }
}
