namespace DM.MovieApi.MovieDb.Collections;

[DataContract]
public class CollectionInfo
{
    [DataMember( Name = "id" )]
    public int Id { get; set; }

    [DataMember( Name = "name" )]
    public string Name { get; set; }

    [DataMember( Name = "poster_path" )]
    public string PosterPath { get; set; }

    [DataMember( Name = "backdrop_path" )]
    public string BackdropPath { get; set; }

    public override string ToString()
    {
        if( string.IsNullOrWhiteSpace( Name ) )
        {
            return "n/a";
        }

        return $"{Name} ({Id})";
    }
}
