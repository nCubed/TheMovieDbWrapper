using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.Shims;

namespace DM.MovieApi.MovieDb.TV
{
    internal class ApiTVShowRequest : ApiRequestBase, IApiTVShowRequest
    {
        private readonly IApiGenreRequest _genreApi;

        [ImportingConstructor]
        public ApiTVShowRequest( IApiSettings settings, IApiGenreRequest genreApi )
            : base( settings )
        {
            _genreApi = genreApi;
        }

        public async Task<ApiQueryResponse<TVShow>> FindByIdAsync( int tvShowId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                { "language", language },
                { "append_to_response", "keywords" },
            };

            string command = $"tv/{tvShowId}";

            ApiQueryResponse<TVShow> response = await base.QueryAsync<TVShow>( command, param );

            return response;
        }

        public async Task<ApiSearchResponse<TVShowInfo>> SearchByNameAsync( string query, int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                { "query", query },
                { "language", language }
            };

            const string command = "search/tv";

            ApiSearchResponse<TVShowInfo> response = await base.SearchAsync<TVShowInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }

        public async Task<ApiQueryResponse<TVShow>> GetLatestAsync( string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                { "language", language },
                { "append_to_response", "keywords" },
            };

            const string command = "tv/latest";

            ApiQueryResponse<TVShow> response = await base.QueryAsync<TVShow>( command, param );

            return response;
        }

        public async Task<ApiSearchResponse<TVShowInfo>> GetTopRatedAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                { "language", language }
            };

            const string command = "tv/top_rated";

            ApiSearchResponse<TVShowInfo> response = await base.SearchAsync<TVShowInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }

        public async Task<ApiSearchResponse<TVShowInfo>> GetPopularAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                { "language", language }
            };

            const string command = "tv/popular";

            ApiSearchResponse<TVShowInfo> response = await base.SearchAsync<TVShowInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }
    }
}
