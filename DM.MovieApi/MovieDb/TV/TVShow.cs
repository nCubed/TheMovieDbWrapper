using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Genres;

namespace DM.MovieApi.MovieDb.TV
{
    [DataContract]
    public class TVShow
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "backdrop_path" )]
        public string BackdropPath { get; set; }

        [DataMember( Name = "created_by" )]
        public IReadOnlyList<TVShowCreator> CreatedBy { get; set; }

        [DataMember( Name = "episode_run_time" )]
        public IReadOnlyList<int> EpisodeRunTime { get; set; }

        [DataMember( Name = "first_air_date" )]
        public DateTime FirstAirDate { get; set; }

        [DataMember( Name = "genres" )]
        public IReadOnlyList<Genre> Genres { get; set; }

        [DataMember( Name = "homepage" )]
        public string Homepage { get; set; }

        [DataMember( Name = "in_production" )]
        public bool InProduction { get; set; }

        [DataMember( Name = "languages" )]
        public IReadOnlyList<string> Languages { get; set; }

        [DataMember( Name = "last_air_date" )]
        public DateTime LastAirDate { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "networks" )]
        public IReadOnlyList<Network> Networks { get; set; }

        [DataMember( Name = "number_of_episodes" )]
        public int NumberOfEpisodes { get; set; }

        [DataMember( Name = "number_of_seasons" )]
        public int NumberOfSeasons { get; set; }

        [DataMember( Name = "origin_country" )]
        public IReadOnlyList<string> OriginCountry { get; set; }

        [DataMember( Name = "original_language" )]
        public string OriginalLanguage { get; set; }

        [DataMember( Name = "original_name" )]
        public string OriginalName { get; set; }

        [DataMember( Name = "overview" )]
        public string Overview { get; set; }

        [DataMember( Name = "popularity" )]
        public double Popularity { get; set; }

        [DataMember( Name = "poster_path" )]
        public string PosterPath { get; set; }

        [DataMember( Name = "production_companies" )]
        public IReadOnlyList<ProductionCompanyInfo> ProductionCompanies { get; set; }

        [DataMember( Name = "seasons" )]
        public IReadOnlyList<Season> Seasons { get; set; }

        public TVShow()
        {
            CreatedBy = new TVShowCreator[0];
            EpisodeRunTime = new int[0];
            Genres = new Genre[0];
            Languages = new string[0];
            Networks = new Network[0];
            OriginCountry = new string[0];
            ProductionCompanies = new ProductionCompanyInfo[0];
            Seasons = new Season[0];
        }

        public override string ToString()
        {
            return string.Format( "{0} ({1:yyyy-MM-dd}) [{2}]", Name, FirstAirDate, Id );
        }
    }
}
