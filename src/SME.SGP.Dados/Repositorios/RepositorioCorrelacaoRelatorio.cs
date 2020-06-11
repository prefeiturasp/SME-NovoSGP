using Dapper;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;

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
    }
}