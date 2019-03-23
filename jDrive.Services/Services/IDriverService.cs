using jDrive.DataModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    public interface IDriverService
    {
        //Driver FindDrivers(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude);
        void AddDriver(Driver driver);
        IEnumerable<Driver> GetDrivers();
        Driver GetDriver(string id);
        void UpdatePosition(string userId, double longitude, double latitude);
    }
}
