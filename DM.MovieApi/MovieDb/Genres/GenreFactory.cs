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

        public static Genre Comedy()
            => new Genre( 35, "Comedy" );

        public static Genre Drama()
            => new Genre( 18, "Drama" );

        public static Genre Family()
            => new Genre( 10751, "Family" );

        public static Genre Fantasy()
            => new Genre( 14, "Fantasy" );

        public static Genre Horror()
            => new Genre( 27, "Horror" );

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

        public static Genre Thriller()
            => new Genre( 53, "Thriller" );

        public static Genre WarAndPolitics()
            => new Genre( 10768, "War & Politics" );
    }
}