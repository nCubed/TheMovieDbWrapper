using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Movies
{
    [DataContract]
    public class MovieCredit
    {
        [DataMember( Name = "id" )]
        public int MovieId { get; set; }

        [DataMember( Name = "cast" )]
        public IReadOnlyList<MovieCastMember> CastMembers { get; set; }

        [DataMember( Name = "crew" )]
        public IReadOnlyList<MovieCrewMember> CrewMembers { get; set; }
    }

    [DataContract]
    public class MovieCastMember
    {
        [DataMember( Name = "id" )]
        public int PersonId { get; set; }

        [DataMember( Name = "cast_id" )]
        public int CastId { get; set; }

        [DataMember( Name = "credit_id" )]
        public string CreditId { get; set; }

        [DataMember( Name = "character" )]
        public string Character { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "order" )]
        public int Order { get; set; }

        [DataMember( Name = "profile_path" )]
        public string ProfilePath { get; set; }

        public override string ToString()
            => $"{Character}: {Name}";
    }

    [DataContract]
    public class MovieCrewMember
    {
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

        [DataMember( Name = "profile_path" )]
        public string ProfilePath { get; set; }

        public override string ToString()
            => $"{Name} | {Department} | {Job}";
    }
}
