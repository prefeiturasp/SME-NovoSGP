using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObservacaoEncaminhamentoNAAPA : IRepositorioBase<EncaminhamentoNAAPAObservacao>
    {
       Task<PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId,string UsuarioLogadoRf, Paginacao paginacao);
    }
}
