using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPAHistoricoAlteracoesMap : SimpleEntityMap<EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        public EncaminhamentoNAAPAHistoricoAlteracoesMap()
        {
            ToTable("encaminhamento_naapa_historico_alteracoes");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.EncaminhamentoNAAPAId), "encaminhamento_naapa_id");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.SecaoEncaminhamentoNAAPAId), "secao_encaminhamento_naapa_id");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.UsuarioId), "usuario_id");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.CamposInseridos), "campos_inseridos");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.CamposAlterados), "campos_alterados");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.DataAtendimento), "data_atendimento");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.DataHistorico), "data_historico");
            Map(nameof(EncaminhamentoNAAPAHistoricoAlteracoes.TipoHistorico), "tipo_historico");
        }
    }
}