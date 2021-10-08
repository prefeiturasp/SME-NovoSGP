using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PerfilEventoTipoMap : BaseMap<PerfilEventoTipo>
    {
        public PerfilEventoTipoMap()
        {
            ToTable("perfil_tipo_evento");
            Map(c => c.EventoTipoId).ToColumn("evento_tipo_id");
            Map(c => c.CodigoPerfil).ToColumn("codigo_perfil");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
