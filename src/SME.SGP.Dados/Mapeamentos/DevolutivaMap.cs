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
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.CodigoComponenteCurricular).ToColumn("componente_curricular_codigo");
            Map(c => c.PeriodoInicio).ToColumn("periodo_inicio");
            Map(c => c.PeriodoFim).ToColumn("periodo_fim");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
