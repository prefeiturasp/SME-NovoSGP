using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class DiarioBordoMap: BaseMap<DiarioBordo>
    {
        public DiarioBordoMap()
        {
            ToTable("diario_bordo");
            Map(a => a.AulaId).ToColumn("aula_id");
            Map(a => a.DevolutivaId).ToColumn("devolutiva_id");
            Map(a => a.ReflexoesReplanejamento).ToColumn("reflexoes_replanejamento");
        }
    }
}
