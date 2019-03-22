using System.Collections.Generic;
using jDrive.DataModel.Models;

namespace jDrive.Services.Services
{
    public class DriverService : IDriverService
    {
        private IRepository<Driver> _repository;
        private JDriveDbContext _context;

        public DriverService(JDriveDbContext context)
        {
            _context = context;
        }

        public DriverService(IRepository<Driver> repository)
        {
            _repository = repository;
        }

        public void AddDriver(Driver driver)
        {
            _repository.Insert(driver);
        }

        public IEnumerable<Driver> GetDrivers()
        {
            return _repository.Table;
        }

        //public Driver FindDrivers(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude)
        //{
        //    throw new NotImplementedException();
        //}
    }
}