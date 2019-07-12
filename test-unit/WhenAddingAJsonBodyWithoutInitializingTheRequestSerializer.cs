using System;
using NUnit.Framework;

namespace EL.Http.UnitTests
{
    public class WhenAddingAJsonBodyWithoutInitializingTheRequestSerializer
    {
        private JsonTest bodyObject;
        private HttpRequest request;

        [SetUp]
        public void SetUp()
        {
            HttpRequestExtensions.InitializeRequestSerializer(requestSerializer: null);

            request = new HttpRequest();
            bodyObject = new JsonTest {StringProperty = Guid.NewGuid().ToString("N"), IntegerProperty = 42};
        }

        [Test]
        public void ShouldJsonSerializeThrow()
        {
            Assert.Throws<InvalidOperationException>(() => request.AddJsonBody(bodyObject));
        }
    }
}