using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendencia : IRepositorioBase<Pendencia>
    {
        Task AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia);
        Task ExcluirPendenciasFechamento(long fechamentoId, TipoPendencia tipoPendencia);
        Task ExclusaoLogicaPendencia(long pendenciaId);
        Task ExclusaoLogicaPendenciaIds(long[] pendenciasIds);
        Task<long[]> ObterIdsPendenciasPorPlanoAEEId(long planoAeeId);
        Task AtualizarStatusPendenciasPorIds(long[] ids, SituacaoPendencia situacaoPendencia);
        Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasPendentes();
        Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasSemPendenciaPerfilUsuario();
        Task<int> ObterModalidadePorPendenciaETurmaId(long pendenciaId, long turmaId);
        Task<IEnumerable<AulasDiasPendenciaDto>> ObterPendenciasParaCargaDiasAulas(int? anoLetivo,long ueid);
        Task AtualizarQuantidadeDiasAulas(long pendenciaId, long quantidadeAulas, long quantidadeDias);
        Task<IEnumerable<Pendencia>> ObterPorIdsAsync(long[] pendenciasId);
        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioSemFiltro(long usuarioId, Paginacao paginacao);
    }
}