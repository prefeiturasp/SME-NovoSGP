using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoAlunoMap: BaseMap<FechamentoAluno>
    {
        public FechamentoAlunoMap()
        {
            ToTable("fechamento_aluno");
            Map(a => a.FechamentoTurmaDisciplinaId).ToColumn("fechamento_turma_disciplina_id");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
