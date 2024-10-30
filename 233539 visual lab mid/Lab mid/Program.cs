using System;

internal class Program
{
    public abstract class User
    {
        private int userId;
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }

     

        public void DisplayProfile()
        {
            Console.WriteLine($"User  Id: {userId}, Name: {Name}, Phone Number: {PhoneNumber}");
        }

        public void Register()
        {
            Console.WriteLine("\nEnter the user id: ");
            userId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\nEnter your name: ");
            Name = Console.ReadLine();

            Console.WriteLine("\nEnter your Phone Number: ");
            PhoneNumber = Console.ReadLine();

            Console.WriteLine("\nThe user has been registered successfully.");
        }

        public bool Login()
        {
            Console.WriteLine("\nEnter the user id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            if (id == userId)
            {
                Console.WriteLine("\nLogin successful!");
                return true;
            }
            else
            {
                Console.WriteLine("\nInvalid user id.");
                return false;
            }
        }
    }

    public class Rider : User
    {
        private List<string> rideHistory = new List<string>();
        public string location;
        public string destination;

        public void RequestRide()
        {
            Console.WriteLine("\nEnter your current location: ");
            location = Console.ReadLine();

            Console.WriteLine("\nEnter your destination: ");
            destination = Console.ReadLine();

            string rideDetails = $"From {location} to {destination}";
            rideHistory.Add(rideDetails);

            Console.WriteLine("\nRide requested successfully.");
        }

        public void ViewRideHistory()
        {
            Console.WriteLine("\nRide History:");
            foreach (var ride in rideHistory)
            {
                Console.WriteLine(ride);
            }
        }

        public override string ToString()
        {
            return $"Rider: {Name}";
        }
    }

    public class Driver : User
    {
        private int driverID;
        public string VehicleDetails { get; private set; }
        public bool IsAvailable { get; private set; }
        private List<string> tripHistory = new List<string>();

        public void AcceptRide(string rideDetails)
        {
            tripHistory.Add(rideDetails);
            Console.WriteLine($"\nDriver {Name} accepted ride: {rideDetails}");
        }

        public void ViewTripHistory()
        {
            Console.WriteLine("\nTrip History:");
            foreach (var trip in tripHistory)
            {
                Console.WriteLine(trip);
            }
        }

        public void ToggleAvailability()
        {
            IsAvailable = !IsAvailable;
            if(IsAvailable)
            {
                Console.WriteLine("Driver is now available.");
            }
            else
            {
                Console.WriteLine("Driver is not available.");
            }
            
        }

        public override string ToString()
        {
            return $"Driver: {Name}";
        }
    }

    public class Trip
    {
        public int TripID { get; set; }
        public string RiderName;
        public string DriverName;
        public string StartLocation { get; set; }
        public string Destination { get; set; }
        public double Fare { get; private set; }
        public bool Status { get; private set; }

        public void CalculateFare(double d)
        {
            Fare = d * 0.5; 
        }

        public void StartTrip()
        {
            Status = true;
            Console.WriteLine("Trip started.");
        }

        public void EndTrip()
        {
            Status = false;
            Console.WriteLine("Trip ended.");
        }

        public void DisplayTripDetails()
        {
            Console.WriteLine($"Trip ID: {TripID}, Rider: {RiderName}, Driver: {DriverName}, Start: {StartLocation}, Destination: {Destination}, Fare: {Fare}, Status: {(Status ? "In Progress" : "Completed")}");
        }
    }

    public class RideSharingSystem
    {
        private List<Rider> registeredRiders = new List<Rider>();
        private List<Driver> registeredDrivers = new List<Driver>();
        public List<Trip> availableTrips = new List<Trip>();
        private int tripCounter = 1;

        public void RegisterUser(User user)
        {
            if (user is Rider)
            {
                registeredRiders.Add((Rider)user);
            }
            else if (user is Driver)
            {
                registeredDrivers.Add((Driver)user);
            }
        }

        public void RequestRide(Rider rider)
        {
            rider.RequestRide();
            foreach (var driver in registeredDrivers)
            {
                if (driver.IsAvailable)
                {
                    Console.WriteLine($"Available driver: {driver.Name}");
                    Trip trip = new Trip
                    {
                        TripID = tripCounter,
                        RiderName = rider.Name,
                        DriverName = driver.Name,
                        StartLocation = rider.location,
                        Destination = rider.destination
                    };
                    availableTrips.Add(trip);
                    tripCounter++;
                    driver.AcceptRide($"From {rider.location} to {rider.destination}");
                    Console.WriteLine("Trip assigned successfully.");
                    return;
                }
            }
            Console.WriteLine("No available drivers at the moment.");
        }

        public void CompleteTrip(int tripID)
        {
            foreach (var trip in availableTrips)
            {
                if (trip.TripID == tripID)
                {
                    trip.EndTrip();
                    Console.WriteLine("Trip completed.");
                    return;
                }
            }
            Console.WriteLine("Trip not found.");
        }

        public void DisplayAllTrips()
        {
            foreach (var trip in availableTrips)
            {
                trip.DisplayTripDetails();
            }
        }
    }

    private static void Main()
    {
        RideSharingSystem rideSharingSystem = new RideSharingSystem();
        Rider rider = new Rider();
        Driver driver = new Driver();

         rider.Register();
         rideSharingSystem.RegisterUser(rider);

        driver.Register();
        rideSharingSystem.RegisterUser(driver);
        driver.ToggleAvailability();


        bool close = true;

        while (close)
        {
            Console.WriteLine("\nWelcome to Ride-Sharing System:");
            Console.WriteLine("1. Register as rider (Rider)");
            Console.WriteLine("2. Register as driver (Driver)");
            Console.WriteLine("3. Request a ride (Rider)");
            Console.WriteLine("4. Accept a ride (Driver)");
            Console.WriteLine("5. Complete a trip (Driver)");
            Console.WriteLine("6. View Ride History (Rider)");
            Console.WriteLine("7. View Trip History (Driver)");
            Console.WriteLine("8. Display All Trips");
            Console.WriteLine(". Exit");
            Console.Write("Choose an option: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {

                case 1:
                    rider.Register();
                    rideSharingSystem.RegisterUser(rider);
                    break;

                case 2:
                    driver.Register();
                    rideSharingSystem.RegisterUser(driver);
                    driver.ToggleAvailability();
                    break;

                case 3:
                    rideSharingSystem.RequestRide(rider);
                    break;
                case 4:
                   
                    if (rideSharingSystem.availableTrips.Count > 0)
                    {
                        var lastTrip = rideSharingSystem.availableTrips[^1]; 
                        driver.AcceptRide($"From {lastTrip.StartLocation} to {lastTrip.Destination}");
                    }
                    else
                    {
                        Console.WriteLine("No rides to accept.");
                    }
                    break;
                case 5:
                    Console.Write("Enter Trip ID to complete: ");
                    int tripID = Convert.ToInt32(Console.ReadLine());
                    rideSharingSystem.CompleteTrip(tripID);
                    break;
                case 6:
                    rider.ViewRideHistory();
                    break;
                case 7:
                    driver.ViewTripHistory();
                    break;
                case 8:
                    rideSharingSystem.DisplayAllTrips();
                    break;
                case 9:
                    close = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
