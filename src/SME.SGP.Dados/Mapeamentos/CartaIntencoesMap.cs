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
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.Planejamento).ToColumn("planejamento");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
