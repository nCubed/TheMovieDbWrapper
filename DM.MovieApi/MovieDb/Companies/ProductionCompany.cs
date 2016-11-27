using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Companies
{
    [DataContract]
    public class ProductionCompany
    {
        [DataMember( Name = "id" )]
        public int Id { get; set; }

        [DataMember( Name = "name" )]
        public string Name { get; set; }

        [DataMember( Name = "description" )]
        public string Description { get; set; }

        [DataMember( Name = "headquarters" )]
        public string Headquarters { get; set; }

        [DataMember( Name = "homepage" )]
        public string Homepage { get; set; }

        [DataMember( Name = "logo_path" )]
        public string LogoPath { get; set; }

        [DataMember( Name = "parent_company" )]
        public ParentCompany ParentCompany { get; set; }

        public override string ToString()
            => $"{Name} ({Id})";
    }
}
