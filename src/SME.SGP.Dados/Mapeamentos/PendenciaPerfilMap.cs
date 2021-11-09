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
            Map(c => c.PerfilCodigo).ToColumn("perfil_codigo");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
        }
    }
}
