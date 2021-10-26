namespace DM.MovieApi
{
    internal interface IApiSettings
    {
        string ApiUrl { get; }

        string BearerToken { get; }
    }
}
