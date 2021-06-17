using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaProfessorMap : DommelEntityMap<PendenciaProfessor>
    {
        public PendenciaProfessorMap()
        {
            ToTable("pendencia_professor");
            Map(a => a.PendenciaId).ToColumn("pendencia_id");
            Map(a => a.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(a => a.ProfessorRf).ToColumn("professor_rf");
        }
    }
}