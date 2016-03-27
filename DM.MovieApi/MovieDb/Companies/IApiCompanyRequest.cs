using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;

namespace DM.MovieApi.MovieDb.Companies
{
    /// <summary>
    /// Interface for retrieving information about a production company.
    /// </summary>
    public interface IApiCompanyRequest : IApiRequest
    {
        /// <summary>
        /// Gets all the basic information about a specific company.
        /// </summary>
        /// <param name="companyId">The company Id is typically found from a Movie or TV query.</param>
        Task<ApiQueryResponse<ProductionCompany>> FindByIdAsyc( int companyId );

        /// <summary>
        /// Get the list of movies associated with a particular company.
        /// </summary>
        /// <param name="companyId">The company Id is typically found from a Movie or TV query.</param>
        /// <param name="pageNumber">Default is page 1. The page number to retrieve; the <see cref="ApiSearchResponse{T}"/> will contain the current page returned and the total number of pages available.</param>
        /// <param name="language">Default is English. The ISO 639-1 language code to retrieve the result from.</param>
        /// <returns></returns>
        Task<ApiSearchResponse<MovieInfo>> GetMoviesAsync( int companyId, int pageNumber = 1, string language = "en" );
    }
}
