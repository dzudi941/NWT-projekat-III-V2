using jDrive.DataModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Services.Specifications
{
    public class RideUserSpecification : Specification<Ride>
    {
        private ApplicationUser _applicationUser;

        public RideUserSpecification(ApplicationUser applicationUser)
        {
            _applicationUser = applicationUser;
        }

        public override Expression<Func<Ride, bool>> ToExpression()
        {
            return ride => _applicationUser is Passenger ?
                    ride.Passenger.Id == _applicationUser.Id :
                    ride.Driver.Id == _applicationUser.Id;
        }
    }
}