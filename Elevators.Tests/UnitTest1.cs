using NUnit.Framework;

[TestFixture]
public class ElevatorTests
{
    [Test]
    public void MoveUp_ShouldIncreaseCurrentFloor()
    {
        // Arrange
        Elevator elevator = new Elevator { CurrentFloor = 0, DestinationFloor = 5, Direction = Elevator.ElevatorDirection.Up, NumberOfPeople = 0 };

        // Act
        elevator.MoveUp();

        // Assert
        Assert.AreEqual(1, elevator.CurrentFloor);
    }

    [Test]
    public void MoveDown_ShouldDecreaseCurrentFloor()
    {
        // Arrange
        Elevator elevator = new Elevator { CurrentFloor = 5, DestinationFloor = 0, Direction = Elevator.ElevatorDirection.Down, NumberOfPeople = 0 };

        // Act
        elevator.MoveDown();

        // Assert
        Assert.AreEqual(4, elevator.CurrentFloor);
    }

    [Test]
    public void CallElevator_ShouldAssignElevatorToClosestStationaryOrMovingInSameDirection()
    {
        // Arrange
        Building building = new Building
        {
            NumberOfFloors = 10,
            NumberOfElevators = 3,
            Elevators = new List<Elevator>
            {
                new Elevator { CurrentFloor = 0, DestinationFloor = 0, Direction = Elevator.ElevatorDirection.Stationary, NumberOfPeople = 0 },
                new Elevator { CurrentFloor = 5, DestinationFloor = 0, Direction = Elevator.ElevatorDirection.Down, NumberOfPeople = 0 },
                new Elevator { CurrentFloor = 10, DestinationFloor = 0, Direction = Elevator.ElevatorDirection.Down, NumberOfPeople = 0 }
            },
            WaitingListUp = new int[10, 1],
            WaitingListDown = new int[10, 1]
        };
        building.SetWaitingList(2, 2, 1);
        building.SetWaitingList(8, 1, 2);

        // Act
        building.CallElevator(4);

        // Assert
        Elevator assignedElevator = building.Elevators[0];
        Assert.AreEqual(assignedElevator.CurrentFloor, 0);
        Assert.AreNotEqual(assignedElevator.DestinationFloor, 4);
    }
}

[TestFixture]
public class BuildingTests
{
    [Test]
    public void SetWaitingList_ShouldSetWaitingListForGivenFloorAndDirection()
    {
        // Arrange
        Building building = new Building
        {
            NumberOfFloors = 10,
            NumberOfElevators = 3,
            Elevators = new List<Elevator>(),
            WaitingListUp = new int[10, 1],
            WaitingListDown = new int[10, 1]
        };

        // Act
        building.SetWaitingList(3, 2, 1);

        // Assert
        Assert.AreEqual(2, building.WaitingListUp[3, 0]);
    }
}
