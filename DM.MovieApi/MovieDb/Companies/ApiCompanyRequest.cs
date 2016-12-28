using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.Shims;

namespace DM.MovieApi.MovieDb.Companies
{
    internal class ApiCompanyRequest : ApiRequestBase, IApiCompanyRequest
    {
        private readonly IApiGenreRequest _genreApi;

        [ImportingConstructor]
        public ApiCompanyRequest( IMovieDbSettings settings, IApiGenreRequest genreApi )
            : base( settings )
        {
            _genreApi = genreApi;
        }

        public async Task<ApiQueryResponse<ProductionCompany>> FindByIdAsync( int companyId )
        {
            string command = $"company/{companyId}";

            ApiQueryResponse<ProductionCompany> response = await base.QueryAsync<ProductionCompany>( command );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> GetMoviesAsync( int companyId, int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            string command = $"company/{companyId}/movies";

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            response.Results.PopulateGenres( _genreApi );

            return response;
        }
    }
}
