namespace DM.MovieApi.MovieDb.People;

[DataContract]
public class PersonMovieCredit
{
    [DataMember( Name = "id" )]
    public int PersonId { get; set; }

    [DataMember( Name = "cast" )]
    public IReadOnlyList<PersonMovieCastMember> CastRoles { get; set; }

    [DataMember( Name = "crew" )]
    public IReadOnlyList<PersonMovieCrewMember> CrewRoles { get; set; }

    public PersonMovieCredit()
    {
        CastRoles = Array.Empty<PersonMovieCastMember>();
        CrewRoles = Array.Empty<PersonMovieCrewMember>();
    }
}

[DataContract]
public class PersonMovieCastMember
{
    [DataMember( Name = "id" )]
    public int MovieId { get; set; }

    [DataMember( Name = "adult" )]
    public bool IsAdultThemed { get; set; }

    [DataMember( Name = "character" )]
    public string Character { get; set; }

    [DataMember( Name = "credit_id" )]
    public string CreditId { get; set; }

    [DataMember( Name = "original_title" )]
    public string OriginalTitle { get; set; }

    [DataMember( Name = "poster_path" )]
    public string PosterPath { get; set; }

    [DataMember( Name = "release_date" )]
    public DateTime ReleaseDate { get; set; }

    [DataMember( Name = "title" )]
    public string Title { get; set; }
}

[DataContract]
public class PersonMovieCrewMember
{
    [DataMember( Name = "id" )]
    public int MovieId { get; set; }

    [DataMember( Name = "adult" )]
    public bool IsAdultThemed { get; set; }

    [DataMember( Name = "credit_id" )]
    public string CreditId { get; set; }

    [DataMember( Name = "department" )]
    public string Department { get; set; }

    [DataMember( Name = "job" )]
    public string Job { get; set; }

    [DataMember( Name = "original_title" )]
    public string OriginalTitle { get; set; }

    [DataMember( Name = "poster_path" )]
    public string PosterPath { get; set; }

    [DataMember( Name = "release_date" )]
    public DateTime ReleaseDate { get; set; }

    [DataMember( Name = "title" )]
    public string Title { get; set; }
}
