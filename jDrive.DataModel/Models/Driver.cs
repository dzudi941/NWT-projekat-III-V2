using System.Collections.Generic;

namespace jDrive.DataModel.Models
{
    public class Driver : ApplicationUser
    {
        public ICollection<Ride> Rides { get; set; }
        public int? RideDiscountNumber { get; set; }
    }

    public enum DriverStatus
    {
        NotAvailable,
        Available,
        PendingRequest,
    }
}