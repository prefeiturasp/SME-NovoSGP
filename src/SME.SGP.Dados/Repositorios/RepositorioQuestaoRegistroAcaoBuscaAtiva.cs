using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoRegistroAcaoBuscaAtiva : RepositorioBase<QuestaoRegistroAcaoBuscaAtiva>, IRepositorioQuestaoRegistroAcaoBuscaAtiva
    {
        public RepositorioQuestaoRegistroAcaoBuscaAtiva(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        {
        }

    }
}
