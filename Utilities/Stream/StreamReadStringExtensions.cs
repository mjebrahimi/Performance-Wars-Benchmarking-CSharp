using System.Text;

public static class StreamReadStringExtensions
{
    #region UsingBinaryReader_ReadString (Has BUG! - Not Asyncable) - Throws exception for 50 text length and returns a 76 characters string for 500 text lengths
    public static string UsingBinaryReader_ReadString(this Stream stream)
    {
        using var binaryReader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        return binaryReader.ReadString();
    }
    #endregion

    #region UsingStreamReader (Bad Performance - Asyncable)
    public static string UsingStreamReader(this Stream stream, bool defaultBufferSize = false, bool detectEncoding = true)
    {
        var bufferSize = defaultBufferSize ? -1 /*DefaultBufferSize = 1024*/ : (int)stream.Length;
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: detectEncoding, bufferSize: bufferSize, leaveOpen: true);
        return reader.ReadToEnd();
    }

    public static async Task<string> UsingStreamReaderAsync(this Stream stream, bool defaultBufferSize = false, bool detectEncoding = true, CancellationToken cancellationToken = default)
    {
        var bufferSize = defaultBufferSize ? -1 /*DefaultBufferSize = 1024*/ : (int)stream.Length;
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: detectEncoding, bufferSize: bufferSize, leaveOpen: true);
        return await reader.ReadToEndAsync(cancellationToken);
    }
    #endregion

    #region ReadBytesConvertToUTF8_GetString (Best Performance - Asyncable)
    public static string ReadBytesConvertToUTF8_GetString(this Stream stream)
    {
        var bytes = stream.ReadAllBytes();
        return Encoding.UTF8.GetString(bytes);
    }

    public static async Task<string> ReadBytesConvertToUTF8_GetStringAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = await stream.ReadAllBytesAsync(cancellationToken);
        return Encoding.UTF8.GetString(bytes);
    }
    #endregion

    #region ReadBytesConvertToUTF8_Encoding (Medium Performance - Asyncable)
    public static string ReadBytesConvertToUTF8_Encoding(this Stream stream)
    {
        var bytes = stream.ReadAllBytes();
        return EncodingGetChars(bytes);
    }

    public static async Task<string> ReadBytesConvertToUTF8_EncodingAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = await stream.ReadAllBytesAsync(cancellationToken);
        return EncodingGetChars(bytes);
    }

    private static string EncodingGetChars(byte[] bytes)
    {
        var charCount = Encoding.UTF8.GetCharCount(bytes);
        var chars = GC.AllocateUninitializedArray<char>(charCount);
        Encoding.UTF8.GetChars(bytes, chars);
        return new(chars);
    }

    public static string ReadBytesConvertToUTF8_Encoding_SpanTrim(this Stream stream)
    {
        var bytes = stream.ReadAllBytes();
        var charCount = Encoding.UTF8.GetMaxCharCount(bytes.Length);
        Span<char> chars = new char[charCount];
        Encoding.UTF8.GetChars(bytes, chars);
        var trimmedChars = chars.TrimEnd('\0');
        return new(trimmedChars);
    }

    public static string ReadBytesConvertToUTF8_Encoding_ArrayResize(this Stream stream)
    {
        var bytes = stream.ReadAllBytes();
        var charCount = Encoding.UTF8.GetMaxCharCount(bytes.Length);
        var chars = GC.AllocateUninitializedArray<char>(charCount);
        var charsRead = Encoding.UTF8.GetChars(bytes, chars);
        Array.Resize(ref chars, charsRead);
        return new(chars);
    }

    #region Has Bug (because UTF8 is variable-width and buffering/chunking it might corrupt characters)
    //private const int _maxBufferSize = 1024; //4096
    //public static string ReadBytesConvertToUTF8_Encoding_ArrayResize_WithBuffer(this Stream stream, int maxBufferSize = 1024)
    //{
    //    var streamLength = (int)stream.Length;
    //    var charCount = Encoding.UTF8.GetMaxCharCount(streamLength);
    //    var chars = GC.AllocateUninitializedArray<char>(charCount);
    //    int charsRead = 0;

    //    var length = Math.Min(streamLength, maxBufferSize);
    //    byte[] buffer = GC.AllocateUninitializedArray<byte>(length);

    //    int bytesRead;
    //    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
    //    {
    //        charsRead += Encoding.UTF8.GetChars(buffer, 0, bytesRead, chars, charsRead);
    //    }

    //    Array.Resize(ref chars, charsRead);
    //    return new(chars);
    //}
    //public static string ReadBytesConvertToUTF8_Encoding_ArrayResize_WithBufferArrayPool(this Stream stream, int maxBufferSize = 1024)
    //{
    //    var streamLength = (int)stream.Length;
    //    var charCount = Encoding.UTF8.GetMaxCharCount(streamLength);
    //    var chars = GC.AllocateUninitializedArray<char>(charCount);
    //    int charsRead = 0;

    //    var length = Math.Min(streamLength, maxBufferSize);
    //    byte[] buffer = ArrayPool<byte>.Shared.Rent(length);

    //    try
    //    {
    //        int bytesRead;
    //        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
    //        {
    //            charsRead += Encoding.UTF8.GetChars(buffer, 0, bytesRead, chars, charsRead);
    //        }
    //    }
    //    finally
    //    {
    //        ArrayPool<byte>.Shared.Return(buffer);
    //    }

    //    Array.Resize(ref chars, charsRead);
    //    return new(chars);
    //}
    #endregion
    #endregion

    #region ReadBytesConvertToUTF8_Decoder (Bad Performance - Asyncable)
    public static string ReadBytesConvertToUTF8_Decoder(this Stream stream)
    {
        var bytes = stream.ReadAllBytes();
        return DecoderGetChars(bytes);
    }

    public static async Task<string> ReadBytesConvertToUTF8_DecoderAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = await stream.ReadAllBytesAsync(cancellationToken);
        return DecoderGetChars(bytes);
    }

    private static string DecoderGetChars(byte[] bytes)
    {
        var decoder = Encoding.UTF8.GetDecoder();
        var charCount = decoder.GetCharCount(bytes, flush: false); // flush value doesn't matter
        var chars = GC.AllocateUninitializedArray<char>(charCount);
        decoder.GetChars(bytes, chars, flush: false); // flush value doesn't matter
        return new(chars);
    }
    #endregion
}