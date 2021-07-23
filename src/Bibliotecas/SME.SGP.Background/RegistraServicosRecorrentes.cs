using Hangfire;
using SME.Background.Core;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Background
{
    public static class RegistraServicosRecorrentes
    {
        public static void Registrar()
        {
            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.ExecutaNotificacaoRegistroFrequencia(), Cron.Daily(2));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoAulasPrevistasUseCase>(c => c.Executar(), Cron.Daily(2));

            // Estamos removendo pois essa rotina está sendo refeita e será executada através do Rabbit.
            // de segunda a sexta as 10, 14 e 16 horas
            //Cliente.ExecutarPeriodicamente<IServicoAbrangencia>(c => c.SincronizarEstruturaInstitucionalVigenteCompleta(), "0 13,17,19 * * 1-5");
            
            //todos os dias à 1 da manhã
            Cliente.ExecutarPeriodicamente<IServicoObjetivosAprendizagem>(c => c.SincronizarObjetivosComJurema(), Cron.Daily(22));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoAlunosFaltososUseCase>(c => c.Executar(), Cron.Daily(2));

            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.NotificarAlunosFaltososBimestre(), Cron.Daily(3));
            
            Cliente.ExecutarPeriodicamente<ISincronizarAulasInfantilUseCase>(c => c.Executar(), Cron.Daily(6));

            // Executa as 04am (vai ser ajustado o UTC corretamente depois no hangfire)
            //Cliente.ExecutarPeriodicamente<IExecutaPendenciaAulaUseCase>(c => c.Executar(), Cron.Daily(4));

            Cliente.ExecutarPeriodicamente<IExecutaSincronismoComponentesCurricularesEolUseCase>(c => c.Executar(), Cron.Daily(4));

            // Executa as 02:00 
            Cliente.ExecutarPeriodicamente<IPendenciasGeraisUseCase>(c => c.Executar(), Cron.Daily(5));

            Cliente.ExecutarPeriodicamente<IExecutaPendenciasProfessorAvaliacaoUseCase>(c => c.Executar(), Cron.Daily(5));

            Cliente.ExecutarPeriodicamente<IExecutaPendenciasAusenciaFechamentoUseCase>(c => c.Executar(), Cron.Daily(5));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoResultadoInsatisfatorioUseCase>(c => c.Executar(), Cron.Daily(5));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoAndamentoFechamentoUseCase>(c => c.Executar(), Cron.Daily(5,15));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoUeFechamentosInsuficientesUseCase>(c => c.Executar(), Cron.Daily(5,15));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoReuniaoPedagogicaUseCase>(c => c.Executar(), Cron.Daily(5,15));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoPeriodoFechamentoUseCase>(c => c.Executar(), Cron.Daily(5,15));

            Cliente.ExecutarPeriodicamente<IPublicarPendenciaAusenciaRegistroIndividualUseCase>(c => c.Executar(), Cron.Daily(2));

            // de segunda a sexta as 11 horas
            Cliente.ExecutarPeriodicamente<IExecutaTrataNotificacoesNiveisCargosUseCase>(c => c.Executar(), "0 14 * * 1-5");

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoInicioFimPeriodoFechamentoUseCase>(c => c.Executar(), Cron.Daily(5, 15));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoFrequenciaUeUseCase>(c => c.Executar(), Cron.Daily(5, 15));

            //Cliente.ExecutarPeriodicamente<IRemoveConexaoIdleUseCase>(c => c.Executar(), Cron.MinuteInterval(30));

            Cliente.ExecutarPeriodicamente<IExecutarSyncGeralGoogleClassroomUseCase>(c => c.Executar(), Cron.Daily(11));
            Cliente.ExecutarPeriodicamente<IExecutaSyncGsaGoogleClassroomUseCase>(c => c.Executar(), Cron.Weekly(System.DayOfWeek.Sunday, 10));

            Cliente.ExecutarPeriodicamente<IExecutaEncerramentoPlanoAEEEstudantesInativosUseCase>(c => c.Executar(), Cron.Daily(8));

            Cliente.ExecutarPeriodicamente<IExecutaPendenciaValidadePlanoAEEUseCase>(c => c.Executar(), Cron.Daily(8));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoPlanoAEEExpiradoUseCase>(c => c.Executar(), Cron.Daily(5));

            Cliente.ExecutarPeriodicamente<IExecutaNotificacaoPlanoAEEEmAbertoUseCase>(c => c.Executar(), Cron.Daily(5));

            Cliente.ExecutarPeriodicamente<IExecutarSincronizacaoInstitucionalSyncUseCase>(c => c.Executar(), Cron.Daily(13));

            Cliente.ExecutarPeriodicamente<IExecutarConsolidacaoMatriculaTurmasUseCase>(c => c.Executar(), Cron.Daily(10));

            // Removido até melhoria de performance prevista
            //Cliente.ExecutarPeriodicamente<IExecutarConsolidacaoFrequenciaTurmaSyncUseCase>(c => c.Executar(), Cron.Daily(6));

            Cliente.ExecutarPeriodicamente<IConciliacaoFrequenciaTurmasCronUseCase>(c => c.Executar(), Cron.Weekly(System.DayOfWeek.Saturday, 23));           

            //De 10 em 10 minutos
            Cliente.ExecutarPeriodicamente<IRabbitDeadletterSyncUseCase>(c => c.Executar(), Cron.MinuteInterval(10));
        }
    }
}