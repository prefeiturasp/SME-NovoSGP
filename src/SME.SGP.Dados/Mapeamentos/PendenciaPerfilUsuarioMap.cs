using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilUsuarioMap : BaseMap<PendenciaPerfilUsuario>
    {
        public PendenciaPerfilUsuarioMap()
        {
            ToTable("pendencia_perfil_usuario");
            Map(c => c.PendenciaPerfilId).ToColumn("pendencia_perfil_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
        }
    }
}
