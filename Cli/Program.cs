using System.Runtime.InteropServices;
using System.Text.Json;
using GenieToolbox.Models.Platforms;
using GenieToolbox.Models.Wishes;

namespace GenieToolbox.Cli;

/// <summary>
/// The main Program class for the entry point for the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide an argument.");
            Environment.Exit(1);
        }

        var internalWishFileFolder = Path.Combine(Wish.AbsoluteAppPath, "_internal", "WishFiles");

        foreach (var wishFile in Directory.GetFiles(internalWishFileFolder, "*.wish"))
        {
            var wish = Wish.FromJson(File.ReadAllText(wishFile));

            if (wish.UniqueName.ToLower() == args[0].ToLower())
            {
                wish.Run().Wait();
                break;
            }
        }


        Console.WriteLine("Wish executed successfully.");
    }
}