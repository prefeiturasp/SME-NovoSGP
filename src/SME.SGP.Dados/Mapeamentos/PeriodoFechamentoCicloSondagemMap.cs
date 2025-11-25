using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoFechamentoCicloSondagemMap : DommelEntityMap<PeriodoFechamentoCicloSondagem>
    {
        public PeriodoFechamentoCicloSondagemMap()
        {
            ToTable("periodo_fechamento_ciclo_sondagem");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.PeriodoFechamentoId).ToColumn("periodo_fechamento_id");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.Ciclo).ToColumn("ciclo");
            Map(c => c.InicioDoFechamento).ToColumn("inicio_fechamento");
            Map(c => c.FinalDoFechamento).ToColumn("final_fechamento");
        }
    }
}