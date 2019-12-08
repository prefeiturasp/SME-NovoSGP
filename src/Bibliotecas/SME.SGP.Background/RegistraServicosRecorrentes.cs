using Hangfire;
using SME.Background.Core;
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
        }
    }
}