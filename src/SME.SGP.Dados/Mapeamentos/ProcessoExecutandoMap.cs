using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class ProcessoExecutandoMap: DommelEntityMap<ProcessoExecutando>
    {
        public ProcessoExecutandoMap()
        {
            ToTable("processo_executando");
            Map(c => c.TipoProcesso).ToColumn("tipo_processo");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.AulaId).ToColumn("aula_id");
        }
    }
}
