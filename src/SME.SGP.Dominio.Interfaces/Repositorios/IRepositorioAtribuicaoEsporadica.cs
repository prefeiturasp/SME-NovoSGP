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

        IEnumerable<AtribuicaoEsporadica> ObterAtribuicoesDatasConflitantes(DateTime dataInicio, DateTime dataFim, string professorRF, string dreCodigo, string ueCodigo, long id = 0);
        AtribuicaoEsporadica ObterUltimaPorRF(string codigoRF, bool somenteInfantil);

        Task<bool> PossuiAtribuicaoPorAnoData(int? anoLetivo, string dreCodigo, string ueCodigo, string codigoRF, DateTime? data);

        Task<IEnumerable<AtribuicaoEsporadica>> ObterAtribuicoesPorRFEAno(string codigoRF, bool somenteInfantil, int anoLetivo, string dreCodigo, string ueCodigo);
    }
}