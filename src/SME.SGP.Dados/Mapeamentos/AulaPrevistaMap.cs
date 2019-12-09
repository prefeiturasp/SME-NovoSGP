using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaPrevistaMap : BaseMap<AulaPrevista>
    {
        public AulaPrevistaMap()
        {
            ToTable("aula_prevista");
            Map(c => c.Quantidade).ToColumn("aulas_previstas");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}
