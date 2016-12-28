using System;

namespace DM.MovieApi.Shims
{
    [AttributeUsage( AttributeTargets.Constructor )]
    internal sealed class ImportingConstructorAttribute : Attribute
    { }
}
