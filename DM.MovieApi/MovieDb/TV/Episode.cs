using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.TV
{
    [DataContract]
    public class Episode
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "air_date")]
        public DateTime AirDate { get; set; }

        [DataMember(Name = "episode_number")]
        public int EpisodeNumber { get; set; }

        [DataMember(Name = "crew")]
        public IReadOnlyList<Crew> Crew { get; set; }

        [DataMember(Name = "guest_stars")]
        public IReadOnlyList<GuestStars> GuestStars { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "overview")]
        public string Overview { get; set; }

        [DataMember(Name = "production_code")]
        public string ProductionCode { get; set; }

        [DataMember(Name = "season_number")]
        public int SeasonNumber { get; set; }

        [DataMember(Name = "still_path")]
        public string StillPath { get; set; }

        [DataMember(Name = "vote_average")]
        public float VoteAverage { get; set; }

        [DataMember(Name = "vote_count")]
        public int VoteCount { get; set; }

        public Episode()
        {
            Crew = Array.Empty<Crew>();
            GuestStars = Array.Empty<GuestStars>();
        }

        public override string ToString()
            => $"({Name} - {AirDate:yyyy-MM-dd})";
    }
}
