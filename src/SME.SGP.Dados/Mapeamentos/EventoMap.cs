using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoMap : BaseMap<Evento>
    {
        public EventoMap()
        {
            ToTable("evento");
            Map(c => c.DataFim).ToColumn("data_fim");
            Map(c => c.DataInicio).ToColumn("data_inicio");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Letivo).ToColumn("letivo");
            Map(c => c.FeriadoId).ToColumn("feriado_id");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.TipoEventoId).ToColumn("tipo_evento_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.EventoPaiId).ToColumn("evento_pai_id");
            Map(c => c.WorkflowAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.TipoPerfilCadastro).ToColumn("tipo_perfil_cadastro");
        }
    }
}