using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class DevolutivaDiarioBordoMap: BaseMap<DevolutivaDiarioBordo>
    {
        public DevolutivaDiarioBordoMap()
        {
            ToTable("devolutiva_diario_bordo");
        }
    }
}
