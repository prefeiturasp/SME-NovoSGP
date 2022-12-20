using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoEncaminhamentoNAAPA : IRepositorioBase<SecaoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(int modalidade, long? encaminhamentoNAAPAId = null);
        Task<IEnumerable<EncaminhamentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaDto(long encaminhamentoNAAPAId);
        Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorModalidade(int modalidade, long? encaminhamentoNAAPAId = null);
        Task<SecaoQuestionarioDto> ObterSecaoQuestionarioDtoPorId(long secaoId);
    }
}
