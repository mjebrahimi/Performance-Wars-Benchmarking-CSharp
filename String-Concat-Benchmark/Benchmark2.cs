using BenchmarkDotNet.Attributes;
using System.Text;

//[DryJob]
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
[Config(typeof(CustomConfig))]
[HideColumns("chars")]
[MemoryDiagnoser]
[KeepBenchmarkFiles(false)]
//[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)] //Don't use because of CustomConfig
public class Benchmark2
{
    #region Utils
    private const string smallEN = "Lorem ipsum dolor sit amet, consectetur adipiscin.";
    private const string mediumEN = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu ege";
    private const string largeEN = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu egestas. Vivamus eget consectetur nulla. Etiam pharetra, mi tincidunt consequat suscipit, nisi ligula mollis leo, non vehicula ante velit faucibus nisl. Nunc sit amet sapien nec justo malesuada fringilla vel.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu egestas. Vivamus eget consectetur nulla. Etiam pharetra, mi tincidunt consequat suscipit, nisi ligula mollis leo, non vehicula ante velit faucibus nisl. Nunc sit amet sapien nec justo malesuada fringilla vel.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu egestas. Vivamus eget consectetur nulla. Etiam pharetra, mi tincidunt consequat suscipit, nisi ligula mollis leo, non vehicula ante velit faucibus nisl. Nunc sit amet sapien nec justo malesuada fringilla vel.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit am";

    private static readonly List<string> texts = new() {
        smallEN,    //50
        mediumEN,   //500
        largeEN,    //2500
    };

    public static IEnumerable<object[]> CreateParams()
    {
        foreach (var text in texts)
        {
            var chars = text.ToCharArray();
            yield return new object[] { chars, chars.Length };
        }
    }
    #endregion

    [Benchmark]
    [ArgumentsSource(nameof(CreateParams))]
    public string StringBuilderCache(char[] chars, int CharLength)
    {
        var builder = System.Text.StringBuilderCache.Acquire();

        var length = chars.Length;
        for (int i = 0; i < length; i++)
            builder.Append(chars[i]);

        return System.Text.StringBuilderCache.GetStringAndRelease(builder);
    }

    [Benchmark]
    [ArgumentsSource(nameof(CreateParams))]
    public string ValueStringBuilder(char[] chars, int CharLength)
    {
        using var builder = new ValueStringBuilder();

        var length = chars.Length;
        for (int i = 0; i < length; i++)
            builder.Append(chars[i]);

        return builder.ToString();
    }

    [Benchmark]
    [ArgumentsSource(nameof(CreateParams))]
    public string StringBuilder(char[] chars, int CharLength)
    {
        var builder = new StringBuilder();

        var length = chars.Length;
        for (int i = 0; i < length; i++)
            builder.Append(chars[i]);

        return builder.ToString();
    }

    [Benchmark]
    [ArgumentsSource(nameof(CreateParams))]
    public string NewString(char[] chars, int CharLength)
    {
        return new string(chars);
    }

    [Benchmark]
    [ArgumentsSource(nameof(CreateParams))]
    public string StringCreate(char[] chars, int CharLength)
    {
        return string.Create(chars.Length, chars, (span, state) =>
        {
            for (int i = 0; i < state.Length; i++)
                span[i] = state[i];
        });
    }
}