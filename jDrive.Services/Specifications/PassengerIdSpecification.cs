using jDrive.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace jDrive.Services.Specifications
{
    public class PassengerIdSpecification : Specification<Passenger>
    {
        private string _passengerId;

        public PassengerIdSpecification(string passengerId)
        {
            _passengerId = passengerId;
        }

        public override Expression<Func<Passenger, bool>> ToExpression()
        {
            return passenger => passenger.Id == _passengerId;
        }
    }
}