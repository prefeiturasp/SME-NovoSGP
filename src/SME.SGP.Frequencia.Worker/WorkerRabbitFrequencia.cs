using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Frequencia.Worker
{
    public class WorkerRabbitFrequencia : WorkerRabbitAplicacao
    {
        private const int TENTATIVA_REPROCESSAR_10 = 10;
        public WorkerRabbitFrequencia(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitFrequencia",
                typeof(RotasRabbitSgpFrequencia))
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Conciliação de Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaAlunoPorTurmaMensal, new ComandoRabbit("Consolidação de frequência do aluno por turma mensal", typeof(IConsolidarFrequenciaAlunoPorTurmaEMesUseCase), true, TENTATIVA_REPROCESSAR_10, ExchangeSgpRabbit.SgpDeadLetterTTL_3));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasao, new ComandoRabbit("Consolidação de evasão da turma", typeof(IConsolidarFrequenciaTurmaEvasaoUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasaoAcumulado, new ComandoRabbit("Consolidação de evasão da turma", typeof(IConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaNotificacaoFrequenciaUe, new ComandoRabbit("Notificar frequências dos alunos no bimestre para UE", typeof(INotificacaoFrequenciaUeUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidacaoFrequenciasTurmasCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas - Carregar", typeof(IConsolidarFrequenciaTurmasUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidacaoFrequenciasTurmasMensalCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas Mensal - Carregar", typeof(IConsolidarFrequenciaTurmasMensalUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidacaoFrequenciasTurmasSemanalCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas Semanal - Carregar", typeof(IConsolidarFrequenciaTurmasSemanalUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno, new ComandoRabbit("Consolidar Registros de Frequência das Turmas no Ano", typeof(IConsolidarFrequenciaTurmasPorAnoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorDre, new ComandoRabbit("Consolidar Registros de Frequência das Turmas por UE", typeof(IConsolidarFrequenciaTurmasPorDREUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaAnoSync, new ComandoRabbit("Iniciar rotina de cálulo de frequência por Ano.", typeof(IConciliacaoFrequenciaAnoSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaDreSync, new ComandoRabbit("Iniciar rotina de cálculo de frequência da DRE", typeof(IConciliacaoFrequenciaTurmaDreSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaUeSync, new ComandoRabbit("Iniciar rotina de cálculo de frequência da UE", typeof(IConciliacaoFrequenciaTurmaUeSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasSync, new ComandoRabbit("Inicia rotina de cálculo de frequência da turma", typeof(IConciliacaoFrequenciaTurmasSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaCalcularFrequenciaGeralSync, new ComandoRabbit("Inicia rotina de cálculo de frequência geral com base em registro frequencia do aluno", typeof(ICalcularFrequenciaGeralUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosSync, new ComandoRabbit("Conciliação de frequência da turma sync", typeof(IConciliacaoFrequenciaTurmasAlunosSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaPorPeriodo, new ComandoRabbit("Conciliação de frequência das turmas por período", typeof(IConciliacaoFrequenciaTurmasPorPeriodoUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosBuscar, new ComandoRabbit("Conciliação de frequência da turma buscar", typeof(IConciliacaoFrequenciaTurmasAlunosBuscarUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaMes, new ComandoRabbit("Conciliação de frequência das turmas mês", typeof(IConciliacaoFrequenciaTurmasMesUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.CarregarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Carregar- migração dados frequencia alunos", typeof(ICarregarRegistroFrequenciaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.SincronizarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Executar sincronização - migração dados frequencia alunos", typeof(IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.FrequenciaDaAulaExcluir, new ComandoRabbit("Executar exclusão de frequencia de aula por aula id", typeof(IExcluirFrequenciaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.AnotacoesFrequenciaDaAulaExcluir, new ComandoRabbit("Executar exclusão de anotações de frequência por aula id", typeof(IExcluirAnotacoesFrequenciaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia, new ComandoRabbit("Consolidação diária dashboard", typeof(IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorUe, new ComandoRabbit("Consolidação diária dashboard por ue", typeof(IExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorTurma, new ComandoRabbit("Consolidação diária dashboard por turma", typeof(IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.NotificacaoFrequencia, new ComandoRabbit("Executar a notificacao de frequencia", typeof(INotificacaoFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.NotifificarRegistroFrequencia, new ComandoRabbit("Notificação do registro de frequênica", typeof(IExecutarNotificacaoRegistroFrequenciaUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase), TENTATIVA_REPROCESSAR_10, ExchangeSgpRabbit.SgpDeadLetterTTL_3));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorUe, new ComandoRabbit("Consolidar Registros de Frequência das Turmas por UE", typeof(IConsolidarFrequenciaTurmasPorUEUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurma, new ComandoRabbit("Consolidar Registros de Frequência por Turma anual", typeof(IConsolidarFrequenciaPorTurmaAnualUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurmaMensal, new ComandoRabbit("Consolidar Registros de Frequência por Turma mensal", typeof(IConsolidarFrequenciaPorTurmaMensalUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurmaSemanal, new ComandoRabbit("Consolidar Registros de Frequência por Turma semanal", typeof(IConsolidarFrequenciaPorTurmaSemanalUseCase)));
            //Tratar a carga referência Aula no registro frequencia aluno
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoAno, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por ano", typeof(ITratarRegistroFrequenciaAlunoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoUe, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por ue", typeof(ITratarRegistroFrequenciaAlunoUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoTurma, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por turma", typeof(ITratarRegistroFrequenciaAlunoTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoAula, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por aula", typeof(ITratarRegistroFrequenciaAlunoAulaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoProcessamento, new ComandoRabbit("Processa a carga referência de aula no registro frequência aluno", typeof(ITratarRegistroFrequenciaAlunoProcessamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarFrequenciaRegistradaAlunosInativos, new ComandoRabbit("Tratar frequência registrada para alunos inativos indevidamente", typeof(IVerificaFrequenciaRegistradaAlunosInativosUseCase)));

            Comandos.Add(RotasRabbitSgpFrequencia.RotaFrequenciaLancamentoAulaSync, new ComandoRabbit("Tratar lançamento de frequências provenientes da integração externa ao SGP", typeof(ILancarFrequenciaAulaUseCase)));
            // Identificar e regularizar frequência aluno com presenças maior que a quantidade de aulas
            Comandos.Add(RotasRabbitSgpFrequencia.IdentificarFrequenciaAlunoPresencasMaiorTotalAulas, new ComandoRabbit("Inicio da identificação de registros de frequência aluno que possuam a quantidade de presenças maior que a quantidade de aulas para providenciar a regularização", typeof(IIdentificarFrequenciaAlunoPresencasMaiorTotalAulasUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUe, new ComandoRabbit("Identificar registros de frequência aluno que possuam a quantidade de presenças maior que a quantidade de aulas para providenciar a regularização por UE", typeof(IIdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RegularizarFrequenciaAlunoPresencasMaiorTotalAulas, new ComandoRabbit("Inicio da regularização dos registros de frequência aluno que estão com a quantidade de presenças maior que a quantidade de aulas", typeof(IRegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RegularizarFrequenciaAlunoPresencasMaiorTotalAulasPorRegistro, new ComandoRabbit("Regularizar o registro de frequência aluno que está com a quantidade de presenças maior que a quantidade de aulas", typeof(IRegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasPorRegistroUseCase)));

            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtiva, new ComandoRabbit("Consolidar reflexo frequência aluno dashboard Busca Ativa", typeof(IConsolidarReflexoFrequenciaBuscaAtivaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaDre, new ComandoRabbit("Consolidar reflexo frequência aluno dashboard Busca Ativa (Dre)", typeof(IConsolidarReflexoFrequenciaBuscaAtivaDreUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaUe, new ComandoRabbit("Consolidar reflexo frequência aluno dashboard Busca Ativa (Ue)", typeof(IConsolidarReflexoFrequenciaBuscaAtivaUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarReflexoFrequenciaBuscaAtivaAluno, new ComandoRabbit("Consolidar reflexo frequência aluno dashboard Busca Ativa (Aluno)", typeof(IConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase)));

            Comandos.Add(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtiva, new ComandoRabbit("Notificar profissionais NAAPA sobre alunos com frequência mensal insuficiente (Busca Ativa)", typeof(IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaDre, new ComandoRabbit("Notificar profissionais NAAPA sobre alunos com frequência mensal insuficiente (Busca Ativa) (Dre)", typeof(IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaUe, new ComandoRabbit("Notificar profissionais NAAPA sobre alunos com frequência mensal insuficiente (Busca Ativa) (Ue)", typeof(IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ExecutarNotificacaoAlunosBaixaFrequenciaBuscaAtivaProfissionaisNAAPA, new ComandoRabbit("Notificar profissionais NAAPA sobre alunos com frequência mensal insuficiente (Busca Ativa)", typeof(IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase)));

            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequencia, new ComandoRabbit("Consolidar informações produtividade de frequência", typeof(IConsolidarInformacoesProdutividadeFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaDre, new ComandoRabbit("Consolidar informações produtividade de frequência (Dre)", typeof(IConsolidarInformacoesProdutividadeFrequenciaDreUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaUe, new ComandoRabbit("Consolidar informações produtividade de frequência (Ue)", typeof(IConsolidarInformacoesProdutividadeFrequenciaUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre, new ComandoRabbit("Consolidar informações produtividade de frequência (Bimestre)", typeof(IConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarInformacoesFrequenciaPainelEducacional, new ComandoRabbit("Consolidar informações de frequência para painel educacional", typeof(IConsolidarInformacoesFrequenciaPainelEducacionalUseCase)));
        }
    }
}
