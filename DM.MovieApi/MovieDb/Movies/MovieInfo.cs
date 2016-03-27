using System;
using System.Collections.Generic;
using System.Linq;
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
        private IReadOnlyList<int> GenreIds { get; set; }

        public IReadOnlyList<Genre> Genres { get; private set; }

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

        internal void PopulateGenres( IEnumerable<Genre> allGenres )
        {
            if( !GenreIds.Any() )
            {
                return;
            }

            // TODO: (K. Chase) [2016-01-03] Look into creating a custom deserializer that will populate Genres.
            Genres = GenreIds
                .Select( x => allGenres.First( y => y.Id == x ) )
                .ToList()
                .AsReadOnly();
        }

        public override string ToString()
        {
            return string.Format( "{0} ({1} - {2:yyyy-MM-dd})", Title, Id, ReleaseDate );
        }
    }
}