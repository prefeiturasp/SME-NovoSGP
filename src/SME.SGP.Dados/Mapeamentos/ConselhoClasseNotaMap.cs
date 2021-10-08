using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseNotaMap: BaseMap<ConselhoClasseNota>
    {
        public ConselhoClasseNotaMap()
        {
            ToTable("conselho_classe_nota");
            Map(c => c.ConselhoClasseAlunoId).ToColumn("conselho_classe_aluno_id");
            Map(c => c.ComponenteCurricularCodigo).ToColumn("componente_curricular_codigo");
            Map(c => c.Nota).ToColumn("nota");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
            Map(c => c.Justificativa).ToColumn("justificativa");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
        }
    }
}
