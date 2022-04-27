using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args) 
            => BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .AddExporter(JsonExporter.Full)
                    .AddJob(Job.ShortRun.AsDefault()));
    }
}
