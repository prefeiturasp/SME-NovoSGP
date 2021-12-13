using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendencia : IRepositorioBase<Pendencia>
    {
        void AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia);
        
        void ExcluirPendenciasFechamento(long fechamentoId, TipoPendencia tipoPendencia);

        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuario(long usuarioId, int[] tiposPendencias, string tituloPendencia, string turmaCodigo, Paginacao paginacao, int? tipoGrupo);


        Task<Pendencia> FiltrarListaPendenciasUsuario(string turmaCodigo, Pendencia pendencia);
        
        Task<long[]> ObterIdsPendenciasPorPlanoAEEId(long planoAeeId);
        
        Task AtualizarStatusPendenciasPorIds(long[] ids, SituacaoPendencia situacaoPendencia);
        
        Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasPendentes();
        
        Task<IEnumerable<PendenciaPendenteDto>> ObterPendenciasSemPendenciaPerfilUsuario();

        Task<int> ObterModalidadePorPendenciaETurmaId(long pendenciaId, long turmaId);
    }
}