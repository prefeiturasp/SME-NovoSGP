using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaDres
    {
        Task<IEnumerable<UnidadeEscolarDto>> ObterEscolasPorDre(string dreId);

        Task<IEnumerable<UnidadeEscolarDto>> ObterEscolasSemAtribuicao(string dreId, int tipoResponsavel);

        Task<IEnumerable<DreConsultaDto>> ObterTodos();
    }
}