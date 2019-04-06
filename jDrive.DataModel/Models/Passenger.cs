using System.Collections.Generic;

namespace jDrive.DomainModel.Models
{
    public class Passenger : ApplicationUser
    {
        public ICollection<Ride> Rides { get; set; }
    }
}