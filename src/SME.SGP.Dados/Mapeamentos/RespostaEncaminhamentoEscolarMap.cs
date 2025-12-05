using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RespostaEncaminhamentoEscolarMap : BaseMap<RespostaEncaminhamentoEscolar>
    {
        public RespostaEncaminhamentoEscolarMap()
        {
            ToTable("encaminhamento_escolar_resposta");

            Map(c => c.QuestaoEncaminhamentoId).ToColumn("questao_encaminhamento_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}