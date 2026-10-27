namespace AdventureGame;

public class Player
{
	private Room _currentRoom;

	public Room CurrentRoom => _currentRoom;

	public Player(Room startingRoom)
	{
		_currentRoom = startingRoom ?? throw new ArgumentNullException(nameof(startingRoom));
	}

	public bool TryMove(string? direction, out string message)
	{
		string normalized = direction?.Trim().ToUpperInvariant() ?? "";

		if (normalized != "NORTH" && normalized != "SOUTH" && normalized != "EAST" && normalized != "WEST")
		{
			message = $"Unknown direction {normalized}. Choose NORTH, SOUTH, EAST, or WEST.";
			return false;
		}

		Room? destination = _currentRoom.GetNeighbor(normalized);
		if (destination is null)
		{
			message = $"There is no exit {normalized} from {_currentRoom.Name}.";
			return false;
		}

		_currentRoom = destination;
		message = $"You move {normalized} to {_currentRoom.Name}.";
		return true;
	}
}