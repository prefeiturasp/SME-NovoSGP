using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoAtendimentoNAAPAMap : BaseMap<ConsolidadoAtendimentoNAAPA>
    {
        public ConsolidadoAtendimentoNAAPAMap()
        {
            ToTable("consolidado_atendimento_naapa");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.Mes).ToColumn("mes");
            Map(c => c.NomeProfissional).ToColumn("nome_profissional");
            Map(c => c.RfProfissional).ToColumn("rf_profissional");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
        }
    }
}