using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RespostaMapeamentoEstudanteMap : BaseEntityMap<RespostaMapeamentoEstudante>
    {
        public RespostaMapeamentoEstudanteMap()
        {
            ToTable("mapeamento_estudante_resposta");

            Map(nameof(RespostaMapeamentoEstudante.QuestaoMapeamentoEstudanteId), "questao_mapeamento_estudante_id");
            Map(nameof(RespostaMapeamentoEstudante.RespostaId), "resposta_id");
            Map(nameof(RespostaMapeamentoEstudante.ArquivoId), "arquivo_id");
            Map(nameof(RespostaMapeamentoEstudante.Texto), "texto");
            Map(nameof(RespostaMapeamentoEstudante.Excluido), "excluido");
        }
    }
}