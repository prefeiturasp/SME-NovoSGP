using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaConsulta : IRepositorioBase<Pendencia>
    {
        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioSemFiltro(long usuarioId, Paginacao paginacao);
        Task<IEnumerable<long>> FiltrarListaPendenciasUsuario(string turmaCodigo, List<Pendencia> pendencias);
        Task<PaginacaoResultadoDto<Pendencia>> ListarPendenciasUsuarioComFiltro(long usuarioId, int[] tiposPendenciasGrupos, string tituloPendencia, string turmaCodigo, Paginacao paginacao);


        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioPerfil(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioAula(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioProfessor(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioFechamento(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioDiarioBordo(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioDevolutiva(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioEncaminhamentoAEE(long usuarioId);
        Task<IEnumerable<Pendencia>> ListarPendenciasUsuarioPlanoAEE(long usuarioId);
    }
}
