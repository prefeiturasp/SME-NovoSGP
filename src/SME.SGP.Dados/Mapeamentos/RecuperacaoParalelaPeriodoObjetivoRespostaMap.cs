using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaPeriodoObjetivoRespostaMap : BaseMap<RecuperacaoParalelaPeriodoObjetivoResposta>
    {
        public RecuperacaoParalelaPeriodoObjetivoRespostaMap()
        {
            ToTable("recuperacao_paralela_periodo_objetivo_resposta");
            Map(nameof(RecuperacaoParalelaPeriodoObjetivoResposta.Excluido), "excluido");
            Map(nameof(RecuperacaoParalelaPeriodoObjetivoResposta.ObjetivoId), "objetivo_id");
            Map(nameof(RecuperacaoParalelaPeriodoObjetivoResposta.PeriodoRecuperacaoParalelaId), "periodo_recuperacao_paralela_id");
            Map(nameof(RecuperacaoParalelaPeriodoObjetivoResposta.RecuperacaoParalelaId), "recuperacao_paralela_id");
            Map(nameof(RecuperacaoParalelaPeriodoObjetivoResposta.RespostaId), "resposta_id");
        }
    }
}