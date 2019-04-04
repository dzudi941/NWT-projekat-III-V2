using jDrive.DataModel.Models;
using System;
using System.Collections.Generic;

namespace jdrive_backend.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public bool HasRegistered { get; set; }
        public string LoginProvider { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double TotalDistance { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public double Rating { get; set; }
        public int? RideDiscountNumber { get; set; }

        public UserInfoViewModel(ApplicationUser applicationUser)
        {
            Id = applicationUser.Id;
            FullName = applicationUser.FullName;
            Email = applicationUser.Email;
            UserType = applicationUser is Driver ? UserType.Driver : UserType.Passenger;
            Latitude = applicationUser.Latitude;
            Longitude = applicationUser.Longitude;
        }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
