# EL.Http

The purpose of this library is to provide an easily-testable abstraction for HTTP that does a good job of representing an actual HTTP request.

Unfortunately, the built-in `System.Net.Http.HttpClient` class of the .NET Core library does some really annoying things like:
* Requiring special handling of certain headers, such as `accept` and `content-type`
* Dividing response headers into `.Headers` and `.Content.Headers`
* Doing special cookie handling by default
* Making it difficult to specify a timeout on a per-request basis

_(But at least it is much better than the old .NET Framework http requests which would throw exceptions on 4xx and 5xx responses...)_

## Configuration

Call `EL.Http.DependencyInjectionConfig.ConfigureServices(services);` to configure DI.
This will make the `IHttpClient` available as a singleton because Microsoft recommends
reusing the underlying `System.Net.Http.HttpClient` for connection pooling.
The default `HttpCLientOptions` are used.

If you have a need to have separate instances of the `EL.Http.HttpClient`,
or if you wish to provide specific `HttpClientOptions`,
you can create a separate instance or override the DI registration.

## Usage

A very simple GET requests can be performed like this:
```csharp
var request = new HttpRequest
{
    Url = "https://example.com/foo/bar"
};
var response = httpClient.Execute(request);
```

Here is a more involved POST example:
```csharp
var request = new HttpRequest
{
    Method = HttpMethod.POST,
    Url = "https://example.com/foo/bar",
    Headers = new HttpHeaders()
        .Add("Authorization", "Bearer token")
        .Add("Content-Type", "application/json"),
    Body = serializedJson
};
var response = httpClient.Execute(request);
```

The client also exposes an `ExecuteAsync` method.

## Version History

### 3.0
- Updated to netcoreapp3.1
- Added `DependencyInjectionConfig` class for configuring DI
- Added default JSON support which removes the need for an `IRequestSerializer`
- Added convenience extension method for JSON deserialization
- Renamed directory structure to match project names

### 2.0
- Changed version schema from 4 parts to 3 parts (dropped the date in the 3rd part)
- Changed the signature of `ExecuteAsync` to accept `IHttpRequest` instead of the concrete type `HttpRequest`. All existing code should just work, no changes required.
- Added `StreamHttpRequest` for requests with a binary body
- Added extension method `AddBasicAuthentication(string username, string password)` on `IHttpRequest`
