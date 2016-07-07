using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Companies
{
    [DataContract]
    public class ProductionCompanyInfo : IEqualityComparer<ProductionCompanyInfo>
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        public ProductionCompanyInfo( int id, string name )
        {
            Id = id;
            Name = name;
        }

        public override bool Equals( object obj )
        {
            var info = obj as ProductionCompanyInfo;
            if( info == null )
            {
                return false;
            }

            return Equals( this, info );
        }

        public bool Equals( ProductionCompanyInfo x, ProductionCompanyInfo y )
            => x.Id == y.Id && x.Name == y.Name;

        public override int GetHashCode()
            => GetHashCode( this );

        public int GetHashCode( ProductionCompanyInfo obj )
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
            if( string.IsNullOrWhiteSpace( Name ) )
            {
                return "n/a";
            }

            return $"{Name} ({Id})";
        }
    }
}