using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoTurmaMap: BaseMap<FechamentoTurma>
    {
        public FechamentoTurmaMap()
        {
            ToTable("fechamento_turma");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
