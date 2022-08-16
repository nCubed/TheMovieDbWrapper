# TheMovieDb.org Wrapper
TheMovieDbWrapper is a C# wrapper for [TheMovieDb.org](https://www.themoviedb.org) API providing cross-platform support for Xamarin, iOS, Android, and all flavors of .NET.

# Installation
[![Nuget Stats for TheMovieDbWrapper](https://buildstats.info/nuget/TheMovieDbWrapper?vWidth=75&dWidth=100)](https://www.nuget.org/packages/TheMovieDbWrapper/)
* The recommened method of installation is to use the [Nuget package manager for TheMovieDbWrapper](https://www.nuget.org/packages/TheMovieDbWrapper/).
* Alternatively, download the [latest release](https://github.com/nCubed/TheMovieDbWrapper/releases) from our Github repo.

## Nuget Install Options

### Option 1: Install from Visual Studio
```
In the Nuget package manager UI, search for: TheMovieDbWrapper and then click the install button.
```

### Option 2: Install from the Nuget Package Manger CLI
```batch
PM> Install-Package TheMovieDbWrapper
```

### Option 3: Install with the .NET CLI
```batch
> dotnet add package TheMovieDbWrapper
```

# v1.0 Breaking Changes :vomiting_face:
```
The v1.0 release on 2021-10-27 introduces a minor breaking change when 
registering your TheMovieDb.org credentials with our MovieDbFactory.
```

* `IMovieDbSettings` has been completely eliminated and simplifies the process of registering your credentials.
  * You no longer need to create a concrete implementation of the interface when registering your credentials.
  * You no longer need to provide TheMovieDb.org api url.
* Your TheMovieDb.org credentials now use their updated authentication using a Bearer Token.
  * Your Bearer token is found in your [TheMovieDb.org account page](https://www.themoviedb.org/settings/api), under the API section: _"API Read Access Token (v4 auth)"_. 
  * Note: This token IS NOT the same as the old _"API Key (v3 auth)"_.

# Common API Requests
The current release supports common requests for movie, tv, and other information, such as:
* TheMovieDb.org configuration
* Movies :movie_camera:	
* Movie Ratings
* TV Shows :tv:
* Movie and TV Genres
* Movie/TV Industry Specific Professions
* Production Companies
* People such as Actors, Actresses, Directors, etc...

# Basic Usage
The `MovieDbFactory` class is the single entry point for retrieving information from TheMovieDb.org API. Before making any requests, you must register your TheMovieDb.org Bearer Token with our `MovieDbFactory` class:

```csharp
// your bearer token can be found on TheMovieDb.org's website under your account settings
// https://www.themoviedb.org/settings/api
string bearerToken = "your-bearer-token-from-TheMovieDb.org";

// RegisterSettings only needs to be called one time when your application starts-up.
MovieDbFactory.RegisterSettings( bearerToken );
```

Once your Bearer Token has been registered, you will then use the `.Create<T>()` method to get a specific API request type. The full signature of the method is:

```csharp
Lazy<T> MovieDbFactory.Create<T>() where T : IApiRequest
```

The `IApiRequest` is a basic Interface providing a constraint for all our request interfaces/classes used in the factory. For example, to retrieve the API for movies:

```csharp
// as the factory returns a Lazy<T> instance, just grab the Value from the Lazy<T>
// and assign to a local variable.
var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
```
## API Interfaces
The following interfaces are used with the `MovieDbFactory.Create<T>()` method:

IApiRequest | Description
:-- | :--
[`IApiConfigurationRequest`](DM.MovieApi/MovieDb/Configuration/IApiConfigurationRequest.cs) | Api for retrieving TheMovieDb.org configuration information.
[`IApiMovieRequest`](DM.MovieApi/MovieDb/Movies/IApiMovieRequest.cs) | Api for retrieving Movies.
[`IApiMovieRatingRequest`](DM.MovieApi/MovieDb/Certifications/IApiMovieRatingRequest.cs) | Api for retrieving movie ratings.
[`IApiTVShowRequest`](DM.MovieApi/MovieDb/TV/IApiTVShowRequest.cs) | Api for retrieving TV shows.
[`IApiGenreRequest`](DM.MovieApi/MovieDb/Genres/IApiGenreRequest.cs) | Api for retrieving Movie and TV genres.
[`IApiCompanyRequest`](DM.MovieApi/MovieDb/Companies/IApiCompanyRequest.cs) | Api for retrieving production companies.
[`IApiProfessionRequest`](DM.MovieApi/MovieDb/IndustryProfessions/IApiProfessionRequest.cs) | Api for retrieving Movie/TV industry specific professions.
[`IApiPeopleRequest`](DM.MovieApi/MovieDb/People/IApiPeopleRequest.cs) | Api for retrieving People.
[`IApiDiscoverRequest`](DM.MovieApi/MovieDb/Discover/IApiDiscoverRequest.cs) | Api for discovering movies.

# More Examples
## Search by Movie Title

```csharp
string bearerToken = "your-bearer-token-from-TheMovieDb.org";

// RegisterSettings only needs to be called one time when your application starts-up.
MovieDbFactory.RegisterSettings( bearerToken );

var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync( "Star Trek" );

foreach( MovieInfo info in response.Results )
{
    Console.WriteLine( $"{info.Title} ({info.ReleaseDate}): {info.Overview}" );
}
```

The above example returns an [`ApiSearchResponse<T>`](DM.MovieApi/ApiResponse/ApiSearchResponse.cs) which provides rich information about the results of your search, including the following:

Member | Type | Description
:-- | :-- | :--
Results | `IReadOnlyList<T>` | The list of results from the search.
PageNumber | `int`| The current page number of the search result.
TotalPages | `int` | The total number of pages found from the search result.
TotalResults | `int` | The total number of results from the search.
ToString() | `string` | Returns `Page x of y (z total results)`.
Error | `ApiError` | Contains specific error information if an error was encountered during the API call to TheMovieDb.org.
RateLimit | `ApiRateLimit` | Contains the current rate limits from your most recent API call to TheMovieDb.org. Note: TheMovieDb.org has removed all rate limits as of December of 2019.

## Find Movie By Id

```csharp
string bearerToken = "your-bearer-token-from-TheMovieDb.org";

// RegisterSettings only needs to be called one time when your application starts-up.
MovieDbFactory.RegisterSettings( bearerToken );

var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

ApiQueryResponse<Movie> response = await movieApi.FindByIdAsync( 140607 );

Movie movie = response.Item;

Console.WriteLine( movie.Id );
Console.WriteLine( movie.Title );
Console.WriteLine( movie.Tagline );
Console.WriteLine( movie.ReleaseDate );
Console.WriteLine( movie.Budget );
```

The above query returns an [`ApiQueryResponse<T>`](DM.MovieApi/ApiResponse/ApiQueryResponse.cs) which returns a single result as well as some common information previously seen in the `ApiSearchResponse`:

Member | Type | Description
:-- | :-- | :--
Item | `T` | The item returned from the API call, where `T` is the specific type returned from the query, such as `MovieInfo`, `Movie`, `MovieCredit`, etc..
ToString() | `string` | Typically returns a well formatted string representation of `T`.
Error | `ApiError` | If an error was encountered, the `Error` property will provide specific error information about the API call to TheMovieDb.org.
RateLimit | `ApiRateLimit` | Contains the current rate limits from your most recent API call to TheMovieDb.org. _Note: TheMovieDb.org has removed all rate limits as of December of 2019._

## Paging a Search Result
```csharp
string bearerToken = "your-bearer-token-from-TheMovieDb.org";

// RegisterSettings only needs to be called one time when your application starts-up.
MovieDbFactory.RegisterSettings( bearerToken );

var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

int pageNumber = 1;
int totalPages;
do
{
    ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync( "Harry", pageNumber );

    // alternatively, just call response.ToString() which will provide the same paged information format as below:
    Console.WriteLine( $"Page {response.PageNumber} of {response.TotalPages} ({response.TotalResults} total results)" );

    foreach( MovieInfo info in response.Results )
    {
        Console.WriteLine( $"{info.Title} ({info.ReleaseDate}): {info.Overview}" );
    }

    totalPages = response.TotalPages;
} while( pageNumber++ < totalPages );
```

# Do you have a comprehensive list of examples?
* The API we've exposed should be fairly straight forward. All interfaces, classes, methods, properties, etc... have full intellisense support. If you need more detailed examples, just ask!
* You may also browse the suite of [integration tests](DM.MovieApi.IntegrationTests/) covering all usages of the API.

