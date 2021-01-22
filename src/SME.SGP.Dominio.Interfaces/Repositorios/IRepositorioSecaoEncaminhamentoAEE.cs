using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoEncaminhamentoAEE : IRepositorioBase<SecaoEncaminhamentoAEE>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoPorEtapa(long etapa, long encaminhamentoAeeId = 0);
    }
}
