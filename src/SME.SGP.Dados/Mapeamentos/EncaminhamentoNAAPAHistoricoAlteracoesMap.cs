using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPAHistoricoAlteracoesMap : DommelEntityMap<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public EncaminhamentoNAAPAHistoricoAlteracoesMap()
        {
            ToTable("encaminhamento_naapa_historico_alteracoes");

            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.EncaminhamentoNAAPAId).ToColumn("encaminhamento_naapa_id");
            Map(c => c.SecaoEncaminhamentoNAAPAId).ToColumn("secao_encaminhamento_naapa_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.CamposInseridos).ToColumn("campos_inseridos");
            Map(c => c.CamposAlterados).ToColumn("campos_alterados");
            Map(c => c.DataAtendimento).ToColumn("data_atendimento");
            Map(c => c.DataHistorico).ToColumn("data_historico");
            Map(c => c.TipoHistorico).ToColumn("tipo_historico");
        }
    }
}
