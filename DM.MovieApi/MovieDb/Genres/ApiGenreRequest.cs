using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.Shims;
using Newtonsoft.Json.Linq;

namespace DM.MovieApi.MovieDb.Genres
{
    internal class ApiGenreRequest : ApiRequestBase, IApiGenreRequest
    {
        // ReSharper disable once InconsistentNaming
        private static readonly List<Genre> _allGenres = new();

        public IReadOnlyList<Genre> AllGenres
        {
            get
            {
                if( _allGenres.Any() == false )
                {
                    var genres = Task.Run( () => GetAllAsync() ).GetAwaiter().GetResult().Item;
                    _allGenres.AddRange( genres );
                }

                return _allGenres.AsReadOnly();
            }
        }

        [ImportingConstructor]
        public ApiGenreRequest( IMovieDbSettings settings )
            : base( settings )
        { }

        public async Task<ApiQueryResponse<Genre>> FindByIdAsync( int genreId, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language}
            };

            string command = $"genre/{genreId}";

            ApiQueryResponse<Genre> response = await base.QueryAsync<Genre>( command, param );

            EnsureAllGenres( response );

            return response;
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

            response.Results.PopulateGenres( this );

            return response;
        }

        internal void ClearAllGenres()
            => _allGenres.Clear();

        private void EnsureAllGenres( ApiQueryResponse<Genre> response )
        {
            if( response.Error != null )
            {
                return;
            }

            if( response.Item == null )
            {
                return;
            }

            if( _allGenres.Contains( response.Item ) == false )
            {
                _allGenres.Add( response.Item );
            }
        }

        private IReadOnlyList<Genre> GenreDeserializer( string json )
        {
            var obj = JObject.Parse( json );

            var arr = ( JArray )obj["genres"];

            // ReSharper disable once PossibleNullReferenceException
            var genres = arr.ToObject<IReadOnlyList<Genre>>();

            return genres;
        }
    }
}
