using System.Collections.Generic;

namespace jDrive.DataModel.Models
{
    public class Passenger : ApplicationUser
    {
        public ICollection<Ride> Rides { get; set; }
    }
}