using jDrive.DataModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    public interface IRideService
    {
        void AddRide(Ride ride);
        Ride AcceptedRide(string userId);
        void FinishRide(int rideId, string usertype, int rating);
        IEnumerable<Ride> GetRides(string userId);
        Ride GetRide(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude);
        IEnumerable<Ride> GetRideRequests(string userId);
        void AcceptRide(int rideId);
    }
}
