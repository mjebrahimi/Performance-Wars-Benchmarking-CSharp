// Ignore Spelling: Mediat

using BenchmarkDotNet.Attributes;
using BenchmarkDotNetVisualizer;
using Microsoft.Extensions.DependencyInjection;
using SlimMediator;

var summary = BenchmarkAutoRunner.Run<MediatorsBenchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "MediatR vs Mediator vs SlimMediator Benchmark",
        GroupByColumns = ["Categories"],
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = true,
        DividerMode = RenderTableDividerMode.EmptyDividerRow
    });

Console.ReadLine();

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)] //more accurate results
//[ShortRunJob]
#endif
[CategoriesColumn]
[MemoryDiagnoser(false)]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
public class MediatorsBenchmark
{
    private readonly SlimMediator.Example.CreateUserCommand slimMediator_CreateCommand = new();
    private readonly SlimMediator.Example.DeleteUserCommand slimMediator_DeletteCommand = new();

    private readonly MediatR.Example.CreateUserCommand mediatR_CreateCommand = new();
    private readonly MediatR.Example.DeleteUserCommand mediatR_DeleteCommand = new();

    private readonly Mediator.Example.CreateUserCommand mediator_CreateCommand = new();
    private readonly Mediator.Example.DeleteUserCommand mediator_DeleteCommand = new();

    private readonly IServiceProvider serviceProvider;

    public MediatorsBenchmark()
    {
        var services = new ServiceCollection();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSlimMediator(config =>
        {
            config.Lifetime = ServiceLifetime.Transient;
            config.RegisterServicesFromAssembly(typeof(MediatR.Example.CreateUserCommand).Assembly);
        });

        services.AddMediatR(config =>
        {
            config.Lifetime = ServiceLifetime.Transient;
            config.RegisterServicesFromAssembly(typeof(MediatR.Example.CreateUserCommand).Assembly);
        });

        services.AddMediator(config =>
        {
            config.ServiceLifetime = ServiceLifetime.Transient;
        });

        serviceProvider = services.BuildServiceProvider();
    }

#pragma warning disable AMNF0001 // Awaitable (Asynchronous) methods should be suffixed with 'Async'
    #region WithoutResponse
    [Benchmark(Description = "SlimMediator"), BenchmarkCategory("WithoutResponse")]
    public Task SlimMediator_WithoutResponse()
    {
        var sender = serviceProvider.GetRequiredService<ISender>();
        return sender.Send(slimMediator_DeletteCommand, CancellationToken.None);
    }

    [Benchmark(Description = "MediatR"), BenchmarkCategory("WithoutResponse")]
    public Task MediatR_WithoutResponse()
    {
        var sender = serviceProvider.GetRequiredService<MediatR.ISender>();
        return sender.Send(mediatR_DeleteCommand, CancellationToken.None);
    }

    [Benchmark(Description = "Mediator"), BenchmarkCategory("WithoutResponse")]
    public async Task Mediator_WithoutResponse()
    {
        var sender = serviceProvider.GetRequiredService<Mediator.ISender>();
        await sender.Send(mediator_DeleteCommand, CancellationToken.None);
    }
    #endregion

    #region WithResponse
    [Benchmark(Description = "SlimMediator"), BenchmarkCategory("WithResponse")]
    public Task<bool> SlimMediator_WithResponse()
    {
        var sender = serviceProvider.GetRequiredService<ISender>();
        return sender.Send(slimMediator_CreateCommand, CancellationToken.None);
    }

    [Benchmark(Description = "MediatR"), BenchmarkCategory("WithResponse")]
    public Task<bool> MediatR_WithResponse()
    {
        var sender = serviceProvider.GetRequiredService<MediatR.ISender>();
        return sender.Send(mediatR_CreateCommand, CancellationToken.None);
    }

    [Benchmark(Description = "Mediator"), BenchmarkCategory("WithResponse")]
    public async Task<bool> Mediator_WithResponse()
    {
        var sender = serviceProvider.GetRequiredService<Mediator.ISender>();
        return await sender.Send(mediator_CreateCommand, CancellationToken.None);
    }
    #endregion
#pragma warning restore AMNF0001 // Awaitable (Asynchronous) methods should be suffixed with 'Async'
}