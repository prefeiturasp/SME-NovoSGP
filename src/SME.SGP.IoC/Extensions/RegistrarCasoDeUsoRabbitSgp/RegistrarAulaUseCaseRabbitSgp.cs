using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarAulaUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IInserirAulaRecorrenteUseCase, InserirAulaRecorrenteUseCase>();
            services.TryAddScoped<IAlterarAulaRecorrenteUseCase, AlterarAulaRecorrenteUseCase>();
            services.TryAddScoped<IExcluirAulaRecorrenteUseCase, ExcluirAulaRecorrenteUseCase>();
            services.TryAddScoped<ICriarAulasInfantilAutomaticamenteUseCase, CriarAulasInfantilAutomaticamenteUseCase>();
            services.TryAddScoped<ICriarAulasInfantilERegenciaUseCase, CriarAulasInfantilERegenciaUseCase>();
            services.TryAddScoped<INotificarExclusaoAulaComFrequenciaUseCase, NotificarExclusaoAulaComFrequenciaUseCase>();
            services.TryAddScoped<IPendenciaAulaUseCase, PendenciaAulaUseCase>();
            services.TryAddScoped<IPendenciaAulaDreUseCase, PendenciaAulaDreUseCase>();
            services.TryAddScoped<IPendenciaAulaDreUeUseCase, PendenciaAulaDreUeUseCase>();
            services.TryAddScoped<IPendenciaAulaDiarioBordoUseCase, PendenciaAulaDiarioBordoUseCase>();
            services.TryAddScoped<ITratarPendenciaDiarioBordoPorTurmaUseCase, TratarPendenciaDiarioBordoPorTurmaUseCase>();
            services.TryAddScoped<ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase, TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase>();
            services.TryAddScoped<IPendenciaAulaAvaliacaoUseCase, PendenciaAulaAvaliacaoUseCase>();
            services.TryAddScoped<IPendenciaAulaFrequenciaUseCase, PendenciaAulaFrequenciaUseCase>();
            services.TryAddScoped<IPendenciaAulaPlanoAulaUseCase, PendenciaAulaPlanoAulaUseCase>();
            services.TryAddScoped<IPendenciaAulaFechamentoUseCase, PendenciaAulaFechamentoUseCase>(); 
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisAulaUseCase, ExecutaVerificacaoPendenciasGeraisAulaUseCase>();
            services.TryAddScoped<IExecutarExclusaoPendenciasAulaUseCase, ExecutarExclusaoPendenciasAulaUseCase>();
            services.TryAddScoped<IExcluirPendenciaDiarioBordoPorAulaIdUseCase, ExcluirPendenciaDiarioBordoPorAulaIdUseCase>();
            services.TryAddScoped<IAlterarAulaFrequenciaTratarUseCase, AlterarAulaFrequenciaTratarUseCase>();
            services.TryAddScoped<INotificarAlunosFaltososDreUseCase, NotificarAlunosFaltososDreUseCase>();
            services.TryAddScoped<INotificarAlunosFaltososDreUeUseCase, NotificarAlunosFaltososDreUeUseCase>();
            services.TryAddScoped<INotificarAlunosFaltososUseCase, NotificarAlunosFaltososUseCase>();
            services.TryAddScoped<ICarregarUesTurmasRegenciaAulaAutomaticaUseCase, CarregarUesTurmasRegenciaAulaAutomaticaUseCase>();
            services.TryAddScoped<ISincronizarUeTurmaAulaRegenciaAutomaticaUseCase, SincronizarUeTurmaAulaRegenciaAutomaticaUseCase>();
            services.TryAddScoped<IImportarAvisoDoMuralGsaUseCase, ImportarAvisoDoMuralGsaUseCase>();
            services.TryAddScoped<INotificacaoAulasPrevistrasSyncUseCase, NotificacaoAulasPrevistrasSyncUseCase>();
            services.TryAddScoped<INotificacaoAulasPrevistrasUseCase, NotificacaoAulasPrevistrasUseCase>();
            services.TryAddScoped<IExcluirNotificacoesPorAulaIdUseCase, ExcluirNotificacoesPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirPlanoAulaPorAulaIdUseCase, ExcluirPlanoAulaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase, ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase>();
            services.TryAddScoped<IExcluirCompensacaoAusenciaPorIdsUseCase, ExcluirCompensacaoAusenciaPorIdsUseCase>();
            services.TryAddScoped<IPendenciaTurmaComponenteSemAulasPorUeUseCase, PendenciaTurmaComponenteSemAulasPorUeUseCase>();
            services.TryAddScoped<IPendenciaTurmaComponenteSemAulasUseCase, PendenciaTurmaComponenteSemAulasUseCase>();
            services.TryAddScoped<IExcluirAulasRecorrentesTerritorioSaberUseCase, ExcluirAulasRecorrentesTerritorioSaberUseCase>();
            services.TryAddScoped<IPendenciaDiarioBordoParaExcluirUseCase, PendenciaDiarioBordoParaExcluirUseCase>();
        }
    }
}
