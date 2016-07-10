using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.People
{
    [Export( typeof( IApiPeopleRequest ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class ApiPeopleRequest : ApiRequestBase, IApiPeopleRequest
    {
        [ImportingConstructor]
        public ApiPeopleRequest( IMovieDbSettings settings )
            : base( settings )
        { }

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