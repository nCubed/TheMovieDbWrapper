using System;
using System.ComponentModel.Composition;
using DM.MovieApi.MovieDb.Certifications;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Configuration;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.IndustryProfessions;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.TV;

namespace DM.MovieApi
{
    [Export( typeof( IMovieDbApi ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class MovieDbApi : IMovieDbApi
    {

        #region Lazy Imports
#pragma warning disable 0649

        [Import]
        private Lazy<IApiCompanyRequest> _companyRequest;

        [Import]
        private Lazy<IApiConfigurationRequest> _configuration;

        [Import]
        private Lazy<IApiGenreRequest> _genres;

        [Import]
        private Lazy<IApiProfessionRequest> _industryProfessions;

        [Import]
        private Lazy<IApiMovieRequest> _movies;

        [Import]
        private Lazy<IApiMovieRatingRequest> _movieRatings;

        [Import]
        private Lazy<IApiTVShowRequest> _television;

#pragma warning restore 0649
        #endregion

        public IApiCompanyRequest Companies { get { return _companyRequest.Value; } }

        public IApiConfigurationRequest Configuration { get { return _configuration.Value; } }

        public IApiGenreRequest Genres { get { return _genres.Value; } }

        public IApiProfessionRequest IndustryProfessions { get { return _industryProfessions.Value; } }

        public IApiMovieRequest Movies { get { return _movies.Value; } }

        public IApiMovieRatingRequest MovieRatings { get { return _movieRatings.Value; } }

        public IApiTVShowRequest Television { get { return _television.Value; } }
    }
}