using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoTelemetria : IServicoTelemetria
    {
        private readonly TelemetriaOptions telemetriaOptions;

        public ServicoTelemetria(IOptions<TelemetriaOptions> telemetriaOptions)
        {
            this.telemetriaOptions = telemetriaOptions.Value ?? throw new ArgumentNullException(nameof(telemetriaOptions));
        }

        public async Task<K> RegistrarComRetornoAsync<T,K>(Func<Task<K>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                await transactionElk.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                 {
                     span.SetLabel(telemetriaNome, telemetriaValor);
                     span.SetLabel("Parametros", parametros);
                     return await acao();
                 });
            }

            return await acao();
        }

        public Task<K> RegistrarComRetornoAsync<K>(Func<Task<K>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return RegistrarComRetornoAsync<K, K>(acao, acaoNome, telemetriaNome, telemetriaValor, parametros);
        }

        public K RegistrarComRetorno<T,K>(Func<K> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                transactionElk.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.SetLabel("Parametros", parametros);
                    return acao();
                });
            }

            return acao();
        }

        public K RegistrarComRetorno<K>(Func<K> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return RegistrarComRetorno<K, K>(acao, acaoNome, telemetriaNome, telemetriaValor, parametros);
        }

        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
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
        }

        public async Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                if (!(transactionElk is null))
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
        }

        public ITransaction Iniciar(string nome, string tipo)
        {
            return  telemetriaOptions.Apm ?
                Agent.Tracer.StartTransaction(nome, tipo) :
                null;
        }

        public void Finalizar(ITransaction transacao)
        {
            if (telemetriaOptions.Apm)
                transacao.End();
        }
    }
}
