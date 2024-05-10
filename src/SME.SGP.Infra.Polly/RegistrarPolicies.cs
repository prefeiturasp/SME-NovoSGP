using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using SME.SGP.Infra;
using System;
using System.Runtime.ConstrainedExecution;

namespace SME.SGP.IoC
{
    public static class RegistrarPolicies
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            var policyRegistry = services.AddPolicyRegistry();
            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, WithRetryAttempt);
            policyRegistry.Add(PoliticaPolly.PublicaFila, policy);
            policyRegistry.Add(PoliticaPolly.SGP, policy);
        }
        
        private static TimeSpan WithRetryAttempt(int retryAttempt)
        {
            var jitter = ConcurrentRandom.Next(0, 30);
            var exponencialValue = Math.Pow(2, retryAttempt);
            return TimeSpan.FromSeconds(exponencialValue) + TimeSpan.FromMilliseconds(jitter);
        }
    }

    public static class ConcurrentRandom
    {
        [ThreadStatic] private static Random? _random;
        private static Random Instance => _random ??= new Random();
        public static int Next(int minValue, int maxValue) => Instance.Next(minValue, maxValue);
    }
}
