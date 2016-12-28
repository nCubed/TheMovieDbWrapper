using System.Collections.Generic;
using System.Threading.Tasks;
using DM.MovieApi.ApiRequest;
using DM.MovieApi.ApiResponse;
using Newtonsoft.Json.Linq;

namespace DM.MovieApi.MovieDb.IndustryProfessions
{
    internal class ApiProfessionRequest : ApiRequestBase, IApiProfessionRequest
    {
        public ApiProfessionRequest( IMovieDbSettings settings )
            : base( settings )
        { }

        public Task<ApiQueryResponse<IReadOnlyList<Profession>>> GetAllAsync()
        {
            const string command = "job/list";

            Task<ApiQueryResponse<IReadOnlyList<Profession>>> response = base.QueryAsync( command, ProfessionDeserializer );

            return response;
        }

        private IReadOnlyList<Profession> ProfessionDeserializer( string json )
        {
            var obj = JObject.Parse( json );

            var arr = (JArray)obj["jobs"];

            var professions = arr.ToObject<IReadOnlyList<Profession>>();

            return professions;
        }
    }
}
