namespace DM.MovieApi.MovieDb.TV;

[DataContract]
public class SeasonInfo
{
    [DataMember( Name = "id" )]
    public int Id { get; set; }

    [DataMember( Name = "air_date" )]
    public DateTime AirDate { get; set; }

    [DataMember( Name = "overview" )]
    public string Overview { get; set; }

    [DataMember( Name = "name" )]
    public string Name { get; set; }

    [DataMember( Name = "poster_path" )]
    public string PosterPath { get; set; }

    [DataMember( Name = "season_number" )]
    public int SeasonNumber { get; set; }

    [DataMember( Name = "episodes" )]
    public IReadOnlyList<Episode> Episodes { get; set; }

    public SeasonInfo()
    {
        Episodes = Array.Empty<Episode>();
    }

    public override string ToString()
        => $"{Name} - {AirDate:yyyy-MM-dd}";
}
