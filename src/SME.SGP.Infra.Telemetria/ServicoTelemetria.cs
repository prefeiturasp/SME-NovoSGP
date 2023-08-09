using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoTelemetria : IServicoTelemetria
    {
        private readonly TelemetryClient insightsClient;
        private readonly TelemetriaOptions telemetriaOptions;

        public ServicoTelemetria(TelemetryClient insightsClient, IOptions<TelemetriaOptions> telemetriaOptions)
        {
            this.insightsClient = insightsClient;
            this.telemetriaOptions = telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));
        }

        public Task<T> RegistrarComRetornoAsync<T>(Func<Task<T>> acao, string acaoNome, string telemetriaNome,
            string telemetriaValor, string parametros = "")
        {
            return RegistrarComRetornoAsync(acao(), acaoNome, telemetriaNome, telemetriaValor, parametros);
        }

        private async Task<T> RegistrarComRetornoAsync<T>(Task<T> acao, string acaoNome, string telemetriaNome,
            string telemetriaValor, string parametros = "")
        {
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            T result = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                await transactionElk.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.SetLabel("Parametros", parametros);
                    result = await acao;
                });
            }
            else
            {
                result = await acao;
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();

                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao,
                    temporizador.Elapsed, true);
            }

            return result;
        }

        public Task<T> RegistrarComRetornoAsync<T1, T2, T3, T>(T1 t1, T2 t2, T3 t3, Func<T1, T2, T3, Task<T>> acao,
            string acaoNome, string telemetriaNome,
            string telemetriaValor, string parametros = "")
        {
            return RegistrarComRetornoAsync(acao(t1, t2, t3), acaoNome, telemetriaNome, telemetriaValor, parametros);
        }

        public T RegistrarComRetorno<T>(Func<T> acao, string acaoNome, string telemetriaNome, string telemetriaValor,
            string parametros = "")
        {
            //A implemementacao principal esta no metodo de async e aqui so repete tudo que esta no async so que executando a funcao sincronamente
            //compensaria chamar o metodo async aqui bloqueando a execucao do resultado para nao duplicar codigo
            return RegistrarComRetornoAsync(() => Task.FromResult(acao()), acaoNome, telemetriaNome, telemetriaValor,
                parametros).GetAwaiter().GetResult();
        }

        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            //Mesmo caso aqui toda implementacao esta no metodo de async
            //compensaria chamar o metodo async aqui bloqueando a execucao do resultado para nao duplicar codigo
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                transactionElk.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    acao();
                });
            }
            else
            {
                acao();
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();
                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao,
                    temporizador.Elapsed, true);
            }
        }

        public async Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome,
            string telemetriaValor, string parametros = "")
        {
            var valueStopwatch = ValueStopwatch.StartNew();

            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                if (transactionElk != null)
                {
                    await transactionElk.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                    {
                        span.SetLabel(telemetriaNome, telemetriaValor);
                        span.SetLabel("Parametros", parametros);
                        await acao();
                    });
                }
                else
                    await acao();                
            }
            else
                await acao();

            if (telemetriaOptions.ApplicationInsights)
            {
                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor,
                    valueStopwatch.StartedAt, valueStopwatch.Elapsed, true);
            }
        }

        public ITransaction? Iniciar(string nome, string tipo)
        {
            return  telemetriaOptions.Apm ?
                Agent.Tracer.StartTransaction(nome, tipo) :
                null;
        }

        public void Finalizar(ITransaction? transacao)
        {
            if (telemetriaOptions.Apm)
                transacao?.End();
        }

        //Se quiser evitar alocacao de stop watch em toda telemetria e nao precisar nada muito avancado de stop watch
        //da pra criar algo do tipo acessivel a partes do projeto
        public readonly struct ValueStopwatch
        {
            private static readonly double Ticks = (double) TimeSpan.TicksPerSecond / (double) Stopwatch.Frequency;

            private readonly long _startTimestamp;

            private readonly DateTime _startDateTime;

            private ValueStopwatch(long startTimestamp, DateTime startDateTime)
            {
                _startDateTime = startDateTime;
                _startTimestamp = startTimestamp;
            }

            public static ValueStopwatch StartNew() => new(GetTimestamp(), DateTime.UtcNow);

            private static long GetTimestamp() => Stopwatch.GetTimestamp();

            private static TimeSpan GetElapsedTime(long startTimestamp, long endTimestamp)
            {
                var delta = endTimestamp - startTimestamp;
                var ticks = (long) (Ticks * delta);
                return new TimeSpan(ticks);
            }

            public DateTime StartedAt => _startDateTime;

            public TimeSpan Elapsed => GetElapsedTime(_startTimestamp, GetTimestamp());
        }
    }
}