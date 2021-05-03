using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.TV
{
    [DataContract]
    public class TVShowEpisodeCredit
    {
        [DataMember( Name = "id" )]
        public int TVShowEpisodeId { get; set; }

        [DataMember( Name = "cast" )]
        public IReadOnlyList<TVShowEpisodeCastMember> CastMembers { get; set; }

        [DataMember( Name = "crew" )]
        public IReadOnlyList<TVShowEpisodeCrewMember> CrewMembers { get; set; }

        [DataMember(Name = "guest_stars")]
        public IReadOnlyList<TVShowEpisodeGuestStarsMember> GuestStarsMembers { get; set; }


        public TVShowEpisodeCredit()
        {
            CastMembers = new TVShowEpisodeCastMember[0];
            CrewMembers = new TVShowEpisodeCrewMember[0];
            GuestStarsMembers = new TVShowEpisodeGuestStarsMember[0];
        }
    }

    [DataContract]
    public class TVShowEpisodeCastMember
    {
        //[DataMember(Name = "adult")]
        //public bool IsAdultFilmStar { get; set; }



        [DataMember( Name = "character" )]
        public string Character { get; set; }

        [DataMember( Name = "credit_id" )]
        public string CreditId { get; set; }

        [DataMember(Name = "gender")]
        public int Gender { get; set; }

        [DataMember(Name = "id")]
        public int PersonId { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "profile_path")]
        public string ProfilePath { get; set; }

        public override string ToString()
        {
            return $"{Character}: {Name}";
        }
    }

    [DataContract]
    public class TVShowEpisodeCrewMember
    {
        [DataMember(Name = "gender")]
        public int Gender { get; set; }

        [DataMember( Name = "id" )]
        public int PersonId { get; set; }

        [DataMember( Name = "credit_id" )]
        public string CreditId { get; set; }

        [DataMember( Name = "department" )]
        public string Department { get; set; }

        [DataMember( Name = "job" )]
        public string Job { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; set; }

        [DataMember(Name = "profile_path")]
        public string ProfilePath { get; set; }

        public override string ToString()
        {
            return $"{Name} | {Department} | {Job}";
        }
    }

    [DataContract]
    public class TVShowEpisodeGuestStarsMember
    {
        [DataMember(Name = "character_name")]
        public string Character { get; set; }

        [DataMember(Name = "credit_id")]
        public string CreditId { get; set; }

        [DataMember(Name = "gender")]
        public int Gender { get; set; }

        [DataMember(Name = "id")]
        public int PersonId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "original_name")]
        public string OriginalName { get; set; }

        [DataMember(Name = "order")]
        public int Order { get; set; }

        [DataMember(Name = "profile_path")]
        public string ProfilePath { get; set; }

        public override string ToString()
        {
            return $"{Character}: {Name}";
        }
    }
}
