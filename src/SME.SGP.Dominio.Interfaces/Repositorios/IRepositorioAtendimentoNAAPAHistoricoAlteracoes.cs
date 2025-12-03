using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtendimentoNAAPAHistoricoAlteracoes
    {
        Task<long> SalvarAsync(EncaminhamentoNAAPAHistoricoAlteracoes entidade);
        Task<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>> ListarPaginadoPorEncaminhamentoNAAPAId(long encaminhamentoNAAPAId, Paginacao paginacao);
    }
}
