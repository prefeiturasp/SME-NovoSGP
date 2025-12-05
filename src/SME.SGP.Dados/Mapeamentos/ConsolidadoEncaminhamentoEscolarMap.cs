using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidadoEncaminhamentoEscolarMap : BaseMap<ConsolidadoEncaminhamentoEscolar>
    {
        public ConsolidadoEncaminhamentoEscolarMap()
        {
            ToTable("consolidado_encaminhamento_escolar");

            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.Situacao).ToColumn("situacao");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
        }
    }
}