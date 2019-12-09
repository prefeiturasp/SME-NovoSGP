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
            Cliente.ExecutarPeriodicamente<IServicoNotificacaoAulaPrevista>(c => c.ExecutaNotificacaoAulaPrevista(), Cron.Daily(2));
        }
    }
}