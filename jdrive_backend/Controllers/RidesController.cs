using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using jdrive_backend.Models;
using jDrive.DataModel.Models;
using jDrive.Services.Services;
using System.Web.Http.Cors;

namespace jdrive_backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/Ride")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RidesController : ApiController
    {
        private IRideService _rideService;
        private IDriverService _driverService;
        private IPassengerService _passengerService;

        public RidesController(IRideService rideService, IDriverService driverService, IPassengerService passengerService)
        {
            _rideService = rideService;
            _driverService = driverService;
            _passengerService = passengerService;
        }

        #region Passenger
        // GET api/Ride/FindDrivers
        [HttpGet]
        [Route("FindDrivers")]
        public IEnumerable<UserInfoViewModel> FindDrivers(string startLatitude, string startLongitude, string finishLatitude, string finishLongitude)
        {
            double startLat = double.Parse(startLatitude);
            double startlong = double.Parse(startLongitude);
            double finishLat = double.Parse(finishLatitude);
            double finishLong = double.Parse(finishLongitude);

            var nearDrivers = new List<UserInfoViewModel>();
            var drivers = _driverService.GetDrivers();
            foreach (var driver in drivers)
            {
                double xDistance = Math.Abs(driver.Latitude - startLat);
                double yDistance = Math.Abs(driver.Longitude - startlong);
                double totalDistance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
                double totalDistanceKm = totalDistance * 111;
                if (totalDistanceKm < 300) //Find drivers in 1km radius
                {
                    DriverStatus driverStatus = _rideService.GetDriverStatus(driver.Id);
                    var nearDriver = new UserInfoViewModel
                    {
                        FullName = driver.FullName,
                        Email = driver.UserName,
                        UserType = "driver",
                        Id = driver.Id,
                        Latitude = driver.Latitude,
                        Longitude = driver.Longitude,
                        TotalDistance = totalDistanceKm,
                        DriverStatus = driverStatus
                    };
                    nearDrivers.Add(nearDriver);
                }
            }

            return nearDrivers.OrderBy(x => x.TotalDistance).ToList();
        }

        //GET api/Ride/SendRequest
        [HttpPost]
        [Route("SendRequest")]
        public IHttpActionResult SendRequest(RideRequestViewModel model)
        {
            var driver = _driverService.GetDriver(model.DriverId);
            var userId = User.Identity.GetUserId();
            var user = _passengerService.GetUser(userId);
            var newRide = new Ride()
            {
                StartLatitude = double.Parse(model.StartLatitude),
                StartLongitude = double.Parse(model.StartLongitude),
                FinishLatitude = double.Parse(model.FinishLatitude),
                FinishLongitude = double.Parse(model.FinishLongitude),
                Driver = driver,
                Passenger = user
            };

            _rideService.AddRide(newRide);

            return Ok();
        }

        //GET api/Ride/CurrentRide
        [HttpGet]
        [Route("CurrentRide")]
        public RideViewModel CurrentRide()
        {
            var userId = User.Identity.GetUserId();
            var acceptedRide = _rideService.AcceptedRide(userId);
            RideViewModel rideViewModel = null;
            if (acceptedRide != null)
            {
                int rideNumber = _rideService.GetRideNumber(acceptedRide.Driver.Id, acceptedRide.Passenger.Id);
                bool match = _driverService.RideNumberMatch(acceptedRide.Driver.Id, rideNumber + 1);//+1 is for current ride.
                rideViewModel = new RideViewModel(acceptedRide, match ? $"This ride is {rideNumber + 1}th and it will have a discount!!!" : "");
            }

            return rideViewModel;
        }

        #endregion

        // GET api/Ride/FinishRide
        [HttpGet]
        [Route("FinishRide")]
        public IHttpActionResult FinishRide(int rideId, string usertype, int rating)
        {
            _rideService.FinishRide(rideId, usertype, rating);
            return Ok();
        }

        // GET api/Ride/GetAllRides
        [HttpGet]
        [Route("GetAllRides")]
        public IEnumerable<RideViewModel> GetAllRides()
        {
            var userId = User.Identity.GetUserId();
            var rides = _rideService.GetRides(userId).Select(x => new RideViewModel(x));

            return rides;
        }


        #region Driver
        // GET api/Ride/UpdatePosition
        [HttpGet]
        [Route("UpdatePosition")]
        public IHttpActionResult UpdatePosition(string longitude, string latitude)
        {
            var userId = User.Identity.GetUserId();
           _driverService.UpdatePosition(userId, double.Parse(longitude), double.Parse(latitude));

            return Ok();
        }

        // GET api/Ride/GetRideRequests
        [HttpGet]
        [Route("GetRideRequests")]
        public IEnumerable<RideViewModel> GetRideRequests()
        {
            var userId = User.Identity.GetUserId();
            var ridesRequest = _rideService.GetRideRequests(userId).Select(x => new RideViewModel(x));

            return ridesRequest;
        }

        // GET api/Ride/AcceptRide
        [HttpGet]
        [Route("AcceptRide")]
        public IHttpActionResult AcceptRide(int rideId)
        {
            _rideService.AcceptRide(rideId);
            return Ok();
        }

        #endregion
    }
}