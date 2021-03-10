﻿using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoAEE : IRepositorioBase<EncaminhamentoAEE>
    {
        Task<PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, string responsavelRf, int anoLetivo, Paginacao paginacao);
        Task<SituacaoAEE> ObterSituacaoEncaminhamentoAEE(long encaminhamentoAEEId);
        Task<EncaminhamentoAEE> ObterEncaminhamentoPorId(long id);
        Task<EncaminhamentoAEE> ObterEncaminhamentoComTurmaPorId(long encaminhamentoId);
        Task<EncaminhamentoAEEAlunoTurmaDto> ObterEncaminhamentoPorEstudante(string codigoEstudante);
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterResponsaveis(long dreId, long ueId, long turmaId, string alunoCodigo, int anoLetivo, int? situacao);
    }
}
