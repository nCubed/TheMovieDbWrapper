using System;
using System.Collections.Generic;
using DM.MovieApi.MovieDb.Keywords;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DM.MovieApi.MovieDb.Movies
{
    internal class KeywordConverter : JsonConverter
    {
        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            throw new NotImplementedException();
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            JToken obj = JToken.Load( reader );

            var arr = ( JArray )obj["keywords"];

            var keywords = arr.ToObject<IReadOnlyList<Keyword>>();

            return keywords;
        }

        public override bool CanConvert( Type objectType )
            => false;
    }
}