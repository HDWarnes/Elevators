public class Elevator
{
    public int CurrentFloor { get; set; }
    public int DestinationFloor { get; set; }
    public ElevatorDirection Direction { get; set; }
    public int NumberOfPeople { get; set; }
    public int Capacity = 10;
    public int Space { get; set; }

    public enum ElevatorDirection
    {
        Stationary,
        Up,
        Down
    }

    public void MoveUp()
    {
        if (Direction == ElevatorDirection.Down)
        {
            Console.WriteLine("Error: Elevator is moving down and cannot move up.");
            return;
        }
        if (CurrentFloor == DestinationFloor)
        {
            Console.WriteLine("Error: Elevator is already at the destination floor.");
            return;
        }
        CurrentFloor++;
        if (CurrentFloor == DestinationFloor)
        {
            Direction = ElevatorDirection.Stationary;
            Console.WriteLine("Elevator at floor " + CurrentFloor + " " + Direction + " with " + NumberOfPeople + " people disembarking");
            NumberOfPeople = 0;
        }
        else
        {
            Direction = ElevatorDirection.Up;
            Console.WriteLine("Elevator at floor " + CurrentFloor + " " + Direction + " with " + NumberOfPeople + " people");

        }
    }

    public void MoveDown()
    {
        if (Direction == ElevatorDirection.Up)
        {
            Console.WriteLine("Error: Elevator is moving up and cannot move down.");
            return;
        }
        if (CurrentFloor == DestinationFloor)
        {
            Console.WriteLine("Error: Elevator is already at the destination floor.");
            return;
        }
        CurrentFloor--;
        if (CurrentFloor == DestinationFloor)
        {
            Direction = ElevatorDirection.Stationary;
            Console.WriteLine("Elevator at floor " + CurrentFloor + " going " + Direction + " with " + NumberOfPeople + " people disembarking");
            NumberOfPeople = 0;
        }
        else
        {
            Direction = ElevatorDirection.Down;
            Console.WriteLine("Elevator at floor " + CurrentFloor + " going " + Direction + " with " + NumberOfPeople + " people");

        }
    }
}


public class Building
{
    public int NumberOfFloors { get; set; }
    public int NumberOfElevators { get; set; }
    public List<Elevator> Elevators { get; set; }
    public int[,] WaitingListUp { get; set; }
    public int[,] WaitingListDown { get; set; }


    public void CallElevator(int floor)
    {
        // Find the closest elevator that is stationary or moving in the same direction
        Elevator closestElevator = null;
        int closestDistance = int.MaxValue;
        foreach (Elevator elevator in Elevators)
        {
            bool isStationaryOrMovingInSameDirection = elevator.Direction == Elevator.ElevatorDirection.Stationary ||
                (elevator.Direction == Elevator.ElevatorDirection.Up && floor >= elevator.CurrentFloor) ||
                (elevator.Direction == Elevator.ElevatorDirection.Down && floor <= elevator.CurrentFloor);
            if (isStationaryOrMovingInSameDirection)
            {
                int distance = Math.Abs(floor - elevator.CurrentFloor);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestElevator = elevator;
                }
            }
        }

        if (closestElevator != null)
        {
            closestElevator.Space = closestElevator.Capacity - closestElevator.NumberOfPeople;
            closestElevator.DestinationFloor = floor;
            Elevator.ElevatorDirection direction = closestElevator.CurrentFloor < floor ? Elevator.ElevatorDirection.Up : Elevator.ElevatorDirection.Down;

            while (closestElevator.CurrentFloor != closestElevator.DestinationFloor)
            {
                int waitingListCount = direction == Elevator.ElevatorDirection.Up ? WaitingListUp[closestElevator.CurrentFloor, 0] : WaitingListDown[closestElevator.CurrentFloor, 0];

                if (waitingListCount > 0)
                {
                    int numberOfPeopleToAdd = waitingListCount < closestElevator.Space ? waitingListCount : closestElevator.Space;
                    closestElevator.NumberOfPeople += numberOfPeopleToAdd;
                    closestElevator.Space -= numberOfPeopleToAdd;

                    if (direction == Elevator.ElevatorDirection.Up)
                    {
                        WaitingListUp[closestElevator.CurrentFloor, 0] -= numberOfPeopleToAdd;
                        Console.WriteLine("Elevator at floor " + closestElevator.CurrentFloor + " going " + closestElevator.Direction + " with " + numberOfPeopleToAdd + " people getting on");
                    }
                    else
                    {
                        WaitingListDown[closestElevator.CurrentFloor, 0] -= numberOfPeopleToAdd;
                        Console.WriteLine("Elevator at floor " + closestElevator.CurrentFloor + " going " + closestElevator.Direction + " with " + numberOfPeopleToAdd + " people getting on");

                    }
                }

                if (direction == Elevator.ElevatorDirection.Up)
                {
                    closestElevator.MoveUp();
                }
                else
                {
                    closestElevator.MoveDown();
                }
            }
        }
    }


    public void SetWaitingList(int floor, int numberOfPeople, int direction)
    {
        if(direction == 1)
        {
            WaitingListUp[floor, 0] = numberOfPeople;

        }
        else if (direction == 2)
        {
            WaitingListDown[floor, 0] = numberOfPeople;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create a new Building object and initialize its properties
        Building building = new Building
        {
            NumberOfFloors = 10,
            NumberOfElevators = 3,
            Elevators = new List<Elevator>(),
            WaitingListUp = new int[10, 1],
            WaitingListDown = new int[10, 1]

        };
        for (int i = 0; i < building.NumberOfElevators; i++)
        {
            building.Elevators.Add(new Elevator
            {
                CurrentFloor = 0,
                DestinationFloor = 0,
                Direction = Elevator.ElevatorDirection.Stationary,
                NumberOfPeople = 0
            });
        }

        // Display the initial status of the building
        Console.WriteLine("Building status:");
        Console.WriteLine("Number of floors: " + building.NumberOfFloors);
        Console.WriteLine("Number of elevators: " + building.NumberOfElevators);
        Console.WriteLine();
        for (int i = building.NumberOfFloors - 1; i >= 0; i--)
        {
            Console.WriteLine("Floor " + i + ": " + (building.WaitingListUp[i, 0]+ building.WaitingListDown[i, 0]) + " people waiting");
        }
        Console.WriteLine();
        foreach (Elevator elevator in building.Elevators)
        {
            Console.WriteLine("Elevator at floor " + elevator.CurrentFloor + " going " + elevator.Direction + " with " + elevator.NumberOfPeople + " people");
        }
        Console.WriteLine();

        // Loop until the user quits the program
        while (true)
        {
            // Display the available options
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Call elevator");
            Console.WriteLine("2. Set waiting list");
            Console.WriteLine("3. Quit");
            Console.WriteLine();

            // Read the user's input and execute the corresponding action
            string input = Console.ReadLine();
            Console.WriteLine();
            switch (input)
            {
                case "1":
                    Console.Write("Enter floor number: ");
                    int floor = int.Parse(Console.ReadLine());
                    while (floor > building.NumberOfFloors)
                    {
                        Console.Write("Enter floor number less than "+ building.NumberOfFloors);
                        floor = int.Parse(Console.ReadLine());
                    }
                    building.CallElevator(floor);
                    
                    Console.WriteLine();
                    break;
                case "2":
                    Console.Write("Enter floor number: ");
                    int floorNumber = int.Parse(Console.ReadLine());
                    Console.Write("Enter number of people waiting: ");
                    int numberOfPeople = int.Parse(Console.ReadLine());
                    Console.Write("Enter direction: ");
                    Console.Write("1. Up 2. Down ");
                    int direction = int.Parse(Console.ReadLine());
                    building.SetWaitingList(floorNumber, numberOfPeople, direction);
                    Console.WriteLine();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    Console.WriteLine();
                    break;
            }

            // Update and display the status of the building and elevators
            Console.WriteLine("Building status:");
            Console.WriteLine("Number of floors: " + building.NumberOfFloors);
            Console.WriteLine("Number of elevators: " + building.NumberOfElevators);
            Console.WriteLine();
            for (int i = building.NumberOfFloors - 1; i >= 0; i--)
            {
                Console.WriteLine("Floor " + i + ": " + (building.WaitingListUp[i, 0] + building.WaitingListDown[i, 0]) + " people waiting");
            }
            Console.WriteLine();
            foreach (Elevator elevator in building.Elevators)
            {
                if (elevator.Direction == Elevator.ElevatorDirection.Stationary)
                {
                    Console.WriteLine("Elevator at floor " + elevator.CurrentFloor );
                }
             
            }
            Console.WriteLine();
        }
    }
}
