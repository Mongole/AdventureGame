using AdventureGame;

public class Program
{
	public static void Main()
	{
		Room gallery = new Room("Gallery", "A long gallery lined with faded banners.");
		Room alias = gallery;
		Room clone = new Room("Gallery", "A long gallery lined with faded banners.");

		Console.WriteLine($"{gallery.Name}: {gallery.Description}");
		Console.WriteLine($"{alias.Name}: {alias.Description}");
		Console.WriteLine($"{clone.Name}: {clone.Description}");
		Console.WriteLine();
		Console.WriteLine($"Alias and Gallery are the same: {alias == gallery}");
		Console.WriteLine($"Clone and Gallery are the same: {clone == gallery}");
		Console.WriteLine();

		alias.Description = "Framed pictures hang from all the walls.";
		clone.Description = "A room filled with framed pictures.";

		Console.WriteLine($"{gallery.Name}: {gallery.Description}");
		Console.WriteLine($"{alias.Name}: {alias.Description}");
		Console.WriteLine($"{clone.Name}: {clone.Description}");
	}
}