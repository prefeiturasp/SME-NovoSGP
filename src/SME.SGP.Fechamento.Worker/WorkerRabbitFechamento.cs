using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Fechamento.Worker
{
    public class WorkerRabbitFechamento : WorkerRabbitMQBase
    {
        public WorkerRabbitFechamento(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitFechamento", 
                typeof(RotasRabbitSgpFechamento))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync, new ComandoRabbit("Consolidação fechamento - Sincronizar Componentes da Turma", typeof(IExecutarConsolidacaoTurmaFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoComponenteTratar, new ComandoRabbit("Consolidação fechamento - Consolidar Componentes da Turma", typeof(IExecutarConsolidacaoTurmaFechamentoComponenteUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseSync, new ComandoRabbit("Consolidação conselho classe - Sincronizar alunos da turma", typeof(IExecutarConsolidacaoTurmaConselhoClasseUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseAlunoTratar, new ComandoRabbit("Consolidação conselho classe - Consolidar aluno da turma", typeof(IExecutarConsolidacaoTurmaConselhoClasseAlunoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores por ue", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresUeUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores por turma", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoTratar, new ComandoRabbit("Consolidação turma conselho classe aluno anos anteriores por aluno", typeof(IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAlunoPorDre, new ComandoRabbit("Atualiza parecer conclusivo do conselho de classe do aluno por DRE", typeof(IReprocessarParecerConclusivoPorDreUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAlunoPorUe, new ComandoRabbit("Atualiza parecer conclusivo do conselho de classe do aluno por UE", typeof(IReprocessarParecerConclusivoPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAlunoPorTurma, new ComandoRabbit("Atualiza parecer conclusivo do conselho de classe do aluno por Turma", typeof(IReprocessarParecerConclusivoPorTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAluno, new ComandoRabbit("Atualiza parecer conclusivo do conselho de classe do aluno", typeof(IReprocessarParecerConclusivoAlunoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaExecutaAtualizacaoSituacaoConselhoClasse, new ComandoRabbit("Executa atualização da situação do conselho de classe", typeof(IAtualizarSituacaoConselhoClasseUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.NotificacaoPeriodoFechamentoReaberturaIniciando, new ComandoRabbit("Executar notificação de período de Fechamento Reabertura iniciando", typeof(INotificacaoPeriodoFechamentoReaberturaIniciandoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.NotificacaoPeriodoFechamentoReaberturaEncerrando, new ComandoRabbit("Executar notificação de período de Fechamento Reabertura encerrando", typeof(INotificacaoPeriodoFechamentoReaberturaEncerrandoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.VerificaPendenciasFechamentoTurma, new ComandoRabbit("Notificar usuário resultado insatisfatório de aluno", typeof(IVerificaPendenciasFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaExecutaVerificacaoPendenciasAusenciaFechamento, new ComandoRabbit("Executa verificação de pendências de fechamento de bimestre", typeof(IExecutaVerificacaoGeracaoPendenciaAusenciaFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaExecutaExclusaoPendenciasAusenciaFechamento, new ComandoRabbit("Executa exclusão de pendências de ausencia de fechamento", typeof(IExecutarExclusaoPendenciasAusenciaFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoAndamentoFechamento, new ComandoRabbit("Executa notificação sobre o andamento do fechamento", typeof(INotificacaoAndamentoFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoAndamentoFechamentoPorUe, new ComandoRabbit("Executa notificação sobre o andamento do fechamento por UE", typeof(INotificacaoAndamentoFechamentoPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoInicioFimPeriodoFechamento, new ComandoRabbit("Executa notificação sobre o início e fim do Periodo de fechamento", typeof(INotificacaoInicioFimPeriodoFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoInicioPeriodoFechamentoUE, new ComandoRabbit("Executa notificação sobre o início do Periodo de fechamento por UE", typeof(INotificacaoInicioPeriodoFechamentoUEUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoFimPeriodoFechamentoUE, new ComandoRabbit("Executa notificação sobre o fim do Periodo de fechamento por UE", typeof(INotificacaoFimPeriodoFechamentoUEUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaGeracaoPendenciasFechamento, new ComandoRabbit("Executa inclusão de fila de Geração de Pendências do Fechamento", typeof(IGerarPendenciasFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoUeFechamentosInsuficientes, new ComandoRabbit("Executa notificação UE sobre fechamento insuficientes", typeof(INotificacaoUeFechamentosInsuficientesUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidarTurmaSync, new ComandoRabbit("Inicia processo de Consolidação Fechamento/Conselho - Consolidar Turmas", typeof(IExecutarConsolidacaoTurmaGeralUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.ConsolidarTurmaTratar, new ComandoRabbit("Consolidação Fechamento/Conselho - Consolidar Turmas", typeof(IExecutarConsolidacaoTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReabertura, new ComandoRabbit("Notificação de Reabertura de Fechamento", typeof(INotificarFechamentoReaberturaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReaberturaDRE, new ComandoRabbit("Notificação de Reabertura de Fechamento DRE", typeof(INotificarFechamentoReaberturaDREUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReaberturaUE, new ComandoRabbit("Notificação de Reabertura de Fechamento UE", typeof(INotificarFechamentoReaberturaUEUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.VarreduraFechamentosTurmaDisciplinaEmProcessamentoPendentes, new ComandoRabbit("Efetua a varredura em busca de fechamentos em processamento pendentes", typeof(IVarreduraFechamentosEmProcessamentoPendentesUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.GerarNotificacaoAlteracaoLimiteDias, new ComandoRabbit("Gera notificacao de alteracao de limite de dias", typeof(IGerarNotificacaoAlteracaoLimiteDiasUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.VerificarPendenciasFechamentoTurmaDisciplina, new ComandoRabbit("Verifica prendencias no fechamento de turma para disciplina", typeof(IVerificarPendenciasFechamentoTurmaDisciplinaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.AlterarPeriodosComHierarquiaInferiorFechamento, new ComandoRabbit("Altera o periodo com hierarquia inferior", typeof(IAlterarPeriodosComHierarquiaInferiorFechamentoUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoResultadoInsatisfatorio, new ComandoRabbit("Notificar usuário resultado insatisfatório de aluno", typeof(INotificarResultadoInsatisfatorioUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoAprovacaoFechamento, new ComandoRabbit("Notificar usuário sobre alteração de nota de fechamento nos anos anteriores", typeof(INotificarAlteracaoNotaFechamentoAgrupadaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaNotificacaoAprovacaoFechamentoPorTurma, new ComandoRabbit("Notificar usuário sobre alteração de nota de fechamento segregado por turma nos anos anteriores", typeof(INotificarAlteracaoNotaFechamentoAgrupadaTurmaUseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaGeracaoFechamentoEdFisica2020, new ComandoRabbit("Processo para gerar fechamento para turmas de Ed. Física do ano letivo de 2020", typeof(IGerarFechamentoTurmaEdFisica2020UseCase)));
            Comandos.Add(RotasRabbitSgpFechamento.RotaGeracaoFechamentoEdFisica2020AlunosTurma, new ComandoRabbit("Processo para gerar fechamento para turmas de Ed. Física do ano letivo de 2020 - separação de filas alunos com turma", typeof(IGerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase)));
        }
    }
}
