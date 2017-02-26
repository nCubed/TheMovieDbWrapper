using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.People;
using DM.MovieApi.MovieDb.TV;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    internal static class ApiResponseUtil
    {
        internal const int TestInitThrottle = 375;
        internal const int PagingThrottle = 225;

        /// <summary>
        /// Slows down the starting of tests to keep themoviedb.org api from denying the request
        /// due to too many requests. This should be placed in the [TestInitialize] method.
        /// </summary>
        public static void ThrottleTests()
        {
            System.Threading.Thread.Sleep( TestInitThrottle );
        }

        public static void AssertErrorIsNull( ApiResponseBase response )
        {
            Assert.IsNull( response.Error, response.Error?.ToString() ?? "Makes Complier Happy" );
        }

        public static void AssertImagePath( string path )
        {
            Assert.IsTrue( path.StartsWith( "/" ), $"Actual: {path}" );

            Assert.IsTrue(
                path.EndsWith( ".jpg" ) || path.EndsWith( ".png" ),
                $"Actual: {path}" );
        }

        public static async Task AssertCanPageSearchResponse<T, TSearch>( TSearch search, int minimumPageCount, int minimumTotalResultsCount,
            Func<TSearch, int, Task<ApiSearchResponse<T>>> apiSearch, Func<T, int> keySelector )
        {
            if( minimumPageCount < 2 )
            {
                Assert.Fail( "minimumPageCount must be greater than 1." );
            }

            var allFound = new List<T>();
            int pageNumber = 1;

            do
            {
                System.Diagnostics.Trace.WriteLine( $"search: {search} | page: {pageNumber}", "ApiResponseUti.AssertCanPageSearchResponse" );
                ApiSearchResponse<T> response = await apiSearch( search, pageNumber );

                AssertErrorIsNull( response );
                Assert.IsNotNull( response.Results );
                Assert.IsTrue( response.Results.Any() );

                if( typeof( T ) == typeof( Movie ) )
                {
                    AssertMovieStructure( (IEnumerable<Movie>)response.Results );
                }
                else if( typeof( T ) == typeof( PersonInfo ) )
                {
                    AssertPersonInfoStructure( (IEnumerable<PersonInfo>)response.Results );
                }

                allFound.AddRange( response.Results );

                Assert.AreEqual( pageNumber, response.PageNumber );

                Assert.IsTrue( response.TotalPages >= minimumPageCount,
                    $"Expected minimum of {minimumPageCount} TotalPages. Actual TotalPages: {response.TotalPages}" );

                pageNumber++;

                // keeps the system from being throttled
                System.Threading.Thread.Sleep( PagingThrottle );
            } while( pageNumber <= minimumPageCount );

            // will be 1 greater than minimumPageCount in the last loop
            Assert.AreEqual( minimumPageCount + 1, pageNumber );

            Assert.IsTrue( allFound.Count >= minimumTotalResultsCount, $"Actual found count: {allFound.Count} | Expected min count: {minimumTotalResultsCount}" );

            if( keySelector == null )
            {
                // people searches will usually have dups since they return movie and tv roles 
                // in separate objects in the same result set.
                return;
            }

            List<IGrouping<int, T>> groupById = allFound
                .ToLookup( keySelector )
                .ToList();

            List<string> dups = groupById
                .Where( x => x.Skip( 1 ).Any() )
                .Select( x => $"({x.Count()}) {x.First().ToString()}" )
                .ToList();

            const string note = "Note: Every now and then themoviedb.org API returns a duplicate; not to be alarmed, just re-run the test until it passes.\r\n\r\n";
            Assert.AreEqual( 0, dups.Count, note + " Duplicates: " + Environment.NewLine + string.Join( Environment.NewLine, dups ) );
        }

        private static void AssertPersonInfoStructure( IEnumerable<PersonInfo> people )
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assert.IsTrue( people.Any() );

            foreach( PersonInfo person in people )
            {
                Assert.IsTrue( person.Id > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( person.Name ) );

                foreach( PersonInfoRole role in person.KnownFor )
                {
                    // not asserting movie/tv dates as some valid dates will be null.
                    if( role.MediaType == MediaType.Movie )
                    {
                        Assert.IsFalse( string.IsNullOrWhiteSpace( role.MovieTitle ) );
                        Assert.IsFalse( string.IsNullOrWhiteSpace( role.MovieOriginalTitle ) );

                        Assert.IsNull( role.TVShowName );
                        Assert.IsNull( role.TVShowOriginalName );
                        Assert.AreEqual( DateTime.MinValue, role.TVShowFirstAirDate );
                    }
                    else
                    {
                        Assert.IsFalse( string.IsNullOrWhiteSpace( role.TVShowName ) );
                        Assert.IsFalse( string.IsNullOrWhiteSpace( role.TVShowOriginalName ) );

                        Assert.IsNull( role.MovieTitle );
                        Assert.IsNull( role.MovieOriginalTitle );
                        Assert.AreEqual( DateTime.MinValue, role.MovieReleaseDate );
                    }

                    AssertGenres( role.GenreIds, role.Genres );
                }
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static void AssertMovieStructure( IEnumerable<Movie> movies )
        {
            // ReSharper disable PossibleMultipleEnumeration
            Assert.IsTrue( movies.Any() );

            foreach( Movie movie in movies )
            {
                AssertMovieStructure( movie );
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static void AssertMovieStructure( Movie movie )
        {
            Assert.IsTrue( movie.Id > 0 );
            Assert.IsFalse( string.IsNullOrWhiteSpace( movie.Title ) );

            foreach( Genre genre in movie.Genres )
            {
                Assert.IsTrue( genre.Id > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( genre.Name ) );
            }

            foreach( ProductionCompanyInfo info in movie.ProductionCompanies )
            {
                Assert.IsTrue( info.Id > 0 );
                Assert.IsFalse( string.IsNullOrWhiteSpace( info.Name ) );
            }

            foreach( Country country in movie.ProductionCountries )
            {
                Assert.IsFalse( string.IsNullOrWhiteSpace( country.Iso3166Code ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( country.Name ) );
            }

            foreach( Language language in movie.SpokenLanguages )
            {
                Assert.IsFalse( string.IsNullOrWhiteSpace( language.Iso639Code ) );
                Assert.IsFalse( string.IsNullOrWhiteSpace( language.Name ) );
            }
        }

        public static void AssertMovieInformationStructure( IEnumerable<MovieInfo> movies )
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.IsTrue( movies.Any() );

            // ReSharper disable once PossibleMultipleEnumeration
            foreach( MovieInfo movie in movies )
            {
                AssertMovieInformationStructure( movie );
            }
        }

        public static void AssertMovieInformationStructure( MovieInfo movie )
        {
            Assert.IsFalse( string.IsNullOrWhiteSpace( movie.Title ) );
            Assert.IsTrue( movie.Id > 0 );

            AssertGenres( movie.GenreIds, movie.Genres );
        }

        public static void AssertTVShowInformationStructure( IEnumerable<TVShowInfo> tvShows )
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.IsTrue( tvShows.Any() );

            // ReSharper disable once PossibleMultipleEnumeration
            foreach( TVShowInfo tvShow in tvShows )
            {
                AssertTVShowInformationStructure( tvShow );
            }
        }

        public static void AssertTVShowInformationStructure( TVShowInfo tvShow )
        {
            Assert.IsTrue( tvShow.Id > 0 );
            Assert.IsFalse( string.IsNullOrEmpty( tvShow.Name ) );

            AssertGenres( tvShow.GenreIds, tvShow.Genres );
        }

        private static void AssertGenres( IReadOnlyList<int> genreIds, IReadOnlyList<Genre> genres )
        {
            Assert.AreEqual( genreIds.Count, genres.Count );

            foreach( Genre genre in genres )
            {
                Assert.IsFalse( string.IsNullOrWhiteSpace( genre.Name ) );
                Assert.IsTrue( genre.Id > 0 );
            }
        }
    }
}
