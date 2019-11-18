using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFrequencia
    {
        IEnumerable<RegistroAusenciaAluno> ObterListaAusenciasPorAula(long aulaId);
    }
}