using jDrive.DomainModel.Models;

namespace jDrive.Services.Services
{
    public interface IDriverService
    {
        void AddDriver(Driver driver);
        Driver GetDriver(string id);
        void UpdatePosition(string userId, double longitude, double latitude);
        void UpdateDriverSettings(string userId, int rideDiscountNumber, double pricePerKm, double discountInPercentage);
    }
}