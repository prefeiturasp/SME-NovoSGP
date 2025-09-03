using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Infra;
using RabbitMQ.Client;
using SME.SGP.Infra.Mensageria.Rotas;
using System.Diagnostics.CodeAnalysis;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;

namespace SME.SGP.PainelEducacional.Worker
{
    [ExcludeFromCodeCoverage]
    public class WorkerRabbitPainelEducacional : WorkerRabbitAplicacao
    {
        public WorkerRabbitPainelEducacional(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitPainelEducacional",
                typeof(RotasRabbitSgpPainelEducacional))
        {

        }
        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarIdepPainelEducacional, new ComandoRabbit("Consolidar idep para painel educacional", typeof(IConsolidarIdepPainelEducacionalUseCase)));
        }
    }
}
