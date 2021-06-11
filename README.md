# Emmersion.Http

The purpose of this library is to provide an easily-testable abstraction for HTTP that does a good job of representing an actual HTTP request.

This has been [open sourced](https://github.com/emmersion/engineering-at-emmersion#open-source)
under the [MIT License](./LICENSE).

## Why?

Unfortunately, the built-in `System.Net.Http.HttpClient` class of the .NET Core library does some really annoying things like:
* Requiring special handling of certain headers, such as `accept` and `content-type`
* Dividing response headers into `.Headers` and `.Content.Headers`
* Doing special cookie handling by default
* Making it difficult to specify a timeout on a per-request basis

_(But at least it is much better than the old .NET Framework http requests which would throw exceptions on 4xx and 5xx responses...)_

## Configuration

Call `Emmersion.Http.DependencyInjectionConfig.ConfigureServices(services);` to configure DI.
This will make the `IHttpClient` available as a singleton because Microsoft recommends
reusing the underlying `System.Net.Http.HttpClient` for connection pooling.
The default `HttpCLientOptions` are used.

If you have a need to have separate instances of the `Emmersion.Http.HttpClient`,
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
public string PostFooBar(string bearerToken, string postData)
{
    var request = new HttpRequest
    {
        Method = HttpMethod.POST,
        Url = "https://example.com/foo/bar",
        Headers = new HttpHeaders()
            .Add("Authorization", $"Bearer {bearerToken}")
            .Add("Content-Type", "text/plain"),
        Body = postData
    };
    var response = httpClient.Execute(request);
    return response.Body;
}
```

The client also exposes a non-blocking `ExecuteAsync` method.

If the request times out, an `HttpTimeoutException` will be thrown.

`StreamHttpRequest` exists if you wish to send a request with the body as a stream.

When working with Json, there are `AddJsonBody` and `DeserializeJsonBody` convenience methods.

## Testing

Here is an example test which uses the [Emmersion.Testing](https://github.com/emmersion/Emmersion.Testing) library:

```csharp
[Test]
public void When_posting_successfully()
{
    var bearerToken = "1234567890";
    var postBody = "This is my example data.";
    HttpRequest request = null;
    var expectedResult = "Thanks. Version 2 uploaded OK.";
    var response = new HttpResponse(200, new HttpHeaders(), expectedResult);
    GetMock<IHttpClient>().Setup(x => x.Execute(IsAny<IHttpRequest>()))
        .Callback<IHttpRequest>(x => request = x as HttpRequest)
        .Returns(response);
    
    var result = ClassUnderTest.PostFooBar(bearerToken, postBody);

    Assert.That(result, Is.EqualTo(expectedResult));
    Assert.That(request.Method, Is.EqualTo(HttpMethod.POST));
    Assert.That(request.Url, Is.EqualTo("https://example.com/foo/bar"));
    Assert.That(request.Headers.GetValue("Authorization"), Is.EqualTo($"Bearer {bearerToken}"));
    Assert.That(request.Headers.GetValue("Content-Type"), Is.EqualTo("text/plain"));
    Assert.That(request.Body, Is.EqualTo(postBody));
}
```

Because we've mocked out the `IHttpClient` we can inspect the details of the request and provide any response we want.
This makes it easy to test what your code will do if the response comes back with a particular status code, malformed json, times out, or something else.
And because the request and response model the HTTP request, it should be simple to verify any data.


## Version History

### 4.0
- Changed namespace from `EL.` to `Emmersion.`

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
