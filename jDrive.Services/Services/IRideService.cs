using jDrive.DomainModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    public interface IRideService
    {
        void AddRide(Ride ride);
        void AcceptRide(int rideId);
        void DeclineRide(int rideId);
        void FinishRide(int rideId, UserType usertype, int rating);

        IEnumerable<Ride> GetRides(string userId);
        IEnumerable<Ride> GetRideRequests(string userId);
        Ride PendingRequest(string userId);
        double GetAverageRating(string userId, UserType userType);
        Ride GetCurrentRide(string userId, out double totalDiscount, out int rideNumber);
        IEnumerable<(Driver, double, DriverStatus, double?)> FindDrivers(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude, double radius);
    }
}
