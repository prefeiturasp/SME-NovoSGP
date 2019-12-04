using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoAvaliacao : IRepositorioBase<TipoAvaliacao>
    {
        Task<PaginacaoResultadoDto<TipoAvaliacao>> ListarPaginado(string nome, Paginacao paginacao);
        Task<bool> VerificarSeJaExistePorNome(string nome, long id);
    }
}