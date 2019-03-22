using jDrive.DataModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Services.Specifications
{
    public class RideNumberSpecification : Specification<Ride>
    {
        private readonly int _rideId;

        public RideNumberSpecification(int id)
        {
            _rideId = id;
        }

        public override Expression<Func<Ride, bool>> ToExpression()
        {
            return ride => ride.Id == _rideId;
        }
    }
}