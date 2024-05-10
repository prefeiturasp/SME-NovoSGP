using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoMapeamentoEstudante : IRepositorioBase<SecaoMapeamentoEstudante>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(long? mapeamentoEstudanteId = null);
        Task<IEnumerable<SecaoMapeamentoEstudante>> ObterSecoesMapeamentoEstudante(long? mapeamentoEstudanteId = null);
    }
}
