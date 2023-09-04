using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Worker.Avaliacao
{
    public class WorkerRabbitAvaliacao : WorkerRabbitMQBase
    {
        public WorkerRabbitAvaliacao(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitAvaliacao",
                typeof(RotasRabbitSgpAvaliacao))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpAvaliacao.RotaExecutaExclusaoPendenciasAusenciaAvaliacao, new ComandoRabbit("Executa exclusão de pendências de ausencia de avaliação", typeof(IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase)));
            Comandos.Add(RotasRabbitSgpAvaliacao.RotaAtividadesSync, new ComandoRabbit("Importar atividades avaliativas do GSA", typeof(IImportarAtividadesGsaUseCase)));
            Comandos.Add(RotasRabbitSgpAvaliacao.RotaAtividadesNotasSync, new ComandoRabbit("Importar notas de atividades avaliativas do GSA", typeof(IImportarNotaAtividadeAvaliativaGsaUseCase)));
            Comandos.Add(RotasRabbitSgpAvaliacao.RotaValidarMediaAlunos, new ComandoRabbit("Validar média de alunos das atividades avaliativas", typeof(IValidarMediaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpAvaliacao.RotaValidarMediaAlunosAtividadeAvaliativa, new ComandoRabbit("Validar média de alunos por atividade avaliativa", typeof(IValidarMediaAlunosAtividadeAvaliativaUseCase)));
            Comandos.Add(RotasRabbitSgpAvaliacao.RotaNotificarUsuarioAlteracaoExtemporanea, new ComandoRabbit("Notificar usuário quando da ocorrência de alteração extemporânea", typeof(INotificarUsuarioAlteracaoExtemporaneaUseCase)));
        }
    }
}
