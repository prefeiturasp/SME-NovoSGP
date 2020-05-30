using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoRelatorioSemestralPAP
    {
        Task<IEnumerable<SecaoRelatorioSemestralPAP>> ObterSecoesVigentes(DateTime dataReferencia);
    }
}
