using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;
        private const string SqlSelectAllVenues = "SELECT v.id, v.name, c.name AS cityName, s.name AS stateName, s.abbreviation, v.description " +
            "FROM venue v JOIN city c ON v.city_id = c.id JOIN state s ON c.state_abbreviation = s.abbreviation " +
            "ORDER BY v.name";
        private const string SqlSelectGetCategories = "SELECT c.name FROM venue v " +
            "JOIN category_venue cv ON v.id = cv.venue_id " +
            "JOIN category c ON c.id = cv.category_id " +
            "WHERE v.id = @venueid " +
            "ORDER BY v.name";
        private const string SqlGetVenueSpaces = "SELECT s.id, s.name, s.is_accessible, s.open_from, s.open_to, s.daily_rate, s.max_occupancy " +
            "FROM venue v JOIN space s ON v.id = s.venue_id " +
            "WHERE v.id = @id";
        private const string SqlSelectUnreservedSpaces = "SELECT TOP 5 s.id, s.name, s.daily_rate, s.is_accessible, s.max_occupancy " +
            "FROM space s " +
            "WHERE venue_id = @venue_id " +
            "AND(s.open_from <= @userStartMonth OR s.open_from IS NULL) " +
            "AND(s.open_to >= @userEndMonth OR s.open_to IS NULL) " +
            "AND s.max_occupancy >= @userOccupancy " +
            "AND s.id NOT IN " +
            "(SELECT s.id FROM reservation r " +
            "JOIN space s on r.space_id = s.id " +
            "WHERE s.venue_id = @venue_id " +
            "AND r.end_date >= @req_from_date " +
            "AND r.start_date <= @req_to_date)";
        private const string SqlCreateReservation = "INSERT INTO reservation (space_id, reserved_for, start_date, end_date, number_of_attendees) " +
            "VALUES(@spaceId, @reservationName, @startDate, @endDate, @userOccupancy); " +
            "SELECT @@IDENTITY AS 'id';";

        //private const string SqlSelectOpenSpaces = 

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Venue> GetAllVenues()
        {
            IList<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectAllVenues, conn);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Venue venue = new Venue();
                        venue.Id = Convert.ToInt32(reader["id"]);
                        venue.Name = Convert.ToString(reader["name"]);
                        venue.Description = Convert.ToString(reader["description"]);
                        venue.StateName = Convert.ToString(reader["stateName"]);
                        venue.CityName = Convert.ToString(reader["cityName"]);
                        venue.StateCode = Convert.ToString(reader["abbreviation"]);
                        venues.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting venues: " + ex.Message);
            }


            return venues;
        }
        public IList<string> GetCategoriesForVenues(Venue venue)
        {
            IList<string> categories = new List<string>();
            try
            {
                string currentID = Convert.ToString(venue.Id);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectGetCategories, conn);

                    command.Parameters.AddWithValue("@venueid", venue.Id);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(reader.GetString(0));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting venues: " + ex.Message);
            }
            return categories;
        }

        public IList<Space> GetSpacesForVenue(string venue_id)
        {
            IList<Space> spaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetVenueSpaces, conn);

                    command.Parameters.AddWithValue("@id", venue_id);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.WheelchairAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        space.OpenMonth = ConvertNullDate(reader, "open_from");
                        space.CloseMonth = ConvertNullDate(reader, "open_to");
                        space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
                        space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        spaces.Add(space);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Problem getting spaces: " + ex.Message);
            }
            return spaces;
        }

        private int ConvertNullDate(SqlDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return -1;
            }
            return Convert.ToInt32(reader[columnName]);
        }

        public IList<Space> GetAllAvailableSpaces(string venue_id, DateTime startDate, int numberOfDays, int occupancy)
        {
            IList<Space> spaces = new List<Space>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectUnreservedSpaces, conn);

                    int userStartMonth = startDate.Month;
                    int userEndMonth = startDate.AddDays(numberOfDays).Month;

                    command.Parameters.AddWithValue("@venue_id", venue_id);
                    command.Parameters.AddWithValue("@userStartMonth", userStartMonth);
                    command.Parameters.AddWithValue("@userEndMonth", userEndMonth);
                    command.Parameters.AddWithValue("@req_from_date", startDate);
                    command.Parameters.AddWithValue("@req_to_date", startDate.AddDays(numberOfDays));
                    command.Parameters.AddWithValue("@userOccupancy", occupancy);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
                        space.WheelchairAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        space.NumberOfDays = Convert.ToInt32(numberOfDays);
                        spaces.Add(space);
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("Problem returning available spaces:" + ex.Message);
            }
            return spaces;
        }

        public Reservation CreateAReservation (string reservationName, DateTime startDate, DateTime endDate, Space space, int occupancy)
        {
            Reservation reservation = new Reservation();
            reservation.ReserveName = reservationName;
            reservation.StartDate = startDate;
            reservation.EndDate = endDate;
            reservation.NumberOfAttendees = occupancy;

            Random rnd = new Random();
            int randomNumber = rnd.Next(1000000, 10000000);
            reservation.ConfirmationNumber = randomNumber;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateReservation, conn);

                    command.Parameters.AddWithValue("@spaceId", space.Id);
                    command.Parameters.AddWithValue("@reservationName", reservationName);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@userOccupancy", occupancy);

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    reservation.Id = Convert.ToInt32(reader["id"]);
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Problem creating reservation: " + ex.Message);
            }
            return reservation;
        }
    }
}
