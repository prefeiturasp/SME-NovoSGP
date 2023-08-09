using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SME.SGP.Infra;

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

        //nao sei se jitter pra esse caso do polly precisaria ser considerado
        //pela politica de execucao exponencial o tempo a cada tentativa.
        //Eu sugeriria escala linear ou logaritmica e nao exponencial para o usuario
        //nao ter a sensacao que o sistema esta demorando muito pra responder
        private static TimeSpan WithRetryAttempt(int retryAttempt)
        {
            var jitter = ConcurrentRandom.Next(0, 30);
            var exponencialValue = Math.Pow(2, retryAttempt);
            return TimeSpan.FromSeconds(exponencialValue) + TimeSpan.FromMilliseconds(jitter);
        }
    }

    //random no .net5 não é thread safe e pode retornar varios calculos com zero para Next,
    //o ideal seria uma instancia por thread no minimo. no .net6 tem o Random.Shared
    public static class ConcurrentRandom
    {
        [ThreadStatic] private static Random? _random;
        private static Random Instance => _random ??= new Random();
        public static int Next(int minValue, int maxValue)
        {
            return Instance.Next(minValue, maxValue);
        }
    }
}
