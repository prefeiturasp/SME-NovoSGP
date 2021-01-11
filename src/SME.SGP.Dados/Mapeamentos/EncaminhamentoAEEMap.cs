using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{ 
    public class EncaminhamentoAEEMap : BaseMap<EncaminhamentoAEE>
    {
        public EncaminhamentoAEEMap()
        {
            ToTable("encaminhamento_aee");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
            Map(a => a.AlunoNome).ToColumn("aluno_nome");
        }
    }
}
