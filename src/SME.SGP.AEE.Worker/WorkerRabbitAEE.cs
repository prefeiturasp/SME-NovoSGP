using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.AEE.Worker
{
    public class WorkerRabbitAEE : WorkerRabbitAplicacao
    {
        public WorkerRabbitAEE(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitAEE",
                typeof(RotasRabbitSgpAEE))
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpAEE.RotaNotificacaoRegistroConclusaoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para registro de conclusão do Encaminhamento AEE", typeof(INotificacaoConclusaoEncaminhamentoAEEUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.RotaNotificacaoEncerramentoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para encerramento do Encaminhamento AEE", typeof(INotificacaoEncerramentoEncaminhamentoAEEUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.RotaNotificacaoDevolucaoEncaminhamentoAEE, new ComandoRabbit("Executa notificação para devolucao do Encaminhamento AEE", typeof(INotificacaoDevolucaoEncaminhamentoAEEUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.RotaEncerrarEncaminhamentoAEEAutomaticoSync, new ComandoRabbit("Iniciar processo de encerramento automático do Encaminhamento AEE", typeof(IEncerrarEncaminhamentoAEEAutomaticoSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpAEE.RotaValidarEncerrarEncaminhamentoAEEAutomatico, new ComandoRabbit("Validar encerramento automático do Encaminhamento AEE", typeof(IValidarEncerrarEncaminhamentoAEEAutomaticoUseCase), true));
            Comandos.Add(RotasRabbitSgpAEE.RotaEncerrarEncaminhamentoAEEEncerrarAutomatico, new ComandoRabbit("Encerrar automaticamente Encaminhamento AEE", typeof(IEncerrarEncaminhamentoAEEAutomaticoUseCase), true));
            Comandos.Add(RotasRabbitSgpAEE.GerarPendenciaValidadePlanoAEE, new ComandoRabbit("Gerar Pendência de Validade do PlanoAEE", typeof(IGerarPendenciaValidadePlanoAEEUseCase), true));
            Comandos.Add(RotasRabbitSgpAEE.NotificarPlanoAEEExpirado, new ComandoRabbit("Excluir plano AEE estudantes inativos", typeof(INotificarPlanosAEEExpiradosUseCase), true));
            Comandos.Add(RotasRabbitSgpAEE.NotificarPlanoAEEEmAberto, new ComandoRabbit("Notificar Plano AEE que estejam abertos", typeof(INotificarPlanosAEEEmAbertoUseCase), true));
            Comandos.Add(RotasRabbitSgpAEE.NotificarPlanoAEEReestruturado, new ComandoRabbit("Enviar Notificação de Reestruturação de PlanoAEE", typeof(IEnviarNotificacaoReestruturacaoPlanoAEEUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.NotificarCriacaoPlanoAEE, new ComandoRabbit("Enviar Notificação de Criação de PlanoAEE", typeof(IEnviarNotificacaoCriacaoPlanoAEEUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.NotificarPlanoAEEEncerrado, new ComandoRabbit("Enviar Notificação de Encerramento de PlanoAEE", typeof(IEnviarNotificacaoEncerramentoPlanoAEEUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.RotaNotificacaoRegistroItineranciaInseridoUseCase, new ComandoRabbit("Enviar Notificação quanto insere um novo Registro de Itinerância", typeof(INotificacaoSalvarItineranciaUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.RotaTransferirPendenciaPlanoAEEParaNovoResponsavel, new ComandoRabbit("Transferir pendência plano aee para novo responsável", typeof(ITransferirPendenciaParaNovoResponsavelUseCase)));

            Comandos.Add(RotasRabbitSgpAEE.AtualizarTabelaPlanoAEETurmaAlunoSync, new ComandoRabbit("Atualiza planos AEE em andamento", typeof(IAtualizaPlanoAEETurmaAlunoSyncUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.AtualizarTabelaPlanoAEETurmaAlunoTratar, new ComandoRabbit("Atualiza tabela plano AEE turma aluno com a turma regular e SRM", typeof(IAtualizaPlanoAEETurmaAlunoTratarUseCase)));

            Comandos.Add(RotasRabbitSgpAEE.AtualizarTabelaEncaminhamentoAEETurmaAlunoSync, new ComandoRabbit("Atualiza encaminhamento AEE em andamento", typeof(IAtualizaEncaminhamentoAEETurmaAlunoSyncUseCase)));
            Comandos.Add(RotasRabbitSgpAEE.AtualizarTabelaEncaminhamentoAEETurmaAlunoTratar, new ComandoRabbit("Atualiza tabela encaminhamento AEE turma aluno com a turma regular e SRM", typeof(IAtualizaEncaminhamentoAEETurmaAlunoTratarUseCase)));
        }
    }
}
