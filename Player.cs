namespace AdventureGame;

public class Player
{
	private Room _currentRoom;

	public Room CurrentRoom => _currentRoom;

	public Player(Room startingRoom)
	{
		_currentRoom = startingRoom ?? throw new ArgumentNullException(nameof(startingRoom));
	}

	public Item? HeldItem { get; private set; }

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

	public bool TryTake(string? itemName, out string message)
	{
		string target = itemName?.Trim() ?? "";
		if (target.Length == 0)
		{
			message = "Specify the item you want to take.";
			return false;
		}

		Item? available = CurrentRoom.Item;
		if (available is null || !string.Equals(available.Name, target, StringComparison.OrdinalIgnoreCase))
		{
			message = $"There is no {target} in {CurrentRoom.Name}.";
			return false;
		}

		if (HeldItem is not null)
		{
			message = $"You are already carrying {HeldItem.Name}.";
			return false;
		}

		CurrentRoom.RemoveItem();
		HeldItem = available;
		message = $"You take the {available.Name}.";
		return true;
	}
}