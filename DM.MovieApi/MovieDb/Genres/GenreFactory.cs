namespace DM.MovieApi.MovieDb.Genres
{
    public static class GenreFactory
    {
        public static Genre Action()
        {
            return new Genre( 28, "Action" );
        }

        public static Genre Adventure()
        {
            return new Genre( 12, "Adventure" );
        }

        public static Genre ActionAndAdventure()
        {
            return new Genre( 10759, "Action & Adventure" );
        }

        public static Genre Comedy()
        {
            return new Genre( 35, "Comedy" );
        }

        public static Genre Drama()
        {
            return new Genre( 18, "Drama" );
        }

        public static Genre Family()
        {
            return new Genre( 10751, "Family" );
        }

        public static Genre Fantasy()
        {
            return new Genre( 14, "Fantasy" );
        }

        public static Genre Horror()
        {
            return new Genre( 27, "Horror" );
        }

        public static Genre Mystery()
        {
            return new Genre( 9648, "Mystery" );
        }

        public static Genre News()
        {
            return new Genre( 10763, "News" );
        }

        public static Genre Reality()
        {
            return new Genre( 10764, "Reality" );
        }

        public static Genre Romance()
        {
            return new Genre( 10749, "Romance" );
        }

        public static Genre ScienceFiction()
        {
            return new Genre( 878, "Science Fiction" );
        }

        public static Genre SciFiAndFantasy()
        {
            return new Genre( 10765, "Sci-Fi & Fantasy" );
        }

        public static Genre Soap()
        {
            return new Genre( 10766, "Soap" );
        }

        public static Genre Thriller()
        {
            return new Genre( 53, "Thriller" );
        }

        public static Genre WarAndPolitics()
        {
            return new Genre( 10768, "War & Politics" );
        }
    }
}