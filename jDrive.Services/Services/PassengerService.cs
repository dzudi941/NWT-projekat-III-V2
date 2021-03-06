﻿using jDrive.DomainModel.Models;
using jDrive.DomainModel;
using jDrive.Specifications.Specifications;
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

        public Passenger GetPassenger(string id)
        {
            return _repository.Find(new UserIdSpecification<Passenger>(id)).FirstOrDefault();
        }
    }
}