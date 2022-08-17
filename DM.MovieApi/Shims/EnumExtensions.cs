using System.ComponentModel;
using System.Reflection;

namespace DM.MovieApi.Shims;

internal static class EnumExtensions
{
    public static string GetDescription( this Enum e )
    {
        DescriptionAttribute attr = e.GetType()
            .GetMember( e.ToString() )
            .First()
            .GetCustomAttribute<DescriptionAttribute>();

        return attr?.Description ?? e.ToString();
    }
}
