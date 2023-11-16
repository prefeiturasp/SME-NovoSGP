using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoRegistroAcaoBuscaAtiva : RepositorioBase<QuestaoRegistroAcaoBuscaAtiva>, IRepositorioQuestaoRegistroAcaoBuscaAtiva
    {
        public RepositorioQuestaoRegistroAcaoBuscaAtiva(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        {
        }

        public Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long registroAcaoSecaoId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<RespostaQuestaoRegistroAcaoBuscaAtivaDto>> ObterRespostasRegistroAcao(long registroAcaoId)
        {
            throw new System.NotImplementedException();
        }
    }
}
