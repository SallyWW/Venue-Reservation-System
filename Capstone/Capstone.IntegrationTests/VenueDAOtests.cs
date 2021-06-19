using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueDAOtests : IntegrationTestBase
    {
        [TestMethod]
        public void GetVenuesShouldReturnOneRow()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);
            
            // Act
            IList<Venue> results = dao.GetAllVenues();
            // Assert
            Assert.IsTrue(results.Count > 0);
            Assert.AreEqual(1, results.Count);
        }
        [TestMethod]
        public void VenueShouldHave1CategoryCalledPartyPlanet()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);
            Venue venue = new Venue();
            venue.Id = 1;

            // Act
            IList<string> result = dao.GetCategoriesForVenues(venue);
            // Assert
            Assert.AreEqual(result[0], "Party Planet");
            Assert.AreEqual(result.Count, 1);
        }
        [TestMethod]
        public void GetSpacesForVenueReturnsSpaces()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);
            Space expected = new Space
            {
                Id = 1,
                Name = "Earth",
                WheelchairAccessible = true,
                OpenMonth = 4,
                CloseMonth = 10,
                DailyRate = 1000,
                MaxOccupancy = 100
            };

            // Act
            IList<Space> results = dao.GetSpacesForVenue("1");

            // Assert
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void GetAllAvailableSpacesFiltersUnavailableSpaces()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);
            Space expected = new Space
            {
                Id = 2,
                Name = "Jupiter",
                WheelchairAccessible = false,
                OpenMonth = 2,
                CloseMonth = 8,
                DailyRate = 2000,
                MaxOccupancy = 500
            };
            DateTime date = new DateTime(2021, 2, 1);

            // Act
            IList<Space> results = dao.GetAllAvailableSpaces("1", date, 100, 250);

            // Assert
            Assert.AreEqual(1, results.Count);
            Space space = results[0];
            Assert.AreEqual(expected.Id, space.Id);
            Assert.AreEqual(expected.Name, space.Name);
            Assert.AreEqual(expected.WheelchairAccessible, space.WheelchairAccessible);
            Assert.AreEqual(expected.DailyRate, space.DailyRate);
            Assert.AreEqual(expected.MaxOccupancy, space.MaxOccupancy);
        }

        [TestMethod]
        public void CreateAReservationSucceeds()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(ConnectionString);
            Space space = new Space
            {
                Id = 1,
            };
            DateTime startDate = new DateTime(2021, 7, 8);
            DateTime endDate = new DateTime(2021, 8, 29);
            // Act
            Reservation result = dao.CreateAReservation("E.T.", startDate, endDate, space, 100);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("E.T.", result.ReserveName);
            Assert.AreEqual(startDate, result.StartDate);
            Assert.AreEqual(endDate, result.EndDate);
            Assert.AreEqual(100, result.NumberOfAttendees);
        }
    }
}
