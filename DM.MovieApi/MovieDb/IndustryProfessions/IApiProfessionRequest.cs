namespace DM.MovieApi.MovieDb.IndustryProfessions;

/// <summary>
/// Interface for retrieving information about Movie/TV industry specific professions.
/// </summary>
public interface IApiProfessionRequest : IApiRequest
{
    /// <summary>
    /// Gets all the Movie/TV industry specific professions.
    /// </summary>
    Task<ApiQueryResponse<IReadOnlyList<Profession>>> GetAllAsync();
}
