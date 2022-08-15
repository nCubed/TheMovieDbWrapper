using System.Collections.Generic;

namespace DM.MovieApi.MovieDb.Discover
{
    public class DiscoverMovieParameterBuilder : IDiscoverMovieParameterBuilder
    {
        private readonly Dictionary<string, IList<string>> _param;

        public DiscoverMovieParameterBuilder()
        {
            _param = new Dictionary<string, IList<string>>();
        }

        public Dictionary<string, string> Build()
        {
            var response = new Dictionary<string, string>();
            foreach( var kvp in _param )
            {
                response.Add( kvp.Key, string.Join( ",", kvp.Value ) );
            }

            return response;
        }

        public IDiscoverMovieParameterBuilder WithCast( int personId )
        {
            if( !_param.ContainsKey( "with_cast" ) )
            {
                _param.Add( "with_cast", new List<string>() );
            }

            _param["with_cast"].Add( personId.ToString() );

            return this;
        }

        public IDiscoverMovieParameterBuilder WithCrew( int personId )
        {
            if( !_param.ContainsKey( "with_crew" ) )
            {
                _param.Add( "with_crew", new List<string>() );
            }

            _param["with_crew"].Add( personId.ToString() );
            return this;
        }

        public IDiscoverMovieParameterBuilder WithGenre( int genreId )
        {
            if( !_param.ContainsKey( "with_genres" ) )
            {
                _param.Add( "with_genres", new List<string>() );
            }

            _param["with_genres"].Add( genreId.ToString() );

            return this;
        }

        public IDiscoverMovieParameterBuilder WithOriginalLanguage( string language )
        {
            if( !_param.ContainsKey( "original_language" ) )
            {
                _param.Add( "original_language", new List<string>() );
            }

            _param["original_language"].Add( language );

            return this;
        }

        public IDiscoverMovieParameterBuilder ExcludeGenre( int genreId )
        {
            if( !_param.ContainsKey( "without_genres" ) )
            {
                _param.Add( "without_genres", new List<string>() );
            }

            _param["without_genres"].Add( genreId.ToString() );

            return this;
        }
    }
}
