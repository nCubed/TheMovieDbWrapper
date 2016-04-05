# TheMovieDb.org Wrapper
TheMovieDbWrapper is a C# wrapper for [themoviedb.org](https://www.themoviedb.org) API.

A nuget package is available directly through Visual Studio: https://www.nuget.org/packages/TheMovieDbWrapper/

##### Common Requests
The current release supports common requests for movie and other miscellaneous information, such as:
* themoviedb.org configuration information
* Movie information
* Movie rating information
* Movie and TV genres
* Movie/TV industry specific professions
* Production company information

## Usage - Setup
In order to use any feature of the wrapper, a concrete implementation of the [IMovieDbSettings](DM.MovieApi/IMovieDbSettings.cs) interface must be created. The interface only contains 2 properties:

1. ApiKey: a private key required to query [themoviedb.org API](https://www.themoviedb.org/documentation/api).
2. ApiUrl: the URL used for api calls to themoviedb.org. This URL should be static, but is included in case an alternative URL is ever provided.

### Usage - MovieDbFactory
The `MovieDbFactory` provides access to all exposed operations for retrieving information from themoviedb.org. The factory exposes the following methods (other methods may be exposed, but these are the important ones):
* `void MovieDbFactory.RegisterSettings( IMovieDbSettings settings )`: Registers your themoviedb.org specific API key with the factory; this method must be called prior to any other operations.
* `Lazy<T> MovieDbFactory.Create<T>() where T : IApiRequest`: Creates the specific API requested. See below (Usages - Interfaces) for all exposed interfaces.

### Usage - Interfaces
The following interfaces can be used to retrieve information:
* `IApiConfigurationRequest`: Provides access for retrieving themoviedb.org configuration information.
* `IApiMovieRequest`: Provides access for retrieving information about Movies.
* `IApiMovieRatingRequest`: Provides access for retrieving movie rating information.
* `IApiGenreRequest`: Provides access for retrieving Movie and TV genres.
* `IApiCompanyRequest`: Provides access for retrieving production company information.
* `IApiProfessionRequest`: Provides access for retrieving information about Movie/TV industry specific professions.

### Usage - Examples
Register your settings first:
```csharp
MovieDbFactory.RegisterSettings( new YourMovieDbSettings() )
```

Retrieve an API request interface (see Usage - Interfaces above for available interfaces):
```csharp
// as the factory returns a Lazy<T> instance, simply grab the Value out of the Lazy<T>
// and assign to a local variable.
var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value
```

Use the API request interface to retrieve information:
```csharp
ApiSearchResponse<MovieInfo> response = await _api.SearchByTitleAsync( "Star Trek" );
```

The [ApiSearchResponse](DM.MovieApi/ApiResponse/ApiSearchResponse.cs) provides rich information about the results of the search:
* `IReadOnlyList<T> Results`: The list of results from the search.
* `int PageNumber`: The current page number of the search result.
* `int TotalPages`: The total number of pages found from the search result.
* `int TotalResults`: The total number of results from the search.
* `ToString()`: returns "Page x of y (z total results)".
* `ApiError Error`: Contains specific error information if an error was encountered during the API call to themoviedb.org.
* `ApiRateLimit RateLimit`: Contains the current rate limits from your most recent API call to themoviedb.org.

#### Usage - Examples: Putting it all together
```csharp
// RegisterSettings only needs to be called one time when your application starts-up.
MovieDbFactory.RegisterSettings( new YourMovieDbSettings() )

var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;

ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync( "Star Trek" );

foreach( MovieInfo info in response.Results )
{
    Console.WriteLine( "{0} ({1}): {2}" info.Title, info.ReleaseDate, info.Overview );
}
```

See the [MovieInfo](DM.MovieApi/MovieDb/Movies/MovieInfo.cs) class for all available information from a movie search response.

#### Usage - Examples: Paging a search result
```csharp
int pageNumber;
var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
do
{
    ApiSearchResponse<MovieInfo> response = await movieApi.SearchByTitleAsync( "Harry" );

    foreach( MovieInfo info in response.Results )
    {
        Console.WriteLine( "{0} ({1}): {2}" info.Title, info.ReleaseDate, info.Overview );
    }
    
    pageNumber++;
} while( pageNumber <= response.TotalPages );
```

### Do you have a comprehensive list of examples?
* The API we've exposed should be fairly straight forward. All interfaces, classes, methods, properties, etc... have full intellisense support. If you need more detailed examples, just ask!
* You may also browse the suite of [integration tests](DM.MovieApi.IntegrationTests/) covering all usages of the API.
