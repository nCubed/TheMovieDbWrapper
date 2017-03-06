using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb
{
    [DataContract]
    public class Image
    {
        [DataMember(Name = "aspect_ratio")]
        public double AspectRatio { get; set; }

        [DataMember(Name = "file_path")]
        public string FilePath { get; set; }

        [DataMember(Name = "height")]
        public int Height { get; set; }

        [DataMember(Name = "iso_639_1")]
        public string Iso6391 { get; set; }

        [DataMember(Name = "vote_average")]
        public double VoteAverage { get; set; }

        [DataMember(Name = "vote_count")]
        public int VoteCount { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }
    }
}
