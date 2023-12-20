using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

/// <summary>
/// Like DebugInProcessConfig but uses DryJob instead of Default
/// </summary>
public class DebugInProcessConfigDry : DebugConfig
{
    public override IEnumerable<Job> GetJobs() => [
            Job.Dry // Job.Dry instead of Job.Default
                .WithToolchain(InProcessEmitToolchain.Instance) //InProcessNoEmitToolchain (A toolchain to run the benchmarks in-process (no emit))
        ];
}