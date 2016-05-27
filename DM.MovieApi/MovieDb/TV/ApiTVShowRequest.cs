using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.TV
{
    [Export( typeof( IApiTVShowRequest ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class ApiTVShowRequest : ApiRequestBase, IApiTVShowRequest
    {
        private readonly IApiGenreRequest _genreApi;

        [ImportingConstructor]
        public ApiTVShowRequest( IMovieDbSettings settings, IApiGenreRequest genreApi ) : base( settings )
        {
            _genreApi = genreApi;
        }

        public async Task<ApiQueryResponse<TVShow>> FindByIdAsync( int tvShowId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            string command = string.Format( "tv/{0}", tvShowId );

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

            return response;
        }

        public async Task<ApiQueryResponse<TVShow>> GetLatestAsync()
        {
            const string command = "tv/latest";

            ApiQueryResponse<TVShow> response = await base.QueryAsync<TVShow>( command );

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

            return response;
        }
    }
}
