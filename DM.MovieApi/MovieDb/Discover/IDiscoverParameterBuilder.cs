using System.Collections.Generic;

namespace DM.MovieApi.MovieDb.Discover
{
    public interface IDiscoverParameterBuilder
    {
        Dictionary<string, string> Build();
    }
}
