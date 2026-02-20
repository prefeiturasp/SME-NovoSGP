using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SolicitacaoRelatorioMap : BaseMap<SolicitacaoRelatorio>
    {
        public SolicitacaoRelatorioMap()
        {
            ToTable("solicitacao_relatorio");
            Map(c => c.FiltrosUsados).ToColumn("filtros_usados");
            Map(c => c.TipoRelatorio).ToColumn("tipo_relatorio");
            Map(c => c.UsuarioQueSolicitou).ToColumn("usuario_que_solicitou");
            Map(c => c.StatusSolicitacao).ToColumn("status_solicitacao");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
