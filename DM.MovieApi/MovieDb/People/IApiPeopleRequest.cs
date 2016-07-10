using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.People
{
    public interface IApiPeopleRequest : IApiRequest
    {
        Task<ApiQueryResponse<Person>> FindByIdAsync( int personId, string language = "en" );

        Task<ApiQueryResponse<PersonMovieCredit>> GetMovieCreditsAsync( int personId, string language = "en" );

        // TODO: (K. Chase) [2016-07-09] Add GetTelevisionCreditsAsync
        // TODO: (K. Chase) [2016-07-09] Add SearchAsync
    }

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
    }
}
