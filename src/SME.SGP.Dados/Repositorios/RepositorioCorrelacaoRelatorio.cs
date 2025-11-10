using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCorrelacaoRelatorio : RepositorioBase<RelatorioCorrelacao>, IRepositorioCorrelacaoRelatorio
    {
        private readonly ISgpContext contexto;

        public RepositorioCorrelacaoRelatorio(ISgpContext contexto, IServicoAuditoria servicoAuditoria) : base(contexto, servicoAuditoria)
        {
            this.contexto = contexto;
        }

        public RelatorioCorrelacao ObterPorCodigoCorrelacao(Guid codigoCorrelacao)
        {
            return contexto.Conexao.QueryFirstOrDefault<RelatorioCorrelacao>("select * from relatorio_correlacao where codigo = @codigoCorrelacao", new { codigoCorrelacao });
        }

        public async Task<RelatorioCorrelacao> ObterCorrelacaoJasperPorCodigoAsync(Guid codigoCorrelacao)
        {
            var query = @"select
                            rc.*,rcj.*
                        from
                            relatorio_correlacao rc
                        left join relatorio_correlacao_jasper rcj on
                            rc.id = rcj.relatorio_correlacao_id
                        where
                            rc.codigo = @codigoCorrelacao";

            var result = await contexto.Conexao.QueryAsync<RelatorioCorrelacao, RelatorioCorrelacaoJasper, RelatorioCorrelacao>(query,
                (correlacao, jasper) =>
                {
                    correlacao.AdicionarCorrelacaoJasper(jasper);
                    return correlacao;
                }, new { codigoCorrelacao });

            if (result.FirstOrDefault() == null)
            {
                throw new NegocioException( $"Não foi possível obter a correlação do relatório pronto: {codigoCorrelacao} | conn: {contexto.ConnectionString} | query: {query}");
            }

            return result.FirstOrDefault();

        }

        public async Task<DataCriacaoRelatorioDto> ObterDataCriacaoRelatorio(Guid codigoCorrelacao)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<DataCriacaoRelatorioDto>("SELECT criado_em AS CriadoEm FROM relatorio_correlacao rc WHERE codigo = @codigoCorrelacao", new { codigoCorrelacao });
        }

        public async Task<RelatorioCorrelacao> ObterPorCodigoCorrelacaoAsync(Guid codigoCorrelacao)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<RelatorioCorrelacao>("select * from relatorio_correlacao where codigo = @codigoCorrelacao", new { codigoCorrelacao });
        }
    }
}