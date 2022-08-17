using System.ComponentModel;

namespace DM.MovieApi.MovieDb.Discover;

public enum DiscoverSortBy
{
    [Description( "popularity" )]
    Popularity,

    [Description( "release_date" )]
    ReleaseDate,

    [Description( "revenue" )]
    Revenue,

    [Description( "primary_release_date" )]
    PrimaryReleaseDate,

    [Description( "original_title" )]
    OriginalTitle,

    [Description( "vote_average" )]
    VoteAverage,

    [Description( "vote_count" )]
    VoteCount
}

public enum SortDirection
{
    [Description( "asc" )]
    Asc,

    [Description( "desc" )]
    Desc
}

public enum FilterExp
{
    [Description( "gte" )]
    GreaterThanOrEqual,

    [Description( "lte" )]
    LessThanOrEqual
}
