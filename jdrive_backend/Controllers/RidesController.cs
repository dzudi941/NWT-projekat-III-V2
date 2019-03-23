using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using jdrive_backend.Models;
using Microsoft.AspNet.Identity.Owin;
using jDrive.DataModel.Models;
using jDrive.Services.Services;
using System.Web.Http.Cors;

namespace jdrive_backend.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Ride")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RidesController : ApiController
    {
        //private JDriveDbContext db = new JDriveDbContext();
        private IRideService _rideService;
        private IDriverService _driverService;
        private IPassengerService _passengerService;

        public RidesController(IRideService rideService, IDriverService driverService, IPassengerService passengerService)
        {
            _rideService = rideService;
            _driverService = driverService;
            _passengerService = passengerService;
        }

        //private ApplicationUserManager _userManager;
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        [HttpGet]
        [Route("test")]
        public UserInfoViewModel Get()
        {
            var nearDrivers = new List<UserInfoViewModel>();
            var drivers = _driverService.GetDrivers();
            //foreach (var driver in drivers)
            //{
            //    var nearDriver = new UserInfoViewModel
            //    {
            //        FullName = driver.FullName,
            //        Email = driver.UserName,
            //        UserType = "driver",
            //        Id = driver.Id,
            //        Latitude = driver.Latitude,
            //        Longitude = driver.Longitude,
            //        TotalDistance = 123,
            //        RequestStatus = RequestStatus.Accepted
            //    };
            //    nearDrivers.Add(nearDriver);
            //}
            //var ride = _rideService.GetRide(2, 2, 2, 2);

            //var userId = User.Identity.GetUserId();
            var driver = _passengerService.GetUser("8a39f106-61bf-4b57-b135-967b9d043d87");
            var nearDriver = new UserInfoViewModel
            {
                FullName = driver.FullName,
                Email = driver.UserName,
                UserType = "driver",
                Id = driver.Id,
                Latitude = driver.Latitude,
                Longitude = driver.Longitude,
                TotalDistance = 123,
                RequestStatus = RequestStatus.Accepted
            };

            return nearDriver;
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
                if (totalDistanceKm < 500) //Find drivers in 1km radius
                {
                    var ride = _rideService.GetRide(startLat, startlong, finishLat, finishLong);

                    RequestStatus requestStatus = ride != null ? ride.RequestStatus : RequestStatus.NotSent;
                    var nearDriver = new UserInfoViewModel
                    {
                        FullName = driver.FullName,
                        Email = driver.UserName,
                        UserType = "driver",
                        Id = driver.Id,
                        Latitude = driver.Latitude,
                        Longitude = driver.Longitude,
                        TotalDistance = totalDistanceKm,
                        RequestStatus = requestStatus
                    };
                    nearDrivers.Add(nearDriver);
                }
            }

            return nearDrivers.OrderBy(x => x.TotalDistance).ToList();
        }

        //GET api/Ride/SendRequest
        [HttpPost]
        [Route("SendRequest")]
        public async Task<IHttpActionResult> SendRequest(RideRequestViewModel model)
        {
            var driver = _driverService.GetDriver(model.DriverId);// db.Drivers.FirstOrDefault(x => x.Id == model.DriverId); //await UserManager.FindByIdAsync(model.DriverId);
            var userId = User.Identity.GetUserId();
            var user = _passengerService.GetUser(userId); //db.Passengers.FirstOrDefault(x => x.Id == userId);//UserManager.FindById(userId);
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
            //db.Rides.Add(newRide);
            //await db.SaveChangesAsync();

            return Ok();
        }

        //GET api/Ride/CheckIfRideIsAccepted
        [HttpGet]
        [Route("CheckIfRideIsAccepted")]
        public RideViewModel CheckIfRideIsAccepted()
        {
            var userId = User.Identity.GetUserId();
            //var user = _passengerService.GetUser(userId);
            //var user = db.Users.FirstOrDefault(x => x.Id == userId) as Passenger;
            var acceptedRide = _rideService.AcceptedRide(userId); //db.Rides.Include(x => x.Driver).Include(x => x.Passenger).FirstOrDefault(x => x.Passenger.Id == userId && x.RequestStatus == RequestStatus.Accepted /*|| x.RequestStatus == RequestStatus.MarkedForFinish)*/);

            return acceptedRide != null ? new RideViewModel(acceptedRide) : null;
        }

        #endregion

        // GET api/Ride/FinishRide
        [HttpGet]
        [Route("FinishRide")]
        public IHttpActionResult FinishRide(int rideId, string usertype, int rating)
        {
            /*var ride = */_rideService.FinishRide(rideId, usertype, rating); //db.Rides.FirstOrDefault(x => x.Id == rideId);
            //if (ride == null) return BadRequest();

            ////if (ride.RequestStatus == RequestStatus.Accepted)
            ////    ride.RequestStatus = RequestStatus.MarkedForFinish;
            ////else if (ride.RequestStatus == RequestStatus.MarkedForFinish)
            ////    ride.RequestStatus = RequestStatus.Finished;
            //if (usertype == "driver" && ride.DriverRating == 0)
            //{
            //    ride.DriverRating = rating;// int.Parse(rating);
            //}
            //else if(ride.PassengerRating == 0)
            //{
            //    ride.PassengerRating = rating;
            //}
            //if(ride.PassengerRating > 0 && ride.DriverRating > 0)
            //    ride.RequestStatus = RequestStatus.Finished;

            //db.SaveChanges();

            return Ok();
        }

        // GET api/Ride/GetAllRides
        [HttpGet]
        [Route("GetAllRides")]
        public IEnumerable<RideViewModel> GetAllRides()
        {
            var userId = /*"8a39f106-61bf-4b57-b135-967b9d043d87"; //*/User.Identity.GetUserId();
            //ApplicationUser user = _driverService.GetDriver(userId);
            //user  = user ?? _passengerService.GetUser(userId);
            var rides = _rideService.GetRides(userId).Select(x => new RideViewModel(x));
            
            //var user = db.Users.FirstOrDefault(x => x.Id == userId);
            //var rides = new List<RideViewModel>();
            //if (user is Passenger)
            //{
            //    rides = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).Where(x => x.Passenger.Id == userId).ToList().Select(x=> new RideViewModel(x)).ToList();
            //}
            //else
            //{
            //    rides = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).Where(x => x.Driver.Id == userId).ToList().Select(x => new RideViewModel(x)).ToList();
            //}

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

            //var user = db.Drivers.FirstOrDefault(x => x.Id == userId);
            //var user = UserManager.FindById(userId);
            //user.Longitude = double.Parse(longitude);
            //user.Latitude = double.Parse(latitude);
            //UserManager.Update(user);
            //db.SaveChanges();

            return Ok();
        }

        // GET api/Ride/GetRideRequests
        [HttpGet]
        [Route("GetRideRequests")]
        public IEnumerable<RideViewModel> GetRideRequests()
        {
            var userId = User.Identity.GetUserId();
            //ApplicationUser user = _driverService.GetDriver(userId);
            //user = user ?? _passengerService.GetUser(userId);

            var ridesRequest = _rideService.GetRideRequests(userId).Select(x => new RideViewModel(x)); //.db.Rides.Include(x => x.Driver).Include(x => x.Passenger).Where(x => x.Driver.Id == userId && x.RequestStatus == RequestStatus.Pending).ToList().Select(x => new RideViewModel(x));

            return ridesRequest;
        }

        // GET api/Ride/AcceptRide
        [HttpGet]
        [Route("AcceptRide")]
        public IHttpActionResult AcceptRide(int rideId)
        {
            _rideService.AcceptRide(rideId);

            //var ride = db.Rides.FirstOrDefault(x => x.Id == rideId);
            //if (ride == null) return BadRequest();

            //ride.RequestStatus = RequestStatus.Accepted;
            //db.SaveChanges();

            return Ok();
        }

        //GET api/Ride/CurrentRide
        [HttpGet]
        [Route("CurrentRide")]
        public RideViewModel CurrentRide()
        {
            var userId = User.Identity.GetUserId();
            //ApplicationUser user = _driverService.GetDriver(userId);
            //user = user ?? _passengerService.GetUser(userId);

            var currentRide = _rideService.AcceptedRide(userId); //db.Rides.Include(x => x.Driver).Include(x => x.Passenger).FirstOrDefault(x => x.Driver.Id == userId && x.RequestStatus == RequestStatus.Accepted /*|| x.RequestStatus == RequestStatus.MarkedForFinish)*/);

            return currentRide != null ? new RideViewModel(currentRide) : null;
        }

        #endregion



        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}