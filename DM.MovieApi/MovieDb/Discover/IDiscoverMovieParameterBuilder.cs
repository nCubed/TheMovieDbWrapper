namespace DM.MovieApi.MovieDb.Discover
{
    public interface IDiscoverMovieParameterBuilder : IDiscoverParameterBuilder
    {
        IDiscoverMovieParameterBuilder WithOriginalLanguage( string language );
        IDiscoverMovieParameterBuilder WithCrew( int personId );
        IDiscoverMovieParameterBuilder WithCast( int personId );
        IDiscoverMovieParameterBuilder WithGenre( int genre );
        IDiscoverMovieParameterBuilder WithoutGenre( int genre );
    }
}
