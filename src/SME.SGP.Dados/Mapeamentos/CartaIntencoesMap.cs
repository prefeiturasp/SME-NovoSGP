using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class CartaIntencoesMap : BaseMap<CartaIntencoes>
    {
        public CartaIntencoesMap()
        {
            ToTable("carta_intencoes");
            Map(x => x.TurmaId).ToColumn("turma_id");
            Map(x => x.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(x => x.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(x => x.Planejamento).ToColumn("planejamento");
        }
    }
}
