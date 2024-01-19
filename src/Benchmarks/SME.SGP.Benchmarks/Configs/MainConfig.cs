using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Validators;

namespace SME.SGP.Benchmarks
{
    public class MainConfig : ManualConfig
    {
        public MainConfig()
        {
            AddValidator(JitOptimizationsValidator.DontFailOnError); // ALLOW NON-OPTIMIZED DLLS            
        }
    }
}
