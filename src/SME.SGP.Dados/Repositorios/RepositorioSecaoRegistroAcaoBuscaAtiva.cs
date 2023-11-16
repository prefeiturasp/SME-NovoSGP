using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoRegistroAcaoBuscaAtiva : RepositorioBase<SecaoRegistroAcaoBuscaAtiva>, IRepositorioSecaoRegistroAcaoBuscaAtiva
    {

        public RepositorioSecaoRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<SecaoQuestionarioDto> ObterSecaoQuestionarioDtoPorId(long secaoId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(long? registroAcaoId = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SecaoRegistroAcaoBuscaAtiva>> ObterSecoesRegistroAcaoBuscaAtiva(long? registroAcaoId = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
