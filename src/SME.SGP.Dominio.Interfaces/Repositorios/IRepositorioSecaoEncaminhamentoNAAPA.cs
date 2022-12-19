using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoEncaminhamentoNAAPA : IRepositorioBase<SecaoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoDtoPorEtapa(List<int> etapas, long? encaminhamentoNAAPAId = null);
        Task<IEnumerable<EncaminhamentoNAAPASecaoItineranciaDto>> ObterSecoesItineranciaEncaminhamentoDto(long encaminhamentoNAAPAId);
        Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorEtapaModalidade(List<int> etapas, int modalidade, long? encaminhamentoNAAPAId = null);
    }
}
