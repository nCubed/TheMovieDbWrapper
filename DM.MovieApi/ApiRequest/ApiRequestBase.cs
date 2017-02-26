using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DM.MovieApi.ApiResponse;
using Newtonsoft.Json;

namespace DM.MovieApi.ApiRequest
{
    public abstract class ApiRequestBase
    {
        private readonly IMovieDbSettings _settings;

        protected ApiRequestBase( IMovieDbSettings settings )
        {
            _settings = settings;
        }

        public async Task<ApiQueryResponse<T>> QueryAsync<T>( string command )
            => await QueryAsync<T>( command, new Dictionary<string, string>() );

        public async Task<ApiQueryResponse<T>> QueryAsync<T>( string command, IDictionary<string, string> parameters )
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };
            settings.Converters.Add( new IsoDateTimeConverterEx() );

            Func<string, T> deserializer = json => JsonConvert.DeserializeObject<T>( json, settings );

            return await QueryAsync( command, parameters, deserializer );
        }

        public async Task<ApiQueryResponse<T>> QueryAsync<T>( string command, Func<string, T> deserializer )
            => await QueryAsync( command, new Dictionary<string, string>(), deserializer );

        public async Task<ApiQueryResponse<T>> QueryAsync<T>( string command, IDictionary<string, string> parameters, Func<string, T> deserializer )
        {
            using( HttpClient client = CreateClient() )
            {
                string cmd = CreateCommand( command, parameters );

                HttpResponseMessage response = await client.GetAsync( cmd ).ConfigureAwait( false );

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );

                if( !response.IsSuccessStatusCode )
                {
                    // rate limit will not exist if there is an error.
                    var error = new ApiQueryResponse<T>
                    {
                        Error = JsonConvert.DeserializeObject<ApiError>( json ),
                        CommandText = response.RequestMessage.RequestUri.ToString(),
                        Json = json,
                    };

                    return error;
                }

                var result = new ApiQueryResponse<T>
                {
                    RateLimit = GetRateLimit( response ),
                    CommandText = response.RequestMessage.RequestUri.ToString(),
                    Json = json,
                };

                T item = deserializer( json );
                result.Item = item;
                return result;
            }
        }

        public async Task<ApiSearchResponse<T>> SearchAsync<T>( string command )
            => await SearchAsync<T>( command, 1 );

        public async Task<ApiSearchResponse<T>> SearchAsync<T>( string command, int pageNumber )
            => await SearchAsync<T>( command, pageNumber, new Dictionary<string, string>() );

        public async Task<ApiSearchResponse<T>> SearchAsync<T>( string command, IDictionary<string, string> parameters )
            => await SearchAsync<T>( command, 1, parameters );

        public async Task<ApiSearchResponse<T>> SearchAsync<T>( string command, int pageNumber, IDictionary<string, string> parameters )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageNumber = pageNumber > 1000 ? 1000 : pageNumber;

            if( !parameters.Keys.Contains( "page", StringComparer.OrdinalIgnoreCase ) )
            {
                parameters.Add( "page", pageNumber.ToString() );
            }

            using( HttpClient client = CreateClient() )
            {
                string cmd = CreateCommand( command, parameters );

                HttpResponseMessage response = await client.GetAsync( cmd ).ConfigureAwait( false );

                string json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );

                // rate limit will not exist if there is an error.
                if( !response.IsSuccessStatusCode )
                {
                    var error = new ApiSearchResponse<T>
                    {
                        // This will throw up if the error is page number = 0; the resultant json will be: {"errors":["page must be greater than 0"]}
                        // in other words, the json will not include a status_code. Asked the api devs and this is a known issue they are working on.
                        // What to do? Nothing really, the page guard at the top of the method will keep the page number > 0.
                        Error = JsonConvert.DeserializeObject<ApiError>( json ),
                        CommandText = response.RequestMessage.RequestUri.ToString(),
                        Json = json,
                    };

                    return error;
                }

                ApiRateLimit rateLimit = GetRateLimit( response );

                var result = JsonConvert.DeserializeObject<ApiSearchResponse<T>>( json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } );

                result.RateLimit = rateLimit;
                result.CommandText = response.RequestMessage.RequestUri.ToString();
                result.Json = json;

                return result;
            }
        }

        protected ApiRateLimit GetRateLimit( HttpResponseMessage response )
        {
            int allowed = int.Parse( response.Headers.GetValues( "X-RateLimit-Limit" ).First() );
            int remaining = int.Parse( response.Headers.GetValues( "X-RateLimit-Remaining" ).First() );
            long reset = long.Parse( response.Headers.GetValues( "X-RateLimit-Reset" ).First() );

            var rateLimit = new ApiRateLimit( allowed, remaining, reset );

            return rateLimit;
        }

        protected HttpClient CreateClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = false,
                UseDefaultCredentials = true,
                AutomaticDecompression = DecompressionMethods.GZip,
            };

            var client = new HttpClient( handler );
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "appliction/json" ) );
            client.BaseAddress = new Uri( _settings.ApiUrl );

            return client;
        }

        protected string CreateCommand( string rootCommand )
            => CreateCommand( rootCommand, new Dictionary<string, string>() );

        protected string CreateCommand( string rootCommand, IDictionary<string, string> parameters )
        {
            string command = $"{rootCommand}?api_key={_settings.ApiKey}";

            string tokens = parameters.Any()
                ? string.Join( "&", parameters.Select( x => x.Key + "=" + x.Value ) )
                : string.Empty;

            if( string.IsNullOrWhiteSpace( tokens ) == false )
            {
                command = $"{command}&{tokens}";
            }

            return command;
        }
    }
}
