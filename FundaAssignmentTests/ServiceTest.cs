using FundaAssignment;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using FundaAssignment.Services;
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
        Mock<IFundaClient> mockFundaClient;
        MakelaarService service;

        public ServiceTest()
        {
            mockFundaClient = new Mock<IFundaClient>();
            mockFundaClient.Setup(f => f.Query("koop", "amsterdam", null)).ReturnsAsync(new List<Listing>
            {
                new Listing
                {
                    MakelaarId = 1,
                    MakelaarNaam = "M1",
                },
                new Listing
                {
                    MakelaarId = 2,
                    MakelaarNaam = "M2",
                },
                new Listing
                {
                    MakelaarId = 3,
                    MakelaarNaam = "M3",
                },
            });

            service = new MakelaarService(mockFundaClient.Object, new MakelaarFactory());
        }

        [Fact]
        public async Task GetTopMakelaars_EmptyList_NoneReturned()
        {
            // Arrange
            mockFundaClient.Reset();
            mockFundaClient.Setup(f => f.Query("koop", "amsterdam", null)).ReturnsAsync(Enumerable.Empty<Listing>());

            // Act
            var topMakelaars = await service.GetTopMakelaars();

            // Assert
            Assert.Empty(topMakelaars);
        }

        [Fact]
        public async Task GetTopMakelaars_FewMakelaarsOneListingEach_AllMakelaarsReturned()
        {
            // Arrange

            // Act
            var topMakelaars = await service.GetTopMakelaars();

            // Assert
            Assert.Equal(3, topMakelaars.Count());
            Assert.Contains(topMakelaars, m => m.MakelaarId == 1);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 2);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 3);
        }

        [Fact]
        public async Task GetTopMakelaars_ManyMakelaarsOneListingEach_FirstMakelaarsReturned()
        {
            // Arrange

            // Act
            var topMakelaars = await service.GetTopMakelaars(2);

            // Assert
            Assert.Equal(2, topMakelaars.Count());
            Assert.Contains(topMakelaars, m => m.MakelaarId == 1);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 2);
            Assert.DoesNotContain(topMakelaars, m => m.MakelaarId == 3);
        }

        [Fact]
        public async Task GetTopMakelaars_ManyMakelaarsManyListings_TopMakelaarsReturned()
        {
            // Arrange
            mockFundaClient.Setup(f => f.Query("koop", "amsterdam", null)).ReturnsAsync(new List<Listing>
            {
                new Listing
                {
                    MakelaarId = 1,
                    MakelaarNaam = "M1",
                },
                new Listing
                {
                    MakelaarId = 2,
                    MakelaarNaam = "M2",
                },
                new Listing
                {
                    MakelaarId = 3,
                    MakelaarNaam = "M3",
                },
                new Listing
                {
                    MakelaarId = 3,
                    MakelaarNaam = "M3",
                },
                new Listing
                {
                    MakelaarId = 2,
                    MakelaarNaam = "M2",
                },
                new Listing
                {
                    MakelaarId = 3,
                    MakelaarNaam = "M3",
                },
                new Listing
                {
                    MakelaarId = 3,
                    MakelaarNaam = "M3",
                },
            });


            // Act
            var topMakelaars = await service.GetTopMakelaars(2);

            // Assert
            Assert.Equal(2, topMakelaars.Count());
            Assert.DoesNotContain(topMakelaars, m => m.MakelaarId == 1);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 2);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 3);
        }
    }
}
