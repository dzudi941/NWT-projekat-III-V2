using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jDrive.DataModel.Models
{
    public class Passenger : ApplicationUser
    {
        public ICollection<Ride> Rides { get; set; }
    }
}