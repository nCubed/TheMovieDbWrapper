namespace DM.MovieApi.MovieDb.Configuration;

internal class ApiConfigurationRequest : ApiRequestBase, IApiConfigurationRequest
{
    [ImportingConstructor]
    public ApiConfigurationRequest( IApiSettings settings )
        : base( settings )
    { }

    public async Task<ApiQueryResponse<ApiConfiguration>> GetAsync()
    {
        ApiQueryResponse<ApiConfiguration> response = await base.QueryAsync<ApiConfiguration>( "configuration" );

        return response;
    }
}
