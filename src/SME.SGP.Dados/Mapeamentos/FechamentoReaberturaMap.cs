using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoReaberturaMap : BaseMap<FechamentoReabertura>
    {
        public FechamentoReaberturaMap()
        {
            ToTable("fechamento_reabertura");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Dre).Ignore();
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Fim).ToColumn("fim");
            Map(c => c.Inicio).ToColumn("inicio");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.TipoCalendario).Ignore();
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.Ue).Ignore();
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.WorkflowAprovacao).Ignore();
            Map(c => c.WorkflowAprovacaoId).ToColumn("wf_aprovacao_id");
        }
    }
}