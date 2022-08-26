using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using SME.SGP.Infra;
using System;

namespace SME.SGP.IoC
{
    public static class RegistrarPolicies
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            Random jitterer = new();
            var policyFila = Policy.Handle<Exception>()
              .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 30)));

            registry.Add(PoliticaPolly.PublicaFila, policyFila);

            var policySgp = Policy.Handle<Exception>()
              .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 30)));

            registry.Add(PoliticaPolly.SGP, policySgp);
        }
    }
}
