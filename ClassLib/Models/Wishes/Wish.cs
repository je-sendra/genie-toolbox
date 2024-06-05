using System.Text.Json;

namespace GenieToolbox.Models.Wishes;

/// <summary>
/// A wish is an isolated application that can be used for a specific purpose.
/// </summary>
public abstract class Wish
{
    public static string AbsoluteAppPath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "GenieToolbox"); // TODO: This should be set by the application.

    /// <summary>
    /// The default constructor for the wish.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="commonName"></param>
    /// <param name="description"></param>
    /// <param name="uniqueName"></param>
    protected Wish(Guid id, string commonName, string description, string uniqueName)
    {
        Id = id;
        CommonName = commonName;
        Description = description;
        UniqueName = uniqueName;
    }

    /// <summary>
    /// The unique identifier for the wish.
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// The common name for the wish.
    /// </summary>
    /// <value></value>
    public string CommonName { get; set; }

    /// <summary>
    /// The description of the wish.
    /// </summary>
    /// <value></value>
    public string Description { get; set; }

    /// <summary>
    /// The unique name of the wish.
    /// </summary>
    /// <value></value>
    public string UniqueName { get; set; }

    /// <summary>
    /// The type of the wish.
    /// </summary>
    /// <value></value>
    public abstract WishType WishType { get; set; }

    /// <summary>
    /// The method that runs the wish.
    /// </summary>
    /// <returns></returns>
    public abstract Task Run();

    /// <summary>
    /// Converts the wish to a JSON string.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static dynamic FromJson(string json)
    {
        var jsonObject = JsonDocument.Parse(json).RootElement;
        var type = jsonObject.GetProperty("WishType").GetInt32();

        object? obj = null;

        switch (type)
        {
            case (int)WishType.ExternalApplication:
                obj = JsonSerializer.Deserialize<ExternalApplicationWish>(json);
                break;
            default:
                throw new InvalidOperationException("Unknown wish type");
        }

        if (obj == null)
        {
            throw new JsonException("JSON could not be parsed into a wish.");
        }

        return obj;
    }


}