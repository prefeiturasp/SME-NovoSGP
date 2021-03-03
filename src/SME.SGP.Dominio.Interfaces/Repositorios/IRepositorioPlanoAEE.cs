using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEE : IRepositorioBase<PlanoAEE>
    {
        Task<PaginacaoResultadoDto<PlanoAEEAlunoTurmaDto>> ListarPaginado(long dreId, long ueId, long turmaId, string alunoCodigo, int? situacao, Paginacao paginacao);

        Task<PlanoAEEResumoDto> ObterPlanoPorEstudante(string codigoEstudante);

        Task<PlanoAEE> ObterPlanoComTurmaPorId(long planoId);
        Task<IEnumerable<PlanoAEE>> ObterPlanosAtivos();
        Task<int> AtualizarSituacaoPlanoPorVersao(long versaoId, int situacao);
        Task<IEnumerable<PlanoAEE>> ObterPorDataFinalVigencia(DateTime dataFim, bool desconsiderarPendencias = true, bool desconsiderarNotificados = false);
    }
}
