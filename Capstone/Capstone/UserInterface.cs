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

        private List<int> menuWalkBack = new List<int>();
        public void Run()
        {
            menuWalkBack = new List<int>();
            menuWalkBack.Add(1);
            bool mainMenu = true;
            while (mainMenu)
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
                    mainMenu = false;
                }
                else
                {
                    Console.WriteLine("Please Enter a Valid Choice");
                    continue;
                }
            }


        }
        public void ListVenues()
        {
            menuWalkBack.Add(2);
            bool listVenues = true;
            while (listVenues)
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
                    listVenues = false;
                    ReturnToPrevious();
                }
                int userInt = Convert.ToInt32(userInput);
                if ((userInt > 0) && (userInt < 16))
                {
                    userVenue = venues[userInt - 1];

                    DisplayTheVenueDetails(userVenue);
                    Console.ReadLine();
                }
            }
        }
        public void DisplayTheVenueDetails(Venue venue)
        {
            IList<string> categoryList = venueDAO.GetCategoriesForVenues(venue);
            Console.WriteLine(venue.Name);
            Console.WriteLine($"Location: {venue.CityName}, {venue.StateCode}");

            // ["hello", "there"] WANT "hello, there"
            // init some var str as ""
            // for each category
            //   if str is not empty ("hello") add comma
            //   str = str + category
            // done
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
            Console.ReadLine();
        }

        //venueDAO.GetCategoriesForVenues(venues[i]);

        //public void CategoriesToWriteLine(Venue venue)
        //{
        //    IList<string> categiories = venueDAO.GetCategoriesForVenues(venue);
        //}

        
        public void ReturnToPrevious()
        {
            // remove last entry in list
            int lastItem = menuWalkBack.Count - 1;
            menuWalkBack.RemoveAt(lastItem);

            //  go to last item in list
            var lastIndex = menuWalkBack.Count - 1;
            MenuSwitch(menuWalkBack[lastIndex]);
        }
        public void MenuSwitch(int menu)
        {
            // remove the current index we're using to go to previous
            if (menuWalkBack.Count > 1)
            {
                int lastIndex = menuWalkBack.Count - 1;
                menuWalkBack.RemoveAt(lastIndex);

            }
            switch (menu)
            {
                case 1:
                    Run();
                    break;
                case 2:
                    ListVenues();
                    break;
                //case 3:
                //    ThirdMenu();
                //    break;
                ////case 4:
                ////    FourthMenu();
                ////    break;
                default:
                    Run();
                    break;

            }
        }
    }
}
