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
    public class MakelaarServiceTest
    {
        MakelaarService service;

        public MakelaarServiceTest()
        {
            service = new MakelaarService(new MakelaarFactory());
        }

        [Fact]
        public void GetTopMakelaars_EmptyList_NoneReturned()
        {
            // Arrange
            var listings = new List<Listing> { };

            // Act
            var topMakelaars = service.GetTopMakelaars(listings);

            // Assert
            Assert.Empty(topMakelaars);
        }

        [Fact]
        public void GetTopMakelaars_FewMakelaarsOneListingEach_AllMakelaarsReturned()
        {
            // Arrange
            var listings = new List<Listing>
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
            };

            // Act
            var topMakelaars = service.GetTopMakelaars(listings, 10);

            // Assert
            Assert.Equal(3, topMakelaars.Count());
            Assert.Contains(topMakelaars, m => m.MakelaarId == 1);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 2);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 3);
        }

        [Fact]
        public void GetTopMakelaars_ManyMakelaarsOneListingEach_FirstMakelaarsReturned()
        {
            // Arrange
            var listings = new List<Listing>
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
            };

            // Act
            var topMakelaars = service.GetTopMakelaars(listings, 2);

            // Assert
            Assert.Equal(2, topMakelaars.Count());
            Assert.Contains(topMakelaars, m => m.MakelaarId == 1);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 2);
            Assert.DoesNotContain(topMakelaars, m => m.MakelaarId == 3);
        }

        [Fact]
        public void GetTopMakelaars_ManyMakelaarsManyListings_TopMakelaarsReturned()
        {
            // Arrange
            var listings = new List<Listing>
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
            };


            // Act
            var topMakelaars = service.GetTopMakelaars(listings, 3);

            // Assert
            Assert.Equal(3, topMakelaars.Count());
            Assert.DoesNotContain(topMakelaars, m => m.MakelaarId == 1);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 2);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 3);
            Assert.Contains(topMakelaars, m => m.MakelaarId == 4);
        }
    }
}
