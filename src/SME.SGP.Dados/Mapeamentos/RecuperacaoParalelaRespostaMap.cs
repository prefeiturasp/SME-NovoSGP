using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaRespostaMap : BaseMap<RecuperacaoParalelaResposta>
    {
        public RecuperacaoParalelaRespostaMap()
        {
            ToTable("recuperacao_paralela_resposta");
            Map(nameof(RecuperacaoParalelaResposta.Descricao), "descricao");
            Map(nameof(RecuperacaoParalelaResposta.DtFim), "dt_fim");
            Map(nameof(RecuperacaoParalelaResposta.DtInicio), "dt_inicio");
            Map(nameof(RecuperacaoParalelaResposta.Excluido), "excluido");
            Map(nameof(RecuperacaoParalelaResposta.Nome), "nome");
            Map(nameof(RecuperacaoParalelaResposta.Sim), "sim");
        }
    }
}