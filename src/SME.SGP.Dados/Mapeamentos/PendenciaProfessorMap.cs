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
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
        }
    }
}