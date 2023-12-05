using BenchmarkDotNet.Running;
using System.Text;

//foreach (var str in new string[] { "a", "س" })
//{
//    var arr1 = WriterTest.GetBytes_UTF8(str);
//    var arr2 = WriterTest.GetBytes_UTF8_BOM(str);
//    var arr3 = WriterTest.GetBytes_UTF16_LE(str);
//    var arr4 = WriterTest.GetBytes_UTF16_BE(str);
//    var arr5 = WriterTest.GetBytes_UTF16_LE_BOM(str);
//    var arr6 = WriterTest.GetBytes_UTF16_BE_BOM(str);

//    var str1 = TestReader.GetString(arr1);
//    var str2 = TestReader.GetString(arr2);
//    var str3 = TestReader.GetString(arr3);
//    var str4 = TestReader.GetString(arr4);
//    var str5 = TestReader.GetString(arr5);
//    var str6 = TestReader.GetString(arr6);
//}

//var tempDirectory = Path.Combine(Path.GetTempPath(), "benchmark_temp");
//Directory.CreateDirectory(tempDirectory);

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig()); //For Debugging
BenchmarkRunner.Run<Benchmark>(new BenchmarkDotNet.Configs.DebugInProcessConfig());

#else

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
BenchmarkRunner.Run<Benchmark>();

#endif

//Cleanup temp files
//Directory.Delete(tempDirectory, true);

Console.ReadLine();

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