using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFrequencia
    {
        List<RegistroFrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);
    }
}