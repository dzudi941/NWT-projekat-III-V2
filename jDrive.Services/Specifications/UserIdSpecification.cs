using jDrive.DataModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Services.Specifications
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