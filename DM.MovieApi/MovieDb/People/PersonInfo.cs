namespace DM.MovieApi.MovieDb.People;

public enum MediaType
{
    Unknown,
    Movie,
    TV,
}

[DataContract]
public class PersonInfo
{
    // TODO: (K. Chase) [2016-07-10] Update all POCO's to explicitly name the Id property, i.e,. PersonId, MovieId, TVShowId.
    [DataMember( Name = "id" )]
    public int Id { get; set; }

    [DataMember( Name = "name" )]
    public string Name { get; set; }

    [DataMember( Name = "adult" )]
    public bool IsAdultFilmStar { get; set; }

    [DataMember( Name = "known_for" )]
    public IReadOnlyList<PersonInfoRole> KnownFor { get; set; }

    [DataMember( Name = "profile_path" )]
    public string ProfilePath { get; set; }

    [DataMember( Name = "popularity" )]
    public double Popularity { get; set; }

    public PersonInfo()
    {
        KnownFor = Array.Empty<PersonInfoRole>();
    }

    public override string ToString()
        => $"{Name} ({Id})";
}

[DataContract]
public class PersonInfoRole
{
    // TODO: (K. Chase) [2016-07-10] Break into type for Movie and TV w/ a custom serializer.
    // re: see TVShowName v MovieTitle (and related)

    /// <summary>
    /// The MovieId or TVShowId as defined by the value of <see cref="MediaType"/>.
    /// </summary>
    [DataMember( Name = "id" )]
    public int Id { get; set; }

    [DataMember( Name = "media_type" )]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// Only populated when <see cref="MediaType"/> is TV.
    /// </summary>
    [DataMember( Name = "name" )]
    public string TVShowName { get; set; }

    /// <summary>
    /// Only populated when <see cref="MediaType"/> is TV.
    /// </summary>
    [DataMember( Name = "original_name" )]
    public string TVShowOriginalName { get; set; }

    /// <summary>
    /// Only populated when <see cref="MediaType"/> is Movie.
    /// </summary>
    [DataMember( Name = "title" )]
    public string MovieTitle { get; set; }

    /// <summary>
    /// Only populated when <see cref="MediaType"/> is Movie.
    /// </summary>
    [DataMember( Name = "original_title" )]
    public string MovieOriginalTitle { set; get; }

    [DataMember( Name = "backdrop_path" )]
    public string BackdropPath { get; set; }

    [DataMember( Name = "poster_path" )]
    public string PosterPath { get; set; }

    /// <summary>
    /// Only populated when <see cref="MediaType"/> is Movie.
    /// </summary>
    [DataMember( Name = "release_date" )]
    public DateTime MovieReleaseDate { get; set; }

    /// <summary>
    /// Only populated when <see cref="MediaType"/> is TV.
    /// </summary>
    [DataMember( Name = "first_air_date" )]
    public DateTime TVShowFirstAirDate { get; set; }

    [DataMember( Name = "overview" )]
    public string Overview { get; set; }

    [DataMember( Name = "adult" )]
    public bool IsAdultThemed { get; set; }

    [DataMember( Name = "video" )]
    public bool IsVideo { get; set; }

    [DataMember( Name = "genre_ids" )]
    internal IReadOnlyList<int> GenreIds { get; set; }

    public IReadOnlyList<Genre> Genres { get; set; }

    [DataMember( Name = "original_language" )]
    public string OriginalLanguage { get; set; }

    [DataMember( Name = "popularity" )]
    public double Popularity { get; set; }

    [DataMember( Name = "vote_count" )]
    public int VoteCount { get; set; }

    [DataMember( Name = "vote_average" )]
    public double VoteAverage { get; set; }

    [DataMember( Name = "origin_country" )]
    public IReadOnlyList<string> OriginCountry { get; set; }

    public PersonInfoRole()
    {
        GenreIds = Array.Empty<int>();
        Genres = Array.Empty<Genre>();
        OriginCountry = Array.Empty<string>();
    }

    public override string ToString()
    {
        return MediaType == MediaType.Movie
            ? $"Movie: {MovieTitle} ({Id} - {MovieReleaseDate:yyyy-MM-dd})"
            : $"TV: {TVShowName} ({Id} - {TVShowFirstAirDate:yyyy-MM-dd})";
    }
}
