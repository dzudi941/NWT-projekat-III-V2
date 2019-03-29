using System.Collections.Generic;
using System.Linq;
using jDrive.DataModel.Models;
using jDrive.Services.Specifications;

namespace jDrive.Services.Services
{
    public class RideService : IRideService
    {
        private IRepository<Ride> _repository;


        public RideService(IRepository<Ride> repository)
        {
            _repository = repository;
        }

        public void AddRide(Ride ride)
        {
            _repository.Insert(ride);
        }

        public void FinishRide(int rideId, string usertype, int rating)
        {
            var ride = _repository.Find(new RideNumberSpecification(rideId)).FirstOrDefault();

            if (usertype == "driver" && ride.DriverRating == 0)
            {
                ride.DriverRating = rating;
            }
            else if (ride.PassengerRating == 0)
            {
                ride.PassengerRating = rating;
            }
            if (ride.PassengerRating > 0 && ride.DriverRating > 0)
                ride.RequestStatus = RequestStatus.Finished;

            _repository.Update(ride);
        }

        public Ride GetRide(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude)
        {
            return _repository.Find(new RideRouteSpecification(startLatitude, startLongitude, finishLatitude, finishLongitude)).FirstOrDefault();
        }

        public IEnumerable<Ride> GetRides(string userId)
        {
            return _repository.Find(new RideUserSpecification(userId)).Where(x=>x.RequestStatus == RequestStatus.Finished);
        }

        public Ride AcceptedRide(string userId)
        {
            return _repository.Find(new RideUserSpecification(userId), nameof(Driver), nameof(Passenger)).FirstOrDefault(x => x.RequestStatus == RequestStatus.Accepted);
        }

        public IEnumerable<Ride> GetRideRequests(string userId)
        {
            return _repository.Find(new RideUserSpecification(userId)).Where(x => x.RequestStatus == RequestStatus.Pending);
        }

        public void AcceptRide(int rideId)
        {
            var ride = _repository.Find(new RideNumberSpecification(rideId)).FirstOrDefault();
            ride.RequestStatus = RequestStatus.Accepted;
            _repository.Update(ride);
        }

        public int GetRideNumber(string driverId, string passengerId)
        {
            return _repository.Find(new AndSpecification<Ride>(new RideUserSpecification(driverId), new RideUserSpecification(passengerId))).Count(x => x.RequestStatus == RequestStatus.Finished);
        }

        public DriverStatus GetDriverStatus(string driverId)
        {
            IEnumerable<Ride> rides = _repository.Find(new RideUserSpecification(driverId));
            if (rides.Any(x => x.RequestStatus == RequestStatus.Pending))
                return DriverStatus.PendingRequest;
            if (rides.Any(x => x.RequestStatus == RequestStatus.Accepted))
                return DriverStatus.NotAvailable;

            return DriverStatus.Available;
        }
    }
}