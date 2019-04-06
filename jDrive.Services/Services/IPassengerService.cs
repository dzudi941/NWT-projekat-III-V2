using jDrive.DomainModel.Models;

namespace jDrive.Services.Services
{
    public interface IPassengerService
    {
        void AddPassenger(Passenger passenger);
        Passenger GetPassenger(string id);
    }
}
