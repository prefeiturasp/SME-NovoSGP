﻿using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanejamentoAnualComponenteMap : BaseMap<PlanejamentoAnualComponente>
    {
        public PlanejamentoAnualComponenteMap()
        {
            ToTable("planejamento_anual_componente");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.PlanejamentoAnualPeriodoEscolarId).ToColumn("planejamento_anual_periodo_escolar_id");
        }
    }
}