using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CMS.Core.Helpers;

public static class ImageHelper
{
    /// <summary>
    /// Resize image from given path
    /// </summary>
    /// <param name="path"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void Resize(string path, int width, int height)
    {
        if (!File.Exists(path))
        {
            return;
        }

        var target = GetResizedName(path, width, height);
        using (var image = Image.Load(path))
        {
            image.Mutate(x => x.Resize(width, height).Grayscale());
            image.Save(target);
        }
    }

    /// <summary>
    /// Resize image from given stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="path"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void Resize(Stream stream, string path, int width, int height)
    {
        var target = GetResizedName(path, width, height);
        using (var image = Image.Load(stream))
        {
            image.Mutate(x => x.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(width, height) }));
            image.Save(target);
        }
    }

    public static string GetBase64StringImage(string path, Tuple<int, int> size = null)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        if (!File.Exists(path))
        {
            return string.Empty;
        }

        // if has resized image, get it
        if (size != null && size.Item1 > 0)
        {
            var resizedPath = GetResizedName(path, size.Item1, size.Item2);
            if (File.Exists(resizedPath))
            {
                path = resizedPath;
            }
        }

        return "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(path));
    }

    /// <summary>
    /// Get file name in resize mode
    /// </summary>
    /// <param name="path">Image/picture.png</param>
    /// <param name="width">96</param>
    /// <param name="height">96</param>
    /// <returns>Image/picture-96x96.png</returns>
    private static string GetResizedName(string path, int width, int height)
    {
        var name = Path.GetFileNameWithoutExtension(path);
        var ext = Path.GetExtension(path);
        var dir = Path.GetDirectoryName(path);

        return Path.Combine(dir, $"{name}-{width}x{height}{ext}");
    }
}
