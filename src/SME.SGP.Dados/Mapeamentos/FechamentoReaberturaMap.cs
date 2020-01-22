using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoReaberturaMap : BaseMap<FechamentoReabertura>
    {
        public FechamentoReaberturaMap()
        {
            ToTable("fechamento_reabertura");
            Map(a => a.Dre).Ignore();
            Map(a => a.DreId).ToColumn("dre_id");

            Map(a => a.Ue).Ignore();
            Map(a => a.UeId).ToColumn("ue_id");

            Map(a => a.TipoCalendario).Ignore();
            Map(a => a.TipoCalendarioId).ToColumn("tipo_calendario_id");

            Map(a => a.WorkflowAprovacao).Ignore();
            Map(a => a.WorkflowAprovacaoId).ToColumn("wf_aprovacao_id");
        }
    }
}