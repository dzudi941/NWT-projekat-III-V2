using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;
using jdrive_backend.Models;
using jDrive.DomainModel.Models;
using jDrive.Services.Services;

namespace jdrive_backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/Ride")]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
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


        // GET api/Ride/GetAllRides
        [HttpGet]
        [Route("GetAllRides")]
        public IEnumerable<RideViewModel> GetAllRides()
        {
            var userId = User.Identity.GetUserId();
            var rides = _rideService.GetRides(userId).Select(x => new RideViewModel(x));

            return rides;
        }

        //GET api/Ride/CurrentRide
        [HttpGet]
        [Route("CurrentRide")]
        public RideViewModel CurrentRide()
        {
            var userId = User.Identity.GetUserId();
            double totalDiscount = 0;
            int rideNumber = 0;
            var currentRide = _rideService.GetCurrentRide(userId, out totalDiscount, out rideNumber);
            RideViewModel rideViewModel = null;
            if (currentRide != null)
            {
                rideViewModel = new RideViewModel(currentRide, totalDiscount, rideNumber);
            }

            return rideViewModel;
        }

        // GET api/Ride/FinishRide
        [HttpGet]
        [Route("FinishRide")]
        public IHttpActionResult FinishRide(int rideId, UserType userType, int rating)
        {
            _rideService.FinishRide(rideId, userType, rating);
            return Ok();
        }

        // GET api/Ride/DeclineRide
        [HttpGet]
        [Route("DeclineRide")]
        public IHttpActionResult DeclineRide(int rideId)
        {
            _rideService.DeclineRide(rideId);
            return Ok();
        }

        #region Passenger
        // GET api/Ride/FindDrivers
        [HttpGet]
        [Route("FindDrivers")]
        public IEnumerable<UserInfoViewModel> FindDrivers(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude, double radius)
        {
            var nearDrivers = _rideService.FindDrivers(startLatitude, startLongitude, finishLatitude, finishLongitude, radius);

            return nearDrivers.Select(x=> new UserInfoViewModel(x.Item1) {
                TotalDistance = x.Item2,
                DriverStatus = x.Item3,
                PriceForRoute = x.Item4
            });
        }

        //GET api/Ride/SendRequest
        [HttpPost]
        [Route("SendRequest")]
        public IHttpActionResult SendRequest(RideRequestViewModel model)
        {
            var driver = _driverService.GetDriver(model.DriverId);
            var userId = User.Identity.GetUserId();
            var passenger = _passengerService.GetPassenger(userId);
            var newRide = new Ride()
            {
                StartLatitude = model.StartLatitude,
                StartLongitude = model.StartLongitude,
                FinishLatitude = model.FinishLatitude,
                FinishLongitude = model.FinishLongitude,
                EstimatedPrice = model.EstimatedPrice,
                Driver = driver,
                Passenger = passenger
            };

            _rideService.AddRide(newRide);

            return Ok();
        }

        //GET api/Ride/PendingRideRequest
        [HttpGet]
        [Route("PendingRideRequest")]
        public RideViewModel PendingRideRequest()
        {
            var userId = User.Identity.GetUserId();
            var pendingRide = _rideService.PendingRequest(userId);

            return pendingRide != null ? new RideViewModel(pendingRide) : null;
        }

        #endregion

        #region Driver
        // GET api/Ride/UpdatePosition
        [HttpGet]
        [Route("UpdatePosition")]
        public IHttpActionResult UpdatePosition(double longitude, double latitude)
        {
            var userId = User.Identity.GetUserId();
           _driverService.UpdatePosition(userId, longitude, latitude);

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