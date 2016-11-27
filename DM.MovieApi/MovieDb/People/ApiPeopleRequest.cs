using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.People
{
    internal class ApiPeopleRequest : ApiRequestBase, IApiPeopleRequest
    {
        private readonly IApiGenreRequest _genreApi;

        public ApiPeopleRequest( IMovieDbSettings settings, IApiGenreRequest genreApi )
            : base( settings )
        {
            _genreApi = genreApi;
        }

        public async Task<ApiQueryResponse<Person>> FindByIdAsync( int personId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language}
            };

            string command = $"person/{personId}";

            ApiQueryResponse<Person> response = await base.QueryAsync<Person>( command, param );

            return response;
        }

        public async Task<ApiSearchResponse<PersonInfo>> SearchByNameAsync( string query, int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"query", query},
                {"include_adult", "false"},
                {"language", language},
            };

            const string command = "search/person";

            ApiSearchResponse<PersonInfo> response = await base.SearchAsync<PersonInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }

        public async Task<ApiQueryResponse<PersonMovieCredit>> GetMovieCreditsAsync( int personId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            string command = $"person/{personId}/movie_credits";

            ApiQueryResponse<PersonMovieCredit> response = await base.QueryAsync<PersonMovieCredit>( command, param );

            return response;
        }

        public async Task<ApiQueryResponse<PersonTVCredit>> GetTVCreditsAsync( int personId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            string command = $"person/{personId}/tv_credits";

            ApiQueryResponse<PersonTVCredit> response = await base.QueryAsync<PersonTVCredit>( command, param );

            return response;
        }
    }
}
