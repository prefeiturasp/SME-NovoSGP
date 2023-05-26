using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoAtendimentoNAAPAMap : DommelEntityMap<ConsolidadoAtendimentoNAAPA>
    {
        public ConsolidadoAtendimentoNAAPAMap()
        {
            ToTable("consolidado_atendimento_naapa");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.Profissional).ToColumn("profissional");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
        }
    }
}