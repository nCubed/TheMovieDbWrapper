using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using Newtonsoft.Json.Linq;

namespace DM.MovieApi.MovieDb.Genres
{
    [Export( typeof( IApiGenreRequest ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class ApiGenreRequest : ApiRequestBase, IApiGenreRequest
    {
        private static Lazy<IReadOnlyList<Genre>> _allGenres;

        public IReadOnlyList<Genre> AllGenres { get { return _allGenres.Value; } }

        [ImportingConstructor]
        public ApiGenreRequest( IMovieDbSettings settings )
            : base( settings )
        {
            if( _allGenres == null || !_allGenres.Value.Any() )
            {
                _allGenres = new Lazy<IReadOnlyList<Genre>>( () =>
                {
                    var genres = Task.Run( () => GetAllAsync() ).GetAwaiter().GetResult().Item;
                    return genres;
                } );
            }
        }

        public async Task<ApiQueryResponse<IReadOnlyList<Genre>>> GetAllAsync( string language = "en" )
        {
            ApiQueryResponse<IReadOnlyList<Genre>> tv = await GetTelevisionAsync( language );
            if( tv.Error != null )
            {
                return tv;
            }

            ApiQueryResponse<IReadOnlyList<Genre>> movies = await GetMoviesAsync( language );
            if( movies.Error != null )
            {
                return movies;
            }

            List<Genre> merged = movies.Item
                .Union( tv.Item )
                .OrderBy( x => x.Name )
                .ToList();

            movies.Item = merged.AsReadOnly();

            return movies;
        }

        public async Task<ApiQueryResponse<IReadOnlyList<Genre>>> GetMoviesAsync( string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            ApiQueryResponse<IReadOnlyList<Genre>> genres = await base.QueryAsync( "genre/movie/list", param, GenreDeserializer );

            return genres;
        }

        public async Task<ApiQueryResponse<IReadOnlyList<Genre>>> GetTelevisionAsync( string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            ApiQueryResponse<IReadOnlyList<Genre>> genres = await base.QueryAsync( "genre/tv/list", param, GenreDeserializer );

            return genres;
        }

        public async Task<ApiSearchResponse<MovieInfo>> FindMoviesByIdAsync( int genreId, int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
                {"include_adult", "false"},
            };

            string command = $"genre/{genreId}/movies";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( AllGenres );

            return response;
        }

        private IReadOnlyList<Genre> GenreDeserializer( string json )
        {
            var obj = JObject.Parse( json );

            var arr = (JArray)obj["genres"];

            var genres = arr.ToObject<IReadOnlyList<Genre>>();

            return genres;
        }
    }
}