using SME.Background.Core;
using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Background
{
    public static class RegistraServicosRecorrentes
    {
        public static void Registrar()
        {
            Cliente.ExecutarPeriodicamente<IServicoCalculoFrequencia>(x => x.CalcularPercentualFrequenciaAlunosPorDisciplinaEPeriodo(DateTime.Now.Year), "0 6,12 * * *");
        }
    }
}