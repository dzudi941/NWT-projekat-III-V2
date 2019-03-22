using jDrive.DataModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    interface IRideService
    {
        void AddRide(Ride ride);
        bool RideIsAccepted(ApplicationUser applicationUser);
        void FinishRide(int rideId, string usertype, int rating);
        IEnumerable<Ride> GetRides(ApplicationUser applicationUser);
        Ride GetRide(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude);
    }
}
