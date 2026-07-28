using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class QuestaoRegistroAcaoBuscaAtivaMap : BaseEntityMap<QuestaoRegistroAcaoBuscaAtiva>
    {
        public QuestaoRegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa_questao");
            Map(nameof(QuestaoRegistroAcaoBuscaAtiva.RegistroAcaoBuscaAtivaSecaoId), "registro_acao_busca_ativa_secao_id");
            Map(nameof(QuestaoRegistroAcaoBuscaAtiva.QuestaoId), "questao_id");
            Map(nameof(QuestaoRegistroAcaoBuscaAtiva.Excluido), "excluido");
        }
    }
}