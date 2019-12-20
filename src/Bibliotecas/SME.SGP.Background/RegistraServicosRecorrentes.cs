using Hangfire;
using SME.Background.Core;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Background
{
    public static class RegistraServicosRecorrentes
    {
        public static void Registrar()
        {
            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.ExecutaNotificacaoFrequencia(), Cron.Daily(2));

            Cliente.ExecutarPeriodicamente<IServicoEventoMatricula>(c => c.ExecutaCargaEventos(), Cron.Daily(6));
            Cliente.ExecutarPeriodicamente<IServicoEventoMatricula>(c => c.ExecutaCargaEventos(), Cron.Daily(12));

            Cliente.ExecutarPeriodicamente<IServicoNotificacaoAulaPrevista>(c => c.ExecutaNotificacaoAulaPrevista(), Cron.Daily(2));

            // de segunda a sexta as 10, 14 e 16 horas
            Cliente.ExecutarPeriodicamente<IServicoAbrangencia>(c => c.SincronizarEstruturaInstitucionalVigenteCompleta(), "0 13,17,19 * * 1-5");

            // de segunda a sexta as 10:30, 14:30 e 16:30 horas
            Cliente.ExecutarPeriodicamente<IServicoAbrangencia>(c => c.SincronizarTurmasEncerradas(), "30 13,17,19 * * 1-5");
        }
    }
}