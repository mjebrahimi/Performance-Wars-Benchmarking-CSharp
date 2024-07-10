using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MySqlBulkInsertExcel_Benchmark;

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, iterationCount: 1)]
#endif
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Benchmark()
{
    private static readonly IServiceProvider serviceProvider;
    private static readonly EfDbContext dbContext;
    private const string connection = "Server=localhost;Port=8085;Database=ExcelPerformance;User=root;Password=123;AllowLoadLocalInfile=true; MinimumPoolSize=32;";

    static Benchmark()
    {
        ThreadPool.SetMinThreads(Environment.ProcessorCount * 2, Environment.ProcessorCount * 2);

        var services = new ServiceCollection();

        services.AddDbContext<EfDbContext>(options =>
            options.UseMySql(connection, ServerVersion.AutoDetect(connection),
                mysqlOptions =>
                {
                    mysqlOptions.EnableRetryOnFailure();
                    mysqlOptions.CommandTimeout(30);
                }
            ).UseSnakeCaseNamingConvention());

        serviceProvider = services.BuildServiceProvider();

        dbContext = serviceProvider.GetRequiredService<EfDbContext>();

        dbContext.Database.Migrate();
    }

    [IterationSetup]
    public void TruncateTables()
    {
        dbContext.Database.ExecuteSqlRaw("""
            SET FOREIGN_KEY_CHECKS=0;
            TRUNCATE TABLE tags;
            TRUNCATE TABLE users;
            SET FOREIGN_KEY_CHECKS=1;
            """);
    }

    [Benchmark]
    public Task Original() => ExcelProcessor.RunOriginalAsync(GetStream(), "tag-name", dbContext);

    [Benchmark]
    public Task OriginalOptimizedMultiThreaded() => ExcelProcessor.RunOriginalOptimizedMultiThreadedAsync(GetStream(), "tag-name", serviceProvider);

    [Benchmark]
    public Task DapperAOTOptimizedMultiThreaded() => ExcelProcessor.RunDapperAOTOptimizedMultiThreadedAsync(GetStream(), "tag-name", dbContext);

    [Benchmark]
    public Task DapperAOTOptimizedMultiThreaded2x() => ExcelProcessor.RunDapperAOTOptimizedMultiThreaded2xAsync(GetStream(), "tag-name", dbContext);

    [Benchmark]
    public Task MySQLConnectorBulkLoaderMultiThreaded() => ExcelProcessor.RunMySQLConnectorBulkLoaderMultiThreadedAsync(GetStream(), "tag-name", dbContext);

    [Benchmark]
    public Task MySQLConnectorBulkLoaderMultiThreaded2x() => ExcelProcessor.RunMySQLConnectorBulkLoaderMultiThreaded2xAsync(GetStream(), "tag-name", dbContext);

    [Benchmark]
    public Task MySQLBulkLoaderMultiThreaded() => ExcelProcessor.RunMySQLBulkLoaderMultiThreadedAsync(GetStream(), "tag-name", dbContext);

    [Benchmark]
    public Task MySQLBulkLoaderMultiThreaded2x() => ExcelProcessor.RunMySQLBulkLoaderMultiThreaded2xAsync(GetStream(), "tag-name", dbContext);

    private Stream GetStream() => File.OpenRead(Path.Combine(AppContext.BaseDirectory, "Csv", "Book1.csv"));
}