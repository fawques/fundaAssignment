using FundaAssignment.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace FundaAssignmentTests
{
    public class FundaClientTest
    {
        Mock<HttpMessageHandler> handlerMock;
        HttpClient mockedHttpClient;

        public FundaClientTest()
        {
            var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var amsterdamJsonString = File.ReadAllText(Path.Join(assemblyDirectory, "Data", "koopAmsterdam.json"));
            var amsterdamTuinJsonString = File.ReadAllText(Path.Join(assemblyDirectory, "Data", "koopAmsterdamTuin.json"));
            handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            SetupHandler(handlerMock, amsterdamJsonString, amsterdamTuinJsonString);
        }

        void SetupHandler(Mock<HttpMessageHandler> handlerMock, string amsterdamJsonString, string amsterdamTuinJsonString)
        {
            // Setup it to return the same page, with HuidigePagina updated to the currently requested page
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => HttpUtility.ParseQueryString(m.RequestUri.Query).Get("zo") == "/amsterdam/"),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Returns((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    int page = int.Parse(HttpUtility.ParseQueryString(request.RequestUri.Query).Get("page"));
                    return Task.FromResult(new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(amsterdamJsonString.Replace("\"HuidigePagina\": 1", $"\"HuidigePagina\": {page}")),
                    });
                })
                .Verifiable();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.Is<HttpRequestMessage>(m => HttpUtility.ParseQueryString(m.RequestUri.Query).Get("zo") == "/amsterdam/tuin/"),
                  ItExpr.IsAny<CancellationToken>()
               )
               .Returns((HttpRequestMessage request, CancellationToken cancellationToken) =>
               {
                   int page = int.Parse(HttpUtility.ParseQueryString(request.RequestUri.Query).Get("page"));
                   return Task.FromResult(new HttpResponseMessage()
                   {
                       StatusCode = HttpStatusCode.OK,
                       Content = new StringContent(amsterdamTuinJsonString.Replace("\"HuidigePagina\": 1", $"\"HuidigePagina\": {page}")),
                   });
               })
               .Verifiable();

            mockedHttpClient = new HttpClient(handlerMock.Object);
        }

        [Fact]
        public async Task QueryListingsWithTuin_OK_WholeListReturned()
        {
            // Arrange
            var fundaClient = new FundaClient(mockedHttpClient);

            // Act
            var listings = await fundaClient.Query("koop", "amsterdam", "tuin");

            // Assert
            var expectedUri = new Uri($"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(34),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri.AbsolutePath == expectedUri.AbsolutePath
                  && HttpUtility.ParseQueryString(req.RequestUri.Query).Get("type") == "koop"
                  && HttpUtility.ParseQueryString(req.RequestUri.Query).Get("zo") == "/amsterdam/tuin/"
               ),
               ItExpr.IsAny<CancellationToken>()
            );

            // The response says 849, but we aren't changing the last page to return 24 items, so the total is greater.
            // In the real API this count would be right.
            Assert.Equal(850, listings.Count());
        }

        [Fact]
        public async Task QueryListings_OK_WholeListReturned()
        {
            // Arrange
            var fundaClient = new FundaClient(mockedHttpClient);

            // Act
            var listings = await fundaClient.Query("koop", "amsterdam", null);

            // Assert
            var expectedUri = new Uri($"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/");
            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(133),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri.AbsolutePath == expectedUri.AbsolutePath
                  && HttpUtility.ParseQueryString(req.RequestUri.Query).Get("type") == "koop"
                  && HttpUtility.ParseQueryString(req.RequestUri.Query).Get("zo") == "/amsterdam/"
               ),
               ItExpr.IsAny<CancellationToken>()
            );

            // The response says 3317, but we aren't changing the last page to return 17 items, so the total is greater.
            // In the real API this count would be right.
            Assert.Equal(3325, listings.Count());
        }
    }
}
