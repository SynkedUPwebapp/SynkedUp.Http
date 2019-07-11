using NUnit.Framework;

namespace EL.Http.UnitTests
{
    [TestFixture]
    public class WhenAddingAHeader
    {
        [SetUp]
        public void SetUp()
        {
            classUnderTest = new HttpHeaders();
            result = classUnderTest.Add(HeaderName, HeaderValue);
        }

        [Test]
        public void ShouldHaveStoredTheValue()
        {
            Assert.That(classUnderTest.GetValue(HeaderName), Is.EqualTo(HeaderValue));
        }

        [Test]
        public void ShouldExist()
        {
            Assert.That(classUnderTest.Exists(HeaderName), Is.True);
        }

        [Test]
        public void ShouldReturnAListWithOnlyOneValue()
        {
            Assert.That(classUnderTest.GetAllValues(HeaderName).Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldReturnAListWithTheValue()
        {
            Assert.That(classUnderTest.GetAllValues(HeaderName), Does.Contain(HeaderValue));
        }

        [Test]
        public void ShouldHaveTheHeaderName()
        {
            Assert.That(classUnderTest.GetAllHeaderNames(), Does.Contain(HeaderName));
        }

        [Test]
        public void ShouldHaveOneHeader()
        {
            Assert.That(classUnderTest.GetAllHeaderNames().Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldBeChainable()
        {
            Assert.That(result, Is.EqualTo(classUnderTest));
        }

        private HttpHeaders classUnderTest;
        private HttpHeaders result;
        private const string HeaderName = "name";
        private const string HeaderValue = "value";
    }

    [TestFixture]
    public class WhenAccessingAHeaderThatDoesNotExist
    {
        [SetUp]
        public void SetUp()
        {
            classUnderTest = new HttpHeaders();
        }

        [Test]
        public void ShouldReturnAnEmptyString()
        {
            Assert.That(classUnderTest.GetValue("does-not-exist"), Is.Empty);
        }

        [Test]
        public void ShouldNotExist()
        {
            Assert.That(classUnderTest.Exists("does-not-exist"), Is.False);
        }

        [Test]
        public void ShouldReturnAnEmptyList()
        {
            Assert.That(classUnderTest.GetAllValues("does-not-exist").Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldHaveNoHeaders()
        {
            Assert.That(classUnderTest.GetAllHeaderNames().Count, Is.EqualTo(0));
        }

        private HttpHeaders classUnderTest;
    }
}
