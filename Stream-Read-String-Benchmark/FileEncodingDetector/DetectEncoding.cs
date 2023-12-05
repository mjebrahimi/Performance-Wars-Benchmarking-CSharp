using System.Text;

namespace FileEncodingDetector;

public static class DetectEncoding
{
    public static void Detect()
    {
        foreach (var str in new string[] { "a", "س" })
        {
            var utf8 = WriterTest.GetBytes_UTF8(str);
            var utf8BOM = WriterTest.GetBytes_UTF8_BOM(str);
            var utf16LE = WriterTest.GetBytes_UTF16_LE(str);
            var utf16BE = WriterTest.GetBytes_UTF16_BE(str);
            var utf16LEBOM = WriterTest.GetBytes_UTF16_LE_BOM(str);
            var utf16BEBOM = WriterTest.GetBytes_UTF16_BE_BOM(str);

            {
                //var aaa1 = TestReader.GetString(utf8);
                //var aaa2 = TestReader.GetString(utf8BOM);
                //var aaa3 = TestReader.GetString(utf16LE);
                //var aaa4 = TestReader.GetString(utf16BE);
                //var aaa5 = TestReader.GetString(utf16LEBOM);
                //var aaa6 = TestReader.GetString(utf16BEBOM);
            }
            {
                var aaa1 = TextFileEncodingDetector.DetectTextByteArrayEncoding(utf8);
                var aaa2 = TextFileEncodingDetector.DetectTextByteArrayEncoding(utf8BOM);
                var aaa3 = TextFileEncodingDetector.DetectTextByteArrayEncoding(utf16LE);
                var aaa4 = TextFileEncodingDetector.DetectTextByteArrayEncoding(utf16BE);
                var aaa5 = TextFileEncodingDetector.DetectTextByteArrayEncoding(utf16LEBOM);
                var aaa6 = TextFileEncodingDetector.DetectTextByteArrayEncoding(utf16BEBOM);
            }
            {
                var aaa1 = EncodingUtilities.DetectEncoding(new MemoryStream(utf8));
                var aaa2 = EncodingUtilities.DetectEncoding(new MemoryStream(utf8BOM));
                var aaa3 = EncodingUtilities.DetectEncoding(new MemoryStream(utf16LE));
                var aaa4 = EncodingUtilities.DetectEncoding(new MemoryStream(utf16BE));
                var aaa5 = EncodingUtilities.DetectEncoding(new MemoryStream(utf16LEBOM));
                var aaa6 = EncodingUtilities.DetectEncoding(new MemoryStream(utf16BEBOM));
            }
        }
    }
}

public static class TestReader
{
    public static string GetString(byte[] bytes)
    {
        using var memoryStream = new MemoryStream(bytes);
        using var streamReader = new StreamReader(memoryStream, detectEncodingFromByteOrderMarks: true);
        return streamReader.ReadToEnd();
    }
}

public static class WriterTest
{
    public static byte[] GetBytes_UTF8(string str)
    {
        var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false); //Without BOM encoding
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, utf8);
        writer.Write(str);
        writer.Flush();
        return memoryStream.ToArray();
    }

    public static byte[] GetBytes_UTF8_BOM(string str)
    {
        var utf8_BOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: false); //With BOM encoding
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, utf8_BOM);
        writer.Write(str);
        writer.Flush();
        return memoryStream.ToArray();
    }

    public static byte[] GetBytes_UTF16_LE(string str)
    {
        var utf16_LE = new UnicodeEncoding(bigEndian: false, byteOrderMark: false); //With BOM encoding
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, utf16_LE);
        writer.Write(str);
        writer.Flush();
        return memoryStream.ToArray();
    }

    public static byte[] GetBytes_UTF16_BE(string str)
    {
        var utf16_BE = new UnicodeEncoding(bigEndian: true, byteOrderMark: false); //With BOM encoding
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, utf16_BE);
        writer.Write(str);
        writer.Flush();
        return memoryStream.ToArray();
    }

    public static byte[] GetBytes_UTF16_LE_BOM(string str)
    {
        var utf16_LE_BOM = new UnicodeEncoding(bigEndian: false, byteOrderMark: true); //With BOM encoding
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, utf16_LE_BOM);
        writer.Write(str);
        writer.Flush();
        return memoryStream.ToArray();
    }

    public static byte[] GetBytes_UTF16_BE_BOM(string str)
    {
        var utf16_BE_BOM = new UnicodeEncoding(bigEndian: true, byteOrderMark: true); //With BOM encoding
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream, utf16_BE_BOM);
        writer.Write(str);
        writer.Flush();
        return memoryStream.ToArray();
    }
}

public static class EncoderTest
{
    public static byte[] GetBytes_UTF8(string str)
    {
        var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false); //Without BOM encoding
        return utf8.GetBytes(str);
    }

    public static byte[] GetBytes_UTF8_BOM(string str)
    {
        var utf8_BOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: false); //With BOM encoding
        return utf8_BOM.GetBytes(str);
    }

    public static byte[] GetBytes_UTF16_LE(string str)
    {
        var utf16_LE = new UnicodeEncoding(bigEndian: false, byteOrderMark: true); //With BOM encoding
        return utf16_LE.GetBytes(str);
    }

    public static byte[] GetBytes_UTF16_BE(string str)
    {
        var utf16_BE = new UnicodeEncoding(bigEndian: true, byteOrderMark: true); //With BOM encoding
        return utf16_BE.GetBytes(str);
    }

    public static byte[] GetBytes_UTF16_LE_BOM(string str)
    {
        var utf16_LE_BOM = new UnicodeEncoding(bigEndian: false, byteOrderMark: true); //With BOM encoding
        return utf16_LE_BOM.GetBytes(str);
    }

    public static byte[] GetBytes_UTF16_BE_BOM(string str)
    {
        var utf16_BE_BOM = new UnicodeEncoding(bigEndian: true, byteOrderMark: true); //With BOM encoding
        return utf16_BE_BOM.GetBytes(str);
    }
}