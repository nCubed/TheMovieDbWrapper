namespace DM.MovieApi.IntegrationTests.Infrastructure;

/// <summary>
/// Bare bones class providing access to the <see cref="ApiRequestBase"/>.
/// </summary>
internal class IntegrationApiRequest : ApiRequestBase
{
    public IntegrationApiRequest( IApiSettings settings )
        : base( settings )
    { }
}
