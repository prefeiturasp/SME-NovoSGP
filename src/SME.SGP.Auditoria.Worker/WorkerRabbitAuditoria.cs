using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Auditoria.Worker.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Infra.Worker;

namespace SME.SGP.Auditoria.Worker
{
    public class WorkerRabbitAuditoria : WorkerRabbitSGP
    {
        public WorkerRabbitAuditoria(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory)
            : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas, telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitAuditoria", typeof(RotasRabbitAuditoria), false)
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitAuditoria.PersistirAuditoriaDB, new ComandoRabbit("Persistir Auditoria no Banco de Dados", typeof(IRegistrarAuditoriaUseCase)));
        }
    }
}