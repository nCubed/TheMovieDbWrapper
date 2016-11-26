using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DM.MovieApi.ApiRequest
{
    public class IsoDateTimeConverterEx : IsoDateTimeConverter
    {
        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            ConditionalTraceReaderValue( reader );

            try
            {
                return base.ReadJson( reader, objectType, existingValue, serializer );
            }
            catch( FormatException )
            {
                int year;
                if( int.TryParse( reader.Value?.ToString(), out year ) )
                {
                    return new DateTime( year, 1, 1 );
                }

                return default( DateTime );
            }
        }

        [Conditional( "DEBUG" )]
        private void ConditionalTraceReaderValue( JsonReader reader )
        {
            string val = reader.Value.ToString();
            if( string.IsNullOrWhiteSpace( val ) )
            {
                val = "<empty>";
            }

            Trace.WriteLine( $"JsonReader.Value: {val}", "IsoDateTimeConverterEx" );
        }
    }
}