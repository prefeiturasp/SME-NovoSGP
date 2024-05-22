using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoProdutividadeFrequencia : RepositorioBase<ConsolidacaoProdutividadeFrequencia>, IRepositorioConsolidacaoProdutividadeFrequencia
    {
        public RepositorioConsolidacaoProdutividadeFrequencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task ExcluirConsolidacoes(string ueCodigo, int anoLetivo)
            => database.Conexao.ExecuteAsync("delete from consolidacao_produtividade_frequencia where ue_id = @ueCodigo and ano_letivo = @anoLetivo",
                                                new { ueCodigo, anoLetivo });
        
    }
}