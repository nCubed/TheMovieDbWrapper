using System;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.TV
{
    [DataContract]
    public class Season
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "air_date" )]
        public DateTime AirDate { get; set; }

        [DataMember( Name = "episode_count" )]
        public int EpisodeCount { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "season_number" )]
        public int SeasonNumber { get; set; }

        public Season( int id, DateTime airDate, int episodeCount, string posterPath, int seasonNumber )
        {
            Id = id;
            AirDate = airDate;
            EpisodeCount = episodeCount;
            PosterPath = posterPath;
            SeasonNumber = seasonNumber;
        }

        public override string ToString()
            => $"({SeasonNumber} - {AirDate:yyyy-MM-dd})";
    }
}
