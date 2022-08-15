using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;

namespace DM.MovieApi.MovieDb.Discover
{
    public interface IApiDiscoverRequest : IApiRequest
    {
        Task<ApiSearchResponse<MovieInfo>> DiscoverMoviesAsync( IDiscoverMovieParameterBuilder paramBuilder, int pageNumber = 1, string language = "en" );
    }
}
