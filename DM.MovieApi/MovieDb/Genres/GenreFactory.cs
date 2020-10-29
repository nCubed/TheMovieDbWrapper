using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DM.MovieApi.MovieDb.Genres
{
    public static class GenreFactory
    {
        public static Genre Action()
            => new Genre( 28, "Action" );

        public static Genre Adventure()
            => new Genre( 12, "Adventure" );

        public static Genre ActionAndAdventure()
            => new Genre( 10759, "Action & Adventure" );

        public static Genre Animation()
            => new Genre( 16, "Animation" );

        public static Genre Comedy()
            => new Genre( 35, "Comedy" );

        public static Genre Crime()
            => new Genre( 80, "Crime" );

        public static Genre Drama()
            => new Genre( 18, "Drama" );

        public static Genre Documentary()
            => new Genre( 99, "Documentary" );

        public static Genre Family()
            => new Genre( 10751, "Family" );

        public static Genre Fantasy()
            => new Genre( 14, "Fantasy" );

        public static Genre History()
            => new Genre( 36, "History" );

        public static Genre Horror()
            => new Genre( 27, "Horror" );

        public static Genre Kids()
            => new Genre( 10762, "Kids" );

        public static Genre Music()
            => new Genre( 10402, "Music" );

        public static Genre Mystery()
            => new Genre( 9648, "Mystery" );

        public static Genre News()
            => new Genre( 10763, "News" );

        public static Genre Reality()
            => new Genre( 10764, "Reality" );

        public static Genre Romance()
            => new Genre( 10749, "Romance" );

        public static Genre ScienceFiction()
            => new Genre( 878, "Science Fiction" );

        public static Genre SciFiAndFantasy()
            => new Genre( 10765, "Sci-Fi & Fantasy" );

        public static Genre Soap()
            => new Genre( 10766, "Soap" );

        public static Genre Talk()
            => new Genre( 10767, "Talk" );

        public static Genre Thriller()
            => new Genre( 53, "Thriller" );

        public static Genre TvMovie()
            => new Genre( 10770, "TV Movie" );

        public static Genre War()
            => new Genre( 10752, "War" );

        public static Genre WarAndPolitics()
            => new Genre( 10768, "War & Politics" );

        public static Genre Western()
            => new Genre( 37, "Western" );

        public static IReadOnlyList<Genre> GetAll()
            => LazyAll.Value;


        private static readonly Lazy<IReadOnlyList<Genre>> LazyAll = new Lazy<IReadOnlyList<Genre>>( () =>
        {
            var all = typeof( GenreFactory )
                .GetTypeInfo()
                .DeclaredMethods
                .Where( x => x.IsStatic )
                .Where( x => x.IsPublic )
                .Where( x => x.ReturnType == typeof( Genre ) )
                .Select( x => ( Genre )x.Invoke( null, null ) )
                .ToList();

            return all.AsReadOnly();
        } );
    }
}
