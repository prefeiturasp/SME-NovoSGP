using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PerfilEventoTipoMap : SimpleEntityMap<PerfilEventoTipo>
    {
        public PerfilEventoTipoMap()
        {
            ToTable("perfil_evento_tipo");
            Map(nameof(PerfilEventoTipo.EventoTipoId), "evento_tipo_id");
            Map(nameof(PerfilEventoTipo.CodigoPerfil), "codigo_perfil");
            Map(nameof(PerfilEventoTipo.Excluido), "excluido");
            Map(nameof(PerfilEventoTipo.AlteradoEm),"alterado_em");
            Map(nameof(PerfilEventoTipo.AlteradoPor),"alterado_por");
            Map(nameof(PerfilEventoTipo.AlteradoRF),"alterado_rf");
            Map(nameof(PerfilEventoTipo.CriadoEm),"criado_em");
            Map(nameof(PerfilEventoTipo.CriadoPor),"criado_por");
            Map(nameof(PerfilEventoTipo.CriadoRF),"criado_rf");
        }
    }
}