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
        private static readonly DateTime MinDate = new( 1900, 1, 1 );

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
            Console.WriteLine( response.CommandText );
            Assert.IsNull( response.Error, response.Error?.ToString() ?? "Makes Compiler Happy" );
        }

        public static void AssertImagePath( string path )
        {
            if( path == null ) return;

            Assert.IsTrue( path.StartsWith( "/" ), $"Actual: {path}" );

            Assert.IsTrue(
                path.EndsWith( ".jpg" ) || path.EndsWith( ".png" ),
                $"Actual: {path}" );
        }

        public static void AssertNoSearchResults<T>( ApiSearchResponse<T> response )
        {
            AssertErrorIsNull( response );

            Assert.AreEqual( 0, response.Results.Count, $"Actual: {response}" );
            Assert.AreEqual( 1, response.PageNumber, $"Actual: {response}" );
            Assert.AreEqual( 0, response.TotalPages, $"Actual: {response}" );
            Assert.AreEqual( 0, response.TotalResults, $"Actual: {response}" );
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

            var priorResults = new Dictionary<int, int>();

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

                if( keySelector == null )
                {
                    allFound.AddRange( response.Results );
                }
                else
                {
                    var current = new List<T>();
                    foreach( T res in response.Results )
                    {
                        int key = keySelector( res );

                        if( priorResults.TryAdd( key, 1 ) )
                        {
                            current.Add( res );
                            continue;
                        }

                        System.Diagnostics.Trace.WriteLine( $"dup on page {response.PageNumber}: {res}" );

                        if( ++priorResults[key] > 2 )
                        {
                            Assert.Fail( "Every now and then themoviedb.org API returns a duplicate from a prior page. " +
                                        "But this time it exceeded our tolerance of one dup.\r\n" +
                                        $"dup: {res}" );
                        }
                    }

                    allFound.AddRange( current );
                }

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
                .Select( x => $"({x.Count()}) {string.Join( " | ", x.Select( y => y.ToString() ) )}" )
                .ToList();

            Assert.AreEqual( 0, dups.Count, "Duplicates: " + Environment.NewLine + string.Join( Environment.NewLine, dups ) );
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
            Assert.IsFalse( string.IsNullOrWhiteSpace( movie.Title ), $"Actual: {movie}" );
            Assert.IsFalse( string.IsNullOrWhiteSpace( movie.OriginalTitle ), $"Actual {movie}" );
            // movie.Overview is sometimes empty

            Assert.IsTrue( movie.Id > 1 );
            Assert.IsTrue( movie.ReleaseDate > MinDate, $"Actual: {movie.ReleaseDate} | {movie}" );

            AssertImagePath( movie.BackdropPath );
            AssertImagePath( movie.PosterPath );
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

        public static void AssertTVShowSeasonInformationStructure( SeasonInfo seasonInfo )
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.IsTrue( seasonInfo.Episodes.Any() );

            // ReSharper disable once PossibleMultipleEnumeration
            foreach( Episode episode in seasonInfo.Episodes )
            {
                AssertTVShowSeasonInformationStructure( episode );
            }
        }

        private static void AssertTVShowSeasonInformationStructure( Episode episode )
        {
            Assert.IsTrue( episode.Id > 0 );
            Assert.IsFalse( episode.AirDate == default );
            Assert.IsTrue( episode.EpisodeNumber > 0 );
            Assert.IsFalse( string.IsNullOrWhiteSpace( episode.Name ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( episode.Overview ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( episode.ProductionCode ) );
            Assert.IsTrue( episode.SeasonNumber > 0 );
            AssertImagePath( episode.StillPath );
            Assert.IsTrue( episode.VoteAverage > 5 );
            Assert.IsTrue( episode.VoteCount > 5 );

            foreach( Crew crew in episode.Crew )
            {
                AssertTvShowCrewStructure( crew );
            }

            foreach( GuestStars guestStars in episode.GuestStars )
            {
                AssertTvShowGuestStarsStructure( guestStars );
            }
        }

        private static void AssertTvShowCrewStructure( Crew crew )
        {
            Assert.IsTrue( crew.Id > 0 );
            Assert.IsFalse( string.IsNullOrWhiteSpace( crew.Job ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( crew.Department ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( crew.CreditId ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( crew.KnownForDepartment ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( crew.Name ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( crew.OriginalName ) );
            Assert.IsTrue( crew.Popularity > 0 );

            if( crew.ProfilePath != null )
            {
                AssertImagePath( crew.ProfilePath );
            }
        }

        private static void AssertTvShowGuestStarsStructure( GuestStars guestStars )
        {
            Assert.IsTrue( guestStars.Id > 0 );
            Assert.IsTrue( guestStars.Order > 0 );
            Assert.IsFalse( string.IsNullOrWhiteSpace( guestStars.Character ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( guestStars.CreditId ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( guestStars.KnownForDepartment ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( guestStars.Name ) );
            Assert.IsFalse( string.IsNullOrWhiteSpace( guestStars.OriginalName ) );
            Assert.IsTrue( guestStars.Popularity > 0 );

            if( guestStars.ProfilePath != null )
            {
                AssertImagePath( guestStars.ProfilePath );
            }
        }
    }
}
