using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Shared
{
    [DataContract]
    public class Images
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "backdrops")]
        public IReadOnlyList<Image> Backdrops { get; set; }

        [DataMember(Name = "posters")]
        public IReadOnlyList<Image> Posters { get; set; }
    }
}
