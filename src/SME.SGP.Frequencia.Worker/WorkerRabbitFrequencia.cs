﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Frequencia.Worker
{
    public class WorkerRabbitFrequencia : WorkerRabbitMQBase
    {
        private const int TENTATIVA_REPROCESSAR_10 = 10;
        public WorkerRabbitFrequencia(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitFrequencia",
                typeof(RotasRabbitSgpFrequencia))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Conciliação de Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaAlunoPorTurmaMensal, new ComandoRabbit("Consolidação de frequência do aluno por turma mensal", typeof(IConsolidarFrequenciaAlunoPorTurmaEMesUseCase), true, TENTATIVA_REPROCESSAR_10, ExchangeSgpRabbit.SgpDeadLetterTTL_3));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasao, new ComandoRabbit("Consolidação de evasão da turma", typeof(IConsolidarFrequenciaTurmaEvasaoUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaNotificacaoFrequenciaUe, new ComandoRabbit("Notificar frequências dos alunos no bimestre para UE", typeof(INotificacaoFrequenciaUeUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidacaoFrequenciasTurmasCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas - Carregar", typeof(IConsolidarFrequenciaTurmasUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno, new ComandoRabbit("Consolidar Registros de Frequência das Turmas no Ano", typeof(IConsolidarFrequenciaTurmasPorAnoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorDre, new ComandoRabbit("Consolidar Registros de Frequência das Turmas por UE", typeof(IConsolidarFrequenciaTurmasPorDREUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaAnoSync, new ComandoRabbit("Iniciar rotina de cálulo de frequência por Ano.", typeof(IConciliacaoFrequenciaAnoSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaDreSync, new ComandoRabbit("Iniciar rotina de cálculo de frequência da DRE", typeof(IConciliacaoFrequenciaTurmaDreSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaUeSync, new ComandoRabbit("Iniciar rotina de cálculo de frequência da UE", typeof(IConciliacaoFrequenciaTurmaUeSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasSync, new ComandoRabbit("Inicia rotina de cálculo de frequência da turma", typeof(IConciliacaoFrequenciaTurmasSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaCalcularFrequenciaGeralSync, new ComandoRabbit("Inicia rotina de cálculo de frequência geral com base em registro frequencia do aluno", typeof(ICalcularFrequenciaGeralUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosSync, new ComandoRabbit("Conciliação de frequência da turma sync", typeof(IConciliacaoFrequenciaTurmasAlunosSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaPorPeriodo, new ComandoRabbit("Conciliação de frequência das turmas por período", typeof(IConciliacaoFrequenciaTurmasPorPeriodoUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaValidacaoAusenciaConciliacaoFrequenciaTurma, new ComandoRabbit("Validação de ausência para conciliação de frequência da turma", typeof(IValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosBuscar, new ComandoRabbit("Conciliação de frequência da turma buscar", typeof(IConciliacaoFrequenciaTurmasAlunosBuscarUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaMes, new ComandoRabbit("Conciliação de frequência das turmas mês", typeof(IConciliacaoFrequenciaTurmasMesUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.CarregarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Carregar- migração dados frequencia alunos", typeof(ICarregarRegistroFrequenciaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.SincronizarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Executar sincronização - migração dados frequencia alunos", typeof(IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.FrequenciaDaAulaExcluir, new ComandoRabbit("Executar exclusão de frequencia de aula por aula id", typeof(IExcluirFrequenciaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.AnotacoesFrequenciaDaAulaExcluir, new ComandoRabbit("Executar exclusão de anotações de frequência por aula id", typeof(IExcluirAnotacoesFrequenciaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia, new ComandoRabbit("Consolidação geral de frequências diarias para o dashboard", typeof(IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoSemanalDashBoardFrequencia, new ComandoRabbit("Consolidação geral de frequências semanais para o dashboard", typeof(IExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoMensalDashBoardFrequencia, new ComandoRabbit("Consolidação geral de frequências mensal para o dashboard", typeof(IExecutaConsolidacaoMensalDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDashBoardFrequenciaPorUe, new ComandoRabbit("Consolidação por UE para o dashboard geral (diário, semanal e mensal)", typeof(IExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.NotificacaoFrequencia, new ComandoRabbit("Executar a notificacao de frequencia", typeof(INotificacaoFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.NotifificarRegistroFrequencia, new ComandoRabbit("Notificação do registro de frequênica", typeof(IExecutarNotificacaoRegistroFrequenciaUseCase), true));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase), TENTATIVA_REPROCESSAR_10, ExchangeSgpRabbit.SgpDeadLetterTTL_3));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDashBoardFrequencia, new ComandoRabbit("Consolidar frequências por tipo período para o dashboard", typeof(IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase), TENTATIVA_REPROCESSAR_10, ExchangeSgpRabbit.SgpDeadLetterTTL_3));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorUe, new ComandoRabbit("Consolidar Registros de Frequência das Turmas por UE", typeof(IConsolidarFrequenciaTurmasPorUEUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurma, new ComandoRabbit("Consolidar Registros de Frequência por Turma", typeof(IConsolidarFrequenciaPorTurmaUseCase)));
            //Tratar a carga referência Aula no registro frequencia aluno
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoAno, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por ano", typeof(ITratarRegistroFrequenciaAlunoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoUe, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por ue", typeof(ITratarRegistroFrequenciaAlunoUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoTurma, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por turma", typeof(ITratarRegistroFrequenciaAlunoTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoAula, new ComandoRabbit("Tratar carga referência Aula no registro frequência aluno por aula", typeof(ITratarRegistroFrequenciaAlunoAulaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoProcessamento, new ComandoRabbit("Processa a carga referência de aula no registro frequência aluno", typeof(ITratarRegistroFrequenciaAlunoProcessamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaTratarFrequenciaRegistradaAlunosInativos, new ComandoRabbit("Tratar frequência registrada para alunos inativos indevidamente", typeof(IVerificaFrequenciaRegistradaAlunosInativosUseCase)));
        }
    }
}
