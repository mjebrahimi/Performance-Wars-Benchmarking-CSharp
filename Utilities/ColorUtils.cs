namespace Utilities;

public class ColorUtils
{
    /// <summary>
    /// Returns hex color value between Red (FF0000) and Green(00FF00 ) from a Scalar value between 0 and 1
    /// </summary>
    /// <param name="value">Scalar value between 0 and 1.</param>
    /// <returns>Hex color value</returns>
    public static string GetColorBetweenRedAndGreen(float value)
    {
        // value must be between [0, 510]
        value = Math.Min(Math.Max(0, value), 1) * 510;

        double redValue;
        double greenValue;
        if (value < 255)
        {
            redValue = 255;
            greenValue = (Math.Sqrt(value) * 16);
        }
        else
        {
            greenValue = 255;
            value -= 255;
            redValue = 255 - (value * value / 255);
        }

        return "#" + ConvertToHex(redValue) + ConvertToHex(greenValue) + "00";

        static string ConvertToHex(double i) => ((int)i).ToString("X2");
    }

    public static string Lighten(string color, double amount = 0.5)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(color);
        return MixColors(color, "#fff", amount);
    }

    public static string Darken(string color, double amount = 0.5)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(color);
        return MixColors(color, "#000", amount);
    }

    public static string MixColors(string colorA, string colorB, double amount = 0.5)
    {
        var rgbA = HexToRGB(colorA);
        var rgbB = HexToRGB(colorB);

        var mixedColor = rgbA.Select((channelA, index) => MixChannels(channelA, rgbB[index], amount)).ToArray();

        return "#" + string.Concat(mixedColor.Select(p => p.ToString("X2")));
    }

    public static int[] HexToRGB(string hexColor)
    {
        return GetChannels(hexColor).Select(p => Convert.ToInt32(p, 16)).ToArray();
    }

    public static string[] GetChannels(string hexColor)
    {
        if (hexColor[0] is '#')
            hexColor = hexColor[1..];

        return (hexColor.Length) switch
        {
            3 => hexColor.Chunk(1).Select(p => new string(p[0], 2)).ToArray(),
            6 => hexColor.Chunk(2).Select(p => new string(p)).ToArray(),
            _ => throw new ArgumentException($"Invalid hex color '{hexColor}'", nameof(hexColor))
        };
    }

    public static int MixChannels(int channelA, int channelB, double amount)
    {
        var a = channelA * (1 - amount);
        var b = channelB * amount;
        return Convert.ToInt32(a + b);
    }
}