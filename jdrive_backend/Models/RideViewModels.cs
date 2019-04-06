using jDrive.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace jdrive_backend.Models
{
    public class RideRequestViewModel
    {
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double FinishLatitude { get; set; }
        public double FinishLongitude { get; set; }
        public string DriverId { get; set; }
        public double EstimatedPrice { get; set; }
    }

    public class RideViewModel
    {
        public int Id { get; set; }
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }
        public double FinishLongitude { get; set; }
        public double FinishLatitude { get; set; }
        public string PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public double EstimatedPrice { get; set; }
        //public string ExtraMessage { get; set; }
        double TotalDiscount { get; set; }
        double RideNumber { get; set; }

        public RideViewModel(Ride ride, double totalDiscount = 0, int rideNumber = 0)
        {
            Id = ride.Id;
            StartLatitude = ride.StartLatitude;
            StartLongitude = ride.StartLongitude;
            FinishLatitude = ride.FinishLatitude;
            FinishLongitude = ride.FinishLongitude;
            PassengerId = ride.Passenger?.Id ?? string.Empty;
            PassengerName = ride.Passenger?.FullName ?? string.Empty;
            DriverId = ride.Driver?.Id ?? string.Empty;
            DriverName = ride.Driver?.FullName ?? string.Empty;
            RequestStatus = ride.RequestStatus;
            EstimatedPrice = ride.EstimatedPrice;
            TotalDiscount = totalDiscount;
            RideNumber = rideNumber;
        }
    }
}