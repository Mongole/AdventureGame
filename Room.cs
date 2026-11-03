namespace AdventureGame;

public class Room
{
	public string Name { get; }
	public string Description { get; set; }
	private readonly List<Item> _items = new List<Item>();

	public IReadOnlyList<Item> Items => _items.AsReadOnly();

	public Room? North { get; private set; }
	public Room? South { get; private set; }
	public Room? East { get; private set; }
	public Room? West { get; private set; }

	public Room(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public Room? GetNeighbor(string direction)
	{
		if (string.IsNullOrWhiteSpace(direction))
		{
			return null;
		}

		return direction.Trim().ToUpperInvariant() switch
		{
			"NORTH" => North,
			"SOUTH" => South,
			"EAST" => East,
			"WEST" => West,
			_ => null
		};
	}

	public void Connect(string direction, Room neighbor)
	{
		if (string.IsNullOrWhiteSpace(direction))
		{
			throw new ArgumentException("A cardinal direction is required.", nameof(direction));
		}

		ArgumentNullException.ThrowIfNull(neighbor);

		string normalized = direction.Trim().ToUpperInvariant();
		string opposite = normalized switch
		{
			"NORTH" => "SOUTH",
			"SOUTH" => "NORTH",
			"EAST" => "WEST",
			"WEST" => "EAST",
			_ => throw new ArgumentException("Use NORTH, SOUTH, EAST, or WEST.", nameof(direction))
		};

		if (ReferenceEquals(this, neighbor))
		{
			throw new InvalidOperationException("A room cannot be its own neighbor.");
		}

		Room? currentHere = GetNeighbor(normalized);
		Room? currentThere = neighbor.GetNeighbor(opposite);

		if ((currentHere is not null && !ReferenceEquals(currentHere, neighbor)) ||
				(currentThere is not null && !ReferenceEquals(currentThere, this)))
		{
			throw new InvalidOperationException("One of these exits is already connected.");
		}

		SetNeighbor(normalized, neighbor);
		neighbor.SetNeighbor(opposite, this);
	}

	public void PlaceItem(Item item)
	{
		ArgumentNullException.ThrowIfNull(item);

		bool found = _items.Exists(current => ReferenceEquals(current, item));

		if (found)
		{
			throw new InvalidOperationException("This room already contains the item.");
		}

		_items.Add(item);
	}

	public void RemoveItem(Item item)
	{
		ArgumentNullException.ThrowIfNull(item);

		int itemIndex = _items.FindIndex(current => ReferenceEquals(current, item));

		if (itemIndex < 0)
		{
			throw new InvalidOperationException("This room does not contain the item.");
		}

		_items.RemoveAt(itemIndex);
	}

	public Item? FindItem(string itemName)
	{
		return _items.Find(item =>
			string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase));
	}

	private void SetNeighbor(string direction, Room neighbor)
	{
		switch (direction)
		{
			case "NORTH":
				North = neighbor;
				break;
			case "SOUTH":
				South = neighbor;
				break;
			case "EAST":
				East = neighbor;
				break;
			case "WEST":
				West = neighbor;
				break;
			default:
				throw new ArgumentException("Use a cardinal direction.", nameof(direction));
		}
	}
}