using Dapper;
using EFCore.BulkExtensions;
using Microsoft.Extensions.DependencyInjection;
using RecordParser.Extensions;

namespace MySqlBulkInsertExcel_Benchmark;

public static class ExcelProcessor
{
    public static async Task RunOriginalAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            using StreamReader textReader = new(stream, leaveOpen: true);

            var readOptions = new VariableLengthReaderRawOptions
            {
                HasHeader = false,
                ContainsQuotedFields = false,
                ColumnCount = 1,
                Separator = ",",
                ParallelismOptions = new()
                {
                    Enabled = true,
                    EnsureOriginalOrdering = true,
                    MaxDegreeOfParallelism = 4
                }
            };

            var records = textReader.ReadRecordsRaw(readOptions, getField =>
            {
                var record = long.Parse(getField(0));
                return record;
            });

            var batchSize = 100000;
            var userBatch = new List<User>(100000);
            var tagBatch = new List<Tag>(100000);

            foreach (var userId in records)
            {
                userBatch.Add(new User { Id = userId, CreatedAt = DateTime.Now });
                tagBatch.Add(new Tag { UserId = userId, Name = excelTagName });
                //Console.WriteLine($"{userId} added");

                if (userBatch.Count >= batchSize)
                {
                    await dbContext.BulkInsertAsync(userBatch);
                    userBatch.Clear();
                }

                if (tagBatch.Count >= batchSize)
                {
                    await dbContext.BulkInsertAsync(tagBatch);
                    tagBatch.Clear();
                }
            }

            if (userBatch.Any())
            {
                await dbContext.BulkInsertAsync(userBatch);
            }

            if (tagBatch.Any())
            {
                await dbContext.BulkInsertAsync(tagBatch);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunOriginalOptimizedMultiThreadedAsync(Stream stream, string excelTagName, IServiceProvider services)
    {
        try
        {
            var records = stream.ReadEachLineAsLong();

            var recordsPerThread = (int)Math.Ceiling((double)records.Count / Environment.ProcessorCount);

            await Parallel.ForEachAsync(records.Chunk(recordsPerThread), async (chunk, __) =>
            {
                var users = chunk.ConvertAll(userId => new User { Id = userId, CreatedAt = DateTime.Now });
                var tags = chunk.ConvertAll(userId => new Tag { UserId = userId, Name = excelTagName });

                await using var scope = services.CreateAsyncScope();
                await using var dbContext = scope.ServiceProvider.GetRequiredService<EfDbContext>();
                await using var _ = await dbContext.DisableChecksAsync();

                var bulkConfig = new BulkConfig
                {
                    SetOutputIdentity = false,
                    SetOutputNonIdentityColumns = false,
                    PreserveInsertOrder = false,
                    BatchSize = chunk.Length,
                    EnableStreaming = true,
                    WithHoldlock = false,
                    CalculateStats = false,
                    OnSaveChangesSetFK = false,
                    IgnoreRowVersion = true,
                    SqlBulkCopyOptions = Microsoft.Data.SqlClient.SqlBulkCopyOptions.TableLock,
                };

                await dbContext.BulkInsertAsync(users, bulkConfig);
                await dbContext.BulkInsertAsync(tags, bulkConfig);

                //await dbContext.BulkInsertAsync(users);
                //await dbContext.BulkInsertAsync(tags);
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunDapperAOTOptimizedMultiThreadedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var records = stream.ReadEachLineAsLong();

            var recordsPerThread = (int)Math.Ceiling((double)records.Count / Environment.ProcessorCount);

            await Parallel.ForEachAsync(records.Chunk(recordsPerThread), async (chunk, __) =>
            {
                var users = chunk.ConvertAll(userId => new User { Id = userId, CreatedAt = DateTime.Now });
                var tags = chunk.ConvertAll(userId => new Tag { UserId = userId, Name = excelTagName });

                await using var connection = dbContext.Database.CreateNewConnection();
                await using var _ = await connection.DisableChecksAsync();

                await using var userDataReader = TypeAccessor.CreateDataReader(users);
                await connection.BulkInsertAsync(userDataReader, "users", new()
                {
                    [0] = "id",
                    [3] = "created_at"
                });

                await using var tagDataReader = TypeAccessor.CreateDataReader(tags);
                await connection.BulkInsertAsync(tagDataReader, "tags", new()
                {
                    [1] = "name",
                    [2] = "user_id"
                });
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunDapperAOTOptimizedMultiThreaded2xAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var records = stream.ReadEachLineAsLong();

            var recordsPerThread = (int)Math.Ceiling((double)records.Count / Environment.ProcessorCount);

            var tasks = new List<Task>();
            foreach (var chunk in records.Chunk(recordsPerThread))
            {
                var taskUser = Task.Run(async () =>
                {
                    var users = chunk.ConvertAll(userId => new User { Id = userId, CreatedAt = DateTime.Now });

                    await using var connection = dbContext.Database.CreateNewConnection();
                    await using var _ = await connection.DisableChecksAsync();

                    await using var userDataReader = TypeAccessor.CreateDataReader(users);
                    await connection.BulkInsertAsync(userDataReader, "users", new()
                    {
                        [0] = "id",
                        [3] = "created_at"
                    });
                });

                var taskTag = Task.Run(async () =>
                {
                    var tags = chunk.ConvertAll(userId => new Tag { UserId = userId, Name = excelTagName });

                    await using var connection = dbContext.Database.CreateNewConnection();
                    await using var _ = await connection.DisableChecksAsync();

                    await using var tagDataReader = TypeAccessor.CreateDataReader(tags);
                    await connection.BulkInsertAsync(tagDataReader, "tags", new()
                    {
                        [1] = "name",
                        [2] = "user_id"
                    });
                });

                tasks.Add(taskUser);
                tasks.Add(taskTag);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLConnectorBulkLoaderMultiThreadedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var streamChunks = await stream.ChunkAsync(100_000);

            await Parallel.ForEachAsync(streamChunks, async (usersChunkStream, __) =>
            {
                var tagsChunkStream = usersChunkStream.MakeCopy();

                await using var connection = dbContext.Database.CreateNewConnection();
                await using var _ = await connection.DisableChecksAsync();

                await connection.BulkInsertAsync(usersChunkStream, "users", ["id"], ["created_at = NOW(6)"]);
                await connection.BulkInsertAsync(tagsChunkStream, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLConnectorBulkLoaderMultiThreadedMemoryOptimizedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var chunks = await stream.ChunkDualAsync(100_000);

            await Parallel.ForEachAsync(chunks, async (chunk, __) =>
            {
                await using var connection = dbContext.Database.CreateNewConnection();
                await using var _ = await connection.DisableChecksAsync();
                await connection.BulkInsertAsync(chunk.Stream1, "users", ["id"], ["created_at = NOW(6)"]);
                await connection.BulkInsertAsync(chunk.Stream2, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLConnectorBulkLoaderMultiThreadedStreamOptimizedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var streamChunks = await stream.ChunkAsync(100_000);

            await Parallel.ForEachAsync(streamChunks, async (streamChunk, __) =>
            {
                using (streamChunk)
                {
                    var stream = new NonDisposableStream(streamChunk);
                    await using var connection = dbContext.Database.CreateNewConnection();
                    await using var _ = await connection.DisableChecksAsync();
                    await connection.BulkInsertAsync(stream, "users", ["id"], ["created_at = NOW(6)"]);
                    stream.Position = 0;
                    await connection.BulkInsertAsync(stream, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
                }

            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLConnectorBulkLoaderMultiThreaded2xAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var streamChunks = await stream.ChunkAsync(100_000);

            var tasks = new List<Task>();
            foreach (var usersChunkStream in streamChunks)
            {
                var tagsChunkStream = usersChunkStream.MakeCopy();

                var userTask = Task.Run(async () =>
                {
                    await using var connection = dbContext.Database.CreateNewConnection();
                    await using var _ = await connection.DisableChecksAsync();
                    await connection.BulkInsertAsync(usersChunkStream, "users", ["id"], ["created_at = NOW(6)"]);
                });

                var tagTask = Task.Run(async () =>
                {
                    await using var connection = dbContext.Database.CreateNewConnection();
                    await using var _ = await connection.DisableChecksAsync();
                    await connection.BulkInsertAsync(tagsChunkStream, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
                });

                tasks.Add(userTask);
                tasks.Add(tagTask);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLBulkLoaderMultiThreadedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var streamChunks = await stream.ChunkAsync(100_000);

            await Parallel.ForEachAsync(streamChunks, async (usersChunkStream, __) =>
            {
                var tagsChunkStream = usersChunkStream.MakeCopy();

                await using var connection = dbContext.Database.CreateNewMySqlDataConnection();
                await using var _ = await connection.DisableChecksAsync();
                await connection.BulkInsertAsync(usersChunkStream, "users", ["id"], ["created_at = NOW(6)"]);
                await connection.BulkInsertAsync(tagsChunkStream, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLBulkLoaderMultiThreadedMemoryOptimizedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var chunks = await stream.ChunkDualAsync(100_000);

            await Parallel.ForEachAsync(chunks, async (chunk, __) =>
            {
                await using var connection = dbContext.Database.CreateNewMySqlDataConnection();
                await using var _ = await connection.DisableChecksAsync();
                await connection.BulkInsertAsync(chunk.Stream1, "users", ["id"], ["created_at = NOW(6)"]);
                await connection.BulkInsertAsync(chunk.Stream2, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLBulkLoaderMultiThreadedStreamOptimizedAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var streamChunks = await stream.ChunkAsync(100_000);

            await Parallel.ForEachAsync(streamChunks, async (streamChunk, __) =>
            {
                using (streamChunk)
                {
                    var stream = new NonDisposableStream(streamChunk);
                    await using var connection = dbContext.Database.CreateNewMySqlDataConnection();
                    await using var _ = await connection.DisableChecksAsync();
                    await connection.BulkInsertAsync(stream, "users", ["id"], ["created_at = NOW(6)"]);
                    stream.Position = 0;
                    await connection.BulkInsertAsync(stream, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
                }
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }

    public static async Task RunMySQLBulkLoaderMultiThreaded2xAsync(Stream stream, string excelTagName, EfDbContext dbContext)
    {
        try
        {
            var streamChunks = await stream.ChunkAsync(100_000);

            var tasks = new List<Task>();
            foreach (var usersChunkStream in streamChunks)
            {
                var tagsChunkStream = usersChunkStream.MakeCopy();

                var userTask = Task.Run(async () =>
                {
                    await using var connection = dbContext.Database.CreateNewMySqlDataConnection();
                    await using var _ = await connection.DisableChecksAsync();
                    await connection.BulkInsertAsync(usersChunkStream, "users", ["id"], ["created_at = NOW(6)"]);
                });

                var tagTask = Task.Run(async () =>
                {
                    await using var connection = dbContext.Database.CreateNewMySqlDataConnection();
                    await using var _ = await connection.DisableChecksAsync();
                    await connection.BulkInsertAsync(tagsChunkStream, "tags", ["user_id"], [$"name = '{excelTagName}'"]);
                });

                tasks.Add(userTask);
                tasks.Add(tagTask);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            throw new Exception($"Excel operation failed: {ex.Message}", ex);
        }
    }
}