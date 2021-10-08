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
            Map(c => c.QuestionarioId).ToColumn("questionario_id");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.Obrigatorio).ToColumn("obrigatorio");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Opcionais).ToColumn("opcionais");
            Map(c => c.SomenteLeitura).ToColumn("somente_leitura");
        }
    }
}
