using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("usuario");
            Map(a => a.CodigoRf).ToColumn("rf_codigo");
        }

    }
}
