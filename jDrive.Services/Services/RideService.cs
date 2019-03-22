using System.Collections.Generic;
using System.Linq;
using jDrive.DataModel.Models;
using jDrive.Services.Specifications;

namespace jDrive.Services.Services
{
    public class RideService : IRideService
    {
        private IRepository<Ride> _repository;
        private JDriveDbContext _context;

        public RideService(JDriveDbContext context)
        {
            _context = context;
        }

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
                ride.DriverRating = rating;// int.Parse(rating);
            }
            else if (ride.PassengerRating == 0)
            {
                ride.PassengerRating = rating;
            }
            if (ride.PassengerRating > 0 && ride.DriverRating > 0)
                ride.RequestStatus = RequestStatus.Finished;
            //_repository.SaveChanges();
        }

        public Ride GetRide(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude)
        {
            return _repository.Find(new RideRouteSpecification(startLatitude, startLongitude, finishLatitude, finishLongitude)).FirstOrDefault();
        }

        public IEnumerable<Ride> GetRides(ApplicationUser applicationUser)
        {
            return _repository.Find(new RideUserSpecification(applicationUser));
        }

        public bool RideIsAccepted(ApplicationUser applicationUser)
        {
            return _repository.Find(new RideUserSpecification(applicationUser)).Any(x => x.RequestStatus == RequestStatus.Accepted);
        }
    }
}