using System;
using System.Collections.Generic;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.People;
using DM.MovieApi.MovieDb.TV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DM.MovieApi.MovieDb.Images
{
    /// <summary>
    /// Expected parent json node is "images". The child node is variable
    /// and should be set as a parameter to the JsonConverter attribute which
    /// will use the ImagesConverter .ctor to create the converter with the
    /// provided parameter.
    /// </summary>
    public class ImagesConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken obj = JToken.Load(reader);

            using (reader = obj.CreateReader())
            {
                if (typeof(Movie) == objectType)
                {
                    Movie movie = new Movie();
                    serializer.Populate(reader, movie);
                    movie.Backdrops = GetImages(obj.SelectToken("images.backdrops"), serializer);
                    movie.Posters = GetImages(obj.SelectToken("images.posters"), serializer);
                    return movie;
                }
                if (typeof(TVShow) == objectType)
                {
                    TVShow show = new TVShow();
                    serializer.Populate(reader, show);
                    show.Backdrops = GetImages(obj.SelectToken("images.backdrops"), serializer);
                    show.Posters = GetImages(obj.SelectToken("images.posters"), serializer);
                    return show;
                }
                if (typeof(Person) == objectType)
                {
                    Person person = new Person();
                    serializer.Populate(reader, person);
                    person.Profiles = GetImages(obj.SelectToken("images.profiles"), serializer);
                    return person;
                }
            }

            return existingValue;
        }

        private List<Image> GetImages(JToken token, JsonSerializer serializer)
        {
            if (token != null)
            {
                var images = new List<Image>();
                using (var reader = token.CreateReader())
                {
                    serializer.Populate(reader, images);
                }
                return images;
            }
            return null;
        }

        public override bool CanConvert(Type objectType)
            => false;
    }
}
