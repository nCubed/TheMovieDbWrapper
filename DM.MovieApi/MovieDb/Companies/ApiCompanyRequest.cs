using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Genres;
using DM.MovieApi.MovieDb.Movies;

namespace DM.MovieApi.MovieDb.Companies
{
    [Export( typeof( IApiCompanyRequest ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class ApiCompanyRequest : ApiRequestBase, IApiCompanyRequest
    {
        private readonly IApiGenreRequest _genreApi;

        [ImportingConstructor]
        public ApiCompanyRequest( IMovieDbSettings settings, IApiGenreRequest genreApi )
            : base( settings )
        {
            _genreApi = genreApi;
        }

        public async Task<ApiQueryResponse<ProductionCompany>> FindByIdAsyc( int companyId )
        {
            string command = string.Format( "company/{0}", companyId );

            ApiQueryResponse<ProductionCompany> response = await base.QueryAsync<ProductionCompany>( command );

            return response;
        }

        public async Task<ApiSearchResponse<MovieInfo>> GetMoviesAsync( int companyId, int pageNumber = 1, string language = "en" )
        {
            var param = new Dictionary<string, string>
            {
                {"language", language},
            };

            string command = string.Format( "company/{0}/movies", companyId );

            ApiSearchResponse<MovieInfo> response = await base.SearchAsync<MovieInfo>( command, pageNumber, param );

            if( response.Error != null )
            {
                return response;
            }

            foreach( MovieInfo info in response.Results )
            {
                info.PopulateGenres( _genreApi.AllGenres );
            }

            return response;
        }
    }
}