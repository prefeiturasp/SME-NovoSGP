using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCiclo : IRepositorioBase<Ciclo>
    {
        CicloDto ObterCicloPorAno(int ano);

        IEnumerable<CicloDto> ObterCiclosPorAnoModalidade(FiltroCicloDto filtroCicloDto);
    }
}