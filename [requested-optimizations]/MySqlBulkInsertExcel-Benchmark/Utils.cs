using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace MySqlBulkInsertExcel_Benchmark;

public static class Utils
{
    public static async Task<byte[]> ReadAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        var bytes = new byte[stream.Length - stream.Position];
        await stream.ReadAsync((Memory<byte>)bytes, cancellationToken);
        return bytes;
    }

    public static MemoryStream MakeCopy(this MemoryStream stream)
    {
        var copyStream = new MemoryStream((int)stream.Length);
        stream.CopyTo(copyStream, (int)stream.Length);
        stream.Position = 0;
        copyStream.Position = 0;
        return copyStream;
    }

    public static async Task<List<MemoryStream>> ChunkAsync(this Stream stream, int maxLines)
    {
        var utf8Bytes = await stream.ReadAllBytesAsync();
        var streams = new List<MemoryStream>();

        var offset = 0;
        var lineCount = 0;
        for (int position = 0; position < utf8Bytes.Length; position++)
        {
            if (position + 1 == utf8Bytes.Length) //End of file
            {
                break;
            }

            if (lineCount == maxLines)
            {
                streams.Add(new MemoryStream(utf8Bytes, offset, position - offset));
                offset = position;
                lineCount = 0;
            }

            if (utf8Bytes[position] == 13 && utf8Bytes[position + 1] == 10) //It's a new "\r\n"
            {
                lineCount++;
                position++;
            }
        }

        if (utf8Bytes.Length != offset)
        {
            streams.Add(new MemoryStream(utf8Bytes, offset, utf8Bytes.Length - offset));
        }

        return streams;
    }

    public static List<long> ReadEachLineAsLong(this Stream stream)
    {
        var records = new List<long>();
        var tempList = new List<byte>();

        var previousByte = 0;
        int currentByte;
        while ((currentByte = stream.ReadByte()) > 0)
        {
            if (previousByte is 0)
            {
                previousByte = currentByte;
                tempList.Add((byte)previousByte);
                continue;
            }

            if (currentByte is not 13 and not 10)
            {
                tempList.Add((byte)currentByte);
            }

            if (previousByte is 13 && currentByte is 10) //It's a new "\r\n"
            {
                var number = ConvertUtf8BytesToLong(tempList);
                records.Add(number);
                tempList.Clear();
            }

            previousByte = currentByte;
        }

        return records;
    }

    public static List<long> ConvertEachLineToLong(Span<byte> utf8Bytes)
    {
        var records = new List<long>();
        var tempList = new List<byte>();
        for (int i = 0; i < utf8Bytes.Length; i++)
        {
            if (i + 1 == utf8Bytes.Length) //End of file
            {
                tempList.Add(utf8Bytes[i]);
                var number = ConvertUtf8BytesToLong(tempList);
                records.Add(number);
                break;
            }

            if (utf8Bytes[i] is 13 && utf8Bytes[i + 1] is 10) //It's a new "\r\n"
            {
                var number = ConvertUtf8BytesToLong(tempList);
                records.Add(number);
                tempList.Clear();
                i++;
            }
            else
            {
                tempList.Add(utf8Bytes[i]);
            }
        }
        return records;
    }

    public static long ConvertUtf8BytesToLong(List<byte> utf8Bytes)
    {
        long result = 0;

        foreach (var utf8Byte in utf8Bytes)
        {
            var chr = (char)utf8Byte;
            int digit = chr - '0'; // Convert char to its numeric value
            result = (result * 10) + digit;
        }

        return result;
    }

    public static int ExecuteNonQuery(this MySqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteNonQuery();
    }

    public static int ExecuteNonQuery(this MySql.Data.MySqlClient.MySqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        return command.ExecuteNonQuery();
    }

    public static async Task<int> ExecuteNonQueryAsync(this MySqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        return await command.ExecuteNonQueryAsync();
    }

    public static async Task<int> ExecuteNonQueryAsync(this MySql.Data.MySqlClient.MySqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        return await command.ExecuteNonQueryAsync();
    }

    public static ValueTask<MySqlBulkCopyResult> BulkInsertAsync(this MySqlConnection connection, DbDataReader reader, string tableName, Dictionary<int, string> mappings)
    {
        var bulkCopy = new MySqlBulkCopy(connection)
        {
            DestinationTableName = tableName,
            ConflictOption = MySqlBulkLoaderConflictOption.Ignore
        };

        foreach (var mapping in mappings)
        {
            bulkCopy.ColumnMappings.Add(new MySqlBulkCopyColumnMapping(mapping.Key, mapping.Value));
        }

        return bulkCopy.WriteToServerAsync(reader);
    }

    public static Task BulkInsertAsync(this MySqlConnection connection, Stream sourceStream, string tableName, string[] columns, string[] expressions)
    {
        var bulkLoader = new MySqlBulkLoader(connection)
        {
            ConflictOption = MySqlBulkLoaderConflictOption.Ignore,
            TableName = tableName,
            LineTerminator = "\r\n",
            SourceStream = sourceStream
        };

        foreach (var column in columns)
        {
            bulkLoader.Columns.Add(column);
        }

        foreach (var expression in expressions)
        {
            bulkLoader.Expressions.Add(expression);
        }

        return bulkLoader.LoadAsync();
    }

    public static Task BulkInsertAsync(this MySql.Data.MySqlClient.MySqlConnection connection, Stream sourceStream, string tableName, string[] columns, string[] expressions)
    {
        var bulkLoader = new MySql.Data.MySqlClient.MySqlBulkLoader(connection)
        {
            ConflictOption = MySql.Data.MySqlClient.MySqlBulkLoaderConflictOption.Ignore,
            TableName = tableName,
            LineTerminator = "\r\n"
        };

        foreach (var column in columns)
        {
            bulkLoader.Columns.Add(column);
        }

        foreach (var expression in expressions)
        {
            bulkLoader.Expressions.Add(expression);
        }

        return bulkLoader.LoadAsync(sourceStream);
    }

    public static MySqlConnection CreateNewConnection(this DatabaseFacade database)
    {
        var connection = database.GetConnectionString();
        return new MySqlConnection(connection);
    }

    public static MySql.Data.MySqlClient.MySqlConnection CreateNewMySqlDataConnection(this DatabaseFacade database)
    {
        var connection = database.GetConnectionString();
        return new MySql.Data.MySqlClient.MySqlConnection(connection);
    }

    public static TOutput[] ConvertAll<T, TOutput>(this T[] source, Converter<T, TOutput> converter)
    {
        var length = source.Length;
        var list = new TOutput[length]; //new List<TOutput>(length);

        for (int i = 0; i < length; i++)
        {
            list[i] = converter(source[i]);
        }

        return list;
    }
}
