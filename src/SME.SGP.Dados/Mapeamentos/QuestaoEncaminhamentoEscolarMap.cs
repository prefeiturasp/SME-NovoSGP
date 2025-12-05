using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Mapeamentos
{
    public class QuestaoEncaminhamentoEscolarMap : BaseMap<QuestaoEncaminhamentoEscolar>
    {
        public QuestaoEncaminhamentoEscolarMap()
        {
            ToTable("encaminhamento_escolar_questao");

            Map(c => c.EncaminhamentoEscolarSecaoId).ToColumn("encaminhamento_escolar_secao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}