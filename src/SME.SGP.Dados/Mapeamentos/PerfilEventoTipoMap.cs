using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PerfilEventoTipoMap : DommelEntityMap<PerfilEventoTipo>
    {
        public PerfilEventoTipoMap()
        {
            ToTable("perfil_evento_tipo");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.EventoTipoId).ToColumn("evento_tipo_id");
            Map(c => c.CodigoPerfil).ToColumn("codigo_perfil");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.AlteradoEm).Ignore();
            Map(c => c.AlteradoPor).Ignore();
            Map(c => c.AlteradoRF).Ignore();
            Map(c => c.CriadoEm).Ignore();
            Map(c => c.CriadoPor).Ignore();
            Map(c => c.CriadoRF).Ignore();
        }
    }
}
