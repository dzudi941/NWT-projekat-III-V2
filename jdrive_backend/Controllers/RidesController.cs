using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using jdrive_backend.Models;
using Microsoft.AspNet.Identity.Owin;
using jDrive.DataModel.Models;

namespace jdrive_backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/Ride")]
    public class RidesController : ApiController
    {
        private JDriveDbContext db = new JDriveDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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

            //decimal distance = Math.Abs(startLat - finishLat)
            var nearDrivers = new List<UserInfoViewModel>();
            var drivers = db.Drivers.ToList();
            foreach (var driver in drivers)
            {
                double xDistance = Math.Abs(driver.Latitude - startLat);
                double yDistance = Math.Abs(driver.Longitude - startlong);
                double totalDistance = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
                double totalDistanceKm = totalDistance * 111;
                if (totalDistanceKm < 500) //Find drivers in 1km radius
                {
                    var ride = db.Rides.FirstOrDefault(x => x.StartLatitude == startLat &&
                    x.StartLongitude == startlong &&
                    x.FinishLatitude == finishLat &&
                    x.FinishLongitude == finishLong);

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
            var driver = db.Drivers.FirstOrDefault(x => x.Id == model.DriverId); //await UserManager.FindByIdAsync(model.DriverId);
            var userId = User.Identity.GetUserId();
            var user = db.Passengers.FirstOrDefault(x => x.Id == userId);//UserManager.FindById(userId);
            var newRide = new Ride()
            {
                StartLatitude = double.Parse(model.StartLatitude),
                StartLongitude = double.Parse(model.StartLongitude),
                FinishLatitude = double.Parse(model.FinishLatitude),
                FinishLongitude = double.Parse(model.FinishLongitude),
                Driver = driver,
                Passenger = user
            };

            db.Rides.Add(newRide);
            await db.SaveChangesAsync();

            return Ok();
        }

        //GET api/Ride/CheckIfRideIsAccepted
        [HttpGet]
        [Route("CheckIfRideIsAccepted")]
        public RideViewModel CheckIfRideIsAccepted()
        {
            var userId = User.Identity.GetUserId();
            //var user = db.Users.FirstOrDefault(x => x.Id == userId) as Passenger;
            var acceptedRide = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).FirstOrDefault(x => x.Passenger.Id == userId && x.RequestStatus == RequestStatus.Accepted /*|| x.RequestStatus == RequestStatus.MarkedForFinish)*/);

            return acceptedRide != null ? new RideViewModel(acceptedRide) : null;
        }

        #endregion

        // GET api/Ride/FinishRide
        [HttpGet]
        [Route("FinishRide")]
        public IHttpActionResult FinishRide(int rideId, string usertype, int rating)
        {
            var ride = db.Rides.FirstOrDefault(x => x.Id == rideId);
            if (ride == null) return BadRequest();

            //if (ride.RequestStatus == RequestStatus.Accepted)
            //    ride.RequestStatus = RequestStatus.MarkedForFinish;
            //else if (ride.RequestStatus == RequestStatus.MarkedForFinish)
            //    ride.RequestStatus = RequestStatus.Finished;
            if (usertype == "driver" && ride.DriverRating == 0)
            {
                ride.DriverRating = rating;// int.Parse(rating);
            }
            else if(ride.PassengerRating == 0)
            {
                ride.PassengerRating = rating;
            }
            if(ride.PassengerRating > 0 && ride.DriverRating > 0)
                ride.RequestStatus = RequestStatus.Finished;

            db.SaveChanges();

            return Ok();
        }

        // GET api/Ride/GetAllRides
        [HttpGet]
        [Route("GetAllRides")]
        public IEnumerable<RideViewModel> GetAllRides()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(x => x.Id == userId);
            var rides = new List<RideViewModel>();
            if (user is Passenger)
            {
                rides = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).Where(x => x.Passenger.Id == userId).ToList().Select(x=> new RideViewModel(x)).ToList();
            }
            else
            {
                rides = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).Where(x => x.Driver.Id == userId).ToList().Select(x => new RideViewModel(x)).ToList();
            }

            return rides;
        }


        #region Driver
        // GET api/Ride/UpdatePosition
        [HttpGet]
        [Route("UpdatePosition")]
        public IHttpActionResult UpdatePosition(string longitude, string latitude)
        {
            var userId = User.Identity.GetUserId();
            //var user = db.Drivers.FirstOrDefault(x => x.Id == userId);
            var user = UserManager.FindById(userId);
            user.Longitude = double.Parse(longitude);
            user.Latitude = double.Parse(latitude);
            UserManager.Update(user);
            //db.SaveChanges();

            return Ok();
        }

        // GET api/Ride/GetRideRequests
        [HttpGet]
        [Route("GetRideRequests")]
        public IEnumerable<RideViewModel> GetRideRequests()
        {
            var userId = User.Identity.GetUserId();
            var ridesRequest = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).Where(x => x.Driver.Id == userId && x.RequestStatus == RequestStatus.Pending).ToList().Select(x => new RideViewModel(x));

            return ridesRequest;
        }

        // GET api/Ride/AcceptRide
        [HttpGet]
        [Route("AcceptRide")]
        public IHttpActionResult AcceptRide(int rideId)
        {
            var ride = db.Rides.FirstOrDefault(x => x.Id == rideId);
            if (ride == null) return BadRequest();

            ride.RequestStatus = RequestStatus.Accepted;
            db.SaveChanges();

            return Ok();
        }

        //GET api/Ride/CurrentRide
        [HttpGet]
        [Route("CurrentRide")]
        public RideViewModel CurrentRide()
        {
            var userId = User.Identity.GetUserId();
            var currentRide = db.Rides.Include(x => x.Driver).Include(x => x.Passenger).FirstOrDefault(x => x.Driver.Id == userId && x.RequestStatus == RequestStatus.Accepted /*|| x.RequestStatus == RequestStatus.MarkedForFinish)*/);

            return currentRide != null ? new RideViewModel(currentRide) : null;
        }

        #endregion



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}