using jDrive.DataModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Services.Specifications
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
            //return ride => _applicationUser is Passenger ?
            //        ride.Passenger.Id == _applicationUser.Id :
            //        ride.Driver.Id == _applicationUser.Id;
            return ride => ride.Passenger.Id == _userId || ride.Driver.Id == _userId;
        }
    }
}