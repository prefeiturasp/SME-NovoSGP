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
            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.ExecutaNotificacaoRegistroFrequencia(), Cron.Daily(2));

            Cliente.ExecutarPeriodicamente<IServicoNotificacaoAulaPrevista>(c => c.ExecutaNotificacaoAulaPrevista(), Cron.Daily(2));

            // de segunda a sexta as 10, 14 e 16 horas
            Cliente.ExecutarPeriodicamente<IServicoAbrangencia>(c => c.SincronizarEstruturaInstitucionalVigenteCompleta(), "0 13,17,19 * * 1-5");

            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.VerificaNotificacaoBimestral(), Cron.Daily(2));

            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.NotificarAlunosFaltosos(), Cron.Daily(2));

            Cliente.ExecutarPeriodicamente<IServicoNotificacaoFrequencia>(c => c.NotificarAlunosFaltososBimestre(), Cron.Daily(3));
        }
    }
}