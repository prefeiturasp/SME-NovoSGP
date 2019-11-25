using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtribuicaoEsporadica : IRepositorioBase<AtribuicaoEsporadica>
    {
        Task<PaginacaoResultadoDto<AtribuicaoEsporadica>> ListarPaginada(Paginacao paginacao, int anoLetivo, string dreId, string ueId, string codigoRF);
    }
}