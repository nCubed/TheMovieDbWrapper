using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Genres
{
    [DataContract]
    public class Genre : IEqualityComparer<Genre>
    {
        [DataMember( Name = "id" )]
        public int Id { get; private set; }

        [DataMember( Name = "name" )]
        public string Name { get; private set; }

        public Genre( int id, string name )
        {
            Id = id;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            var genre = obj as Genre;
            if( genre == null )
            {
                return false;
            }

            return Equals( this, genre );
        }

        public bool Equals( Genre x, Genre y )
        {
            return x.Id == y.Id && x.Name == y.Name;
        }

        public override int GetHashCode()
        {
            return GetHashCode( this );
        }

        public int GetHashCode( Genre obj )
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
