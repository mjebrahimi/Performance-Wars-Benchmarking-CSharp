using BenchmarkDotNetVisualizer;
using Docker.DotNet;
using Docker.DotNet.Models;
using MySqlBulkInsertExcel_Benchmark;
using Testcontainers.MySql;

await using (var mySqlContainer = await RunMySqlTestContainer())
{
    var summary = BenchmarkAutoRunner.Run<Benchmark>();
    await summary.SaveAsImageAsync(
        path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
        options: new ReportHtmlOptions
        {
            Title = "MySQL Bulk Insert Benchmark",
            SpectrumColumns = ["Mean", "Allocated"],
            SortByColumns = ["Mean", "Allocated"],
            HighlightGroups = false
        });

    Console.ReadLine();

    await mySqlContainer.StopAsync();
}

static async Task<MySqlContainer> RunMySqlTestContainer()
{
    using var client = new DockerClientConfiguration().CreateClient();

    var images = await client.Images.ListImagesAsync(new ImagesListParameters());

    //Make sure you have the necessary images on your locale or pull them if not exist

    const string ryukImageName = "testcontainers/ryuk";
    const string ryukVersion = "0.6.0"; //Note the version is tightly coupled with the testcontainers version

    if (images.Any(i => i.RepoTags.Contains($"{ryukImageName}:{ryukVersion}")) is false)
    {
        await client.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = ryukImageName,
            Tag = ryukVersion
        }, new AuthConfig(), new Progress<JSONMessage>());
    }

    const string mySqlTag = "mysql";

    var mySqlImage = images.FirstOrDefault(i => i.RepoTags.Any(t => t.Contains(mySqlTag)));
    if (mySqlImage is null)
    {
        await client.Images.CreateImageAsync(new ImagesCreateParameters
        {
            FromImage = mySqlTag,
            Tag = "9.0.0"
        }, new AuthConfig(), new Progress<JSONMessage>());

        images = await client.Images.ListImagesAsync(new ImagesListParameters());
        mySqlImage = images.FirstOrDefault(i => i.RepoTags.Any(t => t.Contains(mySqlTag)));
    }

    var mySqlContainer = new MySqlBuilder()
        .WithImage(mySqlImage!.RepoTags[0])
        .WithDatabase("ExcelPerformance")
        .WithPassword("123")
        .WithUsername("root")
        .WithPortBinding("8086", "3306")
        .WithAutoRemove(true)
        .WithCleanUp(true)
        .Build();

    await mySqlContainer.StartAsync();

    //Create a checkpoint from database in clean state
    //var dbConnectionString mySqlContainer.GetConnectionString();

    return mySqlContainer;
}