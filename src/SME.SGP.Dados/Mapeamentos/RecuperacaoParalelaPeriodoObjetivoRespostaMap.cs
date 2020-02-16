using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaPeriodoObjetivoRespostaMap : DommelEntityMap<RecuperacaoParalelaPeriodoObjetivoResposta>
    {
        public RecuperacaoParalelaPeriodoObjetivoRespostaMap()
        {
            ToTable("recuperacao_paralela_periodo_objetivo_resposta");
            Map(c => c.ObjetivoId).ToColumn("objetivo_id");
            Map(c => c.PeriodoRecuperacaoParalelaId).ToColumn("periodo_recuperacao_paralela_id");
            Map(c => c.RecuperacaoParalelaId).ToColumn("recuperacao_paralela_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
        }
    }
}