using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

public class CreateBenchmarck 
{
    private const int items = 1_000_000;

    [Benchmark(Baseline = true)]
    public void CreateList()
    {
        List<int> list = Enumerable.Range(0, items).ToList();   
    }
    [Benchmark]
    public void CreateDictionary()
    {
        Dictionary<int, int> dictionary = Enumerable.Range(0, items).ToDictionary(x => x);
    }
    [Benchmark]
    public void CreateImmuntableDictionary()
    {
        ImmutableDictionary<int,int> immutableDictionary = Enumerable.Range(0, items).ToImmutableDictionary(x => x);
    }

    [Benchmark]
    public void CreateFrozenSet()
    {
        FrozenSet<int> frozenSet = Enumerable.Range(0,items).ToFrozenSet();
    }

    [Benchmark]
    public void CreateFrozenDictionary()
    {
        FrozenDictionary<int, int> fronzenDictionary = Enumerable.Range(0, items).ToFrozenDictionary(x=>x);
    }
}

public class LookupBechmarck 
{
    private const int items = 1_000_000;
    private const int iterations = 1_000;

    private readonly List<int> list = Enumerable.Range(0, items).ToList();
    private readonly Dictionary<int, int> dictionary = Enumerable.Range(0, items).ToDictionary(x => x);
    private readonly ImmutableDictionary<int, int> immutableDictionary = Enumerable.Range(0, items).ToImmutableDictionary(x => x);

    private readonly FrozenSet<int> frozenSet = Enumerable.Range(0, items).ToFrozenSet();
    private readonly FrozenDictionary<int, int> frozenDictionary = Enumerable.Range(0, items).ToFrozenDictionary(x => x);

    [Benchmark(Baseline = true)]
    public void LookupList()
    {
        for (var i = 0; i < iterations; i++)
            _ = list.Contains(i);
    }

    [Benchmark]
    public void LookupDictionary()
    {
        for (var i = 0; i < iterations; i++)
            _ = dictionary.ContainsKey(i);
    }

    [Benchmark]
    public void LookupImmutableDictionary()
    {
        for (var i = 0; i < iterations; i++)
            _ = immutableDictionary.ContainsKey(i);
    }

    [Benchmark]
    public void LookupFrozenSet()
    {
        for (var i = 0; i < iterations; i++)
            _ = frozenSet.Contains(i);
    }

    [Benchmark]
    public void LookupFrozenDictionary()
    {
        for (var i = 0; i < iterations; i++)
            _ = frozenDictionary.ContainsKey(i);
    }

}

public class Program
{
    public static void Main(String[] args)
    {
        var config = new ManualConfig()
            .WithOptions(ConfigOptions.DisableOptimizationsValidator)
            .AddValidator(JitOptimizationsValidator.DontFailOnError)
            .AddLogger(ConsoleLogger.Default)
            .AddColumnProvider(DefaultColumnProviders.Instance);

        BenchmarkRunner.Run<CreateBenchmarck>(config);
        BenchmarkRunner.Run<LookupBechmarck>(config);
    }
}
