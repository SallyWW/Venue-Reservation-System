using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class is responsible for representing the main user interface to the user.
    /// </summary>
    /// <remarks>
    /// ALL Console.ReadLine and WriteLine in this class
    /// NONE in any other class. 
    ///  
    /// The only exceptions to this are:
    /// 1. Error handling in catch blocks
    /// 2. Input helper methods in the CLIHelper.cs file
    /// 3. Things your instructor explicitly says are fine
    /// 
    /// No database calls should exist in classes outside of DAO objects
    /// </remarks>
    public class UserInterface
    {
        private readonly string connectionString;

        private readonly VenueDAO venueDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
        }

        //private VenueDAO venuedao = new VenueDAO(connectionString);

        //private List<int> menuWalkBack = new List<int>();
        public void Run()
        {
            //menuWalkBack = new List<int>();
            //menuWalkBack.Add(1);
            //bool mainMenu = true;
            while (true)
            {

                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) List Venues");
                Console.WriteLine("2) Quit");
                string userInput = Console.ReadLine().ToString();
                if (userInput == "1")
                {
                    ListVenues();
                }
                else if (userInput == "2")
                {
                    //mainMenu = false;
                    return;
                }
                else
                {
                    Console.WriteLine("Please Enter a Valid Choice");
                    //continue;
                }
            }


        }
        public void ListVenues()
        {
            //menuWalkBack.Add(2);
            //bool listVenues = true;
            while (true)
            {
                try
                {
                    IList<Venue> venues = venueDAO.GetAllVenues();
                    Console.WriteLine("Which venue would you like to view?");
                    Venue userVenue;
                    for (int i = 0; i < venues.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}) {venues[i].Name}");
                    }
                    Console.WriteLine($"R) Return to Previous Screen");
                    string userInput = Console.ReadLine().ToUpper();
                    if (userInput == "R")
                    {
                        //listVenues = false;
                        //ReturnToPrevious();
                        return;
                    }
                    int userInt = Convert.ToInt32(userInput);
                    if ((userInt > 0) && (userInt < 16))
                    {
                        userVenue = venues[userInt - 1];

                        DisplayTheVenueDetails(userVenue);
                        return;
                    }
                }
                catch(FormatException ex)
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }
        public void DisplayTheVenueDetails(Venue venue)
        {
            while (true)
            {
                IList<string> categoryList = venueDAO.GetCategoriesForVenues(venue);
                Console.WriteLine();
                Console.WriteLine(venue.Name);
                Console.WriteLine($"Location: {venue.CityName}, {venue.StateCode}");

                string catList = "";
                foreach (string category in categoryList)
                {
                    if (catList.Length != 0)
                    {
                        catList += ", ";
                    }
                    catList += category;
                }
                Console.WriteLine($"Categories: {catList}");
                Console.WriteLine();
                Console.WriteLine(venue.Description);
                Console.WriteLine();
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) View Spaces");
                Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("R) Return to Previous Screen");
                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "1":
                        ViewVenueSpaces(venue);
                        break;
                    case "2":
                        bool success = GetReservationDetails(venue);
                        if (success)
                        {
                            return;
                        }
                        break;
                    case "R":
                        return;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid input.");
                        break;

                }
            }
        }

        public void ViewVenueSpaces(Venue venue)
        {
            while (true)
            {
                IList<Space> spaces = venueDAO.GetSpacesForVenue(Convert.ToString(venue.Id));
                Console.WriteLine(venue.Name);
                Console.WriteLine();
                Console.WriteLine($"     Name                               Open   Close   Daily Rate   Max. Occupancy");

                Space userSpace;
                for (int i = 0; i < spaces.Count; i++)
                {
                    Space space = spaces[i];
                    string number = $"#{i + 1}".PadRight(5);
                    string name = space.Name.PadRight(35);
                    string open = Space.Month(space.OpenMonth).PadRight(7);
                    string close = Space.Month(space.CloseMonth).PadRight(8);
                    string rate = space.DailyRate.ToString("C").PadRight(13);
                    string maxOccupancy = space.MaxOccupancy.ToString();
                    Console.WriteLine(number + name + open + close + rate + maxOccupancy);
                    
                }
                Console.WriteLine();
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) Reserve a Space");
                Console.WriteLine("R) Return to Previous Screen");

                string userInput = Console.ReadLine().ToUpper();

                switch (userInput)
                {
                    case "1":
                        GetReservationDetails(venue);
                        break;
                    case "R":
                        return;
                }
            }
        }

        public bool GetReservationDetails (Venue venue)
        {
            while (true)
            {
                Console.WriteLine("When do you need the space? (MM/DD/YYYY)");
                string userDate = Console.ReadLine();
                DateTime date = Convert.ToDateTime(userDate);

                Console.WriteLine("How many days will you need the space?");
                string userDays = Console.ReadLine();
                int days = Convert.ToInt32(userDays);

                Console.WriteLine("How many people will be in attendance?");
                string userOccupancy = Console.ReadLine();
                int occupancy = Convert.ToInt32(userOccupancy);

                IList<Space> spaces = venueDAO.GetAllAvailableSpaces(venue.Id.ToString(), date, days, occupancy);
                if (spaces.Count <=0)
                {
                    Console.WriteLine("No spaces available. Would you like to try a different search? (Y / N)");
                    string userInput = Console.ReadLine().ToUpper();
                    if (userInput == "Y")
                    {
                        continue;
                    }
                    else if (userInput == "N")
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                        return true;
                    }
                }
                
                Console.WriteLine();
                Console.WriteLine("The following spaces are available based on your needs:");

                Console.WriteLine();
                Console.WriteLine($"Space #   Name                               Daily Rate   Max Occup.   Accessible?   Total Cost");
                for (int i = 0; i < spaces.Count; i++)
                {
                    Space space = spaces[i];
                    string number = $"{space.Id}".PadRight(10);
                    string name = space.Name.PadRight(35);
                    string rate = space.DailyRate.ToString("C").PadRight(13);
                    string maxOccupancy = space.MaxOccupancy.ToString().PadRight(13);
                    string accessible = Space.Accessible(space.WheelchairAccessible).PadRight(14);
                    string totalCost = space.TotalCost.ToString("C");
                    Console.WriteLine(number + name + rate + maxOccupancy + accessible + totalCost);
                }
                Console.WriteLine();

                Console.WriteLine("Which space would you like to reserve (enter 0 to cancel)?");
                int spaceNumberToReserve = Convert.ToInt32(Console.ReadLine());

                Space spaceToReserve = new Space();
                foreach(Space space in spaces)
                {
                    if (space.Id == spaceNumberToReserve)
                    {
                        spaceToReserve = space;
                        break;
                    }
                }

                Console.WriteLine("Who is this reservation for?");
                string reservationName = Console.ReadLine();
                Console.WriteLine();

                Reservation reservation = venueDAO.CreateAReservation(reservationName, date, date.AddDays(days), spaceToReserve, occupancy);

                Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below:");
                Console.WriteLine();

                Console.WriteLine($"Confirmation #: {reservation.ConfirmationNumber}");
                Console.WriteLine($"Venue: {venue.Name}");
                Console.WriteLine($"Space: {spaceToReserve.Name}");
                Console.WriteLine($"Reserved for: {reservation.ReserveName}");
                Console.WriteLine($"Attendees: {reservation.NumberOfAttendees}");
                Console.WriteLine($"Arrival Date: {reservation.StartDate}");
                Console.WriteLine($"Depart Date: {reservation.EndDate.Date}");
                Console.WriteLine($"Total Cost: {spaceToReserve.TotalCost:C}");

                Console.WriteLine();
                Console.WriteLine("Please press any key to return to the main menu.");
                Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                return false;
            }
        }
    }
}
