using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PendenciaUsuarioMap : BaseMap<PendenciaUsuario>
    {
        public PendenciaUsuarioMap()
        {
            ToTable("pendencia_usuario");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
        }
    }
}
