using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoTipoMap : BaseEntityMap<EventoTipo>
    {
        public EventoTipoMap()
        {
            ToTable("evento_tipo");
            Map(nameof(EventoTipo.Ativo), "ativo");
            Map(nameof(EventoTipo.Codigo), "codigo");
            Map(nameof(EventoTipo.Concomitancia), "concomitancia");
            Map(nameof(EventoTipo.Dependencia), "dependencia");
            Map(nameof(EventoTipo.Descricao), "descricao");
            Map(nameof(EventoTipo.Excluido), "excluido");
            Map(nameof(EventoTipo.Letivo), "letivo");
            Map(nameof(EventoTipo.LocalOcorrencia), "local_ocorrencia");
            Map(nameof(EventoTipo.TipoData), "tipo_data");
            Map(nameof(EventoTipo.SomenteLeitura), "somente_leitura");
            Map(nameof(EventoTipo.EventoEscolaAqui), "evento_escolaaqui");
        }
    }
}