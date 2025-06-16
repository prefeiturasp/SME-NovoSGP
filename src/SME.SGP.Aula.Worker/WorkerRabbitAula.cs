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
    public class WorkerRabbitAula : WorkerRabbitAplicacao
    {
        public WorkerRabbitAula(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitAula",
                typeof(RotasRabbitSgpAula))
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpAula.RotaInserirAulaRecorrencia, new ComandoRabbit("Inserir aulas recorrentes", typeof(IInserirAulaRecorrenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaAlterarAulaRecorrencia, new ComandoRabbit("Alterar aulas recorrentes", typeof(IAlterarAulaRecorrenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExcluirAulaRecorrencia, new ComandoRabbit("Excluir aulas recorrentes", typeof(IExcluirAulaRecorrenteUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaSincronizarAulasInfantil, new ComandoRabbit("Sincronizar aulas da modalidade Infantil que devem ser criadas ou excluídas", typeof(ICriarAulasInfantilAutomaticamenteUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaCriarAulasInfantilERegenciaAutomaticamente, new ComandoRabbit("Criar aulas da modalidade Infantil automaticamente", typeof(ICriarAulasInfantilERegenciaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoExclusaoAulasComFrequencia, new ComandoRabbit("Notificar usuário sobre a exclusão de aulas com frequência registrada", typeof(INotificarExclusaoAulaComFrequenciaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAula, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDre, new ComandoRabbit("Verifica as pendências de aula por DRE e cria caso exista", typeof(IPendenciaAulaDreUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDreUe, new ComandoRabbit("Verifica as pendências de aula por UE e cria caso exista", typeof(IPendenciaAulaDreUeUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaDiarioBordoUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurma, new ComandoRabbit("Gerar pendências de diário de bordo por turma", typeof(ITratarPendenciaDiarioBordoPorTurmaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurmaAulaComponente, new ComandoRabbit("Gerar pendências de diário de bordo por turma, aula, componente", typeof(ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaAvaliacaoUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaFrequenciaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, new ComandoRabbit("Verifica as pendências de aula e cria caso exista", typeof(IPendenciaAulaPlanoAulaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, new ComandoRabbit("Verifica se as pendências de diário de classe são para turmas/disciplinas em fechamento para geração de pendência de fechamento", typeof(IPendenciaAulaFechamentoUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExcluirPendenciasDiarioBordo, new ComandoRabbit("Executa exclusão de pendências de diário de bordo já resolvidas", typeof(IPendenciaDiarioBordoParaExcluirUseCase)));
            Comandos.Add(RotasRabbitSgpAula.PendenciasGeraisAulas, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisAulaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaExclusaoPendenciasAula, new ComandoRabbit("Executa exclusão de pendências da aula", typeof(IExecutarExclusaoPendenciasAulaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaExclusaoPendenciaDiarioBordoAula, new ComandoRabbit("Executa exclusão de pendencias de diário de bordo por aula", typeof(IExcluirPendenciaDiarioBordoPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, new ComandoRabbit("Verifica pendências de turmas e componentes sem aulas por ue", typeof(IPendenciaTurmaComponenteSemAulasPorUeUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAula, new ComandoRabbit("Verifica pendências de turmas e componentes sem aulas", typeof(IPendenciaTurmaComponenteSemAulasUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaAlterarAulaFrequenciaTratar, new ComandoRabbit("Normaliza as frequências quando há uma alteração de aula única", typeof(IAlterarAulaFrequenciaTratarUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAlunosFaltososDre, new ComandoRabbit("Notificação de alunos faltosos por DRE", typeof(INotificarAlunosFaltososDreUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAlunosFaltososDreUe, new ComandoRabbit("Notificação de alunos faltosos da UE por DRE", typeof(INotificarAlunosFaltososDreUeUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAlunosFaltosos, new ComandoRabbit("Notificação de alunos faltosos", typeof(INotificarAlunosFaltososUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.CarregarDadosUeTurmaRegenciaAutomaticamente, new ComandoRabbit("Carregar dados referentes a ue e turmas de regencia", typeof(ICarregarUesTurmasRegenciaAulaAutomaticaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.SincronizarDadosUeTurmaRegenciaAutomaticamente, new ComandoRabbit("Sincronizar dados referentes a ue e turmas de regencia", typeof(ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaMuralAvisosSync, new ComandoRabbit("Importar avisos do mural do GSA", typeof(IImportarAvisoDoMuralGsaUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAulasPrevistasSync, new ComandoRabbit("Executa carga de notificação de aulas previstas x dadas", typeof(INotificacaoAulasPrevistrasSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.RotaNotificacaoAulasPrevistas, new ComandoRabbit("Executa notificação de aulas previstas x dadas", typeof(INotificacaoAulasPrevistrasUseCase), true));
            Comandos.Add(RotasRabbitSgpAula.NotificacoesDaAulaExcluir, new ComandoRabbit("Executar exclusão de notificações por aula id", typeof(IExcluirNotificacoesPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpAula.PlanoAulaDaAulaExcluir, new ComandoRabbit("Executar exclusão de plano de aula por aula id", typeof(IExcluirPlanoAulaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpAula.RotaExcluirAulasRecorrentesComponentesCurricularesTerritorioSaberDisponibilizado, new ComandoRabbit("Executar exclusão de aulas recorrentes relacionadas a componentes território do saber disponibilizados", typeof(IExcluirAulasRecorrentesTerritorioSaberUseCase)));
        }
    }
}
