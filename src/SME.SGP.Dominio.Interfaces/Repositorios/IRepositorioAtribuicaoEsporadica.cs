using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtribuicaoEsporadica : IRepositorioBase<AtribuicaoEsporadica>
    {
        Task<PaginacaoResultadoDto<AtribuicaoEsporadica>> ListarPaginada(Paginacao paginacao, int anoLetivo, string dreId, string ueId, string codigoRF);

        AtribuicaoEsporadica ObterUltimaPorRF(string codigoRF);

        IEnumerable<AtribuicaoEsporadica> ObterAtribuicoesDatasConflitantes(DateTime dataInicio, DateTime dataFim, string professorRF, long id = 0);
        AtribuicaoEsporadica ObterUltimaPorRF(string codigoRF, bool somenteInfantil);
    }
}