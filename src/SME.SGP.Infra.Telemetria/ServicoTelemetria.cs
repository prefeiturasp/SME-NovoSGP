using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Diagnostics;
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

        public async Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            dynamic result = default;
            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                await transactionElk.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                 {
                     span.SetLabel(telemetriaNome, telemetriaValor);
                     span.SetLabel("Parametros", parametros);
                     result = (await acao()) as dynamic;
                 });
            }
            else
            {
                result = await acao() as dynamic;
            }
            return result;
        }

        public dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            dynamic result = default;
            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                transactionElk.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.SetLabel("Parametros", parametros);
                    result = acao();
                });
            }
            else
            {
                result = acao();
            }
            return result;
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
            try
            {

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
            }
            catch (Exception e)
            {
            }
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
