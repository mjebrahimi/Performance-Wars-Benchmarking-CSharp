using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replace_CharArray_Benchmark
{
    public static class ext
    {
        public static string UsingSpan2(this ReadOnlySpan<char> data)
        {
            if (data.IsWhiteSpace()) return string.Empty;

            Span<char> span = stackalloc char[data.Length]; // DOES NOT heap allocate

            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '0':
                        span[i] = '\u06F0';
                        break;

                    case '1':
                        span[i] = '\u06F1';
                        break;

                    case '2':
                        span[i] = '\u06F2';
                        break;

                    case '3':
                        span[i] = '\u06F3';
                        break;

                    case '4':
                        span[i] = '\u06F4';
                        break;

                    case '5':
                        span[i] = '\u06F5';
                        break;

                    case '6':
                    case '\u0666':
                        span[i] = '\u06F6';
                        break;

                    case '7':
                        span[i] = '\u06F7';
                        break;

                    case '8':
                        span[i] = '\u06F8';
                        break;

                    case '9':
                        span[i] = '\u06F9';
                        break;

                    default:
                        span[i] = data[i];
                        break;
                }
            }

            return new string(span);
        }
    }

    //[ShortRunJob]
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    public class Benchmark
    {
        [Benchmark]
        public void ReplaceString()
        {
            UsingReplaceString("012345678901234567890123456789");
        }

        [Benchmark]
        public void ReplaceChar()
        {
            UsingReplaceChar("012345678901234567890123456789");
        }

        [Benchmark]
        public void StringBuilder()
        {
            UsingStringBuilder("012345678901234567890123456789");
        }

        [Benchmark]
        public void CharArray()
        {
            UsingCharArray("012345678901234567890123456789");
        }

        [Benchmark(Baseline = true)]
        public void Span()
        {
            UsingSpan("012345678901234567890123456789");
        }

        private static string UsingReplaceString(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
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

        private static string UsingReplaceChar(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
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

        private static string UsingStringBuilder(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;

            var strBuilder = new StringBuilder(data);
            for (var i = 0; i < strBuilder.Length; i++)
            {
                switch (strBuilder[i])
                {
                    case '0':
                        strBuilder[i] = '\u06F0';
                        break;

                    case '1':
                        strBuilder[i] = '\u06F1';
                        break;

                    case '2':
                        strBuilder[i] = '\u06F2';
                        break;

                    case '3':
                        strBuilder[i] = '\u06F3';
                        break;

                    case '4':
                        strBuilder[i] = '\u06F4';
                        break;

                    case '5':
                        strBuilder[i] = '\u06F5';
                        break;

                    case '6':
                        strBuilder[i] = '\u06F6';
                        break;

                    case '7':
                        strBuilder[i] = '\u06F7';
                        break;

                    case '8':
                        strBuilder[i] = '\u06F8';
                        break;

                    case '9':
                        strBuilder[i] = '\u06F9';
                        break;

                    default:
                        strBuilder[i] = strBuilder[i];
                        break;
                }
            }

            return strBuilder.ToString();
        }

        private static string UsingCharArray(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;

            var letters = data.ToCharArray();
            for (var i = 0; i < letters.Length; i++)
            {
                switch (letters[i])
                {
                    case '0':
                        letters[i] = '\u06F0';
                        break;

                    case '1':
                        letters[i] = '\u06F1';
                        break;

                    case '2':
                        letters[i] = '\u06F2';
                        break;

                    case '3':
                        letters[i] = '\u06F3';
                        break;

                    case '4':
                        letters[i] = '\u06F4';
                        break;

                    case '5':
                        letters[i] = '\u06F5';
                        break;

                    case '6':
                    case '\u0666':
                        letters[i] = '\u06F6';
                        break;

                    case '7':
                        letters[i] = '\u06F7';
                        break;

                    case '8':
                        letters[i] = '\u06F8';
                        break;

                    case '9':
                        letters[i] = '\u06F9';
                        break;

                    default:
                        letters[i] = letters[i];
                        break;
                }
            }

            return new string(letters);
        }

        private static string UsingSpan(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;

            Span<char> span = stackalloc char[data.Length]; // DOES NOT heap allocate

            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '0':
                        span[i] = '\u06F0';
                        break;

                    case '1':
                        span[i] = '\u06F1';
                        break;

                    case '2':
                        span[i] = '\u06F2';
                        break;

                    case '3':
                        span[i] = '\u06F3';
                        break;

                    case '4':
                        span[i] = '\u06F4';
                        break;

                    case '5':
                        span[i] = '\u06F5';
                        break;

                    case '6':
                    case '\u0666':
                        span[i] = '\u06F6';
                        break;

                    case '7':
                        span[i] = '\u06F7';
                        break;

                    case '8':
                        span[i] = '\u06F8';
                        break;

                    case '9':
                        span[i] = '\u06F9';
                        break;

                    default:
                        span[i] = data[i];
                        break;
                }
            }

            return new string(span);
        }
    }
}
