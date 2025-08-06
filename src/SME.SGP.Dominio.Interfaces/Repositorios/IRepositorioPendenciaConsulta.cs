using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaConsulta : IRepositorioBase<Pendencia>
    {
        Task<IEnumerable<long>> FiltrarListaPendenciasUsuario(string turmaCodigo, List<Pendencia> pendencias);
        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioComFiltro(long usuarioId, int[] tiposPendenciasGrupos, string tituloPendencia, string turmaCodigo, Paginacao paginacao);
    }
}
