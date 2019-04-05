using System.Collections.Generic;
using System.Linq;
using jDrive.DataModel.Models;
using jDrive.Services.Specifications;

namespace jDrive.Services.Services
{
    public class DriverService : IDriverService
    {
        private IRepository<Driver> _repository;

        public DriverService(IRepository<Driver> repository)
        {
            _repository = repository;
        }

        public void AddDriver(Driver driver)
        {
            _repository.Insert(driver);
        }

        public Driver GetDriver(string id)
        {
            return _repository.Find(new UserIdSpecification<Driver>(id)).FirstOrDefault();
        }

        public IEnumerable<Driver> GetDrivers()
        {
            return _repository.Table;
        }

        public bool RideNumberMatch(string userId, int rideNumber)
        {
            var driver = _repository.Find(new UserIdSpecification<Driver>(userId)).FirstOrDefault();
            return rideNumber % driver.RideDiscountNumber == 0;
        }

        public void UpdateDriverSettings(string userId, int rideDiscountNumber, double pricePerKm, double discountInPercentage)
        {
            var driver = _repository.Find(new UserIdSpecification<Driver>(userId)).FirstOrDefault();
            driver.RideDiscountNumber = rideDiscountNumber;
            driver.PricePerKm = pricePerKm;
            driver.DiscountInPercentage = discountInPercentage;
            _repository.Update(driver);
        }

        public void UpdatePosition(string userId, double longitude, double latitude)
        {
            var driver = _repository.Find(new UserIdSpecification<Driver>(userId)).FirstOrDefault();
            driver.Longitude = longitude;
            driver.Latitude = latitude;
            _repository.Update(driver);
        }

        //public void UpdateRideDiscountNumber(string userId, int rideDiscountNumber)
        //{
        //    var driver = _repository.Find(new UserIdSpecification<Driver>(userId)).FirstOrDefault();
        //    driver.RideDiscountNumber = rideDiscountNumber;
        //    _repository.Update(driver);
        //}
    }
}