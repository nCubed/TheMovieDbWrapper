using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.People
{
    [DataContract]
    public class PersonTVCredit
    {
        [DataMember( Name = "id" )]
        public int PersonId { get; set; }

        [DataMember( Name = "cast" )]
        public IReadOnlyList<PersonTVCastMember> CastRoles { get; set; }

        [DataMember( Name = "crew" )]
        public IReadOnlyList<PersonTVCrewMember> CrewRoles { get; set; }
    }

    [DataContract]
    public class PersonTVCastMember
    {
        [DataMember( Name = "id" )]
        public int TVShowId { get; set; }

        [DataMember( Name = "character" )]
        public string Character { get; set; }

        [DataMember( Name = "credit_id" )]
        public string CreditId { get; set; }

        [DataMember( Name = "episode_count" )]
        public int EpisodeCount { get; set; }

        [DataMember( Name = "first_air_date" )]
        public DateTime FirstAirDate { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }
    }

    [DataContract]
    public class PersonTVCrewMember
    {
        [DataMember( Name = "id" )]
        public int TVShowId { get; set; }

        [DataMember( Name = "credit_id" )]
        public string CreditId { get; set; }

        [DataMember( Name = "department" )]
        public string Department { get; set; }

        [DataMember( Name = "episode_count" )]
        public int EpisodeCount { get; set; }

        [DataMember( Name = "first_air_date" )]
        public DateTime FirstAirDate { get; set; }

        [DataMember( Name = "job" )]
        public string Job { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }
    }
}
