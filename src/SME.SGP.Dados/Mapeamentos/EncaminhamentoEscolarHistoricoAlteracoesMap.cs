using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EncaminhamentoEscolarHistoricoAlteracoesMap : DommelEntityMap<EncaminhamentoEscolarHistoricoAlteracoes>
    {
        public EncaminhamentoEscolarHistoricoAlteracoesMap()
        {
            ToTable("encaminhamento_escolar_historico_alteracoes");

            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.EncaminhamentoEscolarId).ToColumn("encaminhamento_escolar_id");
            Map(c => c.SecaoEncaminhamentoEscolarId).ToColumn("secao_encaminhamento_escolar_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.CamposInseridos).ToColumn("campos_inseridos");
            Map(c => c.CamposAlterados).ToColumn("campos_alterados");
            Map(c => c.DataAtendimento).ToColumn("data_atendimento");
            Map(c => c.DataHistorico).ToColumn("data_historico");
            Map(c => c.TipoHistorico).ToColumn("tipo_historico");
        }
    }
}