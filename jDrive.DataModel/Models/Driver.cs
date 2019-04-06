using System.Collections.Generic;

namespace jDrive.DomainModel.Models
{
    public class Driver : ApplicationUser
    {
        public ICollection<Ride> Rides { get; set; }
        public int? RideDiscountNumber { get; set; }
        public double? PricePerKm { get; set; }
        public double? DiscountInPercentage { get; set; }
    }

    public enum DriverStatus
    {
        NotAvailable,
        Available,
        PendingRequest,
    }
}