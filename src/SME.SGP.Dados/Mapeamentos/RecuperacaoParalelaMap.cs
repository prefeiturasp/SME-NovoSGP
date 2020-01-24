using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaMap : DommelEntityMap<RecuperacaoParalela>
    {
        public RecuperacaoParalelaMap()
        {
            ToTable("recuperacao_paralela");
            Map(c => c.ObjetivoId).ToColumn("objetivo_id");
            Map(c => c.PeriodoRecuperacaoParalelaId).ToColumn("periodo_recuperacao_paralela_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}