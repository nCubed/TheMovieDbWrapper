using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Keywords
{
    [DataContract]
    public class Keyword : IEqualityComparer<Keyword>
    {
        /// <summary>
        /// The keyword Id as identified by theMovieDB.org.
        /// </summary>
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        /// <summary>
        /// The keyword.
        /// </summary>
        [DataMember( Name = "name" )]
        public string Name { get; set; }

        public Keyword( int id, string name )
        {
            Id = id;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            var genre = obj as Keyword;
            if( genre == null )
            {
                return false;
            }

            return Equals( this, genre );
        }

        public bool Equals( Keyword x, Keyword y )
        {
            return x.Id == y.Id && x.Name == y.Name;
        }

        public override int GetHashCode()
        {
            return GetHashCode( this );
        }

        public int GetHashCode( Keyword obj )
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format( "{0} ({1})", Name, Id );
        }
    }
}
