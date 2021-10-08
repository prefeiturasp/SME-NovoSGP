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
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.DevolutivaId).ToColumn("devolutiva_id");
            Map(c => c.Planejamento).ToColumn("planejamento");
            Map(c => c.ReflexoesReplanejamento).ToColumn("reflexoes_replanejamento");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
        }
    }
}
