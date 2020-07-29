using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoAlunoMap : BaseMap<ComunicadoAluno>
    {
        public ComunicadoAlunoMap()
        {
            ToTable("comunicado_aluno");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
        }
    }
}
