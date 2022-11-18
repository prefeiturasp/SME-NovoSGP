using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase
    {
        Task<IEnumerable<SecaoQuestionarioDto>> Executar(long encaminhamentoNAAPAId);
    }
}
