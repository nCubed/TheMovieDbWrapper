using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DM.MovieApi.MovieDb.Configuration
{
    [DataContract]
    public class ApiConfiguration
    {
        [DataMember( Name = "images" )]
        public ImageConfiguration Images { get; private set; }

        [DataMember( Name = "change_keys" )]
        public IReadOnlyList<string> ChangeKeys { get; private set; }

        public override string ToString()
        {
            if( Images != null && !string.IsNullOrWhiteSpace( Images.RootUrl ) )
            {
                return Images.RootUrl;
            }

            return "not set";
        }
    }
}
