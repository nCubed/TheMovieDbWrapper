using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DM.MovieApi.Shims
{
    public static class CollectionExtensions
    {
        public static IReadOnlyList<T> AsReadOnly<T>( this List<T> list )
        {
            return new ReadOnlyCollection<T>( list );
        }
    }
}
