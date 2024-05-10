using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class CompensacaoAusenciaDisciplinaRegenciaMap: BaseMap<CompensacaoAusenciaDisciplinaRegencia>
    {
        public CompensacaoAusenciaDisciplinaRegenciaMap()
        {
            ToTable("compensacao_ausencia_disciplina_regencia");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.CompensacaoAusenciaId).ToColumn("compensacao_ausencia_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
        }
    }
}
