using System;
using System.Collections.Generic;
using Xunit;
using System.Text;
using FundaAssignment.Services;
using Moq;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using System.Threading.Tasks;

namespace FundaAssignmentTests
{
    public class MainServiceTest
    {
        Mock<IFundaClient> mockFundaClient;
        MainService service;

        public MainServiceTest()
        {
            mockFundaClient = new Mock<IFundaClient>();
            service = new MainService(mockFundaClient.Object, new MakelaarService(new MakelaarFactory()));
        }

        [Fact]
        public async Task CalculateTopMakelaars_TopMakelaarsCalculated()
        {
            // Arrange
            mockFundaClient.Setup(f => f.Query("koop", "amsterdam", null))
                .ReturnsAsync(new List<Listing> {
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
                    new Listing
                    {
                        MakelaarId = 4,
                        MakelaarNaam = "M4",
                    },
                    new Listing
                    {
                        MakelaarId = 4,
                        MakelaarNaam = "M4",
                    },
                });

            mockFundaClient.Setup(f => f.Query("koop", "amsterdam", "tuin"))
                .ReturnsAsync(new List<Listing> {
                    new Listing
                    {
                        MakelaarId = 2,
                        MakelaarNaam = "M2",
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
                        MakelaarId = 4,
                        MakelaarNaam = "M4",
                    },
                    new Listing
                    {
                        MakelaarId = 4,
                        MakelaarNaam = "M4",
                    },
                });

            // Act
            var topMakelaarsResult = await service.CalculateTopMakelaars();

            // Assert
            mockFundaClient.Verify(f => f.Query("koop", "amsterdam", null), Times.Once);
            mockFundaClient.Verify(f => f.Query("koop", "amsterdam", "tuin"), Times.Once);

            Assert.Collection(topMakelaarsResult.TopAmsterdam,
                m => Assert.Equal(3, m.MakelaarId),
                m => Assert.Equal(2, m.MakelaarId),
                m => Assert.Equal(4, m.MakelaarId),
                m => Assert.Equal(1, m.MakelaarId));

            Assert.Collection(topMakelaarsResult.TopAmsterdamTuin,
                m => Assert.Equal(2, m.MakelaarId),
                m => Assert.Equal(4, m.MakelaarId),
                m => Assert.Equal(3, m.MakelaarId));
        }
    }
}
