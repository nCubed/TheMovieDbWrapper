using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.Shims;

namespace DM.MovieApi.MovieDb.Movies
{
    internal class ApiMovieRequest : ApiRequestBase, IApiMovieRequest
    {
        private readonly IApiGenreRequest _genreApi;

        [ImportingConstructor]
        public ApiMovieRequest( IMovieDbSettings settings, IApiGenreRequest genreApi )
            : base( settings )
        {
            _genreApi = genreApi;
        }

        public async Task<ApiQueryResponse<Movie>> FindByIdAsync( int movieId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            string command = $"movie/{movieId}";

            ApiQueryResponse<Movie> response = await base.QueryAsync<Movie>( command, param );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> SearchByTitleAsync( string query, int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"query", query},
                {"include_adult", "false"},
                {"language", language},
            };

            const string command = "search/movie";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }

        public async Task<ApiQueryResponse<Movie>> GetLatestAsync( string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            const string command = "movie/latest";

            ApiQueryResponse<Movie> response = await base.QueryAsync<Movie>( command, param );

            return response;
        }

        public async Task<ApiSearchResponse<Movie>> GetNowPlayingAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            const string command = "movie/now_playing";

            ApiSearchResponse<Movie> response = await base.SearchAsync<Movie>( command, pageNumber, param );

            return response;
        }

        public async Task<ApiSearchResponse<Movie>> GetUpcomingAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"append_to_response", "keywords"},
            };

            const string command = "movie/upcoming";

            ApiSearchResponse<Movie> response = await base.SearchAsync<Movie>( command, pageNumber, param );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> GetTopRatedAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            const string command = "movie/top_rated";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> GetPopularAsync( int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            const string command = "movie/popular";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }

        public async Task<ApiQueryResponse<MovieCredit>> GetCreditsAsync( int movieId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            string command = $"movie/{movieId}/credits";

            ApiQueryResponse<MovieCredit> response = await base.QueryAsync<MovieCredit>( command, param );

            return response;
        }
    }
}
