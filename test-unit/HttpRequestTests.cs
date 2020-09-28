﻿using System;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EL.Http.UnitTests
{
    public class WhenAddingAJsonBody
    {
        private JsonTest bodyObject;

        private HttpRequest request;

        [SetUp]
        public void SetUp()
        {
            HttpRequestExtensions.InitializeRequestSerializer(new JsonSerializer());

            request = new HttpRequest();
            bodyObject = new JsonTest {StringProperty = Guid.NewGuid().ToString("N"), IntegerProperty = 42};
            request.AddJsonBody(bodyObject);
        }

        [Test]
        public void ShouldJsonSerializeTheBody()
        {
            var deserializedBody = JsonConvert.DeserializeObject<JsonTest>(request.Body);
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
}