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
    internal abstract class ApiRequestBase
    {
        private readonly IApiSettings _settings;

        protected ApiRequestBase( IApiSettings settings )
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

            return await QueryAsync( command, parameters, Deserializer );

            T Deserializer( string json )
                => JsonConvert.DeserializeObject<T>( json, settings );
        }

        public async Task<ApiQueryResponse<T>> QueryAsync<T>( string command, Func<string, T> deserializer )
            => await QueryAsync( command, new Dictionary<string, string>(), deserializer );

        public async Task<ApiQueryResponse<T>> QueryAsync<T>( string command, IDictionary<string, string> parameters, Func<string, T> deserializer )
        {
            using HttpClient client = CreateClient();
            string cmd = CreateCommand( command, parameters );

            HttpResponseMessage response = await client.GetAsync( cmd ).ConfigureAwait( false );

            string json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );

            if( response.IsSuccessStatusCode == false )
            {
                // rate limit will not exist if there is an error.
                var error = new ApiQueryResponse<T>
                {
                    Error = JsonConvert.DeserializeObject<ApiError>( json ),
                    CommandText = response.RequestMessage?.RequestUri?.ToString() ?? "response message/uri was null",
                    Json = json,
                };

                return error;
            }

            var result = new ApiQueryResponse<T>
            {
                // ReSharper disable PossibleNullReferenceException
                CommandText = response.RequestMessage.RequestUri.ToString(),
                // ReSharper restore PossibleNullReferenceException
                Json = json,
            };

            T item = deserializer( json );
            result.Item = item;
            return result;
        }

        // ReSharper disable once UnusedMember.Global
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

            using HttpClient client = CreateClient();
            string cmd = CreateCommand( command, parameters );

            HttpResponseMessage response = await client.GetAsync( cmd ).ConfigureAwait( false );

            string json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );

            // rate limit will not exist if there is an error.
            if( response.IsSuccessStatusCode == false )
            {
                var error = new ApiSearchResponse<T>
                {
                    // This will throw up if the error is page number = 0; the resultant json will be: {"errors":["page must be greater than 0"]}
                    // in other words, the json will not include a status_code. Asked the api devs and this is a known issue they are working on.
                    // What to do? Nothing really, the page guard at the top of the method will keep the page number > 0.
                    Error = JsonConvert.DeserializeObject<ApiError>( json ),
                    CommandText = response.RequestMessage?.RequestUri?.ToString() ?? "response message/uri was null",
                    Json = json,
                };

                return error;
            }

            var result = JsonConvert.DeserializeObject<ApiSearchResponse<T>>( json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } );

            // ReSharper disable PossibleNullReferenceException
            result.CommandText = response.RequestMessage.RequestUri.ToString();
            // ReSharper restore PossibleNullReferenceException
            result.Json = json;

            return result;
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
            client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", _settings.BearerToken );
            client.BaseAddress = new Uri( _settings.ApiUrl );

            return client;
        }

        protected string CreateCommand( string rootCommand )
            => CreateCommand( rootCommand, new Dictionary<string, string>() );

        protected string CreateCommand( string rootCommand, IDictionary<string, string> parameters )
        {
            string tokens = parameters.Any()
                ? string.Join( "&", parameters.Select( x => $"{x.Key}={WebUtility.UrlEncode( x.Value )}" ) )
                : string.Empty;

            if( string.IsNullOrWhiteSpace( tokens ) == false )
            {
                rootCommand += $"?{tokens}";
            }

            return rootCommand;
        }
    }
}
