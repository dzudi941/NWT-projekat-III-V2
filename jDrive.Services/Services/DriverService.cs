using System.Linq;
using jDrive.DomainModel.Models;
using jDrive.DomainModel;
using jDrive.Specifications.Specifications;

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
    }
}