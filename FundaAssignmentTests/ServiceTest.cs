using FundaAssignment;
using FundaAssignment.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FundaAssignmentTests
{
    public class ServiceTest
    {
        [Fact]
        public async Task GetTopMarkelaars_EmptyList_NoneReturned()
        {
            Mock<IFundaClient> mockFundaClient = new Mock<IFundaClient>();
            mockFundaClient.Setup(f => f.Query("koop", "amsterdam", null)).ReturnsAsync(Enumerable.Empty<Listing>());

            SampleService service = new SampleService(mockFundaClient.Object);
            var topMarkelaars = await service.GetTopMarkelaars();

            Assert.Empty(topMarkelaars);
        }
    }
}
