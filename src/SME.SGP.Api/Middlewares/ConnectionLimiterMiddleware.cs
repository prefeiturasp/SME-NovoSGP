using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Middlewares
{
    public class ConnectionLimiterMiddleware
    {
        // ... membros existentes (Limit, Key, _next)

        private readonly IDatabase _redisDb;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy; // Novo membro para a política

        public ConnectionLimiterMiddleware(RequestDelegate next, IConnectionMultiplexer redis,
                                          AsyncCircuitBreakerPolicy circuitBreakerPolicy) // Injeção da política
        {
            _next = next;
            _redisDb = redis.GetDatabase();
            _circuitBreakerPolicy = circuitBreakerPolicy;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            long currentConnections = 0;
            bool isCircuitOpen = false;

            try
            {
                // 1. **EXECUÇÃO COM POLÍTICA:** Envolve a lógica crítica do Redis
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    currentConnections = await _redisDb.StringIncrementAsync(Key);
                });
            }
            catch (BrokenCircuitException)
            {
                // 2. **CIRCUITO ABERTO:** Lógica de fallback imediato
                isCircuitOpen = true;
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Serviço de Limite (Redis) indisponível. Circuito Aberto. Tente novamente mais tarde.");
                return;
            }
            catch (Exception ex) when (ex is RedisConnectionException || ex is RedisTimeoutException)
            {
                // 3. **FALHA NO REDIS:** Se a exceção não quebrou o circuito, trate como 503
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Falha na conexão com o sistema de contagem (Redis). Tente novamente mais tarde.");
                return;
            }

            // Se o código chegou aqui, o acesso ao Redis foi bem-sucedido ou o circuito estava fechado/meio-aberto.
            if (!isCircuitOpen)
            {
                try
                {
                    // Lógica de Limite (como no exemplo anterior)
                    if (currentConnections > Limit)
                    {
                        // Lógica de decremento precisa ser protegida!
                        await DecrementProtectedByCircuitBreaker(Key);
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Limite de conexões atingido (429).");
                        return;
                    }

                    // Prosseguir com a requisição
                    await _next(context);
                }
                finally
                {
                    // Decremento garantido após o processamento
                    if (currentConnections <= Limit)
                    {
                        await DecrementProtectedByCircuitBreaker(Key);
                    }
                }
            }
        }

        // Método auxiliar para proteger o Decrement (para evitar duplicação de código)
        private async Task DecrementProtectedByCircuitBreaker(string key)
        {
            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    await _redisDb.StringDecrementAsync(key);
                });
            }
            catch (Exception ex) when (ex is BrokenCircuitException || ex is RedisConnectionException || ex is RedisTimeoutException)
            {
                // Logar a falha no decremento. Neste caso, a contagem ficará temporariamente 
                // inflacionada até que o circuito feche ou o serviço reinicie.
                Console.WriteLine($"[CRÍTICO] Falha ao decrementar o contador do Redis: {ex.Message}");
            }
        }
    }
}
