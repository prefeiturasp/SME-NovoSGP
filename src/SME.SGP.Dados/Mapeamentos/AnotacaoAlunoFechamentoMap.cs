using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AnotacaoAlunoFechamentoMap: BaseMap<AnotacaoAlunoFechamento>
    {
        public AnotacaoAlunoFechamentoMap()
        {
            ToTable("anotacao_aluno_fechamento");
            Map(a => a.FechamentoTurmaDisciplinaId).ToColumn("fechamento_turma_disciplina_id");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}
