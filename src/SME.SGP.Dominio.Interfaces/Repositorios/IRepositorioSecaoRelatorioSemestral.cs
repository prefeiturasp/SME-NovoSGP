using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoRelatorioSemestral
    {
        Task<IEnumerable<SecaoRelatorioSemestral>> ObterSecoesVigentes(DateTime dataReferencia);
    }
}
