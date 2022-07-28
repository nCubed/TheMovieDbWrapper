using System.Collections.Generic;

namespace DM.MovieApi.MovieDb.Discover
{
    public interface IDiscoverParameterBuilder
    {
        IDiscoverParameterBuilder WithOriginalLanguage(string language);
        IDiscoverParameterBuilder WithCrew(int personId);
        IDiscoverParameterBuilder WithCast(int personId);
        IDiscoverParameterBuilder WithGenre(int genre);
        IDiscoverParameterBuilder WithoutGenre(int genre);
        Dictionary<string,string> Build();
    }
}
