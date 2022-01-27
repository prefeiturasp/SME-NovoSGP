using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAula : IRepositorioBase<Aula>
    {
        void SalvarVarias(IEnumerable<Aula> aulas);
        Task ExcluirPeloSistemaAsync(long[] idsAulas);
    }
}