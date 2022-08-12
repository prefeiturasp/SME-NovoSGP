﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Frequencia.Worker
{
    public class WorkerRabbitFrequencia : WorkerRabbitMQBase
    {
        public WorkerRabbitFrequencia(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitFrequencia",
                typeof(RotasRabbitSgpFrequencia))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Conciliação de Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaAlunoPorTurmaMensal, new ComandoRabbit("Consolidação de frequência do aluno por turma mensal", typeof(IConsolidarFrequenciaAlunoPorTurmaEMesUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoFrequenciaTurmaEvasao, new ComandoRabbit("Consolidação de evasão da turma", typeof(IConsolidarFrequenciaTurmaEvasaoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaNotificacaoFrequenciaUe, new ComandoRabbit("Notificar frequências dos alunos no bimestre para UE", typeof(INotificacaoFrequenciaUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidacaoFrequenciasTurmasCarregar, new ComandoRabbit("Consolidação de Registros de Frequência das Turmas - Carregar", typeof(IConsolidarFrequenciaTurmasUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno, new ComandoRabbit("Consolidar Registros de Frequência das Turmas no Ano", typeof(IConsolidarFrequenciaTurmasPorAnoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorDre, new ComandoRabbit("Consolidar Registros de Frequência das Turmas por UE", typeof(IConsolidarFrequenciaTurmasPorDREUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaAnoSync, new ComandoRabbit("Iniciar rotina de cálulo de frequência por Ano.", typeof(IConciliacaoFrequenciaAnoSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaDreSync, new ComandoRabbit("Iniciar rotina de cálculo de frequência da DRE", typeof(IConciliacaoFrequenciaTurmaDreSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaUeSync, new ComandoRabbit("Iniciar rotina de cálculo de frequência da UE", typeof(IConciliacaoFrequenciaTurmaUeSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasSync, new ComandoRabbit("Inicia rotina de cálculo de frequência da turma", typeof(IConciliacaoFrequenciaTurmasSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaCalcularFrequenciaGeralSync, new ComandoRabbit("Inicia rotina de cálculo de frequência geral com base em registro frequencia do aluno", typeof(ICalcularFrequenciaGeralUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosSync, new ComandoRabbit("Conciliação de frequência da turma sync", typeof(IConciliacaoFrequenciaTurmasAlunosSyncUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaPorPeriodo, new ComandoRabbit("Conciliação de frequência das turmas por período", typeof(IConciliacaoFrequenciaTurmasPorPeriodoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaValidacaoAusenciaConciliacaoFrequenciaTurma, new ComandoRabbit("Validação de ausência para conciliação de frequência da turma", typeof(IValidacaoAusenciaConcolidacaoFrequenciaTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmasAlunosBuscar, new ComandoRabbit("Conciliação de frequência da turma buscar", typeof(IConciliacaoFrequenciaTurmasAlunosBuscarUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConciliacaoFrequenciaTurmaMes, new ComandoRabbit("Conciliação de frequência das turmas mês", typeof(IConciliacaoFrequenciaTurmasMesUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.CarregarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Carregar- migração dados frequencia alunos", typeof(ICarregarRegistroFrequenciaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.SincronizarDadosAlunosFrequenciaMigracao, new ComandoRabbit("Executar sincronização - migração dados frequencia alunos", typeof(IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.FrequenciaDaAulaExcluir, new ComandoRabbit("Executar exclusão de frequencia de aula por aula id", typeof(IExcluirFrequenciaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.AnotacoesFrequenciaDaAulaExcluir, new ComandoRabbit("Executar exclusão de anotações de frequência por aula id", typeof(IExcluirAnotacoesFrequenciaPorAulaIdUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia, new ComandoRabbit("Consolidação geral de frequências diarias para o dashboard", typeof(IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoSemanalDashBoardFrequencia, new ComandoRabbit("Consolidação geral de frequências semanais para o dashboard", typeof(IExecutaConsolidacaoSemanalDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoMensalDashBoardFrequencia, new ComandoRabbit("Consolidação geral de frequências mensal para o dashboard", typeof(IExecutaConsolidacaoMensalDashBoardFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDashBoardFrequenciaPorUe, new ComandoRabbit("Consolidação por UE para o dashboard geral (diário, semanal e mensal)", typeof(IExecutaConsolidacaoDashBoardFrequenciaPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.NotificacaoFrequencia, new ComandoRabbit("Executar a notificacao de frequencia", typeof(INotificacaoFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.NotifificarRegistroFrequencia, new ComandoRabbit("Notificação do registro de frequênica", typeof(IExecutarNotificacaoRegistroFrequenciaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaCalculoFrequenciaPorTurmaComponente, new ComandoRabbit("Cálculo de frequência por Turma e Componente", typeof(ICalculoFrequenciaTurmaDisciplinaUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.RotaConsolidacaoDashBoardFrequencia, new ComandoRabbit("Consolidar frequências por tipo período para o dashboard", typeof(IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasPorUe, new ComandoRabbit("Consolidar Registros de Frequência das Turmas por UE", typeof(IConsolidarFrequenciaTurmasPorUEUseCase)));
            Comandos.Add(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurma, new ComandoRabbit("Consolidar Registros de Frequência por Turma", typeof(IConsolidarFrequenciaPorTurmaUseCase)));
        }
    }
}
