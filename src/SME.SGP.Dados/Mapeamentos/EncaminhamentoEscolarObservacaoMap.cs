using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EncaminhamentoEscolarObservacaoMap : BaseMap<EncaminhamentoEscolarObservacao>
    {
        public EncaminhamentoEscolarObservacaoMap()
        {
            ToTable("encaminhamento_escolar_observacao");

            Map(c => c.EncaminhamentoEscolarId).ToColumn("encaminhamento_escolar_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Observacao).ToColumn("observacao");
        }
    }
}