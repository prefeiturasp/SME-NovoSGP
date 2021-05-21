using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoMatriculaTurmaMap : DommelEntityMap<ConsolidacaoMatriculaTurma>
    {
        public ConsolidacaoMatriculaTurmaMap()
        {
            ToTable("consolidacao_matricula_turma");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
        }
    }
}
