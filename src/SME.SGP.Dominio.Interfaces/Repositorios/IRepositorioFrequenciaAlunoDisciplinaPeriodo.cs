using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodo : IRepositorioBase<FrequenciaAlunoDisciplinaPeriodo>
    {
        FrequenciaAlunoDisciplinaPeriodo Obter(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim);
    }
}