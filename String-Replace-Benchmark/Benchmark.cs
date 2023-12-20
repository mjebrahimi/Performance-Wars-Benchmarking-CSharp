using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using System;
using System.Text;

namespace Replace_CharArray_Benchmark
{
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser(displayGenColumns: false)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    public class Benchmark
    {
        [Benchmark]
        public string ToCharArray()
        {
            return UsingToCharArray("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public string StringCreate()
        {
            return UsingStringCreate("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public string Span()
        {
            return UsingSpan("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public string StringBuilder()
        {
            return UsingStringBuilder("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public string ReplaceChar()
        {
            return UsingReplaceChar("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public string ReplaceString()
        {
            return UsingReplaceString("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        public static string UsingReplaceString(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;
            return data
                .Replace("0", "\u06F0")
                .Replace("1", "\u06F1")
                .Replace("2", "\u06F2")
                .Replace("3", "\u06F3")
                .Replace("4", "\u06F4")
                .Replace("5", "\u06F5")
                .Replace("6", "\u06F6")
                .Replace("7", "\u06F7")
                .Replace("8", "\u06F8")
                .Replace("9", "\u06F9");
        }

        public static string UsingReplaceChar(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;
            return data
                .Replace('0', '\u06F0')
                .Replace('1', '\u06F1')
                .Replace('2', '\u06F2')
                .Replace('3', '\u06F3')
                .Replace('4', '\u06F4')
                .Replace('5', '\u06F5')
                .Replace('6', '\u06F6')
                .Replace('7', '\u06F7')
                .Replace('8', '\u06F8')
                .Replace('9', '\u06F9');
        }

        public static string UsingStringBuilder(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;

            var strBuilder = new StringBuilder(data);
            for (var i = 0; i < strBuilder.Length; i++)
            {
                strBuilder[i] = (strBuilder[i]) switch
                {
                    '0' => '\u06F0',
                    '1' => '\u06F1',
                    '2' => '\u06F2',
                    '3' => '\u06F3',
                    '4' => '\u06F4',
                    '5' => '\u06F5',
                    '6' => '\u06F6',
                    '7' => '\u06F7',
                    '8' => '\u06F8',
                    '9' => '\u06F9',
                    char c => c,
                };
            }

            return strBuilder.ToString();
        }

        public static string UsingStringCreate(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;

            return string.Create(data.Length, data, (chars, context) =>
            {
                for (int i = 0; i < context.Length; i++)
                {
                    chars[i] = (context[i]) switch
                    {
                        '0' => '\u06F0',
                        '1' => '\u06F1',
                        '2' => '\u06F2',
                        '3' => '\u06F3',
                        '4' => '\u06F4',
                        '5' => '\u06F5',
                        '6' => '\u06F6',
                        '7' => '\u06F7',
                        '8' => '\u06F8',
                        '9' => '\u06F9',
                        _ => context[i],
                    };
                }
            });
        }

        public static string UsingToCharArray(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;

            var chars = data.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = (chars[i]) switch
                {
                    '0' => '\u06F0',
                    '1' => '\u06F1',
                    '2' => '\u06F2',
                    '3' => '\u06F3',
                    '4' => '\u06F4',
                    '5' => '\u06F5',
                    '6' => '\u06F6',
                    '7' => '\u06F7',
                    '8' => '\u06F8',
                    '9' => '\u06F9',
                    _ => chars[i],
                };
            }

            return new string(chars);
        }

        public static string UsingSpan(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return data;

            Span<char> span = data.ToCharArray();
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = (span[i]) switch
                {
                    '0' => '\u06F0',
                    '1' => '\u06F1',
                    '2' => '\u06F2',
                    '3' => '\u06F3',
                    '4' => '\u06F4',
                    '5' => '\u06F5',
                    '6' => '\u06F6',
                    '7' => '\u06F7',
                    '8' => '\u06F8',
                    '9' => '\u06F9',
                    _ => span[i],
                };
            }

            return new string(span);
        }
    }
}
