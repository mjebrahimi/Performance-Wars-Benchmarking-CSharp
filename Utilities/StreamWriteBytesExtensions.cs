//using Microsoft.IO;
using System.Text;

public static class StreamWriteBytesExtensions
{
    #region UsingBinaryWriter (Not Asyncable)
    public static void UsingBinaryWriter(this Stream stream, byte[] bytes)
    {
        if (stream is MemoryStream memory)
        {
            memory.Write(bytes, 0, bytes.Length);
        }
        else
        {
            using var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            binaryWriter.Write(bytes, 0, bytes.Length);
        }
    }

    public static void UsingBinaryWriterSpan(this Stream stream, byte[] bytes)
    {
        if (stream is MemoryStream memory)
        {
            memory.Write((ReadOnlySpan<byte>)bytes);
        }
        else
        {
            using var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            binaryWriter.Write((ReadOnlySpan<byte>)bytes);
        }
    }
    #endregion

    #region WriteAllBytes (Asyncable)
    public static void WriteAllBytes(this Stream stream, byte[] bytes)
    {
        if (stream is MemoryStream memory)
            memory.Write(bytes, 0, bytes.Length);
        else
            stream.Write(bytes, 0, bytes.Length);
    }

    public static void WriteAllBytesSpan(this Stream stream, byte[] bytes)
    {
        if (stream is MemoryStream memory)
            memory.Write((ReadOnlySpan<byte>)bytes);
        else
            stream.Write((ReadOnlySpan<byte>)bytes);
    }

    public static Task WriteAllBytesAsync(this Stream stream, byte[] bytes, CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memory)
            return memory.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        else
            return stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
    }

    public static ValueTask WriteAllBytesSpanAsync(this Stream stream, byte[] bytes, CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memory)
            return memory.WriteAsync((ReadOnlyMemory<byte>)bytes, cancellationToken);
        else
            return stream.WriteAsync((ReadOnlyMemory<byte>)bytes, cancellationToken);
    }
    #endregion

    #region UsingMemoryStream (Bad Performance - Asyncable)
    //    public static byte[] UsingMemoryStream(this Stream stream)
    //    {
    //        if (stream is MemoryStream memory)
    //            return memory.ToArray();

    //        using var memoryStream = new MemoryStream();
    //        stream.CopyTo(memoryStream);
    //        return memoryStream.ToArray();
    //    }

    //    public static async Task<byte[]> UsingMemoryStreamAsync(this Stream stream, CancellationToken cancellationToken = default)
    //    {
    //        if (stream is MemoryStream memory)
    //            return memory.ToArray();

    //        using var memoryStream = new MemoryStream();
    //#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
    //        await stream.CopyToAsync(memoryStream, cancellationToken);
    //#else
    //        await stream.CopyToAsync(memoryStream, 81920, cancellationToken);
    //#endif
    //        return memoryStream.ToArray();
    //    }
    #endregion

    #region UsingRecyclableMemoryStream (Bad Performance - Asyncable)
    //    private readonly static RecyclableMemoryStreamManager recyclableMemoryStreamManager = new();
    //    public static byte[] UsingRecyclableMemoryStream(this Stream stream)
    //    {
    //        if (stream is MemoryStream memory)
    //            return memory.ToArray();

    //        using var memoryStream = recyclableMemoryStreamManager.GetStream();
    //        stream.CopyTo(memoryStream);
    //        return memoryStream.ToArray();
    //    }

    //    public static async Task<byte[]> UsingRecyclableMemoryStreamAsync(this Stream stream, CancellationToken cancellationToken = default)
    //    {
    //        if (stream is MemoryStream memory)
    //            return memory.ToArray();

    //        using var memoryStream = recyclableMemoryStreamManager.GetStream();
    //#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
    //        await stream.CopyToAsync(memoryStream, cancellationToken);
    //#else
    //        await stream.CopyToAsync(memoryStream, 81920, cancellationToken);
    //#endif
    //        return memoryStream.ToArray();
    //    }
    #endregion
}