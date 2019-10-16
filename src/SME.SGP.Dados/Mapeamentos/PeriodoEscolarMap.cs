using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoEscolarMap : BaseMap<PeriodoEscolar>
    {
        public PeriodoEscolarMap()
        {
            ToTable("periodo_escolar");
            Map(c => c.TipoCalendario).ToColumn("tipo_calendario_id");
            Map(c => c.PeriodoFim).ToColumn("periodo_fim");
            Map(c => c.PeriodoInicio).ToColumn("periodo_inicio");
        }
    }
}
