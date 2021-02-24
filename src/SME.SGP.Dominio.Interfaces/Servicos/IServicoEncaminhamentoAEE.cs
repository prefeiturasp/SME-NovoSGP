using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoEncaminhamentoAEE
    {
        Task<bool> RemoverPendenciasCP(long turmaId, long encaminhamentoAEEId);
        Task<bool> RemovePendenciaCEFAI(long turmaId, long encaminhamentoAEEId);
        Task GerarPendenciaEncaminhamentoAEECP(SituacaoAEE situacao, long encaminhamentoId);
        Task<IEnumerable<UsuarioEolRetornoDto>> ObterPAEETurma(Turma turma);
        Task<long> ObtemUsuarioCEFAIDaDre(string codigoDre);
        Task<List<long>> ListarUsuariosIdPorCodigoUe(string codigoUe);
    }
}