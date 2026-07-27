using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class OpcaoRespostaMap : BaseEntityMap<OpcaoResposta>
    {
        public OpcaoRespostaMap()
        {
            ToTable("opcao_resposta");
            Map(nameof(OpcaoResposta.QuestaoId), "questao_id");
            Map(nameof(OpcaoResposta.Ordem), "ordem");
            Map(nameof(OpcaoResposta.Nome), "nome");
            Map(nameof(OpcaoResposta.Observacao), "observacao");
            Map(nameof(OpcaoResposta.Excluido), "excluido");
        }
    }
}