using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PendenciaPerfilMap : BaseMap<PendenciaPerfil>
    {
        public PendenciaPerfilMap()
        {
            ToTable("pendencia_perfil");
            Map(c => c.Cargo).ToColumn("cargo");
            Map(c => c.Nivel).ToColumn("nivel");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
        }
    }
}
