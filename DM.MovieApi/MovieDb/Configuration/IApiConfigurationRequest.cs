namespace DM.MovieApi.MovieDb.Configuration;

/// <summary>
/// Interface for retrieving themoviedb.org configuration information.
/// </summary>
public interface IApiConfigurationRequest : IApiRequest
{
    /// <summary>
    /// <para>Get themoviedb.org system wide configuration information. Some elements of themoviedb.org
    /// API require knowledge of the configuration data. The purpose of the <see cref="ApiConfiguration"/>
    /// is to try and keep the actual API responses as light as possible.</para>
    /// <para>It is recommended you cache this data within your application and check for updates every few days.
    /// This method currently holds the data relevant to building image URLs as well as the change key map.</para>
    /// </summary>
    Task<ApiQueryResponse<ApiConfiguration>> GetAsync();
}
