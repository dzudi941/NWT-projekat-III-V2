using jDrive.DataModel.Models;
using jDrive.Services.Specifications;
using System.Linq;

namespace jDrive.Services.Services
{
    public class PassengerService : IPassengerService
    {
        private IRepository<Passenger> _repository;

        public PassengerService(IRepository<Passenger> repository)
        {
            _repository = repository;
        }

        public void AddPassenger(Passenger passenger)
        {
            _repository.Insert(passenger);
        }

        public Passenger GetUser(string id)
        {
            return _repository.Find(new UserIdSpecification<Passenger>(id)).FirstOrDefault();
        }
    }
}