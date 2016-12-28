using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb
{
    [DataContract]
    public class Language : IEqualityComparer<Language>
    {
        [DataMember( Name = "iso_639_1" )]
        public string Iso639Code { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        public Language( string iso639Code, string name )
        {
            Iso639Code = iso639Code;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            var language = obj as Language;
            if( language == null )
            {
                return false;
            }

            return Equals( this, language );
        }

        public bool Equals( Language x, Language y )
            => x.Iso639Code == y.Iso639Code && x.Name == y.Name;

        public override int GetHashCode()
            => GetHashCode( this );

        public int GetHashCode( Language obj )
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + obj.Iso639Code.GetHashCode();
                hash = hash * 23 + obj.Name.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            if( string.IsNullOrWhiteSpace( Name ) )
            {
                return "n/a";
            }

            return $"{Name} ({Iso639Code})";
        }
    }
}
