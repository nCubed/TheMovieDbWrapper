using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb;
using DM.MovieApi.MovieDb.Companies;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.TV;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DM.MovieApi.IntegrationTests
{
    internal static class ApiResponseUtil
    {
        /// <summary>
        /// Slows down the starting of tests to keep themoviedb.org api from denying the request
        /// due to too many requests. This should be placed in the [TestInitialize] method.
        /// </summary>
        public static void ThrottleTests()
        {
            System.Threading.Thread.Sleep( 375 );
        }

        public static void AssertErrorIsNull<T>( ApiQueryResponse<T> response )
        {
            Assert.IsNull( response.Error, response.Error == null ? "Makes Complier Happy" : response.Error.ToString() );
        }

        public static void AssertErrorIsNull<T>( ApiSearchResponse<T> response )
        {
            Assert.IsNull( response.Error, response.Error == null ? "Makes Complier Happy" : response.Error.ToString() );
        }

        public static void AssertImagePath( string path )
        {
            Assert.IsTrue( path.StartsWith( "/" ), string.Format( "Actual: {0}", path ) );

            Assert.IsTrue(
                path.EndsWith( ".jpg" ) || path.EndsWith( ".png" ),
                string.Format( "Actual: {0}", path ) );
        }

        public static async Task AssertCanPageSearchResponse<T, TSearch>( TSearch search, int minimumPageCount, int minimumMovieCount,
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
                ApiSearchResponse<T> response = await apiSearch( search, pageNumber );

                AssertErrorIsNull( response );
                Assert.IsNotNull( response.Results );
                Assert.IsTrue( response.Results.Any() );

                if( typeof( T ) == typeof( Movie ) )
                {
                    AssertMovieStructure( ( IEnumerable<Movie> )response.Results );
                }

                allFound.AddRange( response.Results );

                Assert.AreEqual( pageNumber, response.PageNumber );

                Assert.IsTrue( response.TotalPages >= minimumPageCount,
                    string.Format( "Expected minimum of {0} TotalPages. Actual TotalPages: {1}",
                        minimumPageCount, response.TotalPages ) );

                pageNumber++;

                // keeps the system from being throttled
                System.Threading.Thread.Sleep( 50 );
            } while( pageNumber <= minimumPageCount );

            // will be 1 greater than minimumPageCount in the last loop
            Assert.AreEqual( minimumPageCount + 1, pageNumber );

            Assert.IsTrue( allFound.Count >= minimumMovieCount );

            List<IGrouping<int, T>> groupById = allFound
                .ToLookup( keySelector )
                .ToList();

            List<string> dups = groupById
                .Where( x => x.Skip( 1 ).Any() )
                .Select( x => string.Format( "({0}) {1}", x.Count(), x.First().ToString() ) )
                .ToList();

            const string note = "Note: Every now and then themoviedb.org API returns a duplicate; not to be alarmed, just re-run the test until it passes.\r\n\r\n";
            Assert.AreEqual( 0, dups.Count, note + " Duplicates: " + Environment.NewLine + string.Join( Environment.NewLine, dups ) );
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

            Assert.AreEqual( movie.GenreIds.Count, movie.Genres.Count );
            if( movie.GenreIds.Count > 0 )
            {
                foreach( Genre genre in movie.Genres )
                {
                    Assert.IsFalse( string.IsNullOrWhiteSpace( genre.Name ) );
                    Assert.IsTrue( genre.Id > 0 );
                }
            }
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

            Assert.AreEqual( tvShow.GenreIds.Count, tvShow.Genres.Count );
            if( tvShow.GenreIds.Count > 0 )
            {
                foreach( Genre genre in tvShow.Genres )
                {
                    Assert.IsFalse( string.IsNullOrWhiteSpace( genre.Name ) );
                    Assert.IsTrue( genre.Id > 0 );
                }
            }
        }
    }
}
