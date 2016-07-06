using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DM.MovieApi.MovieDb.Movies
{
    [DataContract]
    public class Keyword
    {
        [DataMember(Name = "id")]
        public int KeywordId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
    [DataContract]
    public class Keywords
    {
        [DataMember(Name = "id")]
        public int MovieId { get; set; }
        [DataMember(Name = "keywords")]
        public List<Keyword> AssociatedKeywords { get; set; }
    }
}
