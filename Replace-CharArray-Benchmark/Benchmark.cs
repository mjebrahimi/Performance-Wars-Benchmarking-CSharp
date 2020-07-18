using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Text;

namespace Replace_CharArray_Benchmark
{
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    public class Benchmark
    {
        [Benchmark(Baseline = true)]
        public void ToCharArray()
        {
            UsingToCharArray("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public void StringCreate()
        {
            UsingStringCreate("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public void StringBuilder()
        {
            UsingStringBuilder("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public void ReplaceChar()
        {
            UsingReplaceChar("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
        }

        [Benchmark]
        public void ReplaceString()
        {
            UsingReplaceString("abcdefghijklmnopqrstuvwxyz012345678901234567890123456789abcdefghijklmnopqrstuvwxyz");
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

        private static string UsingStringCreate(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;

            return string.Create(data.Length, data, (chars, context) =>
            {
                for (int i = 0; i < data.Length; i++)
                {
                    chars[i] = (data[i]) switch
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
                        _ => data[i],
                    };
                }
            });
        }

        private static string UsingToCharArray(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;

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
    }
}
