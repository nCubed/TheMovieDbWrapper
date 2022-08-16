namespace DM.MovieApi.MovieDb.Discover;

public class DiscoverMovieParameterBuilder : IDiscoverMovieParameterBuilder
{
    private readonly Dictionary<string, IList<string>> _param;

    public DiscoverMovieParameterBuilder()
    {
        _param = new Dictionary<string, IList<string>>();
    }

    public Dictionary<string, string> Build()
    {
        var param = new Dictionary<string, string>();

        foreach( var kvp in _param )
        {
            param.Add( kvp.Key, string.Join( ",", kvp.Value ) );
        }

        return param;
    }

    public IDiscoverMovieParameterBuilder WithCast( int personId )
    {
        AddParamType( "with_cast" );

        _param["with_cast"].Add( personId.ToString() );

        return this;
    }

    public IDiscoverMovieParameterBuilder WithCrew( int personId )
    {
        AddParamType( "with_crew" );

        _param["with_crew"].Add( personId.ToString() );

        return this;
    }

    public IDiscoverMovieParameterBuilder WithGenre( int genreId )
    {
        AddParamType( "with_genres" );

        _param["with_genres"].Add( genreId.ToString() );

        return this;
    }

    public IDiscoverMovieParameterBuilder WithOriginalLanguage( string language )
    {
        AddParamType( "original_language" );

        _param["original_language"].Add( language );

        return this;
    }

    public IDiscoverMovieParameterBuilder ExcludeGenre( int genreId )
    {
        AddParamType( "without_genres" );

        _param["without_genres"].Add( genreId.ToString() );

        return this;
    }

    private void AddParamType( string name )
        => _param.TryAdd( name, new List<string>() );
}
