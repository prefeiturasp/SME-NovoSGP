﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCorrelacaoRelatorio : RepositorioBase<RelatorioCorrelacao>, IRepositorioCorrelacaoRelatorio
    {
        private readonly ISgpContext contexto;

        public RepositorioCorrelacaoRelatorio(ISgpContext contexto) : base(contexto)
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

            return result.FirstOrDefault();

        }
    }
}