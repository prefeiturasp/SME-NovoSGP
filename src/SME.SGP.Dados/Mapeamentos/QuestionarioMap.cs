using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class QuestionarioMap : BaseMap<Questionario>
    {
        public QuestionarioMap()
        {
            ToTable("questionario");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
