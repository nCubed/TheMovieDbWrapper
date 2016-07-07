using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.Movies
{
    [DataContract]
    public class MovieInfo
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "title" )]
        public string Title { get; set; }

        [DataMember( Name = "adult" )]
        public bool IsAdultThemed { get; set; }

        [DataMember( Name = "backdrop_path" )]
        public string BackdropPath { get; set; }

        [DataMember( Name = "genre_ids" )]
        internal IReadOnlyList<int> GenreIds { get; set; }

        public IReadOnlyList<Genre> Genres { get; set; }

        [DataMember( Name = "original_title" )]
        public string OriginalTitle { get; set; }

        [DataMember( Name = "overview" )]
        public string Overview { get; set; }

        [DataMember( Name = "release_date" )]
        public DateTime ReleaseDate { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; set; }

        [DataMember( Name = "video" )]
        public bool Video { get; set; }

        [DataMember( Name = "vote_average" )]
        public double VoteAverage { get; set; }

        [DataMember( Name = "vote_count" )]
        public int VoteCount { get; set; }

        public MovieInfo()
        {
            GenreIds = new int[0];
            Genres = new Genre[0];
        }

        public override string ToString()
            => $"{Title} ({Id} - {ReleaseDate:yyyy-MM-dd})";
    }
}