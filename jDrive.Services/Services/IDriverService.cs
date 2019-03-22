using jDrive.DataModel.Models;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    interface IDriverService
    {
        //Driver FindDrivers(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude);
        void AddDriver(Driver driver);
        IEnumerable<Driver> GetDrivers();
    }
}
