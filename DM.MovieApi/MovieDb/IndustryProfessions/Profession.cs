using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.IndustryProfessions
{
    [DataContract]
    public class Profession
    {
        [DataMember( Name = "department" )]
        public string Department { get; set; }

        [DataMember( Name = "job_list" )]
        public IReadOnlyList<string> Jobs { get; set; }

        public override string ToString()
        {
            return string.Format( "{0} {1} jobs", Department, Jobs.Count );
        }
    }
}
