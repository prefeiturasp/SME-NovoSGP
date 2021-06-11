using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class QuestaoMap : BaseMap<Questao>
    {
        public QuestaoMap()
        {
            ToTable("questao");
            Map(a => a.QuestionarioId).ToColumn("questionario_id");
            Map(a => a.SomenteLeitura).ToColumn("somente_leitura");
        }
    }
}
