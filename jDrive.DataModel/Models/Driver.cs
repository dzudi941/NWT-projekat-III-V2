using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jDrive.DataModel.Models
{
    public class Driver : ApplicationUser
    {
        public ICollection<Ride> Rides { get; set; }
        public int? RideDiscountNumber { get; set; }
    }
}