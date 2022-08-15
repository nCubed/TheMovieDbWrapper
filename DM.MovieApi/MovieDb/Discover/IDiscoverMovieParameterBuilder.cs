using System.Collections.Generic;

namespace DM.MovieApi.MovieDb.Discover
{
    public interface IDiscoverMovieParameterBuilder
    {
        Dictionary<string, string> Build();
        IDiscoverMovieParameterBuilder WithOriginalLanguage( string language );
        IDiscoverMovieParameterBuilder WithCrew( int personId );
        IDiscoverMovieParameterBuilder WithCast( int personId );
        IDiscoverMovieParameterBuilder WithGenre( int genre );
        IDiscoverMovieParameterBuilder WithoutGenre( int genre );
    }
}
