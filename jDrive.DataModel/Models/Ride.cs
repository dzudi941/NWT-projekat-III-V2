namespace jDrive.DataModel.Models
{
    public class Ride
    {
        public int Id { get; set; }
        public double StartLongitude { get; set; }
        public double StartLatitude { get; set; }
        public double FinishLongitude { get; set; }
        public double FinishLatitude { get; set; }
        public Passenger Passenger { get; set; }
        public Driver Driver { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int DriverRating { get; set; }
        public int PassengerRating { get; set; }
    }

    public enum RequestStatus
    {
        Pending,
        Accepted,
        Rejected,
        Finished,
        NotSent
    }
}