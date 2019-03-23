using jDrive.DataModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    public interface IDriverService
    {
        void AddDriver(Driver driver);
        IEnumerable<Driver> GetDrivers();
        Driver GetDriver(string id);
        void UpdatePosition(string userId, double longitude, double latitude);
    }
}
