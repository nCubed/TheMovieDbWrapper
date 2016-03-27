using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DM.MovieApi.MovieDb.Collections;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.Movies
{
    [DataContract]
    public class Movie
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "title" )]
        public string Title { get; set; }

        [DataMember( Name = "adult" )]
        public bool IsAdultThemed { get; set; }

        [DataMember( Name = "backdrop_path" )]
        public string BackdropPath { get; set; }

        [DataMember( Name = "belongs_to_collection" )]
        public CollectionInfo MovieCollectionInfo { get; set; }

        [DataMember( Name = "budget" )]
        public int Budget { get; set; }

        [DataMember( Name = "genres" )]
        public IReadOnlyList<Genre> Genres { get; set; }

        [DataMember( Name = "homepage" )]
        public string Homepage { get; set; }

        [DataMember( Name = "imdb_id" )]
        public string ImdbId { get; set; }

        /// <summary>
        /// ISO 3166-1 code.
        /// </summary>
        [DataMember( Name = "original_language" )]
        public string OriginalLanguage { get; set; }

        [DataMember( Name = "original_title" )]
        public string OriginalTitle { get; set; }

        [DataMember( Name = "overview" )]
        public string Overview { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "production_companies" )]
        public IReadOnlyList<ProductionCompanyInfo> ProductionCompanies { get; set; }

        [DataMember( Name = "production_countries" )]
        public IReadOnlyList<Country> ProductionCountries { get; set; }

        [DataMember( Name = "release_date" )]
        public DateTime ReleaseDate { get; set; }

        [DataMember( Name = "revenue" )]
        public int Revenue { get; set; }

        [DataMember( Name = "runtime" )]
        public int Runtime { get; set; }

        [DataMember( Name = "spoken_languages" )]
        public IReadOnlyList<Language> SpokenLanguages { get; set; }

        [DataMember( Name = "status" )]
        public string Status { get; set; }

        [DataMember( Name = "tagline" )]
        public string Tagline { get; set; }

        [DataMember( Name = "video" )]
        public bool IsVideo { get; set; }

        [DataMember( Name = "vote_average" )]
        public double VoteAverage { get; set; }

        [DataMember( Name = "vote_count" )]
        public int VoteCount { get; set; }

        public Movie()
        {
            Genres = new Genre[0];
            ProductionCompanies = new ProductionCompanyInfo[0];
            ProductionCountries = new Country[0];
            SpokenLanguages = new Language[0];
        }

        public override string ToString()
        {
            return string.Format( "{0} ({1}) [{2}]", Title, ReleaseDate.ToString( "yyyy-MM-dd" ), Id );
        }
    }
}
