using Elastic.Apm.Api;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoRegistroAcaoBuscaAtiva : IRepositorioBase<SecaoRegistroAcaoBuscaAtiva>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(long? registroAcaoId = null);
        Task<IEnumerable<SecaoRegistroAcaoBuscaAtiva>> ObterSecoesRegistroAcaoBuscaAtiva(long? registroAcaoId = null);
    }
}
