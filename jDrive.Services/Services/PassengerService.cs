using jDrive.DataModel.Models;

namespace jDrive.Services.Services
{
    public class PassengerService : IPassengerService
    {
        private IRepository<Passenger> _repository;
        private JDriveDbContext _context;

        public PassengerService(JDriveDbContext context)
        {
            _context = context;
        }

        public PassengerService(IRepository<Passenger> repository)
        {
            _repository = repository;
        }

        public void AddPassenger(Passenger passenger)
        {
            _repository.Insert(passenger);
        }
    }
}