using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoReaberturaMap : BaseEntityMap<FechamentoReabertura>
    {
        public FechamentoReaberturaMap()
        {
            ToTable("fechamento_reabertura");
            Map(nameof(FechamentoReabertura.DreId), "dre_id");
            Map(nameof(FechamentoReabertura.UeId), "ue_id");
            Map(nameof(FechamentoReabertura.TipoCalendarioId), "tipo_calendario_id");
            Map(nameof(FechamentoReabertura.WorkflowAprovacaoId), "wf_aprovacao_id");
            Map(nameof(FechamentoReabertura.AprovadorId), "aprovador_id");
            Map(nameof(FechamentoReabertura.Descricao), "descricao");
            Map(nameof(FechamentoReabertura.Excluido), "excluido");
            Map(nameof(FechamentoReabertura.Fim), "fim");
            Map(nameof(FechamentoReabertura.Inicio), "inicio");
            Map(nameof(FechamentoReabertura.Migrado), "migrado");
            Map(nameof(FechamentoReabertura.Status), "status");
            Map(nameof(FechamentoReabertura.AprovadoEm), "aprovado_em");
            Map(nameof(FechamentoReabertura.Aplicacao), "aplicacao");
        }
    }
}