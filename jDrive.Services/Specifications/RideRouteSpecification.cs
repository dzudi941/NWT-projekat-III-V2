using jDrive.DataModel.Models;
using System;
using System.Linq.Expressions;

namespace jDrive.Services.Specifications
{
    public class RideRouteSpecification : Specification<Ride>
    {
        private double _startLatitude, _startLongitude, _finishLatitude, _finishLongitude;

        public RideRouteSpecification(double startLatitude, double startLongitude, double finishLatitude, double finishLongitude)
        {
            _startLatitude = startLatitude;
            _startLongitude = startLongitude;
            _finishLatitude = finishLatitude;
            _finishLongitude = finishLongitude;
        }

        public override Expression<Func<Ride, bool>> ToExpression()
        {
            return ride => ride.StartLatitude == _startLatitude &&
                    ride.StartLongitude == _startLongitude &&
                    ride.FinishLatitude == _finishLatitude &&
                    ride.FinishLongitude == _finishLongitude;
        }
    }
}