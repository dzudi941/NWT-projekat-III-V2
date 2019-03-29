//using jDrive.DataModel.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Web;

//namespace jDrive.Services.Specifications
//{
//    public class DriverIdSpecification : Specification<Driver>
//    {
//        private string _driverId;

//        public DriverIdSpecification(string driverId)
//        {
//            _driverId = driverId;
//        }

//        public override Expression<Func<Driver, bool>> ToExpression()
//        {
//            return driver => driver.Id == _driverId;
//        }
//    }
//}