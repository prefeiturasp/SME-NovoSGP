using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendencia : IRepositorioBase<Pendencia>
    {
        void AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia);
        void ExcluirPendenciasFechamento(long fechamentoId, TipoPendencia tipoPendencia);
        void ExclusaoLogicaPendencia(long pendenciaId);
        void ExclusaoLogicaPendenciaIds(long[] pendenciasIds);
        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioComFiltro(long usuarioId, int[] tiposPendenciasGrupos, string tituloPendencia, string turmaCodigo, Paginacao paginacao);
        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioSemFiltro(long usuarioId, Paginacao paginacao);
        Task<IEnumerable<long>> FiltrarListaPendenciasUsuario(string turmaCodigo, List<Pendencia> pendencias);
        Task<long[]> ObterIdsPendenciasPorPlanoAEEId(long planoAeeId);
        Task AtualizarStatusPendenciasPorIds(long[] ids, SituacaoPendencia situacaoPendencia);
        Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasPendentes();
        Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasSemPendenciaPerfilUsuario();
        Task<int> ObterModalidadePorPendenciaETurmaId(long pendenciaId, long turmaId);
        Task<IEnumerable<AulasDiasPendenciaDto>> ObterPendenciasParaCargaDiasAulas(int? anoLetivo,long ueid);
        Task AtualizarQuantidadeDiasAulas(long pendenciaId, long quantidadeAulas, long quantidadeDias);
    }
}