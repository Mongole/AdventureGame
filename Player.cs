namespace AdventureGame;

public class Player
{
	private Room _currentRoom;

	public Room CurrentRoom => _currentRoom;

	public Player(Room startingRoom)
	{
		_currentRoom = startingRoom ?? throw new ArgumentNullException(nameof(startingRoom));
	}

	private readonly List<Item> _inventory = new List<Item>();

	public IReadOnlyList<Item> Inventory => _inventory.AsReadOnly();

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

	public bool TryTake(string itemName, out string message)
	{
		Item? item = CurrentRoom.FindItem(itemName);

		if (item is null)
		{
			message = $"There is no {itemName} in {CurrentRoom.Name}.";
			return false;
		}

		CurrentRoom.RemoveItem(item);
		_inventory.Add(item);
		message = $"You take the {item.Name}.";
		return true;
	}

	public bool TryDrop(string itemName, out string message)
	{
		int matchIndex = _inventory.FindIndex(item =>
			string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase));

		if (matchIndex < 0)
		{
			message = $"You are not carrying {itemName}.";
			return false;
		}

		Item item = _inventory[matchIndex];

		CurrentRoom.PlaceItem(item);
		_inventory.RemoveAt(matchIndex);

		message = $"You drop the {item.Name}.";
		return true;
	}
}