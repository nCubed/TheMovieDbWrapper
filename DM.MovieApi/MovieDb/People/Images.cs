using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.People
{
    [DataContract]
    public class Images
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "profiles")]
        public IReadOnlyList<Image> Profiles { get; set; }
    }
}
