using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class DevolutivaMap: BaseMap<Devolutiva>
    {
        public DevolutivaMap()
        {
            ToTable("devolutiva");
            Map(a => a.CodigoComponenteCurricular).ToColumn("componente_curricular_codigo");
            Map(a => a.PeriodoInicio).ToColumn("periodo_inicio");
            Map(a => a.PeriodoFim).ToColumn("periodo_fim");
        }
    }
}
