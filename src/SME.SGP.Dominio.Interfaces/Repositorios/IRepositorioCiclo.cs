using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCiclo : IRepositorioBase<Ciclo>
    {
        CicloDto ObterCicloPorAno(int ano);

        IEnumerable<CicloDto> ObterCiclosPorTurma(IEnumerable<FiltroCicloDto> filtroCicloDtos);
    }
}