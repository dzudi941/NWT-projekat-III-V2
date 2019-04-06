using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace jDrive.DomainModel.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }

    public enum UserType
    {
        Driver,
        Passenger
    }
}