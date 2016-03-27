using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Companies
{
    [DataContract]
    public class ParentCompany
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "logo_path" )]
        public string LogoPath { get; set; }

        public override string ToString()
        {
            if( string.IsNullOrWhiteSpace( Name ) )
            {
                return "n/a";
            }

            return string.Format( "{0} ({1})", Name, Id );
        }
    }
}