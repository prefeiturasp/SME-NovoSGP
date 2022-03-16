using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseMap: BaseMap<ConselhoClasse>
    {
        public ConselhoClasseMap()
        {
            ToTable("conselho_classe");
            Map(c => c.FechamentoTurmaId).ToColumn("fechamento_turma_id");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
        }
    }
}
