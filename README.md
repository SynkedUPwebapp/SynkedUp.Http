# EL.Http

The purpose of this library is to provide an easily-testable abstraction for HTTP that does a good job of representing an actual HTTP request.

Unfortunately, the built-in `System.Net.Http.HttpClient` class of the .NET Core library does some really annoying things like:
* Requiring special handling of certain headers, such as `accept` and `content-type`
* Dividing response headers into `.Headers` and `.Content.Headers`
* Doing special cookie handling by default
* Making it difficult to specify a timeout on a per-request basis

_(But at least it is much better than the old .NET Framework http requests which would throw exceptions on 4xx and 5xx responses...)_

## Configuration

To use this library, you only need an `EL.Http.HttpClient` instance.
There is no required DI configuration, though you may wish to specify an `AddSingleton<IHttpClient, HttpClient>()`.
As with the underlying `System.Net.Http.HttpClient` you may get better performance by reusing the client rather than creating new ones each time,
though separate instances may be useful if you need a separate connection pool.

When creating the `HttpClient`, you may optionally provide an `HttpClientOptions` instance to specify additional options.

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
