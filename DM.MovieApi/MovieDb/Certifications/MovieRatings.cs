using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Certifications
{
    [DataContract]
    public class Certification
    {
        [DataMember( Name = "certification" )]
        public string Rating { get; set; }

        [DataMember( Name = "meaning" )]
        public string Meaning { get; set; }

        [DataMember( Name = "order" )]
        public int Order { get; set; }

        public override string ToString()
            => $"{Rating}: {Meaning.Substring( 75 )}";
    }

    [DataContract]
    public class MovieRatings
    {
        [DataMember( Name = "AU" )]
        public IReadOnlyList<Certification> Australia { get; set; }

        [DataMember( Name = "CA" )]
        public IReadOnlyList<Certification> Canada { get; set; }

        [DataMember( Name = "FR" )]
        public IReadOnlyList<Certification> France { get; set; }

        [DataMember( Name = "DE" )]
        public IReadOnlyList<Certification> Germany { get; set; }

        [DataMember( Name = "IN" )]
        public IReadOnlyList<Certification> India { get; set; }

        [DataMember( Name = "NZ" )]
        public IReadOnlyList<Certification> NewZealand { get; set; }

        [DataMember( Name = "US" )]
        public IReadOnlyList<Certification> UnitedStates { get; set; }

        [DataMember( Name = "GB" )]
        public IReadOnlyList<Certification> UnitedKingdom { get; set; }

        public MovieRatings()
        {
            UnitedStates = new Certification[0];
            Canada = new Certification[0];
            Australia = new Certification[0];
            Germany = new Certification[0];
            France = new Certification[0];
            NewZealand = new Certification[0];
            India = new Certification[0];
            UnitedKingdom = new Certification[0];
        }
    }
}
