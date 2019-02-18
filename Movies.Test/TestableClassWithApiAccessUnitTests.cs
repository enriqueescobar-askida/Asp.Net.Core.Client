namespace Movies.Test
{
    using Moq;
    using Moq.Protected;
    using Movies.Client;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class TestableClassWithApiAccessUnitTests
    {
        [Fact]
        public void GetMovie_On401Response_MustThrowUnauthorizedApiAccessException()
        {
            HttpClient httpClient = new HttpClient(new Return401UnauthorizedResponseHandler());
            TestableClassWithApiAccess testableClass = new TestableClassWithApiAccess(httpClient);
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.ThrowsAsync<UnauthorizedApiAccessException>(
                () => testableClass.GetMovie(cancellationTokenSource.Token));
        }

        [Fact]
        public void GetMovie_On401Response_MustThrowUnauthorizedApiAccessException_WithMoq()
        {
            Mock<HttpMessageHandler> unauthorizedResponseHttpMessageHandlerMock = new Mock<HttpMessageHandler>();

            unauthorizedResponseHttpMessageHandlerMock.Protected()
                 .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               ).ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.Unauthorized
               });

            HttpClient httpClient = new HttpClient(unauthorizedResponseHttpMessageHandlerMock.Object);
            TestableClassWithApiAccess testableClass = new TestableClassWithApiAccess(httpClient);
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.ThrowsAsync<UnauthorizedApiAccessException>(
                () => testableClass.GetMovie(cancellationTokenSource.Token));
        }
    }
}
