using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool WheelchairAccessible { get; set; }
        public int OpenMonth { get; set; }
        public int CloseMonth { get; set; }
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
        public int NumberOfDays { get; set; }

        public decimal TotalCost
        {
            get
            {
                return DailyRate * NumberOfDays;
            }
        }

        public static string Month(int monthIndex)
        {
            switch (monthIndex)
            {
                case 1:
                    return "Jan.";
                case 2:
                    return "Feb.";
                case 3:
                    return "Mar.";
                case 4:
                    return "Apr.";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "Aug.";
                case 9:
                    return "Sept.";
                case 10:
                    return "Oct.";
                case 11:
                    return "Nov.";
                case 12:
                    return "Dec.";
                default: return "";
            }
        }
        public static string Accessible(bool isAccessible)
        {
            if (isAccessible)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
                
        }
    }
}
