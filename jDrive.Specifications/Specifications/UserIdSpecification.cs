using jDrive.DomainModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Specifications.Specifications
{
    public class UserIdSpecification<T> : Specification<T>
    {
        private string _driverId;

        public UserIdSpecification(string id)
        {
            _driverId = id;
        }


        public override Expression<Func<T, bool>> ToExpression()
        {
            return driver => (driver as ApplicationUser).Id == _driverId;
        }
    }
}