using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using GenieToolbox.Models.Platforms;

namespace GenieToolbox.Models.Wishes;

/// <summary>
/// An external application wish is a wish that is an external application.
/// </summary>
public class ExternalApplicationWish : Wish
{
    /// <summary>
    /// The default constructor for the external application wish.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="commonName"></param>
    /// <param name="description"></param>
    /// <param name="uniqueName"></param>
    /// <param name="supportedPlatforms"></param>
    public ExternalApplicationWish(Guid id, string commonName, string description, string uniqueName, List<SupportedPlatform> supportedPlatforms) : base(id, commonName, description, uniqueName)
    {
        SupportedPlatforms = supportedPlatforms;
    }

    /// <summary>
    /// The supported platforms for the external application.
    /// </summary>
    /// <value></value>
    public List<SupportedPlatform> SupportedPlatforms { get; set; }

    /// <inheritdoc/>
    public override WishType WishType { get; set; } = WishType.ExternalApplication;

    /// <inheritdoc/>
    public override Task Run()
    {
        // Find the current runtime platform in the list of support platforms.
        Platform currentPlatform;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            currentPlatform = Platform.Linux;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            currentPlatform = Platform.Windows;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            currentPlatform = Platform.OSX;
        }
        else
        {
            throw new PlatformNotSupportedException("The platform is not supported.");
        }

        var platform = SupportedPlatforms.Find(x => x.Platform == currentPlatform);

        // If the platform is not supported, throw an exception.
        if (platform == null)
        {
            throw new PlatformNotSupportedException("The platform is not supported.");
        }

        // Create the complete executable path.
        var completeExecutablePath = Path.Combine(AbsoluteAppPath, UniqueName, platform.FilePath);

        // Run the external application.
        Process process = new Process();
        process.StartInfo.FileName = completeExecutablePath;

        process.Start();

        return Task.CompletedTask;
    }

    public string ToJsonString() // TODO: This method should not exist.
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        return JsonSerializer.Serialize(this, options);
    }
}