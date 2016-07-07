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
        public int Id { get; private set; }

        [DataMember( Name = "title" )]
        public string Title { get; private set; }

        [DataMember( Name = "adult" )]
        public bool IsAdultThemed { get; private set; }

        [DataMember( Name = "backdrop_path" )]
        public string BackdropPath { get; private set; }

        [DataMember( Name = "genre_ids" )]
        internal IReadOnlyList<int> GenreIds { get; set; }

        public IReadOnlyList<Genre> Genres { get; internal set; }

        [DataMember( Name = "original_title" )]
        public string OriginalTitle { get; private set; }

        [DataMember( Name = "overview" )]
        public string Overview { get; private set; }

        [DataMember( Name = "release_date" )]
        public DateTime ReleaseDate { get; private set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; private set; }

        [DataMember( Name = "video" )]
        public bool Video { get; private set; }

        [DataMember( Name = "vote_average" )]
        public double VoteAverage { get; private set; }

        [DataMember( Name = "vote_count" )]
        public int VoteCount { get; private set; }

        public MovieInfo()
        {
            GenreIds = new int[0];
            Genres = new Genre[0];
        }

        public override string ToString()
            => $"{Title} ({Id} - {ReleaseDate:yyyy-MM-dd})";
    }
}