using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.Shims;

namespace DM.MovieApi.MovieDb.Discover
{
    internal class ApiDisoverRequest : ApiRequestBase, IApiDiscoverRequest
    {
        private readonly IApiGenreRequest _genreApi;

        [ImportingConstructor]
        public ApiDisoverRequest(IApiSettings settings, IApiGenreRequest genreApi) : base(settings)
        {
            _genreApi = genreApi;
        }

        public async Task<ApiSearchResponse<MovieInfo>> DiscoverMoviesAsync(IDiscoverMovieParameterBuilder paramBuilder, int pageNumber = 1, string language = "en")
        {
            var param = paramBuilder.Build();
            param.Add("language", language);
            const string command = "discover/movie";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>(command, pageNumber, param);
            response.Results.PopulateGenres(_genreApi);
            return response;
        }
    }
}
