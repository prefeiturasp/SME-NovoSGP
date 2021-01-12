using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoAEE : IRepositorioBase<EncaminhamentoAEE>
    {
        Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, Paginacao paginacao);
        Task<SituacaoAEE> ObterSituacaoEncaminhamentoAEE(long encaminhamentoAEEId);
    }
}
