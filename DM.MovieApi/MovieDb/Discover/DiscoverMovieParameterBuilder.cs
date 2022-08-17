namespace DM.MovieApi.MovieDb.Discover;

public class DiscoverMovieParameterBuilder
{
    private readonly Dictionary<string, HashSet<string>> _param;

    public DiscoverMovieParameterBuilder()
    {
        _param = new Dictionary<string, HashSet<string>>();
    }

    /// <summary>
    /// Builds all parameters for use with an <see cref="ApiRequest.IApiRequest"/>.
    /// Typically called by the internal engine.
    /// </summary>
    public Dictionary<string, string> Build()
    {
        var param = new Dictionary<string, string>();

        foreach( var kvp in _param )
        {
            param.Add( kvp.Key, string.Join( ",", kvp.Value ) );
        }

        return param;
    }

    /// <summary>
    /// <para>Only include movies that have one of the cast members.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder WithCast( params int[] personId )
    {
        AddMultiParamType( "with_cast", personId );

        return this;
    }

    /// <summary>
    /// <para>Only include movies that have one of the crew members.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder WithCrew( params int[] personId )
    {
        AddMultiParamType( "with_crew", personId );

        return this;
    }

    /// <summary>
    /// <para>Only include movies that have either a cast or crew member with the
    /// provided value.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder WithPeople( params int[] personId )
    {
        AddMultiParamType( "with_people", personId );

        return this;
    }

    /// <summary>
    /// <para>Add for each genre to be included in the query.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder WithGenre( params int[] genreId )
    {
        AddMultiParamType( "with_genres", genreId );

        return this;
    }

    /// <summary>
    /// <para>Add for each genre to be included in the query.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder WithGenre( params Genre[] genre )
        => WithGenre( genre.Select( x => x.Id ).ToArray() );

    /// <summary>
    /// <para>Add for each genre to be excluded from the query.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder ExcludeGenre( params int[] genreId )
    {
        AddMultiParamType( "without_genres", genreId );

        return this;
    }

    /// <summary>
    /// <para>Add for each genre to be excluded from the query.</para>
    /// <para>May be invoked more than once to add additional values.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder ExcludeGenre( params Genre[] genre )
        => ExcludeGenre( genre.Select( x => x.Id ).ToArray() );

    /// <summary>
    /// <para>Specify an ISO 639-1 string to filter results by their original language value.</para>
    /// <para>Invoking more than once will overwrite the prior value.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder WithOriginalLanguage( string language )
    {
        AddSingleParamType( "with_original_language", language );

        return this;
    }

    /// <summary>
    /// <para>Return the results sorted. Default value is popularity descending.</para>
    /// <para>Invoking more than once will overwrite the prior value.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder SortBy( DiscoverSortBy sort, SortDirection dir )
    {
        string value = $"{sort.GetDescription()}.{dir.GetDescription()}";

        AddSingleParamType( "sort_by", value );

        return this;
    }

    /// <summary>
    /// <para>A filter to limit the results to a specific primary release year.</para>
    /// <para>Invoking more than once will overwrite the prior value.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder PrimaryReleaseYear( int year )
    {
        AddSingleParamType( "primary_release_year", year.ToString() );

        return this;
    }

    /// <summary>
    /// <para>A filter to limit the results to a specific year (looking at all release dates).</para>
    /// <para>Invoking more than once will overwrite the prior value.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder Year( int year )
    {
        AddSingleParamType( "year", year.ToString() );

        return this;
    }

    /// <summary>
    /// <para>Filter and only include movies that have a primary release date constrained
    /// by the FilterExp.</para>
    /// <para>May be invoked once for each type of FilterExp;
    /// subsequent invocations will overwrite the prior value.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder PrimaryReleaseDate( DateTime date, FilterExp exp )
    {
        string key = $"primary_release_date.{exp.GetDescription()}";

        AddSingleParamType( key, date.ToString( "yyyy-MM-dd" ) );

        return this;
    }

    /// <summary>
    /// <para>Filter and only include movies that have a release date (looking at all release
    /// dates) constrained by the FilterExp.</para>
    /// <para>May be invoked once for each type of FilterExp;
    /// subsequent invocations will overwrite the prior value.</para>
    /// </summary>
    public DiscoverMovieParameterBuilder ReleaseDate( DateTime date, FilterExp exp )
    {
        string key = $"release_date.{exp.GetDescription()}";

        AddSingleParamType( key, date.ToString( "yyyy-MM-dd" ) );

        return this;
    }

    private void AddMultiParamType( string name, IEnumerable<string> values )
    {
        _param.TryAdd( name, new HashSet<string>( StringComparer.OrdinalIgnoreCase ) );

        foreach( string value in values )
        {
            _param[name].Add( value );
        }
    }

    private void AddMultiParamType( string name, IEnumerable<int> values )
        => AddMultiParamType( name, values.Select( x => x.ToString() ) );

    private void AddSingleParamType( string name, string value )
        => _param[name] = new HashSet<string>( new[] { value } );
}
