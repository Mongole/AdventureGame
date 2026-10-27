namespace AdventureGame;

public class Room
{
	public string Name { get; }
	public string Description { get; set; }

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