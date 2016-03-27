using System.ComponentModel.Composition;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;

namespace DM.MovieApi.MovieDb.Configuration
{
    [Export( typeof( IApiConfigurationRequest ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class ApiConfigurationRequest : ApiRequestBase, IApiConfigurationRequest
    {
        [ImportingConstructor]
        public ApiConfigurationRequest( IMovieDbSettings settings )
            : base( settings )
        { }

        public async Task<ApiQueryResponse<ApiConfiguration>> GetAsync()
        {
            ApiQueryResponse<ApiConfiguration> response = await base.QueryAsync<ApiConfiguration>( "configuration" );

            return response;
        }
    }
}