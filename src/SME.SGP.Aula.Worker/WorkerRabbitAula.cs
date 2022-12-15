using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aula.Worker
{
    public class WorkerRabbitAula : WorkerRabbitMQBase
    {
        public WorkerRabbitAula(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitAula",
                typeof(RotasRabbitSgpAula))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpAula.RotaInserirAulaRecorrencia, new ComandoRabbit("Inserir aulas recorrentes", typeof(IInserirAulaRecorrenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaAlterarAulaRecorrencia, new ComandoRabbit("Alterar aulas recorrentes", typeof(IAlterarAulaRecorrenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExcluirAulaRecorrencia, new ComandoRabbit("Excluir aulas recorrentes", typeof(IExcluirAulaRecorrenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaSincronizarAulasInfatil, new ComandoRabbit("Sincronizar aulas da modalidade Infantil que devem ser criadas ou excluídas", typeof(ICriarAulasInfantilAutomaticamenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaCriarAulasInfatilAutomaticamente, new ComandoRabbit("Criar aulas da modalidade Infantil automaticamente", typeof(ICriarAulasInfantilUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoExclusaoAulasComFrequencia, new ComandoRabbit("Notificar usuário sobre a exclusão de aulas com frequência registrada", typeof(INotificarExclusaoAulaComFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAula, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDre, new ComandoRabbit("Verifica as pendências de aula por DRE e cria caso exista", typeof(IPendenciaAulaDreUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDreUe, new ComandoRabbit("Verifica as pendências de aula por UE e cria caso exista", typeof(IPendenciaAulaDreUeUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaDiarioBordoUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurma, new ComandoRabbit("Gerar pendências de diário de bordo por turma", typeof(ITratarPendenciaDiarioBordoPorTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurmaAulaComponente, new ComandoRabbit("Gerar pendências de diário de bordo por turma, aula, componente", typeof(ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaAvaliacaoUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaPlanoAulaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.PendenciasGeraisAulas, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisAulaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaExclusaoPendenciasAula, new ComandoRabbit("Executa exclusão de pendências da aula", typeof(IExecutarExclusaoPendenciasAulaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaExclusaoPendenciaDiarioBordoAula, new ComandoRabbit("Executa exclusão de pendencias de diário de bordo por aula", typeof(IExcluirPendenciaDiarioBordoPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaAlterarAulaFrequenciaTratar, new ComandoRabbit("Normaliza as frequências quando há uma alteração de aula única", typeof(IAlterarAulaFrequenciaTratarUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAlunosFaltosos, new ComandoRabbit("Conciliação de frequência da turma buscar", typeof(INotificarAlunosFaltososUseCase)));
            Comandos.Add(RotasRabbitSgpAula.CarregarDadosUeTurmaRegenciaAutomaticamente, new ComandoRabbit("Carregar dados referentes a ue e turmas de regencia", typeof(ICarregarUesTurmasRegenciaAulaAutomaticaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.SincronizarDadosUeTurmaRegenciaAutomaticamente, new ComandoRabbit("Sincronizar dados referentes a ue e turmas de regencia", typeof(ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.SincronizarAulasRegenciaAutomaticamente, new ComandoRabbit("Sincronizar aulas automáticas de regência", typeof(ISincronizarAulasRegenciaAutomaticamenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaMuralAvisosSync, new ComandoRabbit("Importar avisos do mural do GSA", typeof(IImportarAvisoDoMuralGsaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAulasPrevistasSync, new ComandoRabbit("Executa carga de notificação de aulas previstas x dadas", typeof(INotificacaoAulasPrevistrasSyncUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAulasPrevistas, new ComandoRabbit("Executa notificação de aulas previstas x dadas", typeof(INotificacaoAulasPrevistrasUseCase)));
            Comandos.Add(RotasRabbitSgpAula.NotificacoesDaAulaExcluir, new ComandoRabbit("Executar exclusão de notificações por aula id", typeof(IExcluirNotificacoesPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpAula.PlanoAulaDaAulaExcluir, new ComandoRabbit("Executar exclusão de plano de aula por aula id", typeof(IExcluirPlanoAulaPorAulaIdUseCase)));
        }
    }
}
