using AdventureGame;

Room entrance = new Room("Entrance", "A dry stone chamber with daylight spilling through the doorway.");
Room gallery = new Room("Gallery", "A long gallery lined with faded banners.");
Room archive = new Room("Archive", "An archive whose shelves hold dust-covered chronicles.");

entrance.Connect("EAST", gallery);
gallery.Connect("NORTH", archive);

string entranceBefore = entrance.Description;
gallery.Description = "A long gallery with a freshly opened skylight.";

Console.WriteLine("Independent-state demonstration:");
Console.WriteLine($"Entrance description unchanged: {entrance.Description == entranceBefore}");
Console.WriteLine($"Entrance and Gallery are distinct: {!ReferenceEquals(entrance, gallery)}");
Console.WriteLine($"Entrance EAST refers to Gallery: {ReferenceEquals(entrance.East, gallery)}");
Console.WriteLine();

Player player = new Player(entrance);

Console.WriteLine("Adventure Game — Movement Lab");
Console.WriteLine("Commands: GO <direction>, LOOK, QUIT.");
PrintRoom(player.CurrentRoom);

while (true)
{
	Console.Write("> ");
	string? input = Console.ReadLine();

	if (input is null)
	{
		Console.WriteLine("Input ended. Closing the game.");
		break;
	}

	string trimmed = input.Trim();
	if (trimmed.Length == 0)
	{
		Console.WriteLine("Enter GO <direction>, LOOK, or QUIT.");
		continue;
	}

	string[] words = trimmed.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
	string command = words[0].ToUpperInvariant();

	if (command == "QUIT")
	{
		if (words.Length != 1)
		{
			Console.WriteLine("QUIT takes no arguments.");
			continue;
		}

		Console.WriteLine("You leave the dungeon.");
		break;
	}

	if (command == "LOOK")
	{
		if (words.Length != 1)
		{
			Console.WriteLine("LOOK takes no arguments.");
			continue;
		}

		PrintRoom(player.CurrentRoom);
		continue;
	}

	if (command == "GO")
	{
		if (words.Length != 2)
		{
			Console.WriteLine("Use GO followed by one direction: NORTH, SOUTH, EAST, or WEST.");
			continue;
		}

		bool moved = player.TryMove(words[1], out string message);
		Console.WriteLine(message);

		if (moved)
		{
			PrintRoom(player.CurrentRoom);
		}
		else
		{
			Console.WriteLine($"You remain in {player.CurrentRoom.Name}.");
		}

		continue;
	}

	Console.WriteLine("Unknown command. Use GO <direction>, LOOK, or QUIT.");
}

static void PrintRoom(Room room)
{
	Console.WriteLine($"{room.Name}: {room.Description}");
	Console.WriteLine($"  NORTH: {room.North?.Name ?? "(none)"}");
	Console.WriteLine($"  SOUTH: {room.South?.Name ?? "(none)"}");
	Console.WriteLine($"  EAST: {room.East?.Name ?? "(none)"}");
	Console.WriteLine($"  WEST: {room.West?.Name ?? "(none)"}");
}