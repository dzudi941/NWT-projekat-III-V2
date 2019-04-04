using jDrive.DataModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    public interface IRideService
    {
        void AddRide(Ride ride);
        void AcceptRide(int rideId);
        void DeclineRide(int rideId);
        void FinishRide(int rideId, UserType usertype, int rating);

        Ride AcceptedRide(string userId);
        IEnumerable<Ride> GetRides(string userId);
        Ride GetRide(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude);
        IEnumerable<Ride> GetRideRequests(string userId);
        int GetRideNumber(string driverId, string passengerId);
        DriverStatus GetDriverStatus(string driverId);
        Ride PendingRequest(string userId);
        double GetAverageRating(string userId, UserType userType);
    }
}
