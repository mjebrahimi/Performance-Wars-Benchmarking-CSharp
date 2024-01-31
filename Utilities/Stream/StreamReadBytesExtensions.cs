using Microsoft.IO;
using System.Text;

public static class StreamReadBytesExtensions
{
    public static byte[] GetTrimmedBuffer(this MemoryStream stream)
    {
        var length = (int)stream.Length;
        var bytes = stream.GetBuffer();
        if (length < bytes.Length)
            Array.Resize(ref bytes, length);
        return bytes;
    }

    #region UsingBinaryReader_ReadBytes (Best Performance - Not Asyncable)
    public static byte[] UsingBinaryReader_ReadBytes(this Stream stream)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        using var binaryReader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        return binaryReader.ReadBytes((int)stream.Length);
    }
    #endregion

    #region ReadAllBytes (Best Performance - Asyncable)
    public static byte[] ReadAllBytes(this Stream stream)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        var bytes = GC.AllocateUninitializedArray<byte>((int)stream.Length);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
        stream.Read(bytes);
#else
        stream.Read(bytes, 0, bytes.Length);
#endif
        return bytes;
    }

    public static async Task<byte[]> ReadAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        var bytes = GC.AllocateUninitializedArray<byte>((int)stream.Length);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
        await stream.ReadAsync(bytes, cancellationToken);
#else
        await stream.ReadAsync(bytes, 0, bytes.Length, cancellationToken);
#endif
        return bytes;
    }
    #endregion

    #region UsingMemoryStream (Bad Performance - Asyncable)
    public static byte[] UsingMemoryStream(this Stream stream)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream); //DefaultCopyBufferSize = 81920
        return memoryStream.ToArray();
    }

    public static byte[] UsingMemoryStreamOptimized(this Stream stream)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        var length = (int)stream.Length;
        using var memoryStream = new MemoryStream(capacity: length);
        stream.CopyTo(memoryStream, length);
        return memoryStream.GetBuffer();
    }

    public static async Task<byte[]> UsingMemoryStreamAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        using var memoryStream = new MemoryStream();
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
        await stream.CopyToAsync(memoryStream, cancellationToken); //bufferSize = 81920
#else
        await stream.CopyToAsync(memoryStream, bufferSize: 81920, cancellationToken);
#endif
        return memoryStream.ToArray();
    }
    #endregion

    #region UsingRecyclableMemoryStream (Bad Performance - Asyncable)
    private readonly static RecyclableMemoryStreamManager recyclableMemoryStreamManager = new();
    public static byte[] UsingRecyclableMemoryStream(this Stream stream)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        using var memoryStream = recyclableMemoryStreamManager.GetStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public static async Task<byte[]> UsingRecyclableMemoryStreamAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memory)
            return memory.ToArray();

        using var memoryStream = recyclableMemoryStreamManager.GetStream();
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
        await stream.CopyToAsync(memoryStream, cancellationToken);
#else
        await stream.CopyToAsync(memoryStream, 81920, cancellationToken);
#endif
        return memoryStream.ToArray();
    }
    #endregion
}