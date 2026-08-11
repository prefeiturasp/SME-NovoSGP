using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SolicitacaoRelatorioMap : BaseMap<SolicitacaoRelatorio>
    {
        public SolicitacaoRelatorioMap()
        {
            ToTable("solicitacao_relatorio");

            Map(nameof(SolicitacaoRelatorio.FiltrosUsados), "filtros_usados");
            Map(nameof(SolicitacaoRelatorio.ExtensaoRelatorio), "extensao_relatorio");
            Map(nameof(SolicitacaoRelatorio.Relatorio), "relatorio");
            Map(nameof(SolicitacaoRelatorio.UsuarioQueSolicitou), "usuario_que_solicitou");
            Map(nameof(SolicitacaoRelatorio.StatusSolicitacao), "status_solicitacao");
            Map(nameof(SolicitacaoRelatorio.Excluido), "excluido");
            Map(nameof(SolicitacaoRelatorio.SolicitadoEm), "solicitado_em");
        }
    }
}