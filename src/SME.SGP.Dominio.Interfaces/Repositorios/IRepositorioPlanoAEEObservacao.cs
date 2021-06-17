using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEEObservacao : IRepositorioBase<PlanoAEEObservacao>
    {
        Task<IEnumerable<PlanoAEEObservacaoDto>> ObterObservacoesPlanoPorId(long planoId, string usuarioRF);
    }
}
