using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarFrequenciaUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<ICalculoFrequenciaTurmaDisciplinaUseCase, CalculoFrequenciaTurmaDisciplinaUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaAlunoPorTurmaEMesUseCase, ConsolidarFrequenciaAlunoPorTurmaEMesUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmaEvasaoUseCase, ConsolidarFrequenciaTurmaEvasaoUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase, ConsolidarFrequenciaTurmaEvasaoAcumuladoUseCase>();
            services.TryAddScoped<INotificacaoFrequenciaUeUseCase, NotificacaoFrequenciaUeUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasUseCase, ConsolidarFrequenciaTurmasUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorAnoUseCase, ConsolidarFrequenciaTurmasPorAnoUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorDREUseCase, ConsolidarFrequenciaTurmasPorDREUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaAnoSyncUseCase, ConciliacaoFrequenciaAnoSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmaDreSyncUseCase, ConciliacaoFrequenciaTurmaDreSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmaUeSyncUseCase, ConciliacaoFrequenciaTurmaUeSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasSyncUseCase, ConciliacaoFrequenciaTurmasSyncUseCase>();
            services.TryAddScoped<ICalcularFrequenciaGeralUseCase, CalcularFrequenciaGeralUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosSyncUseCase, ConciliacaoFrequenciaTurmasAlunosSyncUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasPorPeriodoUseCase, ConciliacaoFrequenciaTurmasPorPeriodoUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasAlunosBuscarUseCase, ConciliacaoFrequenciaTurmasAlunosBuscarUseCase>();
            services.TryAddScoped<IConciliacaoFrequenciaTurmasMesUseCase, ConciliacaoFrequenciaTurmasMesUseCase>();
            services.TryAddScoped<ICarregarRegistroFrequenciaAlunosUseCase, CarregarRegistroFrequenciaAlunosUseCase>();
            services.TryAddScoped<IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase, ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase>();
            services.TryAddScoped<IExcluirFrequenciaPorAulaIdUseCase, ExcluirFrequenciaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirAnotacoesFrequenciaPorAulaIdUseCase, ExcluirAnotacoesFrequenciaPorAulaIdUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase>();
            services.TryAddScoped<IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase, ExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase>();
            services.TryAddScoped<INotificacaoFrequenciaUseCase, NotificacaoFrequenciaUseCase>();
            services.TryAddScoped<IExecutarNotificacaoRegistroFrequenciaUseCase, ExecutarNotificacaoRegistroFrequenciaUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasPorUEUseCase, ConsolidarFrequenciaTurmasPorUEUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaPorTurmaAnualUseCase, ConsolidarFrequenciaPorTurmaAnualUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaPorTurmaMensalUseCase, ConsolidarFrequenciaPorTurmaMensalUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaPorTurmaSemanalUseCase, ConsolidarFrequenciaPorTurmaSemanalUseCase>();

            services.TryAddScoped<IVerificaFrequenciaRegistradaAlunosInativosUseCase, VerificaFrequenciaRegistradaAlunosInativosUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasMensalUseCase, ConsolidarFrequenciaTurmasMensalUseCase>();
            services.TryAddScoped<IConsolidarFrequenciaTurmasSemanalUseCase, ConsolidarFrequenciaTurmasSemanalUseCase>();

            //Tratar a carga referência Aula no registro frequencia aluno
            services.TryAddScoped<ITratarRegistroFrequenciaAlunoUseCase, TratarRegistroFrequenciaAlunoUseCase>();
            services.TryAddScoped<ITratarRegistroFrequenciaAlunoUeUseCase, TratarRegistroFrequenciaAlunoUeUseCase>();
            services.TryAddScoped<ITratarRegistroFrequenciaAlunoTurmaUseCase, TratarRegistroFrequenciaAlunoTurmaUseCase>();
            services.TryAddScoped<ITratarRegistroFrequenciaAlunoAulaUseCase, TratarRegistroFrequenciaAlunoAulaUseCase>();
            services.TryAddScoped<ITratarRegistroFrequenciaAlunoProcessamentoUseCase, TratarRegistroFrequenciaAlunoProcessamentoUseCase>();

            services.TryAddScoped<ILancarFrequenciaAulaUseCase, LancarFrequenciaAulaUseCase>();
            services.TryAddScoped<IIdentificarFrequenciaAlunoPresencasMaiorTotalAulasUseCase, IdentificarFrequenciaAlunoPresencasMaiorTotalAulasUseCase>();
            services.TryAddScoped<IIdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase, IdentificarFrequenciaAlunoPresencasMaiorTotalAulasPorUeUseCase>();
            services.TryAddScoped<IRegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasUseCase, RegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasUseCase>();
            services.TryAddScoped<IRegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasPorRegistroUseCase, RegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasPorRegistroUseCase>();
            services.TryAddScoped<IConsolidarReflexoFrequenciaBuscaAtivaUseCase, ConsolidarReflexoFrequenciaBuscaAtivaUseCase>();
            services.TryAddScoped<IConsolidarReflexoFrequenciaBuscaAtivaDreUseCase, ConsolidarReflexoFrequenciaBuscaAtivaDreUseCase>();
            services.TryAddScoped<IConsolidarReflexoFrequenciaBuscaAtivaUeUseCase, ConsolidarReflexoFrequenciaBuscaAtivaUeUseCase>();
            services.TryAddScoped<IConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase, ConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase>();

            services.TryAddScoped<IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase, ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase>();
            services.TryAddScoped<IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase, ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaDreUseCase>();
            services.TryAddScoped<IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase, ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUeUseCase>();
            services.TryAddScoped<IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase, ExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaProfissionaisUseCase>();

            services.TryAddScoped<IConsolidarInformacoesProdutividadeFrequenciaUseCase, ConsolidarInformacoesProdutividadeFrequenciaUseCase>();
            services.TryAddScoped<IConsolidarInformacoesProdutividadeFrequenciaDreUseCase, ConsolidarInformacoesProdutividadeFrequenciaDreUseCase>();
            services.TryAddScoped<IConsolidarInformacoesProdutividadeFrequenciaUeUseCase, ConsolidarInformacoesProdutividadeFrequenciaUeUseCase>();
            services.TryAddScoped<IConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase, ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase>();
            services.TryAddScoped<IConsolidarInformacoesFrequenciaPainelEducacionalUseCase, ConsolidarInformacoesFrequenciaPainelEducacionalUseCase>();
        }
    }
}
