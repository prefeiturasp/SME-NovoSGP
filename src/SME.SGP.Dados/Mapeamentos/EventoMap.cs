using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoMap : BaseMap<Evento>
    {
        public EventoMap()
        {
            ToTable("evento");
            Map(nameof(Evento.DataFim), "data_fim");
            Map(nameof(Evento.DataInicio), "data_inicio");
            Map(nameof(Evento.Descricao), "descricao");
            Map(nameof(Evento.DreId), "dre_id");
            Map(nameof(Evento.EventoPaiId), "evento_pai_id");
            Map(nameof(Evento.Excluido), "excluido");
            Map(nameof(Evento.FeriadoId), "feriado_id");
            Map(nameof(Evento.Letivo), "letivo");
            Map(nameof(Evento.Migrado), "migrado");
            Map(nameof(Evento.Nome), "nome");
            Map(nameof(Evento.Status), "status");
            Map(nameof(Evento.TipoCalendarioId), "tipo_calendario_id");
            Map(nameof(Evento.TipoEventoId), "tipo_evento_id");
            Map(nameof(Evento.TipoPerfilCadastro), "tipo_perfil_cadastro");
            Map(nameof(Evento.UeId), "ue_id");
            Map(nameof(Evento.WorkflowAprovacaoId), "wf_aprovacao_id");
        }
    }
}