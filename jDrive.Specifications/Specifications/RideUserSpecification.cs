using jDrive.DomainModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Specifications.Specifications
{
    public class RideUserSpecification : Specification<Ride>
    {
        private string _userId;

        public RideUserSpecification(string userId)
        {
            _userId = userId;
        }

        public override Expression<Func<Ride, bool>> ToExpression()
        {
            return ride => ride.Passenger.Id == _userId || ride.Driver.Id == _userId;
        }
    }
}