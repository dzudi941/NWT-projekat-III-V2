using System;
using System.Collections.Generic;
using System.Linq;
using jDrive.DomainModel.Models;
using jDrive.DomainModel;
using jDrive.Specifications.Specifications;

namespace jDrive.Services.Services
{
    public class RideService : IRideService
    {
        private IRepository<Ride> _rideRepository;
        private IRepository<Driver> _driverRepository;


        public RideService(IRepository<Ride> rideRepository, IRepository<Driver> driverRepository)
        {
            _rideRepository = rideRepository;
            _driverRepository = driverRepository;
        }

        public void AddRide(Ride ride)
        {
            _rideRepository.Insert(ride);
        }

        public void FinishRide(int rideId, UserType usertype, int rating)
        {
            var ride = _rideRepository.Find(new RideNumberSpecification(rideId)).FirstOrDefault();

            if (usertype == UserType.Driver && ride.DriverRating == 0)
            {
                ride.DriverRating = rating;
            }
            else if (ride.PassengerRating == 0)
            {
                ride.PassengerRating = rating;
            }
            if (ride.PassengerRating > 0 && ride.DriverRating > 0)
                ride.RequestStatus = RequestStatus.Finished;

            _rideRepository.Update(ride);
        }

        public IEnumerable<Ride> GetRides(string userId)
        {
            return _rideRepository.Find(new RideUserSpecification(userId)).Where(x=>x.RequestStatus == RequestStatus.Finished);
        }

        public Ride AcceptedRide(string userId)
        {
            return _rideRepository.Find(new RideUserSpecification(userId), nameof(Driver), nameof(Passenger)).FirstOrDefault(x => x.RequestStatus == RequestStatus.Accepted);
        }

        public Ride PendingRequest(string userId)
        {
            return _rideRepository.Find(new RideUserSpecification(userId), nameof(Driver), nameof(Passenger)).FirstOrDefault(x => x.RequestStatus == RequestStatus.Pending);
        }

        public IEnumerable<Ride> GetRideRequests(string userId)
        {
            return _rideRepository.Find(new RideUserSpecification(userId)).Where(x => x.RequestStatus == RequestStatus.Pending);
        }

        public void AcceptRide(int rideId)
        {
            var ride = _rideRepository.Find(new RideNumberSpecification(rideId)).FirstOrDefault();
            ride.RequestStatus = RequestStatus.Accepted;
            _rideRepository.Update(ride);
        }

        public void DeclineRide(int rideId)
        {
            var ride = _rideRepository.Find(new RideNumberSpecification(rideId)).FirstOrDefault();
            ride.RequestStatus = RequestStatus.Rejected;
            _rideRepository.Update(ride);
        }

        private double TotalDiscount(Ride ride, out int rideNumber)
        {
            var ridesCount = _rideRepository.Find(new RideUserSpecification(ride.Driver.Id).And(new RideUserSpecification(ride.Passenger.Id))).Count(x => x.RequestStatus == RequestStatus.Finished);
            var driver = _driverRepository.Find(new UserIdSpecification<Driver>(ride.Driver.Id)).FirstOrDefault();
            double totalDiscount = 0;
            if (ridesCount % driver.RideDiscountNumber == 0)
            {
                totalDiscount = (driver.DiscountInPercentage.Value / 100) * ride.EstimatedPrice;
            }
            rideNumber = ridesCount + 1;
            return totalDiscount;
        }

        public Ride GetCurrentRide(string userId, out double totalDiscount, out int rideNumber)
        {
            Ride acceptedRide = _rideRepository.Find(new RideUserSpecification(userId), nameof(Driver), nameof(Passenger)).FirstOrDefault(x => x.RequestStatus == RequestStatus.Accepted);
            if (acceptedRide != null)
            {
                totalDiscount = TotalDiscount(acceptedRide, out rideNumber);
                return acceptedRide;
            }
            totalDiscount = 0;
            rideNumber = 0;
            return null;
        }

        public double GetAverageRating(string userId, UserType userType)
        {
            var rides = GetRides(userId);

            return rides.Count() > 0 ? rides.Average(x => userType == UserType.Driver ? x.DriverRating : x.PassengerRating) : 0;
        }

        public IEnumerable<(Driver, double, DriverStatus, double?)> FindDrivers(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude, double radius)
        {
            var nearDrivers = new List<(Driver, double, DriverStatus, double?)>();
            var drivers = _driverRepository.Table;
            foreach (var driver in drivers)
            {
                var totalDistanceKm = GetTotalDistanceInKm(driver.Latitude, driver.Longitude, startLatitude, startLongitude);
                if (totalDistanceKm < radius)
                {
                    DriverStatus driverStatus = GetDriverStatus(driver.Id);
                    var priceForRoute = GetTotalDistanceInKm(startLatitude, startLongitude, finishLatitude, finishLongitude) * driver.PricePerKm;

                    nearDrivers.Add((driver, totalDistanceKm, driverStatus, priceForRoute));
                }
            }

            return nearDrivers.OrderBy(x => x.Item2).ToList();
        }


        private double GetTotalDistanceInKm(double aLatitude, double aLongitude, double bLatitude, double bLongitude)
        {
            double xDistance = Math.Abs(aLatitude - bLatitude);
            double yDistance = Math.Abs(aLongitude - bLongitude);
            double totalDistance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
            return totalDistance * 111;
        }

        private DriverStatus GetDriverStatus(string driverId)
        {
            IEnumerable<Ride> rides = _rideRepository.Find(new RideUserSpecification(driverId));
            if (rides.Any(x => x.RequestStatus == RequestStatus.Pending))
                return DriverStatus.PendingRequest;
            if (rides.Any(x => x.RequestStatus == RequestStatus.Accepted))
                return DriverStatus.NotAvailable;

            return DriverStatus.Available;
        }
    }
}