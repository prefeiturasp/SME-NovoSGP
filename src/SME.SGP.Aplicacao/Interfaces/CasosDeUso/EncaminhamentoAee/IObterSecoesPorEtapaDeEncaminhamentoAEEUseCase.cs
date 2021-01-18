using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterSecoesPorEtapaDeEncaminhamentoAEEUseCase
    {
        Task<IEnumerable<SecaoQuestionarioDto>> Executar(long encaminhamentoAeeId);
    }
}
