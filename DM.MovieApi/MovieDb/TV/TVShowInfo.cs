using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DM.MovieApi.MovieDb.Genres;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace DM.MovieApi.MovieDb.TV
{
    [DataContract]
    public class TVShowInfo
    {
        [DataMember( Name = "id" )]
        public int Id { get; private set; }

        [DataMember( Name = "name" )]
        public string Name { get; private set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; private set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "backdrop_path" )]
        public string BackdropPath { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; private set; }

        [DataMember( Name = "vote_average" )]
        public double VoteAverage { get; private set; }

        [DataMember( Name = "vote_count" )]
        public int VoteCount { get; private set; }

        [DataMember( Name = "overview" )]
        public string Overview { get; private set; }

        [DataMember( Name = "first_air_date" )]
        public DateTime FirstAirDate { get; private set; }

        [DataMember( Name = "origin_country" )]
        public IReadOnlyList<string> OriginCountry { get; private set; }

        [DataMember( Name = "genre_ids" )]
        internal IReadOnlyList<int> GenreIds { get; set; }

        public IReadOnlyList<Genre> Genres { get; internal set; }

        [DataMember( Name = "original_language" )]
        public string OriginalLanguage { get; private set; }

        public TVShowInfo()
        {
            OriginCountry = new string[0];
            GenreIds = new int[0];
            Genres = new Genre[0];
        }

        public override string ToString()
            => $"{Name} ({Id} - {FirstAirDate:yyyy-MM-dd})";
    }
}
