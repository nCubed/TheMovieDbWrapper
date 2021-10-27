<style>
table td:first-child > code {
  white-space: nowrap;
}
</style>


# TheMovieDb.org Wrapper
TheMovieDbWrapper is a C# wrapper for [TheMovieDb.org](https://www.themoviedb.org) API providing cross-platform support for Xamarin, iOS, Android, and all flavors of .NET.

A nuget package is available directly through Visual Studio: https://www.nuget.org/packages/TheMovieDbWrapper/

## v1.0 Breaking Changes :vomiting_face:
```
The v1.0 release on 2021-10-27 introduces a minor breaking change when registering your TheMovieDb.org credentials with our MovieDbFactory.
```

* `IMovieDbSettings` has been completely eliminated and simplifies the process of registering your credentials.
  * You no longer need to create a concrete implementation of the interface when registering your credentials.
  * You no longer need to provide TheMovieDb.org api url.
* Your TheMovieDb.org credentials now use their updated authentication using a Bearer Token.
  * Your Bearer token is found in your [TheMovieDb.org account page](https://www.themoviedb.org/settings/api), under the API section: _"API Read Access Token (v4 auth)"_. 
  * Note: This token IS NOT the same as the old _"API Key (v3 auth)"_.

## Common API Requests
The current release supports common requests for movie, tv, and other information, such as:
* TheMovieDb.org configuration
* Movies :movie_camera:	
* Movie Ratings
* TV Shows :tv:
* Movie and TV Genres
* Movie/TV Industry Specific Professions
* Production Companies
* People such as Actors, Actresses, Directors, etc...

## Basic Usage
The `MovieDbFactory` class is the single entry point for retrieving information from TheMovieDb.org API. Before making any requests, you simply need to register your TheMovieDb.org Bearer Token with our `MovieDbFactory` class:

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

The `IApiRequest` is simply an Interface providing a constraint for all our request interfaces/classes used in the factory. For example, to retrieve the API about movies:

```csharp
// as the factory returns a Lazy<T> instance, simply grab the Value out of the Lazy<T>
// and assign to a local variable.
var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
```
## API Interfaces
The following interfaces are used with the `MovieDbFactory.Create<T>()` method:

`IApiRequest` | Description
-|-
[`IApiConfigurationRequest`](DM.MovieApi/MovieDb/Configuration/IApiConfigurationRequest.cs) | Provides access for retrieving TheMovieDb.org configuration information.
[`IApiMovieRequest`](DM.MovieApi/MovieDb/Movies/IApiMovieRequest.cs) | Provides access for retrieving information about Movies.
[`IApiMovieRatingRequest`](DM.MovieApi/MovieDb/Certifications/IApiMovieRatingRequest.cs) | Provides access for retrieving movie rating information.
[`IApiTVShowRequest`](DM.MovieApi/MovieDb/TV/IApiTVShowRequest.cs) | Provides access for retrieving information about TV shows.
[`IApiGenreRequest`](DM.MovieApi/MovieDb/Genres/IApiGenreRequest.cs) | Provides access for retrieving Movie and TV genres.
[`IApiCompanyRequest`](DM.MovieApi/MovieDb/Companies/IApiCompanyRequest.cs) | Provides access for retrieving production company information.
[`IApiProfessionRequest`](DM.MovieApi/MovieDb/IndustryProfessions/IApiProfessionRequest.cs) | Provides access for retrieving information about Movie/TV industry specific professions.
[`IApiPeopleRequest`](DM.MovieApi/MovieDb/People/IApiPeopleRequest.cs) | Provides access for retrieving information about People.

## More Examples

### Search by Movie Title

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

The above example returns an [`ApiSearchResponse<T>`](DM.MovieApi/ApiResponse/ApiSearchResponse.cs) which provides rich information about the results of your search:

Member | Description
-|-
`IReadOnlyList<T> Results` | The list of results from the search.
`int PageNumber` | The current page number of the search result.
`int TotalPages` | The total number of pages found from the search result.
`int TotalResults` | The total number of results from the search.
`ToString()` | returns "Page x of y (z total results)".
`ApiError Error` | Contains specific error information if an error was encountered during the API call to TheMovieDb.org.
`ApiRateLimit RateLimit` | Contains the current rate limits from your most recent API call to TheMovieDb.org. Note: TheMovieDb.org has removed all rate limits as of December of 2019.

### Find Movie By Id

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

The above query returns an [`ApiQueryResponse<T>`](DM.MovieApi/ApiResponse/ApiQueryResponse.cs) which returns a single result as well as some common information seen in the `ApiSearchResponse` in the prior example:

Member | Description
-|-
`T Item` | The item returned from the API call, where T is the specific query such as `MovieInfo`, `Movie`, `MovieCredit`, etc..
`ToString()` | Typically returns a well formatted string of `T`.
`ApiError Error` | If an error was encountered, this will provide specific error information from the API call to TheMovieDb.org.
`ApiRateLimit RateLimit` | Contains the current rate limits from your most recent API call to TheMovieDb.org. Note: TheMovieDb.org has removed all rate limits as of December of 2019.

## Paging a search result
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

## Do you have a comprehensive list of examples?
* The API we've exposed should be fairly straight forward. All interfaces, classes, methods, properties, etc... have full intellisense support. If you need more detailed examples, just ask!
* You may also browse the suite of [integration tests](DM.MovieApi.IntegrationTests/) covering all usages of the API.

