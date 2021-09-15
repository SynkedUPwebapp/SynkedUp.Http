using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Emmersion.Http.UnitTests
{
    public class WhenAddingAJsonBody
    {
        private JsonTest bodyObject;

        private HttpRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new HttpRequest();
            bodyObject = new JsonTest {StringProperty = Guid.NewGuid().ToString("N"), IntegerProperty = 42};
            request.AddJsonBody(bodyObject);
        }

        [Test]
        public void ShouldJsonSerializeTheBody()
        {
            var deserializedBody = JsonSerializer.Deserialize<JsonTest>(request.Body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            Assert.That(deserializedBody, Is.Not.Null);
            Assert.That(deserializedBody.StringProperty, Is.EqualTo(bodyObject.StringProperty));
            Assert.That(deserializedBody.IntegerProperty, Is.EqualTo(bodyObject.IntegerProperty));
        }

        [Test]
        public void ShouldSetTheContentType()
        {
            Assert.That(request.Headers.GetValue("Content-Type"), Is.EqualTo("application/json"));
        }
    }

    public class WhenAddingBasicAuthentication
    {
        private const string Authorization = "Authorization";
        private IHttpRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new HttpRequest();
            request.AddBasicAuthentication("username", "password");
        }
        
        [Test]
        public void HasAuthorizationHeader()
        {
            Assert.That(request.Headers.Exists(Authorization), Is.True);
        }

        [Test]
        public void AuthorizationHeaderHasCorrectValue()
        {
            Assert.That(request.Headers.GetValue(Authorization), Is.EqualTo("Basic dXNlcm5hbWU6cGFzc3dvcmQ="));
        }
    }
    
    public class WhenAddingAFormUrlEncodedBody
    {
        private Dictionary<string, string> bodyObject;

        private HttpRequest request;

        [SetUp]
        public async Task SetUp()
        {
            request = new HttpRequest();
            bodyObject = new Dictionary<string, string>
            {
                ["simple"] = "simon",
                ["url"] = "https://example.com"
            };
            await request.AddFormUrlEncodedBody(bodyObject);
        }

        [Test]
        public void ShouldSerializeTheBody()
        {
            Assert.That(request.Body, Is.EqualTo("simple=simon&url=https%3A%2F%2Fexample.com"));
        }

        [Test]
        public void ShouldSetTheContentType()
        {
            Assert.That(request.Headers.GetValue("Content-Type"), Is.EqualTo("application/x-www-form-urlencoded"));
        }
    }
}