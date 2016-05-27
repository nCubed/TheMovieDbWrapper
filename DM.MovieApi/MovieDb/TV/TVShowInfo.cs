using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.TV
{
    [DataContract]
    public class TVShowInfo
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "backdrop_path" )]
        public string BackdropPath { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; set; }

        [DataMember( Name = "vote_average" )]
        public double VoteAverage { get; set; }

        [DataMember( Name = "vote_count" )]
        public int VoteCount { get; set; }

        [DataMember( Name = "overview" )]
        public string Overview { get; set; }

        [DataMember( Name = "first_air_date" )]
        public DateTime FirstAirDate { get; set; }

        [DataMember( Name = "origin_country" )]
        public IReadOnlyList<string> OriginCountry { get; set; }

        [DataMember( Name = "genre_ids" )]
        public IReadOnlyList<int> GenreIds { get; set; }

        public IReadOnlyList<Genre> Genres { get; set; }

        [DataMember( Name = "original_language" )]
        public string OriginalLanguage { get; set; }

        public TVShowInfo()
        {
            OriginCountry = new string[0];
            GenreIds = new int[0];
            Genres = new Genre[0];
        }

        public override string ToString()
        {
            return string.Format( "{0} ({1} - {2:yyyy-MM-dd})", Name, Id, FirstAirDate );
        }
    }
}
