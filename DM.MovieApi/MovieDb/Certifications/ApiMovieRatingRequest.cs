using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using Newtonsoft.Json.Linq;

namespace DM.MovieApi.MovieDb.Certifications
{
    [Export( typeof( IApiMovieRatingRequest ) )]
    [PartCreationPolicy( CreationPolicy.NonShared )]
    internal class ApiMovieRatingRequest : ApiRequestBase, IApiMovieRatingRequest
    {
        [ImportingConstructor]
        public ApiMovieRatingRequest( IMovieDbSettings settings )
            : base( settings )
        { }

        public async Task<ApiQueryResponse<MovieRatings>> GetMovieRatingsAsync()
        {
            const string command = "certification/movie/list";

            ApiQueryResponse<MovieRatings> response = await base.QueryAsync( command, RatingsDeserializer );

            return response;
        }

        private MovieRatings RatingsDeserializer( string json )
        {
            var obj = JObject.Parse( json );

            JToken certs = obj["certifications"];

            var ratings = certs.ToObject<MovieRatings>();

            Func<IEnumerable<Certification>, IReadOnlyList<Certification>> reorder =
                list => list.OrderBy( x => x.Order ).ThenBy( x => x.Rating ).ToList().AsReadOnly();

            ratings.Australia = reorder( ratings.Australia );
            ratings.Canada = reorder( ratings.Canada );
            ratings.France = reorder( ratings.France );
            ratings.Germany = reorder( ratings.Germany );
            ratings.India = reorder( ratings.India );
            ratings.NewZealand = reorder( ratings.NewZealand );
            ratings.UnitedKingdom = reorder( ratings.UnitedKingdom );
            ratings.UnitedStates = reorder( ratings.UnitedStates );

            return ratings;
        }
    }
}