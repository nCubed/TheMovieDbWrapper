using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DM.MovieApi.MovieDb.Images;
using Newtonsoft.Json;

namespace DM.MovieApi.MovieDb.People
{
    [DataContract]
    [JsonConverter(typeof(ImagesConverter))]
    public class Person
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "also_known_as" )]
        public IReadOnlyList<string> AlsoKnownAs { get; set; }

        [DataMember( Name = "adult" )]
        public bool IsAdultFilmStar { get; set; }

        [DataMember( Name = "biography" )]
        public string Biography { get; set; }

        [DataMember( Name = "birthday" )]
        public DateTime Birthday { get; set; }

        [DataMember( Name = "deathday" )]
        public DateTime? Deathday { get; set; }

        [DataMember( Name = "gender" )]
        public Gender Gender { get; set; }

        [DataMember( Name = "homepage" )]
        public string Homepage { get; set; }

        [DataMember( Name = "imdb_id" )]
        public string ImdbId { get; set; }

        [DataMember( Name = "place_of_birth" )]
        public string PlaceOfBirth { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; set; }

        [DataMember( Name = "profile_path" )]
        public string ProfilePath { get; set; }

        [DataMember(Name = "profiles")]
        public IReadOnlyList<Image> Profiles { get; set; }

        public Person()
        {
            AlsoKnownAs = new string[0];
        }

        public override string ToString()
            => Name;
    }
}
