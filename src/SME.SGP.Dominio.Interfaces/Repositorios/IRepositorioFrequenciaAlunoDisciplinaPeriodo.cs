using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodo : IRepositorioBase<FrequenciaAluno>
    {
        FrequenciaAluno Obter(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim, TipoFrequenciaAluno tipoFrequencia);
    }
}