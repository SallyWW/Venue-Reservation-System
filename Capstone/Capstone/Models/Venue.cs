using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Venue
    {
        public int      Id              { get; set; }
        public string   Name            { get; set; }
        public int      CityId          { get; set; }
        public string   Description     { get; set; }
        public string   StateName       { get; set; }
        public string   CityName        { get; set; }
        public string   StateCode       { get; set; }

        public List<string> Categories  { get; set; }

    }
}
